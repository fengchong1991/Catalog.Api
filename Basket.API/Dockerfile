FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY . .
RUN dotnet publish Basket.API/Basket.API.csproj -c Release -o /app

FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Basket.API.dll"]