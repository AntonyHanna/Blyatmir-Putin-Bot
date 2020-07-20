FROM mcr.microsoft.com/dotnet/core/runtime:2.1
FROM mcr.microsoft.com/dotnet/core/sdk:2.1

# prep the directories we'll be using
RUN mkdir /build-output
RUN mkdir /source-code

# copy the source code to container
COPY ./ /source-code/

# build the source code
RUN dotnet publish "./source-code/Blyatmir Putin Bot.sln" -c Release -o /build-output/

# install the required linux packages
RUN apt-get update && apt-get install -y \
	libopus-dev \
	libsodium-dev \
	ffmpeg
	
# start the program
ENTRYPOINT ["dotnet", "/build-otput/Blyatmir Putin Bot.dll"]