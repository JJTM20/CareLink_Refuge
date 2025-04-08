FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY CareLink_Refugee.csproj .

RUN dotnet restore CareLink_Refugee.csproj

COPY . .

RUN dotnet build CareLink_Refugee.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish CareLink_Refugee.csproj -c Release -o /app/publish

FROM base AS final
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "CareLink_Refugee.dll"]
