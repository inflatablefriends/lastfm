param(
    [string] $versionSuffix
)

$root = (split-path -parent $MyInvocation.MyCommand.Definition) + '\..'
$version = [System.Reflection.Assembly]::LoadFile("$root\src\IF.Lastfm.Core\bin\Release\IF.Lastfm.Core.dll").GetName().Version

if ($versionSuffix)
{
    $versionStr = "{0}.{1}.{2}-{3}" -f ($version.Major, $version.Minor, $version.Build, $versionSuffix)
}
else
{
    $versionStr = $version.ToString()
}

Write-Host "Setting .nuspec version tag to $versionStr"

$content = (Get-Content $root\.nuget\Inflatable.Lastfm.nuspec) 
$content = $content -replace '\$version',$versionStr

$content | Out-File $root\.nuget\Inflatable.Lastfm.$versionStr.compiled.nuspec

& $root\.nuget\NuGet.exe pack $root\.nuget\Inflatable.Lastfm.$versionStr.compiled.nuspec

# publish to appveyor feeds //TODO fix this
# appveyor PushArtifact Inflatable.Lastfm.$versionStr.nupkg