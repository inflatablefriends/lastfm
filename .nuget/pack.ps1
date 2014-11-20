$root = (split-path -parent $MyInvocation.MyCommand.Definition) + '\..'
$version = [System.Reflection.Assembly]::LoadFile("$root\src\IF.Lastfm.Core\bin\Release\IF.Lastfm.Core.dll").GetName().Version
$versionStr = $version.ToString()

Write-Host "Setting .nuspec version tag to $versionStr"

$content = (Get-Content $root\.nuget\IF.Lastfm.nuspec) 
$content = $content -replace '\$version',$versionStr

$content | Out-File $root\.nuget\IF.Lastfm.compiled.nuspec

& $root\.nuget\NuGet.exe pack $root\.nuget\IF.Lastfm.compiled.nuspec