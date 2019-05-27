FROM coffee:common-latest AS build
WORKDIR /src
COPY Barista.Common/ Barista.Common/
COPY Barista.Contracts/ Barista.Contracts/
COPY Barista.Identity/ Barista.Identity/
WORKDIR /src/Barista.Identity
RUN dotnet restore Barista.Identity.csproj
RUN dotnet publish Barista.Identity.csproj -c Release -o /app
WORKDIR /app
ENTRYPOINT ["dotnet", "Barista.Identity.dll"]
