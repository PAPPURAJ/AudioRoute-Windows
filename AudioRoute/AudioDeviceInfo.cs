namespace AudioRoute;

using NAudio.CoreAudioApi;

public sealed record AudioDeviceInfo(string Id, string Name, DeviceState State, bool IsDefault)
{
    public bool IsActive => State.HasFlag(DeviceState.Active);

    public string StateLabel =>
        State switch
        {
            DeviceState.Active => "Active",
            DeviceState.Disabled => "Disabled",
            DeviceState.NotPresent => "Not Present",
            DeviceState.Unplugged => "Unplugged",
            _ => State.ToString()
        };

    public override string ToString()
    {
        if (IsDefault)
        {
            return IsActive ? $"{Name} (Default)" : $"{Name} (Default, {StateLabel})";
        }

        return IsActive ? Name : $"{Name} ({StateLabel})";
    }
}
