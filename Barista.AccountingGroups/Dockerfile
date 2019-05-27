ARG sourceimage=nesatfit/coffee
FROM ${sourceimage}:common-latest AS build
WORKDIR /src
COPY Barista.Common/ Barista.Common/
COPY Barista.Contracts/ Barista.Contracts/
COPY Barista.AccountingGroups/ Barista.AccountingGroups/
WORKDIR /src/Barista.AccountingGroups
RUN dotnet restore Barista.AccountingGroups.csproj
RUN dotnet publish Barista.AccountingGroups.csproj -c Release -o /app
WORKDIR /app
ENTRYPOINT ["dotnet", "Barista.AccountingGroups.dll"]
