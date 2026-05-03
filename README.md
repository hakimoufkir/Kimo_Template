# Kimo Clean Architecture Template

A `dotnet new` template for scaffolding a .NET 10 Clean Architecture solution with ASP.NET Core Web API and Vue 3 SPA.

## Stack

- **Backend:** ASP.NET Core Web API (.NET 10), EF Core, SQL Server, Hangfire, Serilog, OIDC
- **Frontend:** Vue 3, TypeScript, Vite, PrimeVue, Tailwind CSS
- **Architecture:** Clean Architecture (Domain → Application → Infrastructure → Web)

## Solution Structure

```
source/
  {Company}.{Project}.Web                 Main web host — API + Vue SPA
  {Company}.{Project}.Application         Commands, queries, models, request pipelines
  {Company}.{Project}.Domain              Domain entities and aggregates
  {Company}.{Project}.Infrastructure      EF Core, repositories, external integrations
  {Company}.{Project}.Infrastructure.Jobs Hangfire job definitions
  {Company}.{Project}.WorkerService       Windows Service host for Hangfire
tests/
  *.UnitTests                             xUnit test projects per layer
```

## Using the Template

### Install

```bash
dotnet new install ./template-pack/KimoTemplate.1.0.0.nupkg
```

### Scaffold a new solution

```bash
dotnet new kimo -n "Acme.MyProject"
```

`Kimo.ZLApp` is replaced with `Acme.MyProject` everywhere — file names, directories, namespaces, and project references. Private `Kimo.FX.*` package references are left untouched.

### Uninstall

```bash
dotnet new uninstall KimoTemplate
```

## Prerequisites

- .NET 10 SDK
- Node.js and npm
- SQL Server / SQL Server LocalDB
- `C:\local-nuget` registered as a NuGet source (contains private `Kimo.FX.*` packages)

The generated project includes a `NuGet.config` that pre-registers `C:\local-nuget` automatically.

## Local Development

### Backend

```bash
dotnet run --project source\{Company}.{Project}.Web
```

- Applies EF Core migrations on startup
- Runs on `https://localhost:5001`
- Uses `Development` environment by default (local IdentityServer at `https://localhost:7443`)

### Frontend

```bash
cd source\{Company}.{Project}.Web\ClientApp
npm install
npm run dev
```

Runs on `https://localhost:5002`.

## Authentication

OIDC-based authentication with environment-specific authorities:

| Environment | Authority |
|---|---|
| Development | `https://localhost:7443` |
| Production | `https://kimo-id-server.azurewebsites.net` |

## NuGet Package

The template is packaged in `template-pack/KimoTemplate.csproj` and produces `KimoTemplate.1.0.0.nupkg`.

To rebuild the package after changes:

```bash
cd template-pack
dotnet pack KimoTemplate.csproj -o .
```
