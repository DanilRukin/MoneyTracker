dotnet pack MoneyTracker.SharedConstants/MoneyTracker.SharedConstants.csproj --configuration Release --output ./artifacts

dotnet nuget push artifacts\*.nupkg --source "https://nuget.pkg.github.com/DanilRukin/index.json" --api-key <your api key>

pause
