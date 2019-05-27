ARG sourceimage=nesatfit/coffee
FROM ${sourceimage}:common-latest AS build
WORKDIR /src
COPY Barista.Common/ Barista.Common/
COPY Barista.Contracts/ Barista.Contracts/
COPY Barista.Products/ Barista.Products/
WORKDIR /src/Barista.Products
RUN dotnet restore Barista.Products.csproj
RUN dotnet publish Barista.Products.csproj -c Release -o /app
WORKDIR /app
ENTRYPOINT ["dotnet", "Barista.Products.dll"]
