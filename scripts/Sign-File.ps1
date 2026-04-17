param(
    [Parameter(Mandatory = $true)]
    [string]$FilePath,
    [string]$CertPath = "",
    [string]$CertPassword = "",
    [string]$TimestampUrl = "http://timestamp.digicert.com",
    [string]$Description = "AudioRoute",
    [string]$DescriptionUrl = "https://github.com/PAPPURAJ/AudioRoute-Windows"
)

$ErrorActionPreference = "Stop"

if (-not (Test-Path $FilePath)) {
    throw "File not found: $FilePath"
}

if ([string]::IsNullOrWhiteSpace($CertPath) -or -not (Test-Path $CertPath)) {
    Write-Host "Code-sign certificate not configured. Skipping signing for $FilePath"
    exit 0
}

$signtoolCandidates = @(
    (Get-Command signtool -ErrorAction SilentlyContinue | Select-Object -ExpandProperty Source),
    "C:\Program Files (x86)\Windows Kits\10\App Certification Kit\signtool.exe",
    "C:\Program Files (x86)\Windows Kits\10\bin\x64\signtool.exe"
) | Where-Object { $_ -and (Test-Path $_) }

$signtool = $signtoolCandidates | Select-Object -First 1

if (-not $signtool) {
    throw "signtool.exe not found."
}

$arguments = @(
    "sign",
    "/fd", "SHA256",
    "/td", "SHA256",
    "/tr", $TimestampUrl,
    "/f", $CertPath,
    "/d", $Description,
    "/du", $DescriptionUrl
)

if (-not [string]::IsNullOrWhiteSpace($CertPassword)) {
    $arguments += @("/p", $CertPassword)
}

$arguments += $FilePath

& $signtool @arguments

if ($LASTEXITCODE -ne 0) {
    throw "signtool failed with exit code $LASTEXITCODE for $FilePath"
}

Write-Host "Signed: $FilePath"
