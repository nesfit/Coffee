FROM coffee:common-latest AS build
WORKDIR /src
COPY Barista.Common/ Barista.Common/
COPY Barista.Contracts/ Barista.Contracts/
COPY Barista.Swipe/ Barista.Swipe/
WORKDIR /src/Barista.Swipe
RUN dotnet restore Barista.Swipe.csproj
RUN dotnet publish Barista.Swipe.csproj -c Release -o /app
WORKDIR /app
ENTRYPOINT ["dotnet", "Barista.Swipe.dll"]
