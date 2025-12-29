
# get the sdk to allow us to build
FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine as build-env

# prep the directories we'll be using
RUN mkdir /build-output /source-code

# copy the source code to container
COPY ./ source-code/

# build the source code

RUN dotnet publish "./source-code/blyatmir-putin-bot.sln" \
	-c Release \
	-o /build-output/ \
	--runtime alpine-x64

# gets the core runtime to allow for running the program
FROM mcr.microsoft.com/dotnet/runtime:7.0-alpine

# install the required linux packages
RUN apk update && apk add \
	icu-dev \
	opus-dev \
	libsodium-dev \
	ffmpeg ; \
	rm -rf /var/lib/apt/lists/*

RUN mkdir /build-output/

COPY --from=build-env /build-output/ /build-output/
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# start the program
ENTRYPOINT ["dotnet", "/build-output/ConsoleApp1.dll"]
