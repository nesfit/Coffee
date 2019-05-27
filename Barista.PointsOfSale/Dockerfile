ARG sourceimage=nesatfit/coffee
FROM ${sourceimage}:common-latest AS build
WORKDIR /src
COPY Barista.Common/ Barista.Common/
COPY Barista.Contracts/ Barista.Contracts/
COPY Barista.PointsOfSale/ Barista.PointsOfSale/
WORKDIR /src/Barista.PointsOfSale
RUN dotnet restore Barista.PointsOfSale.csproj
RUN dotnet publish Barista.PointsOfSale.csproj -c Release -o /app
WORKDIR /app
ENTRYPOINT ["dotnet", "Barista.PointsOfSale.dll"]
