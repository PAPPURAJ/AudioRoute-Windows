namespace AudioRoute;

using System.Diagnostics;

public partial class Form1 : Form
{
    private readonly AudioRouterService _router = new();
    private readonly Dictionary<string, int> _targetVolumes = new(StringComparer.OrdinalIgnoreCase);
    private List<AudioDeviceInfo> _devices = [];
    private bool _isRefreshingDevices;
    private string _deviceSnapshot = string.Empty;

    public Form1()
    {
        InitializeComponent();
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(1040, 720);
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        deviceRefreshTimer.Start();
        RefreshDevices();
        SetStatus("Choose a source device and one or more extra playback devices.");
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        deviceRefreshTimer.Stop();
        _router.Dispose();
        base.OnFormClosed(e);
    }

    private bool RefreshDevices(bool preserveStatus = false)
    {
        if (_isRefreshingDevices)
        {
            return false;
        }

        _isRefreshingDevices = true;
        var selectedSourceId = (comboSource.SelectedItem as AudioDeviceInfo)?.Id;
        var selectedTargets = GetSelectedTargetIds();
        var previousSnapshot = _deviceSnapshot;

        try
        {
            _devices = _router.GetPlaybackDevices().ToList();
            _deviceSnapshot = string.Join("|", _devices.Select(device => $"{device.Id}:{device.Name}:{device.State}:{device.IsDefault}"));

            var deviceIds = _devices.Select(device => device.Id).ToHashSet(StringComparer.OrdinalIgnoreCase);
            var staleIds = _targetVolumes.Keys.Where(id => !deviceIds.Contains(id)).ToArray();

            foreach (var staleId in staleIds)
            {
                _targetVolumes.Remove(staleId);
            }

            foreach (var device in _devices)
            {
                _targetVolumes.TryAdd(device.Id, 85);
            }

            comboSource.BeginUpdate();
            comboSource.Items.Clear();

            foreach (var device in _devices)
            {
                comboSource.Items.Add(device);
            }

            var sourceDevice = _devices.FirstOrDefault(device => device.Id == selectedSourceId && device.IsActive)
                ?? _devices.FirstOrDefault(device => device.IsDefault && device.IsActive)
                ?? _devices.FirstOrDefault(device => device.IsActive)
                ?? _devices.FirstOrDefault();

            comboSource.SelectedItem = sourceDevice;
            comboSource.EndUpdate();

            PopulateTargetList(selectedTargets);
            RebuildVolumePanel();
            refreshInfoLabel.Text = $"Last scan: {DateTime.Now:HH:mm:ss}";

            if (!preserveStatus)
            {
                SetStatus(_devices.Count == 0
                    ? "No playback devices found."
                    : "Choose the device already playing your movie as the source, then select the extra speakers or headphones.");
            }

            UpdateUiState();
            return !string.Equals(previousSnapshot, _deviceSnapshot, StringComparison.Ordinal);
        }
        catch (Exception ex)
        {
            SetStatus($"Could not read audio devices: {ex.Message}");
            return false;
        }
        finally
        {
            _isRefreshingDevices = false;
        }
    }

    private void PopulateTargetList(HashSet<string> selectedTargetIds)
    {
        checkedTargets.BeginUpdate();
        checkedTargets.Items.Clear();

        var sourceId = (comboSource.SelectedItem as AudioDeviceInfo)?.Id;

        foreach (var device in _devices.Where(device => device.Id != sourceId))
        {
            var index = checkedTargets.Items.Add(device);
            checkedTargets.SetItemChecked(index, selectedTargetIds.Contains(device.Id));
        }

        checkedTargets.EndUpdate();
    }

    private HashSet<string> GetSelectedTargetIds()
    {
        return checkedTargets.CheckedItems
            .OfType<AudioDeviceInfo>()
            .Select(device => device.Id)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    private void RebuildVolumePanel()
    {
        targetVolumePanel.SuspendLayout();
        targetVolumePanel.Controls.Clear();

        var selectedTargets = checkedTargets.CheckedItems.OfType<AudioDeviceInfo>().ToArray();

        if (selectedTargets.Length == 0)
        {
            targetVolumePanel.Controls.Add(new Label
            {
                AutoSize = true,
                Margin = new Padding(6),
                MaximumSize = new Size(270, 0),
                Text = "Select one or more extra devices to set their individual volume."
            });

            targetVolumePanel.ResumeLayout();
            return;
        }

        foreach (var device in selectedTargets)
        {
            var volumePercent = _targetVolumes.TryGetValue(device.Id, out var stored) ? stored : 85;
            var currentDevice = device;

            var card = new Panel
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, 0, 10),
                Padding = new Padding(12),
                Size = new Size(300, 92)
            };

            var nameLabel = new Label
            {
                AutoEllipsis = true,
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold, GraphicsUnit.Point),
                Location = new Point(12, 12),
                Size = new Size(210, 22),
                Text = currentDevice.Name
            };

