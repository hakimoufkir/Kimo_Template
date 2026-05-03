FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["source/Kimo.ZLApp.WorkerService/Kimo.ZLApp.WorkerService.csproj", "source/Kimo.ZLApp.WorkerService/"]
RUN dotnet restore "source/Kimo.ZLApp.WorkerService/Kimo.ZLApp.WorkerService.csproj"
COPY . .
WORKDIR "/src/source/Kimo.ZLApp.WorkerService"
RUN dotnet build "Kimo.ZLApp.WorkerService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Kimo.ZLApp.WorkerService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kimo.ZLApp.WorkerService.dll"]
