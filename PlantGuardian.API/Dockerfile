FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["PlantGuardian.API/PlantGuardian.API.csproj", "PlantGuardian.API/"]
RUN dotnet restore "PlantGuardian.API/PlantGuardian.API.csproj"
COPY PlantGuardian.API/ PlantGuardian.API/
WORKDIR "/src/PlantGuardian.API"
RUN dotnet publish "PlantGuardian.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PlantGuardian.API.dll"]
