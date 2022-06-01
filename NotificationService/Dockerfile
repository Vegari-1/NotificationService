FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app
EXPOSE 5005

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src
COPY NotificationService/NotificationService.csproj NotificationService/
RUN dotnet restore NotificationService/NotificationService.csproj
COPY . .
WORKDIR /src/NotificationService
RUN dotnet build NotificationService.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish NotificationService.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "NotificationService.dll"]