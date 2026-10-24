using Jacob_C.Shared.Features.Examples;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Jacob_C.Web.Features.Examples.Update;

public static class UpdateExample
{
    public static RouteGroupBuilder MapUpdateExample(this RouteGroupBuilder examples)
    {
        examples.MapPut("/{id:guid}", Handle);
        return examples;
    }

    private static async Task<IResult> Handle(
        Guid id,
        ExampleRequest request,
        ExampleDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var errors = Validate(request);
        if (errors is not null)
        {
            return TypedResults.ValidationProblem(errors);
        }

        var example = await dbContext.Examples
            .SingleOrDefaultAsync(item => item.Id == id, cancellationToken);

        if (example is null)
        {
            return TypedResults.NotFound();
        }

        example.Name = request.Name!.Trim();
        example.Email = request.Email!.Trim();

        await dbContext.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok(new ExampleResponse(example.Id, example.Name, example.Email));
    }

    private static Dictionary<string, string[]>? Validate(ExampleRequest request)
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

        return errors.Count == 0 ? null : errors;
    }
}
