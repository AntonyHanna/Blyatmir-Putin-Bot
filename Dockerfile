FROM mcr.microsoft.com/dotnet/core/runtime:2.1
COPY ./src/bin/Release/netcoreapp2.1/publish/ app/
ENTRYPOINT ["dotnet", "app/Blyatmir Putin Bot.dll"]
