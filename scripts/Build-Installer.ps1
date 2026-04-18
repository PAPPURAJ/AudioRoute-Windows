param(
    [string]$AppVersion = "1.0.10",
    [string]$OutputSuffix = "",
    [string]$Version = "",
    [string]$PublisherName = "Pappuraj Bhottacharjee",
    [string]$PublisherUrl = "https://pappuraj.com",
    [string]$SigningCertPath = "",
    [string]$SigningCertPassword = ""
)

$ErrorActionPreference = "Stop"

$root = Split-Path -Path $PSScriptRoot -Parent
$publishScript = Join-Path $PSScriptRoot "Publish-SelfContained.ps1"
$signScript = Join-Path $PSScriptRoot "Sign-File.ps1"
$verifySignatureScript = Join-Path $PSScriptRoot "Verify-Signature.ps1"
$issPath = Join-Path $root "installer\AudioRoute.iss"
$publishDirName = if ([string]::IsNullOrWhiteSpace($OutputSuffix)) { "win-x64" } else { "win-x64-$OutputSuffix" }
$publishDir = Join-Path $root "dist\portable\$publishDirName"
$portableExe = Join-Path $publishDir "AudioRoute.exe"
$shouldVerifySigning = -not [string]::IsNullOrWhiteSpace($SigningCertPath) -and (Test-Path $SigningCertPath)

& $publishScript -OutputSuffix $OutputSuffix -Version $Version

if ($LASTEXITCODE -ne 0 -or -not (Test-Path $publishDir)) {
    throw "Portable publish failed. Installer build cannot continue."
}

& $signScript -FilePath $portableExe -CertPath $SigningCertPath -CertPassword $SigningCertPassword

if ($shouldVerifySigning) {
    & $verifySignatureScript -FilePath $portableExe
}

$isccCandidates = @(
    (Get-Command iscc -ErrorAction SilentlyContinue | Select-Object -ExpandProperty Source),
    "C:\Program Files (x86)\Inno Setup 6\ISCC.exe",
    (Join-Path $env:LOCALAPPDATA "Programs\Inno Setup 6\ISCC.exe")
) | Where-Object { $_ -and (Test-Path $_) }

$iscc = $isccCandidates | Select-Object -First 1

if (-not $iscc) {
    throw "Inno Setup compiler not found. Install it with: winget install JRSoftware.InnoSetup"
}

& $iscc "/DAppVersion=$AppVersion" "/DPublishDir=$publishDir" "/DAppPublisherName=$PublisherName" "/DAppPublisherUrl=$PublisherUrl" $issPath

if ($LASTEXITCODE -ne 0) {
    throw "Inno Setup compile failed with exit code $LASTEXITCODE."
}

$installerExe = Get-ChildItem (Join-Path $root "dist\installer\AudioRoute-Setup-$AppVersion*.exe") |
    Sort-Object LastWriteTime -Descending |
    Select-Object -First 1

if ($null -eq $installerExe) {
    throw "Installer output file was not found."
}

& $signScript -FilePath $installerExe.FullName -CertPath $SigningCertPath -CertPassword $SigningCertPassword

if ($shouldVerifySigning) {
    & $verifySignatureScript -FilePath $installerExe.FullName
}

Write-Host "Installer output: $(Join-Path $root 'dist\installer')"
