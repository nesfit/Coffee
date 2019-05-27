ARG sourceimage=nesatfit/coffee
FROM ${sourceimage}:common-latest AS build
WORKDIR /src
COPY Barista.Common/ Barista.Common/
COPY Barista.Contracts/ Barista.Contracts/
COPY Barista.StockItems/ Barista.StockItems/
WORKDIR /src/Barista.StockItems
RUN dotnet restore Barista.StockItems.csproj
RUN dotnet publish Barista.StockItems.csproj -c Release -o /app
WORKDIR /app
ENTRYPOINT ["dotnet", "Barista.StockItems.dll"]
