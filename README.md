# Jacob_C

## Requirements

- .NET 10 SDK
- Docker
- Node.js and npm

## Setup

```powershell
Push-Location src/Jacob_C.Frontend
npm ci
Pop-Location
docker compose up -d
dotnet tool restore
$env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet ef database update --project src/Jacob_C.Web/Jacob_C.Web.csproj
dotnet run --project src/Jacob_C.Web/Jacob_C.Web.csproj
```

## Commands

- Tests: `dotnet test Jacob_C.slnx` (Docker required for integration tests)
- Tailwind watch: `npm run css:watch` from `src/Jacob_C.Frontend`
