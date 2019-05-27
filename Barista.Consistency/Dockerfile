ARG sourceimage=nesatfit/coffee
FROM ${sourceimage}:common-latest AS build
WORKDIR /src
COPY Barista.Common/ Barista.Common/
COPY Barista.Contracts/ Barista.Contracts/
COPY Barista.Consistency/ Barista.Consistency/
WORKDIR /src/Barista.Consistency
RUN dotnet restore Barista.Consistency.csproj
RUN dotnet publish Barista.Consistency.csproj -c Release -o /app
WORKDIR /app
ENTRYPOINT ["dotnet", "Barista.Consistency.dll"]
