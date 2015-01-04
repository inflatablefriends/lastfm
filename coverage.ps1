$outputDir = ".\tmp"

if (!(Test-Path $outputDir))
{
    New-Item -ItemType Directory -Path $outputDir
}

# OpenCover
$openCover = ".\packages\OpenCover.4.5.3207\OpenCover.Console.exe"
$testlib = "C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\MSTest.exe"
$testdll = ".\src\IF.Lastfm.Core.Tests\bin\Debug\IF.Lastfm.Core.Tests.dll"
$integrationTestDll = ".\src\IF.Lastfm.Core.Tests.Integration\bin\Debug\IF.Lastfm.Core.Tests.Integration.dll"
$testargs = "/testcontainer:$testdll /testcontainer:$integrationTestDll "
$output = "$outputDir\coverage.xml"

$openCoverCommand = "$openCover -register:user -mergebyhash `"-target:$testlib`" `"-targetargs:$testargs`" -output:$output"

# ReportGenerator
$repgen = ".\packages\ReportGenerator.2.0.1.0\ReportGenerator.exe"
$reportOutputDir = "$outputDir\report"
$repgenCommand = "$repgen -reports:`"$output`" -targetDir:`"$reportOutputDir`""

# --------

echo "Running OpenCover...\n"
iex $openCoverCommand

echo "\nGenerating report... \n"
iex $repgenCommand