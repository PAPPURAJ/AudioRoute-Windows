param(
    [string]$Configuration = "Release",
    [string]$Runtime = "win-x64",
    [string]$OutputSuffix = ""
)

$ErrorActionPreference = "Stop"

$root = Split-Path -Path $PSScriptRoot -Parent
$project = Join-Path $root "AudioRoute\AudioRoute.csproj"
$portableName = if ([string]::IsNullOrWhiteSpace($OutputSuffix)) { $Runtime } else { "$Runtime-$OutputSuffix" }
$publishDir = Join-Path $root "dist\portable\$portableName"
$zipPath = Join-Path $root "dist\AudioRoute-$portableName-portable.zip"

if (Test-Path $publishDir) {
    Remove-Item $publishDir -Recurse -Force
}

dotnet publish $project `
    -c $Configuration `
    -r $Runtime `
    --self-contained true `
    -p:PublishSingleFile=true `
    -p:IncludeNativeLibrariesForSelfExtract=true `
    -p:EnableCompressionInSingleFile=true `
    -p:DebugType=None `
    -p:DebugSymbols=false `
    -o $publishDir

if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}

if (Test-Path $zipPath) {
    Remove-Item $zipPath -Force
}

Compress-Archive -Path (Join-Path $publishDir "*") -DestinationPath $zipPath

Write-Host "Portable output: $publishDir"
Write-Host "Portable zip:    $zipPath"
