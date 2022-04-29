#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["DocuScraper.Services.RingCentral.csproj", "./"]
RUN dotnet restore "./DocuScraper.Services.RingCentral.csproj"
COPY . .
RUN dotnet build "DocuScraper.Services.RingCentral.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DocuScraper.Services.RingCentral.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DocuScraper.Services.RingCentral.dll"]