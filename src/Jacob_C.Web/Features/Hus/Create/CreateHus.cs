using Jacob_C.Shared.Features.Hus.Create;

namespace Jacob_C.Web.Features.Hus.Create;

public static class CreateHus
{
    public static RouteGroupBuilder MapCreateHus(this RouteGroupBuilder Hus)
    {
        Hus.MapPost("/", Handle);
        return Hus;
    }

    internal static async Task<IResult> Handle(
        CreateHusRequest request,
        ExampleDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var errors = ValidateRequest(request);
        if (errors is not null)
        {
            return TypedResults.ValidationProblem(errors);
        }

        var hus = new Hus
        {
            Id = Guid.NewGuid(),
            Name = request.Name!.Trim(),
            Kvm = request.Kvm,
            Damer = request.Damer
        };

        dbContext.Examples.Add(hus);
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = new CreateHusResponse(hus.Id, hus.Name, hus.Kvm, hus.Damer);
        return TypedResults.Created($"/api/examples/{hus.Id}", response);
    }

    public static Dictionary<string, string[]>? ValidateRequest(CreateHusRequest request)
    {
        var errors = new Dictionary<string, string[]>();
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            errors[nameof(request.Name)] = ["Name is required."];
        }

        return errors.Count == 0 ? null : errors;
    }
}
