# Packs Blazor.UI.Localizer as a NuGet package (+ symbols) into ./artifacts
param(
    [string]$Configuration = "Release",
    [string]$Version
)

$ErrorActionPreference = "Stop"
$project = Join-Path $PSScriptRoot "Blazor.UI.Localizer\Blazor.UI.Localizer.csproj"
$outDir = Join-Path $PSScriptRoot "artifacts"

$packArgs = @(
    "pack", $project,
    "-c", $Configuration,
    "-o", $outDir,
    "--nologo"
)

if ($Version) {
    $packArgs += "-p:PackageVersion=$Version"
    $packArgs += "-p:Version=$Version"
}

Write-Host "dotnet $($packArgs -join ' ')"
& dotnet @packArgs
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Get-ChildItem $outDir -Filter "Blazor.UI.Localizer*.*nupkg" | ForEach-Object {
    Write-Host "Created: $($_.FullName)"
}
