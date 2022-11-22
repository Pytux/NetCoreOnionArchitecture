FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Infraestructure/Persistence/Persistence.csproj", "Persistence/"]
RUN dotnet restore "Infraestructure/Persistence/Persistence.csproj"
COPY . .
WORKDIR "/src/Persistence"
RUN dotnet build "Persistence.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Persistence.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Persistence.dll"]
