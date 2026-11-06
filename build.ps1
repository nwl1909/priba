# Local build for modern PCs. Requires .NET 8 SDK: https://dotnet.microsoft.com/download/dotnet/8.0
param([string]$Configuration = "Release")
$ErrorActionPreference = "Stop"
$out = Join-Path $PSScriptRoot "dist"
dotnet build "$PSScriptRoot\Interface\CTFAK.Cli\CTFAK.Cli.csproj" -c $Configuration -o $out
dotnet build "$PSScriptRoot\Plugins\CTFAK.Decompiler\CTFAK.Decompiler.csproj" -c $Configuration -o "$out\Plugins"
dotnet build "$PSScriptRoot\Plugins\Dumper\Dumper.csproj" -c $Configuration -o "$out\Plugins"
Get-ChildItem "$out\Plugins" -File | Where-Object { $_.Name -notin 'CTFAK.Decompiler.dll','CTFAK.Decompiler.pdb','Dumper.dll','Dumper.pdb' } | Remove-Item -Force
Remove-Item "$out\Plugins\runtimes" -Recurse -Force -ErrorAction SilentlyContinue
if (-not (Test-Path "$out\x64\CTFAK-Native.dll")) {
  Write-Host "Download ctfakrequirements.zip and extract it into $out" -ForegroundColor Yellow
  Write-Host "https://github.com/CTFAK/.github/raw/main/ctfakrequirements.zip"
}
Write-Host "Done: $out"
