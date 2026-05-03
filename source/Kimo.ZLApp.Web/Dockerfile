FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Kimo.ZLApp.Web/Kimo.ZLApp.Web.csproj", "Kimo.ZLApp.Web/"]
RUN dotnet restore "Kimo.ZLApp.Web/Kimo.ZLApp.Web.csproj"
COPY . .
WORKDIR "/src/WebApplication1"
RUN dotnet build "Kimo.ZLApp.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Kimo.ZLApp.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kimo.ZLApp.Web.dll"]
