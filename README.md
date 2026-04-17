# AudioRoute

AudioRoute is a standalone Windows desktop app that fans one playback device out to multiple speakers or headphones.

It does not require Voicemeeter, VB-Cable, or any other extra software.

## What it does

- Reads the mixed audio already playing on one selected Windows playback device
- Replays that same audio to any number of extra speakers or headphones
- Lets you set separate volume for each mirrored target
- Refreshes the Windows device list automatically

## Important limit

This standalone design copies audio after Windows already sent it to the source device.

That means:

- the source device always starts first
- mirrored devices can be behind the source
- Bluetooth headphones can have extra delay because of their own codec and buffering

So this version matches your original install requirement, but it cannot guarantee perfect sync across multiple Bluetooth headphones.

## How to use

1. Connect your speaker, wired headphones, and Bluetooth headphones.
2. In Windows, make one device the device your movie is already using.
3. Open `AudioRoute.exe`.
4. Pick that same device as `Source`.
5. Check the extra output devices you want to mirror to.
6. Press `Start Routing`.
7. If Bluetooth is unstable, raise the latency buffer.

## Run

```powershell
cd "D:\My Projects\Windows\AudioRoute\AudioRoute"
dotnet run
```

## Portable build

```powershell
cd "D:\My Projects\Windows\AudioRoute"
powershell -ExecutionPolicy Bypass -File .\scripts\Publish-SelfContained.ps1 -OutputSuffix r8
```

Output:

- `dist\portable\win-x64-r8\AudioRoute.exe`
- `dist\AudioRoute-win-x64-r8-portable.zip`

## Installer

```powershell
cd "D:\My Projects\Windows\AudioRoute"
powershell -ExecutionPolicy Bypass -File .\scripts\Build-Installer.ps1 -AppVersion 1.0.10 -OutputSuffix r8
```

Output:

- `dist\installer\AudioRoute-Setup-1.0.10.exe`
