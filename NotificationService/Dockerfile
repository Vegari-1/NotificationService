FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
ARG github_username
ARG github_pat
WORKDIR /src
COPY NotificationService/NotificationService.csproj NotificationService/
COPY config/NuGet.Config .
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