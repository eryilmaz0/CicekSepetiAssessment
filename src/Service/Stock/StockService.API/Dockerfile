﻿FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Service/Stock/StockService.API/StockService.API.csproj", "src/Service/Stock/StockService.API/"]
RUN dotnet restore "src/Service/Stock/StockService.API/StockService.API.csproj"
COPY . .
WORKDIR "/src/src/Service/Stock/StockService.API"
RUN dotnet build "StockService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StockService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StockService.API.dll"]
