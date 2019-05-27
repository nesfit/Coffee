ARG sourceimage=nesatfit/coffee
FROM ${sourceimage}:common-latest AS build
WORKDIR /src
COPY Barista.Common/ Barista.Common/
COPY Barista.Contracts/ Barista.Contracts/
COPY Barista.Mqtt/ Barista.Mqtt/
WORKDIR /src/Barista.Mqtt
RUN dotnet restore Barista.Mqtt.csproj
RUN dotnet publish Barista.Mqtt.csproj -c Release -o /app
WORKDIR /app
ENTRYPOINT ["dotnet", "Barista.Mqtt.dll"]
