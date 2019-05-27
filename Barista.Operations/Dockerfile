ARG sourceimage=nesatfit/coffee
FROM ${sourceimage}:common-latest AS build
WORKDIR /src
COPY Barista.Common/ Barista.Common/
COPY Barista.Contracts/ Barista.Contracts/
COPY Barista.Operations/ Barista.Operations/
WORKDIR /src/Barista.Operations
RUN dotnet restore Barista.Operations.csproj
RUN dotnet publish Barista.Operations.csproj -c Release -o /app
WORKDIR /app
ENTRYPOINT ["dotnet", "Barista.Operations.dll"]
