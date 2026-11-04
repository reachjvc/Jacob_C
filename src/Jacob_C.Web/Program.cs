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
builder.Services.AddDbContext<HusDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// ============================== SHOWCASE ==============================
// A place to try our logic directly, without the browser or the API.
// It runs every time the app starts, and prints to the terminal.
// It must stay ABOVE app.Run(), because nothing below app.Run() ever runs.
// Sanity check "er jacob en Tate fanatiker?" = ja
using (var scope = app.Services.CreateScope())
{
    // Get the database helper, so we can hand it to our logic.
    var dbContext = scope.ServiceProvider.GetRequiredService<HusDbContext>();

    Console.WriteLine();
    Console.WriteLine("============================ SHOWCASE: CREATE ============================");
    Console.WriteLine($"Houses in the database before: {await dbContext.Huse.CountAsync()}");

    // --- 1. A valid house: we expect it to be created ---
    var request = new CreateHusRequest("Showcase-hus", 123.5, 2);

    Console.WriteLine();
    Console.WriteLine("1) Sending a VALID house to CreateHus.Handle:");
    Console.WriteLine($"     Name  = {request.Name}");
    Console.WriteLine($"     Kvm   = {request.Kvm}");
    Console.WriteLine($"     Damer = {request.Damer?.ToString() ?? "null (empty)"}");

    var result = await CreateHus.Handle(request, dbContext, CancellationToken.None);

    if (result is Created<CreateHusResponse> created)
    {
        var hus = created.Value!;
        Console.WriteLine("   Answer: Created (success)");
        Console.WriteLine($"     Id       = {hus.Id}");
        Console.WriteLine($"     Name     = {hus.Name}");
        Console.WriteLine($"     Kvm      = {hus.Kvm}");
        Console.WriteLine($"     Damer    = {hus.Damer?.ToString() ?? "null (empty)"}");
        Console.WriteLine($"     Location = {created.Location}");
    }
    else
    {
        Console.WriteLine($"   Answer: NOT created. Got: {result}");
    }

    // --- 2. An invalid house (no name): we expect it to be rejected ---
    var badRequest = new CreateHusRequest("", 50, null);

    Console.WriteLine();
    Console.WriteLine("2) Sending an INVALID house (empty name) to CreateHus.Handle:");
    Console.WriteLine($"     Name  = \"{badRequest.Name}\"");

    var badResult = await CreateHus.Handle(badRequest, dbContext, CancellationToken.None);

    if (badResult is ValidationProblem problem)
    {
        Console.WriteLine("   Answer: Rejected (validation problem)");
        foreach (var (field, messages) in problem.ProblemDetails.Errors)
        {
            Console.WriteLine($"     {field}: {string.Join(", ", messages)}");
        }
    }
    else
    {
        Console.WriteLine($"   Answer: NOT rejected?! Got: {badResult}");
    }

    Console.WriteLine();
    Console.WriteLine($"Houses in the database after: {await dbContext.Huse.CountAsync()}");
    Console.WriteLine("==========================================================================");
    Console.WriteLine();

    // ------------------------- READ (next task) -------------------------
    // var readResult = await ReadHus.Handle(created.Value!.Id, dbContext, CancellationToken.None);
    // Print what came back, and check it matches what we created above.

    // ------------------------ UPDATE (later task) -----------------------
    // var updateRequest = new UpdateHusRequest("Showcase-hus (updated)", 200, 3);
    // var updateResult = await UpdateHus.Handle(created.Value!.Id, updateRequest, dbContext, CancellationToken.None);
    // Print the result, then read the house again to prove it changed.
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

app.MapHusEndpoints();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Jacob_C.Frontend._Imports).Assembly);

app.Run();

namespace Jacob_C.Web
{
    public partial class Program { }
}
