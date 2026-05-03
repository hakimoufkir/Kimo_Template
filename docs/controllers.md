# Controllers

This document describes the HTTP controllers exposed by ZLApp.

## Web API

Controller source root:

- `source/Kimo.ZLApp.Web`

ZLApp currently exposes two API controllers:

- `ForecastController`
- `LocationController`

Both controllers use the shared request pipeline through `IRequestExecutor` and expose versioned endpoints under `/api/v1/...`.

## ForecastController

File:

- `source/Kimo.ZLApp.Web/Forecasts/ForecastController.cs`

Purpose:

- manage weather forecast data
- list forecasts with filtering, sorting, and pagination
- retrieve one forecast by id
- create forecasts
- delete forecasts

Base route:

- `api/v{version:apiVersion}/forecasts`
- current version in use: `v1`

Authorization:

- `GET` endpoints require `AppPermissions.WeatherForecasts.Read`
- `POST` requires `AppPermissions.WeatherForecasts.Create`
- `DELETE` requires `AppPermissions.WeatherForecasts.Delete`

Endpoints:

- `GET /api/v1/forecasts`
  - returns paginated forecast data
  - accepts query parameters for filtering, sorting, and pagination
- `GET /api/v1/forecasts/{id}`
  - returns a single forecast by numeric id
- `POST /api/v1/forecasts`
  - creates a new forecast from the request body
- `DELETE /api/v1/forecasts?id=1&id=2`
  - deletes one or more forecasts by query-string ids

Notes:

- The controller delegates all work to commands and queries.
- Filtering and sorting are built from `QueryParameters`.
- The endpoint contracts are designed for the Vue frontend data table.

## LocationController

File:

- `source/Kimo.ZLApp.Web/Locations/LocationController.cs`

Purpose:

- return the list of available locations

Base route:

- `api/v{version:apiVersion}/locations`
- current version in use: `v1`

Authorization:

- controller has `[Authorize]`
- `GET` also requires `AppPermissions.Locations.Read`

Endpoints:

- `GET /api/v1/locations`
  - returns all locations as a list

Notes:

- This is the endpoint used by the frontend `/locations` page.
- If the token is missing `locations.read`, the frontend should route to `/unauthorized` before the request is made.

## Summary

ZLApp controller design is intentionally thin:

- controllers translate HTTP to commands and queries
- authorization is expressed at endpoint level with permission constants
- business logic lives in the application layer, not in the controller classes
