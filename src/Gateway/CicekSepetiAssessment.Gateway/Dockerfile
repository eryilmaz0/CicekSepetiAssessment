FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Gateway/CicekSepetiAssessment.Gateway/CicekSepetiAssessment.Gateway.csproj", "src/Gateway/CicekSepetiAssessment.Gateway/"] 
RUN dotnet restore "src/Gateway/CicekSepetiAssessment.Gateway/CicekSepetiAssessment.Gateway.csproj"
COPY . .
WORKDIR "/src/src/Gateway/CicekSepetiAssessment.Gateway/"
RUN dotnet build "CicekSepetiAssessment.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CicekSepetiAssessment.Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CicekSepetiAssessment.Gateway.dll"]
