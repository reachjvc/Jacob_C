# Jacob_C

Learning project using Blazor WebAssembly, ASP.NET Core, Entity Framework Core, and PostgreSQL.

## Run

Start PostgreSQL with `docker compose up -d`, then run the web app with:

```powershell
dotnet tool restore
$env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet ef database update --project src/Jacob_C.Web/Jacob_C.Web/Jacob_C.Web.csproj
dotnet run --project src/Jacob_C.Web/Jacob_C.Web/Jacob_C.Web.csproj
```

The web app hosts the WebAssembly client and keeps database access on the server. The local PostgreSQL credentials are for development only.

The integration test project is ready to use `Testcontainers.PostgreSql` for PostgreSQL-backed tests.

## Example API slice

`POST /api/examples` accepts `ExampleRequest` and returns `ExampleResponse`. `GET /api/examples/{id}` reads it back. The route mapping and handlers live in `ExampleEndpoints`.

Example request body:

```json
{
  "name": "First example"
}
```
