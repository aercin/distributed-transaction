#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/services/order/api/api.csproj", "src/services/order/api/"]
COPY ["src/services/order/application/application.csproj", "src/services/order/application/"]
COPY ["src/services/order/domain/domain.csproj", "src/services/order/domain/"]
COPY ["src/core/core-domain/core-domain.csproj", "src/core/core-domain/"]
COPY ["src/services/order/infrastructure/infrastructure.csproj", "src/services/order/infrastructure/"]
COPY ["src/core/core-infrastructure/core-infrastructure.csproj", "src/core/core-infrastructure/"]
COPY ["src/core/core-application/core-application.csproj", "src/core/core-application/"]
RUN dotnet restore "src/services/order/api/api.csproj"
COPY . .
WORKDIR "/src/src/services/order/api"
RUN dotnet build "api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "api.dll"]
