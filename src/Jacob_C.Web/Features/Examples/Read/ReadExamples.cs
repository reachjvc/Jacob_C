using Jacob_C.Shared.Features.Examples;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Jacob_C.Web.Features.Examples.Read;

public static class ReadExamples
{
    public static RouteGroupBuilder MapReadExamples(this RouteGroupBuilder examples)
    {
        examples.MapGet("/", GetAll);
        examples.MapGet("/{id:guid}", GetById);
        return examples;
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
            : TypedResults.Ok(new ExampleResponse(example.Id, example.Name, example.Email));
    }
}
