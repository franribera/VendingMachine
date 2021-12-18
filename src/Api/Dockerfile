FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /sln
COPY ["VendingMachine.sln", "VendingMachine.sln"]
COPY ["docker-compose.dcproj", "docker-compose.dcproj"]
COPY ["src/Api/Api.csproj", "src/Api/"]
COPY ["test/Api.UnitTests/Api.UnitTests.csproj", "test/Api.UnitTests/"]

RUN dotnet restore "/sln/VendingMachine.sln"
COPY . .

RUN dotnet publish "/sln/src/Api/Api.csproj" -c Release --no-restore -o /app/publish

FROM build AS testing

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]