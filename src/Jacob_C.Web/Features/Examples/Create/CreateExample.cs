using Jacob_C.Shared.Features.Examples;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Jacob_C.Web.Features.Examples.Create;

public static class CreateExample
{
    public static RouteGroupBuilder MapCreateExample(this RouteGroupBuilder examples)
    {
        examples.MapPost("/", Handle);
        return examples;
    }

    private static async Task<IResult> Handle(
        ExampleRequest request,
        ExampleDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var errors = ValidateRequest(request);
        if (errors is not null)
        {
            return TypedResults.ValidationProblem(errors);
        }

        var example = new ExampleDomain
        {
            Id = Guid.NewGuid(),
            Name = request.Name!.Trim(),
            Email = request.Email!.Trim()
        };

        dbContext.Examples.Add(example);
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = new ExampleResponse(example.Id, example.Name, example.Email);
        return TypedResults.Created($"/api/examples/{example.Id}", response);
    }

    public static Dictionary<string, string[]>? ValidateRequest(ExampleRequest request)
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
