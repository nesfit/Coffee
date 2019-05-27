ARG sourceimage=nesatfit/coffee
FROM ${sourceimage}:common-latest AS build
WORKDIR /src
COPY Barista.Common/ Barista.Common/
COPY Barista.Contracts/ Barista.Contracts/
COPY Barista.Accounting/ Barista.Accounting/
WORKDIR /src/Barista.Accounting
RUN dotnet restore Barista.Accounting.csproj
RUN dotnet publish Barista.Accounting.csproj -c Release -o /app
WORKDIR /app
ENTRYPOINT ["dotnet", "Barista.Accounting.dll"]
