param(
    [Parameter(Mandatory = $true)]
    [string]$FilePath
)

$ErrorActionPreference = "Stop"

if (-not (Test-Path $FilePath)) {
    throw "File not found: $FilePath"
}

$signature = Get-AuthenticodeSignature -FilePath $FilePath

if ($signature.Status -ne [System.Management.Automation.SignatureStatus]::Valid) {
    $statusMessage = if ([string]::IsNullOrWhiteSpace($signature.StatusMessage)) { $signature.Status.ToString() } else { $signature.StatusMessage }
    throw "Authenticode signature verification failed for '$FilePath'. Status: $statusMessage"
}

Write-Host "Verified signature: $FilePath"