            var valueLabel = new Label
            {
                Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point),
                Location = new Point(228, 14),
                Size = new Size(54, 20),
                Text = $"{volumePercent}%",
                TextAlign = ContentAlignment.TopRight
            };

            var slider = new TrackBar
            {
                AutoSize = false,
                LargeChange = 10,
                Location = new Point(12, 46),
                Maximum = 100,
                Minimum = 0,
                Size = new Size(270, 30),
                SmallChange = 5,
                TickFrequency = 10,
                Value = volumePercent
            };

            slider.Scroll += (_, _) => UpdateTargetVolume(currentDevice.Id, slider.Value, valueLabel);

            card.Controls.Add(nameLabel);
            card.Controls.Add(valueLabel);
            card.Controls.Add(slider);
            targetVolumePanel.Controls.Add(card);
        }

        targetVolumePanel.ResumeLayout();
    }

    private void UpdateTargetVolume(string deviceId, int volumePercent, Label valueLabel)
    {
        _targetVolumes[deviceId] = volumePercent;
        valueLabel.Text = $"{volumePercent}%";

        if (_router.IsRunning)
        {
            _router.SetTargetVolume(deviceId, volumePercent / 100f);
        }
    }

    private void UpdateUiState()
    {
        var isRunning = _router.IsRunning;
        buttonStart.Enabled = !isRunning && comboSource.SelectedItem is not null && checkedTargets.CheckedItems.Count > 0;
        buttonStop.Enabled = isRunning;
        buttonRefresh.Enabled = !isRunning;
        comboSource.Enabled = !isRunning;
        checkedTargets.Enabled = !isRunning;
        latencyInput.Enabled = !isRunning;
        autoRefreshCheckBox.Enabled = !isRunning;
    }

    private void buttonRefresh_Click(object sender, EventArgs e)
    {
        RefreshDevices(preserveStatus: true);
        SetStatus("Device list refreshed.");
    }

    private void buttonOpenMixer_Click(object sender, EventArgs e)
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "ms-settings:apps-volume",
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            SetStatus($"Could not open App Volume Mixer: {ex.Message}");
        }
    }

    private void comboSource_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_isRefreshingDevices)
        {
            return;
        }

        PopulateTargetList(GetSelectedTargetIds());
        RebuildVolumePanel();
        UpdateUiState();
    }

    private void checkedTargets_ItemCheck(object sender, ItemCheckEventArgs e)
    {
        BeginInvoke(new Action(() =>
        {
            RebuildVolumePanel();
            UpdateUiState();
        }));
    }

    private void buttonStart_Click(object sender, EventArgs e)
    {
        var source = comboSource.SelectedItem as AudioDeviceInfo;
        var targets = checkedTargets.CheckedItems
            .OfType<AudioDeviceInfo>()
            .Select(device => new TargetDeviceRoute(device.Id, _targetVolumes[device.Id] / 100f))
            .ToArray();

        if (source is null)
        {
            SetStatus("Choose a source device first.");
            return;
        }

        if (!source.IsActive)
        {
            SetStatus("The selected source device is not active in Windows.");
            return;
        }

        if (targets.Length == 0)
        {
            SetStatus("Select at least one target device.");
            return;
        }

        if (checkedTargets.CheckedItems.OfType<AudioDeviceInfo>().Any(device => !device.IsActive))
        {
            SetStatus("One or more selected target devices are not active in Windows.");
            return;
        }

        try
        {
            _router.Start(source.Id, targets, (int)latencyInput.Value);
            SetStatus($"Routing audio from {source.Name} to {targets.Length} extra device(s).");
            UpdateUiState();
        }
        catch (Exception ex)
        {
            SetStatus(ex.Message);
            MessageBox.Show(this, ex.Message, "AudioRoute", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void buttonStop_Click(object sender, EventArgs e)
    {
        _router.Stop();
        SetStatus("Routing stopped.");
        UpdateUiState();
    }

    private void deviceRefreshTimer_Tick(object sender, EventArgs e)
    {
        if (_router.IsRunning || !autoRefreshCheckBox.Checked)
        {
            return;
        }

        if (RefreshDevices(preserveStatus: true))
        {
            SetStatus("Audio device list changed. The app refreshed automatically.");
        }
    }

    private void autoRefreshCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        if (autoRefreshCheckBox.Checked)
        {
            RefreshDevices(preserveStatus: true);
            SetStatus("Automatic device refresh is on.");
        }
        else
        {
            SetStatus("Automatic device refresh is off.");
        }
    }

    private void SetStatus(string message)
    {
        statusLabel.Text = message;
    }
}
