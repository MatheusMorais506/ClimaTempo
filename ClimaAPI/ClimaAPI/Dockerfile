# Estágio 1: Runtime base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Estágio 2: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Forçar o local dos pacotes dentro do container (ignora Windows)
ENV NUGET_PACKAGES=/root/.nuget/fallbackpackages

# Criar um nuget.config limpo dentro do container
RUN echo '<?xml version="1.0" encoding="utf-8"?> \
<configuration> \
  <packageSources> \
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" /> \
  </packageSources> \
</configuration>' > nuget.config

# Copia apenas csproj e restaura pacotes
COPY ["ClimaAPI.csproj", "./"]
RUN dotnet restore "ClimaAPI.csproj" --configfile nuget.config

# Copia todo o código depois do restore
COPY . .

# Publica a aplicação
RUN dotnet publish "ClimaAPI.csproj" -c Release -o /app/publish --no-restore --configfile nuget.config

# Estágio 3: Final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ClimaAPI.dll"]
