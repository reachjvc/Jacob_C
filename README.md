# Jacob_C

Learning project using Blazor WebAssembly, ASP.NET Core, Entity Framework Core, and PostgreSQL.

## Run

Start PostgreSQL with `docker compose up -d`, then run the web app with:

```powershell
dotnet tool restore
$env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet ef database update --project src/Jacob_C.Web/Jacob_C.Web.csproj
dotnet run --project src/Jacob_C.Web/Jacob_C.Web.csproj
```

Add future database migrations to the example slice with:

```powershell
dotnet ef migrations add MigrationName --project src/Jacob_C.Web/Jacob_C.Web.csproj --output-dir Features/Examples/Migrations
```

The web app hosts the WebAssembly frontend and keeps database access on the server. The local PostgreSQL credentials are for development only.

The integration test project is ready to use `Testcontainers.PostgreSql` for PostgreSQL-backed tests.

## Example vertical slice

The solution has three projects: `Jacob_C.Web` hosts the API and serves the WebAssembly frontend, `Jacob_C.Frontend` contains the client UI, and `Jacob_C.Shared` contains DTOs referenced by both. Each project keeps the example code under `Features/Examples`:

- `Web/Features/Examples`: domain, API endpoints, EF Core context, and migrations.
- `Frontend/Features/Examples`: the browser page that creates and reads examples through the API.
- `Shared/Features/Examples`: `ExampleRequest` and `ExampleResponse` DTOs.

`POST /api/examples` accepts `ExampleRequest` and returns `ExampleResponse`. `GET /api/examples/{id}` reads it back.

Example request body:

```json
{
  "name": "First example"
}
```
