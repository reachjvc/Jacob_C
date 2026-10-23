using Jacob_C.Web.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Jacob_C.Web.Features.Examples;

public static class ExampleEndpoints
{
    public static IEndpointRouteBuilder MapExampleEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var examples = endpoints.MapGroup("/api/examples");
        examples.MapPost("/", Create);
        examples.MapGet("/{id:guid}", GetById);

        return endpoints;
    }

    private static async Task<IResult> Create(
        ExampleRequest request,
        AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var example = ExampleDomain.Create(request.Name);
        if (example is null)
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                [nameof(request.Name)] = ["Name is required and must be 100 characters or fewer."]
            });
        }

        dbContext.Examples.Add(example);
        await dbContext.SaveChangesAsync(cancellationToken);

        return TypedResults.Created($"/api/examples/{example.Id}", ToResponse(example));
    }

    private static async Task<IResult> GetById(
        Guid id,
        AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var example = await dbContext.Examples
            .AsNoTracking()
            .SingleOrDefaultAsync(item => item.Id == id, cancellationToken);

        return example is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(ToResponse(example));
    }

    private static ExampleResponse ToResponse(ExampleDomain example) => new(example.Id, example.Name);
}
