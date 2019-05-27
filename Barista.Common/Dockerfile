FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src

COPY Barista.Contracts/Barista.Contracts.csproj ./Barista.Contracts/
RUN dotnet restore Barista.Contracts/Barista.Contracts.csproj
RUN dotnet publish Barista.Contracts/Barista.Contracts.csproj -c Release -o /app

COPY Barista.Common/Barista.Common.csproj ./Barista.Common/
RUN dotnet restore Barista.Common/Barista.Common.csproj
RUN dotnet publish Barista.Common/Barista.Common.csproj -c Release -o /app
