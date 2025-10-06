FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Jogos.ApiService/Jogos.ApiService.csproj", "Jogos.ApiService/"]
COPY ["Jogos.Service.Application/Jogos.Service.Application.csproj", "Jogos.Service.Application/"]
COPY ["Jogos.Service.Domain/Jogos.Service.Domain.csproj", "Jogos.Service.Domain/"]
COPY ["Jogos.Service.Infrastructure/Jogos.Service.Infrastructure.csproj", "Jogos.Service.Infrastructure/"]
RUN dotnet restore "./Jogos.ApiService/Jogos.ApiService.csproj"
COPY . .
WORKDIR "/src/Jogos.ApiService"
RUN dotnet build "./Jogos.ApiService.csproj" -c $BUILD_CONFIGURATION -o /app/build

Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Jogos.ApiService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Jogos.ApiService.dll"]