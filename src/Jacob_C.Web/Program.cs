using Jacob_C.Web.Components;
using Jacob_C.Shared.Features.Hus.Create;
using Jacob_C.Web.Features.Hus;
using Jacob_C.Web.Features.Hus.Create;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddDbContext<ExampleDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// TEMP: sanity check CreateHus by creating 10 houses on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ExampleDbContext>();
    for (var i = 1; i <= 10; i++)
    {
        var request = new CreateHusRequest($"Hus {i}", 50 + i * 10, i % 3 == 0 ? null : i);
        var result = await CreateHus.Handle(request, dbContext, CancellationToken.None);
        Console.WriteLine(result is Created<CreateHusResponse> created
            ? $"Created: {created.Value}"
            : $"Failed: {result}");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapExampleEndpoints();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Jacob_C.Frontend._Imports).Assembly);

app.Run();

namespace Jacob_C.Web
{
    public partial class Program { }
}
