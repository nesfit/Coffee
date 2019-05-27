ARG sourceimage=nesatfit/coffee
FROM ${sourceimage}:common-latest AS build
WORKDIR /src
COPY Barista.Common/ Barista.Common/
COPY Barista.Contracts/ Barista.Contracts/
COPY Barista.Users/ Barista.Users/
WORKDIR /src/Barista.Users
RUN dotnet restore Barista.Users.csproj
RUN dotnet publish Barista.Users.csproj -c Release -o /app
WORKDIR /app
ENTRYPOINT ["dotnet", "Barista.Users.dll"]
