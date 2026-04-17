param(
    [string]$AppVersion = "1.0.10",
    [string]$OutputSuffix = ""
)

$ErrorActionPreference = "Stop"

$root = Split-Path -Path $PSScriptRoot -Parent
$publishScript = Join-Path $PSScriptRoot "Publish-SelfContained.ps1"
$issPath = Join-Path $root "installer\AudioRoute.iss"
$publishDirName = if ([string]::IsNullOrWhiteSpace($OutputSuffix)) { "win-x64" } else { "win-x64-$OutputSuffix" }
$publishDir = Join-Path $root "dist\portable\$publishDirName"

& $publishScript -OutputSuffix $OutputSuffix

$isccCandidates = @(
    (Get-Command iscc -ErrorAction SilentlyContinue | Select-Object -ExpandProperty Source),
    "C:\Program Files (x86)\Inno Setup 6\ISCC.exe",
    (Join-Path $env:LOCALAPPDATA "Programs\Inno Setup 6\ISCC.exe")
) | Where-Object { $_ -and (Test-Path $_) }

$iscc = $isccCandidates | Select-Object -First 1

if (-not $iscc) {
    throw "Inno Setup compiler not found. Install it with: winget install JRSoftware.InnoSetup"
}

& $iscc "/DAppVersion=$AppVersion" "/DPublishDir=$publishDir" $issPath

if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}

Write-Host "Installer output: $(Join-Path $root 'dist\installer')"
