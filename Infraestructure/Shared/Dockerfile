FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Infraestructure/Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "Infraestructure/Shared/Shared.csproj"
COPY . .
WORKDIR "/src/Shared"
RUN dotnet build "Shared.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Shared.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Shared.dll"]
