# get the sdk to allow us to build
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build-env

# prep the directories we'll be using
RUN mkdir /build-output /source-code

# copy the source code to container
COPY ./ /source-code/

# build the source code & install the required linux packages
RUN dotnet publish "./source-code/Blyatmir Putin Bot.sln" -c Release -o /build-output/ ; \
apt-get update && apt-get --no-install-recommends install -y \
	libopus-dev \
	libsodium-dev \
	ffmpeg ; \
	rm -rf /var/lib/apt/lists/*


# gets the core runtime to allow for running the program
FROM mcr.microsoft.com/dotnet/core/runtime:3.1

RUN mkdir /build-output/

COPY --from=build-env /build-output/ /build-output/
	
# start the program
ENTRYPOINT ["dotnet", "/build-output/Blyatmir Putin Bot.dll"]