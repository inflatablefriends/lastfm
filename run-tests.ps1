$ErrorActionPreference = 'Continue'

$testPaths = @(
  "src\IF.Lastfm.Core.Tests\",
  "src\IF.Lastfm.Core.Tests.Integration\",
  "src\IF.Lastfm.SQLite.Tests.Integration\"
)

foreach ($path in $testPaths) {
  dotnet test $path
}