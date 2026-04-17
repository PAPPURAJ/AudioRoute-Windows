param(
    [string]$Configuration = "Release",
    [string]$Runtime = "win-x64",
    [string]$OutputSuffix = "",
    [string]$Version = ""
)

$ErrorActionPreference = "Stop"

$root = Split-Path -Path $PSScriptRoot -Parent
$project = Join-Path $root "AudioRoute\AudioRoute.csproj"
$portableName = if ([string]::IsNullOrWhiteSpace($OutputSuffix)) { $Runtime } else { "$Runtime-$OutputSuffix" }
$publishDir = Join-Path $root "dist\portable\$portableName"
$zipPath = Join-Path $root "dist\AudioRoute-$portableName-portable.zip"
$versionOverrideArgs = @()

if (-not [string]::IsNullOrWhiteSpace($Version)) {
    $versionParts = $Version.Split('.')

    if ($versionParts.Length -eq 3) {
        $assemblyVersion = "$Version.0"
    }
    elseif ($versionParts.Length -eq 4) {
        $assemblyVersion = $Version
    }
    else {
        throw "Version '$Version' must have 3 or 4 numeric parts."
    }

    $versionOverrideArgs = @(
        "-p:Version=$Version",
        "-p:AssemblyVersion=$assemblyVersion",
        "-p:FileVersion=$assemblyVersion",
        "-p:InformationalVersion=$Version"
    )
}

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
    @versionOverrideArgs `
    -o $publishDir

if ($LASTEXITCODE -ne 0) {
    throw "dotnet publish failed with exit code $LASTEXITCODE."
}

if (Test-Path $zipPath) {
    Remove-Item $zipPath -Force
}

Compress-Archive -Path (Join-Path $publishDir "*") -DestinationPath $zipPath

Write-Host "Portable output: $publishDir"
Write-Host "Portable zip:    $zipPath"
