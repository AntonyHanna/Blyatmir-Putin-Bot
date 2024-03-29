# get the sdk to allow us to build
FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine as build-env

# prep the directories we'll be using
RUN mkdir /build-output /source-code

# copy the source code to container
COPY ./ source-code/

# build the source code
RUN dotnet publish "./source-code/blyatmir-putin-bot.sln" -c Release -o /build-output/

# gets the core runtime to allow for running the program
FROM mcr.microsoft.com/dotnet/runtime:5.0-alpine

ENV ISDOCKER = 1

# install the required linux packages
RUN apk update && apk add \
	opus-dev \
	libsodium-dev \
	ffmpeg ; \
	rm -rf /var/lib/apt/lists/*

RUN mkdir /build-output/

COPY --from=build-env /build-output/ /build-output/
	
# start the program
ENTRYPOINT ["dotnet", "/build-output/blyatmir-putin.dll"]
