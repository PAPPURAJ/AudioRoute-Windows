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

## GitHub release automation

This repo now includes two GitHub Actions workflows:

- `.github/workflows/create-release-tag.yml`
  - Runs automatically on every push to `master`.
  - Finds the latest `vX.Y.Z` tag and pushes the next tag automatically.
  - In the same workflow run, it also builds the portable EXE, portable ZIP, and installer EXE.
  - It creates the GitHub Release and uploads those files directly.
  - Default bump is `patch`.
  - If the latest commit message contains `[minor]` or `#minor`, it bumps `minor`.
  - If the latest commit message contains `[major]` or `#major`, it bumps `major`.
  - It can still be run manually from the Actions tab if you want.
- `.github/workflows/build-release.yml`
  - Runs only as a fallback when a tag like `v1.0.11` is pushed manually from outside GitHub Actions.
  - Builds the portable EXE, portable ZIP, and installer EXE.
  - Creates a GitHub Release and uploads all three files as release assets.

## SmartScreen and publisher warning

`Unknown publisher` happens because Windows only trusts signed installers and executables.

This repo now supports optional Authenticode signing during the GitHub release build:

- add `WINDOWS_SIGNING_CERT_BASE64` as a GitHub Actions secret containing your `.pfx` certificate encoded as base64
- add `WINDOWS_SIGNING_CERT_PASSWORD` as the certificate password

If those secrets are present, the release workflow signs:

- `AudioRoute.exe`
- `AudioRoute-Setup-<version>.exe`

Without a real code-signing certificate, SmartScreen warnings cannot be fully solved.

Release flow:

1. Push to `master`.
2. `Create Release Tag` automatically creates the next tag.
3. That same workflow builds the EXE and installer and publishes the GitHub Release.
4. Download the EXE or installer from the GitHub Release page.

Examples:

- normal commit message: patch bump
- commit message with `[minor]`: minor bump
- commit message with `[major]`: major bump
