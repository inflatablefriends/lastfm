param(
    [string] $versionSuffix
)

function CompileNuspec([string]$dllPath, [string]$nuspecname)
{
	Write-Host "Setting $nuspecname .nuspec version tag to $versionStr" -Foreground green

	$content = (Get-Content $root\.nuget\$nuspecname.nuspec) 
	$content = $content -replace '\$version',$versionStr

	$content | Out-File "$root\.nuget\$nuspecname.$versionStr.compiled.nuspec"

	& $root\.nuget\NuGet.exe pack "$root\.nuget\$nuspecname.$versionStr.compiled.nuspec"
}

. ".\Package-Versions.ps1"

$root = (split-path -parent $MyInvocation.MyCommand.Definition) + '\..'

$dllPath = "$root\src\IF.Lastfm.Core\bin\Release\IF.Lastfm.Core.dll"
$version = [System.Reflection.AssemblyName]::GetAssemblyName($dllPath).Version

if ($versionSuffix) {
	$versionStr = "{0}.{1}.{2}-{3}" -f ($version.Major, $version.Minor, $version.Build, $versionSuffix)
}
else {
	$versionStr = $version.ToString()
}

CompileNuspec $dllPath "Inflatable.Lastfm"

if ([string]::IsNullOrEmpty($sqliteVersion)){
	Write-Host "Couldn't read version to use for SQLite package" -Foreground red
}
else {
	if ($versionStr.startswith($sqliteVersion)) {
		CompileNuspec "$root\src\IF.Lastfm.SQLite\bin\Release\IF.Lastfm.SQLite.dll" "Inflatable.Lastfm.SQLite"
	}
	else {
		Write-Host "Skipping SQLite package, build version is not $sqliteVersion" -Foreground yellow
	}
}

# publish to appveyor feeds //TODO fix this
# appveyor PushArtifact Inflatable.Lastfm.$versionStr.nupkg
