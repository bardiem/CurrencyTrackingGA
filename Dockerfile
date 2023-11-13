FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CurrencyTrackingGA/CurrencyTrackingGA.csproj", "CurrencyTrackingGA/"]
RUN dotnet restore "CurrencyTrackingGA/CurrencyTrackingGA.csproj"
COPY . .
WORKDIR "/src/CurrencyTrackingGA"
RUN dotnet build "CurrencyTrackingGA.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CurrencyTrackingGA.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CurrencyTrackingGA.dll"]