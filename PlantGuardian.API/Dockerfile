FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PlantGuardian.API.csproj", "./"]
RUN dotnet restore "./PlantGuardian.API.csproj"
COPY . .
RUN dotnet publish "PlantGuardian.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PlantGuardian.API.dll"]
