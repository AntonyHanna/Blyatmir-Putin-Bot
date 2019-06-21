FROM mcr.microsoft.com/dotnet/core/runtime:2.1
COPY ./src/bin/Releases/netcoreapp2.1/publish /app
ENTRYPOINT ["dotnet", "Blyatmir Putin Bot.dll"]
