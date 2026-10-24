using Jacob_C.Shared.Features.Examples;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Jacob_C.Web.Features.Examples;

public static class ExampleEndpoints
{
    public static IEndpointRouteBuilder MapExampleEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var examples = endpoints.MapGroup("/api/examples");
        examples.MapGet("/", GetAll);
        examples.MapPost("/", Create);
        examples.MapPut("/{id:guid}", Update);
        examples.MapDelete("/{id:guid}", Delete);
        examples.MapGet("/{id:guid}", GetById);

        return endpoints;
    }

    private static async Task<IResult> GetAll(
        ExampleDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var examples = await dbContext.Examples
            .AsNoTracking()
            .OrderBy(example => example.Name)
            .Select(example => new ExampleResponse(example.Id, example.Name, example.Email))
            .ToListAsync(cancellationToken);

        return TypedResults.Ok(examples);
    }

    private static async Task<IResult> Create(
        ExampleRequest request,
        ExampleDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var validation = Validate(request);
        if (validation is not null)
        {
            return validation;
        }

        var example = new ExampleDomain { Id = Guid.NewGuid() };
        Apply(request, example);

        dbContext.Examples.Add(example);
        await dbContext.SaveChangesAsync(cancellationToken);

        return TypedResults.Created($"/api/examples/{example.Id}", ToResponse(example));
    }

    private static async Task<IResult> GetById(
        Guid id,
        ExampleDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var example = await dbContext.Examples
            .AsNoTracking()
            .SingleOrDefaultAsync(item => item.Id == id, cancellationToken);

        return example is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(ToResponse(example));
    }

    private static async Task<IResult> Update(
        Guid id,
        ExampleRequest request,
        ExampleDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var validation = Validate(request);
        if (validation is not null)
        {
            return validation;
        }

        var example = await dbContext.Examples
            .SingleOrDefaultAsync(item => item.Id == id, cancellationToken);

        if (example is null)
        {
            return TypedResults.NotFound();
        }

        Apply(request, example);
        await dbContext.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok(ToResponse(example));
    }

    private static async Task<IResult> Delete(
        Guid id,
        ExampleDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var example = await dbContext.Examples
            .SingleOrDefaultAsync(item => item.Id == id, cancellationToken);

        if (example is null)
        {
            return TypedResults.NotFound();
        }

        dbContext.Examples.Remove(example);
        await dbContext.SaveChangesAsync(cancellationToken);

        return TypedResults.NoContent();
    }

    private static IResult? Validate(ExampleRequest request)
    {
        var errors = new Dictionary<string, string[]>();
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            errors[nameof(request.Name)] = ["Name is required."];
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            errors[nameof(request.Email)] = ["Email is required."];
        }

        return errors.Count == 0 ? null : TypedResults.ValidationProblem(errors);
    }

    private static void Apply(ExampleRequest request, ExampleDomain example)
    {
        example.Name = request.Name!.Trim();
        example.Email = request.Email!.Trim();
    }

    private static ExampleResponse ToResponse(ExampleDomain example) =>
        new(example.Id, example.Name, example.Email);
}
