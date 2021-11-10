





























# Test Coverage
dotnet test --collect:"Xplat Code Coverage"
reportgenerator -reports:"./**/coverage.cobertura.xml" -targetdir:"./TestCoverageReport" -reporttypes:"html"
