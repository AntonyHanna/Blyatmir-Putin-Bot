FROM mcr.microsoft.com/dotnet/core/runtime:2.1
COPY ./build app/
RUN apt-get update && apt-get install -y \
	libopus-dev \
	libsodium-dev \
	ffmpeg
	
ENTRYPOINT ["dotnet", "app/Blyatmir Putin Bot.dll"]