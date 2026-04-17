namespace AudioRoute;

using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

public sealed class AudioRouterService : IDisposable
{
    private readonly object _syncLock = new();
    private WasapiLoopbackCapture? _capture;
    private readonly Dictionary<string, TargetPlayback> _targetPlaybacks = new(StringComparer.OrdinalIgnoreCase);
    private bool _disposed;

    public bool IsRunning { get; private set; }

    public IReadOnlyList<AudioDeviceInfo> GetPlaybackDevices()
    {
        using var enumerator = new MMDeviceEnumerator();
        var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.All);
        string? defaultId = null;

        try
        {
            defaultId = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia).ID;
        }
        catch
        {
        }

        return devices
            .Select(device => new AudioDeviceInfo(
                device.ID,
                device.FriendlyName,
                device.State,
                string.Equals(device.ID, defaultId, StringComparison.OrdinalIgnoreCase)))
            .OrderByDescending(device => device.IsDefault)
            .ThenByDescending(device => device.IsActive)
            .ThenBy(device => device.Name, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    public void Start(string sourceDeviceId, IEnumerable<TargetDeviceRoute> targetRoutes, int latencyMs)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        var distinctTargets = targetRoutes
            .Where(route => !string.IsNullOrWhiteSpace(route.DeviceId))
            .DistinctBy(route => route.DeviceId, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        if (distinctTargets.Length == 0)
        {
            throw new InvalidOperationException("Select at least one target device.");
        }

        Stop();

        using var enumerator = new MMDeviceEnumerator();
        var sourceDevice = enumerator.GetDevice(sourceDeviceId);

        if (!sourceDevice.State.HasFlag(DeviceState.Active))
        {
            throw new InvalidOperationException("The selected source device is not active in Windows.");
        }

        var capture = new WasapiLoopbackCapture(sourceDevice);
        var playbacks = new Dictionary<string, TargetPlayback>(StringComparer.OrdinalIgnoreCase);

        try
        {
            foreach (var route in distinctTargets)
            {
                if (string.Equals(route.DeviceId, sourceDeviceId, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var targetDevice = enumerator.GetDevice(route.DeviceId);

                if (!targetDevice.State.HasFlag(DeviceState.Active))
                {
                    continue;
                }

                var bufferedProvider = new BufferedWaveProvider(capture.WaveFormat)
                {
                    DiscardOnBufferOverflow = true,
                    ReadFully = true,
                    BufferDuration = TimeSpan.FromMilliseconds(Math.Max(latencyMs * 8, 1600))
                };

                var volumeProvider = new VolumeSampleProvider(bufferedProvider.ToSampleProvider())
                {
                    Volume = Math.Clamp(route.Volume, 0f, 1f)
                };

                var output = new WasapiOut(targetDevice, AudioClientShareMode.Shared, false, Math.Max(80, latencyMs));
                output.Init(new SampleToWaveProvider(volumeProvider));
                output.Play();

                playbacks.Add(route.DeviceId, new TargetPlayback(output, bufferedProvider, volumeProvider));
            }

            if (playbacks.Count == 0)
            {
                throw new InvalidOperationException("None of the selected target devices are active in Windows.");
            }

            capture.DataAvailable += CaptureOnDataAvailable;
            capture.RecordingStopped += CaptureOnRecordingStopped;
            capture.StartRecording();

            lock (_syncLock)
            {
                _capture = capture;

                foreach (var pair in playbacks)
                {
                    _targetPlaybacks[pair.Key] = pair.Value;
                }

                IsRunning = true;
            }
        }
        catch
        {
            capture.Dispose();

            foreach (var playback in playbacks.Values)
            {
                playback.Dispose();
            }

            throw;
        }
    }

    public void Stop()
    {
        lock (_syncLock)
        {
            if (_capture is not null)
            {
                _capture.DataAvailable -= CaptureOnDataAvailable;
                _capture.RecordingStopped -= CaptureOnRecordingStopped;

                try
                {
                    _capture.StopRecording();
                }
                catch
                {
                }

                _capture.Dispose();
                _capture = null;
            }

            foreach (var playback in _targetPlaybacks.Values)
            {
                playback.Dispose();
            }

            _targetPlaybacks.Clear();
            IsRunning = false;
        }
    }

    public void SetTargetVolume(string deviceId, float volume)
    {
        lock (_syncLock)
        {
            if (_targetPlaybacks.TryGetValue(deviceId, out var playback))
            {
                playback.VolumeProvider.Volume = Math.Clamp(volume, 0f, 1f);
            }
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        Stop();
        _disposed = true;
    }

    private void CaptureOnDataAvailable(object? sender, WaveInEventArgs e)
    {
        lock (_syncLock)
        {
            if (!IsRunning || _targetPlaybacks.Count == 0)
            {
                return;
            }

            foreach (var playback in _targetPlaybacks.Values)
            {
                TrimBufferedAudio(playback.BufferedProvider);
                playback.BufferedProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
            }
        }
    }

    private void CaptureOnRecordingStopped(object? sender, StoppedEventArgs e)
    {
        if (_disposed)
        {
            return;
        }

        Stop();
    }

    private static void TrimBufferedAudio(BufferedWaveProvider bufferedProvider)
    {
        var maxBufferedBytes = bufferedProvider.WaveFormat.AverageBytesPerSecond;
        var bufferedBytes = bufferedProvider.BufferedBytes;

        if (bufferedBytes <= maxBufferedBytes)
        {
            return;
        }

        var bytesToDrop = bufferedBytes - maxBufferedBytes;
        var discardBuffer = new byte[Math.Min(bytesToDrop, 16384)];

        while (bytesToDrop > 0)
        {
            var bytesRead = bufferedProvider.Read(discardBuffer, 0, Math.Min(discardBuffer.Length, bytesToDrop));

            if (bytesRead <= 0)
            {
                break;
            }

            bytesToDrop -= bytesRead;
        }
    }

    private sealed class TargetPlayback : IDisposable
    {
        public TargetPlayback(WasapiOut output, BufferedWaveProvider bufferedProvider, VolumeSampleProvider volumeProvider)
        {
            Output = output;
            BufferedProvider = bufferedProvider;
            VolumeProvider = volumeProvider;
        }

        public WasapiOut Output { get; }

        public BufferedWaveProvider BufferedProvider { get; }

        public VolumeSampleProvider VolumeProvider { get; }

        public void Dispose()
        {
            try
            {
                Output.Stop();
            }
            catch
            {
            }

            Output.Dispose();
        }
    }
}
