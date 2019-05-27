FROM coffee:common-latest AS build
WORKDIR /src
COPY Barista.Common/ Barista.Common/
COPY Barista.Contracts/ Barista.Contracts/
COPY Barista.Offers/ Barista.Offers/
WORKDIR /src/Barista.Offers
RUN dotnet restore Barista.Offers.csproj
RUN dotnet publish Barista.Offers.csproj -c Release -o /app
WORKDIR /app
ENTRYPOINT ["dotnet", "Barista.Offers.dll"]
