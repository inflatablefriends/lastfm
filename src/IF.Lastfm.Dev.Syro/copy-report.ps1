Param(
	[Parameter(Mandatory=$true)][string]$outputDir,
	[Parameter(Mandatory=$true)][string]$targetDir
)

Copy-Item "$($outputDir)PROGRESS.md" "$($targetDir)PROGRESS.md"
