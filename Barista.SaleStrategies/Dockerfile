FROM coffee:common-latest AS build
WORKDIR /src
COPY Barista.Common/ Barista.Common/
COPY Barista.Contracts/ Barista.Contracts/
COPY Barista.SaleStrategies/ Barista.SaleStrategies/
WORKDIR /src/Barista.SaleStrategies
RUN dotnet restore Barista.SaleStrategies.csproj
RUN dotnet publish Barista.SaleStrategies.csproj -c Release -o /app
WORKDIR /app
ENTRYPOINT ["dotnet", "Barista.SaleStrategies.dll"]
