using Jacob_C.Web.Features.Examples;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using Testcontainers.PostgreSql;

namespace Jacob_C.Testing.Infrastructure;

public sealed class ExampleApiFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:17-alpine").Build();

    private ExampleApiFactory _factory = null!;
    private Respawner _respawner = null!;

    public HttpClient Client { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        _factory = new ExampleApiFactory(_postgres.GetConnectionString());
        Client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });

        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ExampleDbContext>();

        await dbContext.Database.MigrateAsync();
        await dbContext.Database.OpenConnectionAsync();
        _respawner = await Respawner.CreateAsync(dbContext.Database.GetDbConnection(), new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            TablesToIgnore = ["__EFMigrationsHistory"]
        });
        await dbContext.Database.CloseConnectionAsync();
    }

    public async Task ResetDatabaseAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ExampleDbContext>();

        await dbContext.Database.OpenConnectionAsync();
        await _respawner.ResetAsync(dbContext.Database.GetDbConnection());
    }

    public async Task DisposeAsync()
    {
        if (_factory is not null)
        {
            await _factory.DisposeAsync();
        }

        await _postgres.DisposeAsync();
    }
}
