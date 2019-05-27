FROM coffee:common-latest AS build
WORKDIR /src
COPY Barista.Common/ Barista.Common/
COPY Barista.Contracts/ Barista.Contracts/
COPY Barista.Api/ Barista.Api/
WORKDIR /src/Barista.Api
RUN dotnet restore Barista.Api.csproj
RUN dotnet publish Barista.Api.csproj -c Release -o /app
WORKDIR /app
ENTRYPOINT ["dotnet", "Barista.Api.dll"]
