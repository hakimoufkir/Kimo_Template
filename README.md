# ZLApp

ZLApp is a small ASP.NET Core and Vue application used to manage and display weather forecasts and locations.

It contains:

- a versioned ASP.NET Core API
- a Vue 3 frontend
- SQL Server persistence through EF Core
- OIDC authentication against the local or hosted IdentityServer

## Solution Structure

- `source/Kimo.ZLApp.Web`
  Main web host. Serves the API and the Vue frontend.
- `source/Kimo.ZLApp.Application`
  Application layer with commands, queries, models, and request pipelines.
- `source/Kimo.ZLApp.Infrastructure`
  EF Core, database access, and external integrations.
- `tests`
  Test projects.
- `docs`
  Project documentation.

## Main Features

- list, create, and delete weather forecasts
- list locations
- permission-based authorization
- automatic database migrations on startup
- environment split for local and production IdentityServer integration

## Local Development

Requirements:

- .NET SDK
- Node.js and npm
- SQL Server / SQL Server Express

### Backend

Run the web app:

```powershell
cd C:\Users\AbdelhakimOufkir\RiderProjects\ZLApp
dotnet run --project source\Kimo.ZLApp.Web\Kimo.ZLApp.Web.csproj
```

Important notes:

- the backend applies EF Core migrations on startup
- local development uses the `Development` environment configuration
- local development is configured to use the local IdentityServer at `https://localhost:7443`

### Frontend

Run the Vue app:

```powershell
cd C:\Users\AbdelhakimOufkir\RiderProjects\ZLApp\source\Kimo.ZLApp.Web\ClientApp
npm install
npm run dev
```

Default local frontend URL:

- `https://localhost:5002`

Default local backend URL:

- `https://localhost:5001`

## Authentication

ZLApp uses OIDC with two environment targets:

- Development
  - backend authority: `https://localhost:7443`
  - frontend authority: `https://localhost:7443`
- Production
  - backend authority: `https://kimo-id-server.azurewebsites.net`
  - frontend authority: `https://kimo-id-server.azurewebsites.net`

The frontend is now English-only.

## API Overview

Current controllers:

- `ForecastController`
- `LocationController`

Controller reference:

- [docs/controllers.md](./docs/controllers.md)

Main endpoints:

- `GET /api/v1/forecasts`
- `GET /api/v1/forecasts/{id}`
- `POST /api/v1/forecasts`
- `DELETE /api/v1/forecasts?id=1&id=2`
- `GET /api/v1/locations`

## Database

ZLApp uses SQL Server.

Check the active connection strings in:

- `source/Kimo.ZLApp.Web/appsettings.json`
- `source/Kimo.ZLApp.Web/appsettings.Development.json`
- `source/Kimo.ZLApp.Web/appsettings.Production.json`

## Notes

- authorization is permission-based
- frontend route guards and API calls both depend on the token permissions
- `403` responses are redirected to `/unauthorized` in the frontend
- claims are read from the OIDC user profile and, when needed, from the access token payload
