param(
    [string] $versionSuffix
)

$root = (split-path -parent $MyInvocation.MyCommand.Definition) + '\..'

function CompileNuspec([string]$dllPath, [string]$nuspecname)
{
	$version = [System.Reflection.Assembly]::LoadFile($dllPath).GetName().Version

	if ($versionSuffix)
	{
	    $versionStr = "{0}.{1}.{2}-{3}" -f ($version.Major, $version.Minor, $version.Build, $versionSuffix)
	}
	else
	{
	    $versionStr = $version.ToString()
	}

	Write-Host "Setting $nuspecname .nuspec version tag to $versionStr"

	$content = (Get-Content $root\.nuget\$nuspecname.nuspec) 
	$content = $content -replace '\$version',$versionStr

	$content | Out-File $root\.nuget\$nuspecname.$versionStr.compiled.nuspec

	& $root\.nuget\NuGet.exe pack $root\.nuget\$nuspecname.$versionStr.compiled.nuspec
}

CompileNuspec "$root\src\IF.Lastfm.Core\bin\Release\IF.Lastfm.Core.dll" "Inflatable.Lastfm"
CompileNuspec "$root\src\IF.Lastfm.SQLite\bin\Release\IF.Lastfm.SQLite.dll" "Inflatable.Lastfm.SQLite"

# publish to appveyor feeds //TODO fix this
# appveyor PushArtifact Inflatable.Lastfm.$versionStr.nupkg