using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Jacob_C.Web.Features.Examples.Delete;

public static class DeleteExample
{
    public static RouteGroupBuilder MapDeleteExample(this RouteGroupBuilder examples)
    {
        examples.MapDelete("/{id:guid}", Handle);
        return examples;
    }

    private static async Task<IResult> Handle(
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
}
