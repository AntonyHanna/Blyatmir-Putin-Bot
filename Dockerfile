FROM mcr.microsoft.com/dotnet/core/runtime:2.1
COPY ./build app/
ENTRYPOINT ["dotnet", "app/Blyatmir Putin Bot.dll"]