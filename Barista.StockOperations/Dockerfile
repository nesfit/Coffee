FROM coffee:common-latest AS build
WORKDIR /src
COPY Barista.Common/ Barista.Common/
COPY Barista.Contracts/ Barista.Contracts/
COPY Barista.StockOperations/ Barista.StockOperations/
WORKDIR /src/Barista.StockOperations
RUN dotnet restore Barista.StockOperations.csproj
RUN dotnet publish Barista.StockOperations.csproj -c Release -o /app
WORKDIR /app
ENTRYPOINT ["dotnet", "Barista.StockOperations.dll"]
