# Jacob_C

Learning project using Blazor WebAssembly, ASP.NET Core, Entity Framework Core, and PostgreSQL.

## Run

Install Node.js and npm for Tailwind CSS. Start PostgreSQL with `docker compose up -d`, then run the web app with:

```powershell
Push-Location src/Jacob_C.Frontend
npm ci
Pop-Location
dotnet tool restore
$env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet ef database update --project src/Jacob_C.Web/Jacob_C.Web.csproj
dotnet run --project src/Jacob_C.Web/Jacob_C.Web.csproj
```

The Frontend project builds Tailwind CSS as part of its .NET build. Run `npm run css:watch` from `src/Jacob_C.Frontend` while editing styles.

Add future database migrations to the example slice with:

```powershell
dotnet ef migrations add MigrationName --project src/Jacob_C.Web/Jacob_C.Web.csproj --output-dir Features/Examples/Migrations
```

The web app hosts the WebAssembly frontend and keeps database access on the server. The local PostgreSQL credentials are for development only.

The integration test project is ready to use `Testcontainers.PostgreSql` for PostgreSQL-backed tests.

## Example vertical slice

The solution has three projects: `Jacob_C.Web` hosts the API and serves the WebAssembly frontend, `Jacob_C.Frontend` contains the client UI, and `Jacob_C.Shared` contains DTOs referenced by both. The example uses the same child slices in the API and UI:

```text
Web/Features/Examples/
├── Create/CreateExample.cs
├── Read/ReadExamples.cs
├── Update/UpdateExample.cs
├── Delete/DeleteExample.cs
├── ExampleDomain.cs
├── ExampleDbContext.cs
└── Migrations/

Frontend/Features/Examples/
├── Create/CreateExampleForm.razor
├── Read/ReadExamplesList.razor
├── Update/UpdateExampleForm.razor
├── Delete/DeleteExampleButton.razor
└── ExamplesPage.razor

Shared/Features/Examples/
├── ExampleRequest.cs
└── ExampleResponse.cs
```

Each API endpoint and UI component lives in its operation slice. The Web parent keeps shared domain and storage; the Frontend parent composes the page; DTOs stay in Shared.

The API supports `GET /api/examples`, `GET /api/examples/{id}`, `POST /api/examples`, `PUT /api/examples/{id}`, and `DELETE /api/examples/{id}`. `ExampleRequest` carries a name and email. `ExampleResponse` also carries the ID.

Example request body:

```json
{
  "name": "First example",
  "email": "first@example.com"
}
```
