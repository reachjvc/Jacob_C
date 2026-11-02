using Jacob_C.Web.Features.Hus.Create;

namespace Jacob_C.Web.Features.Hus;

public static class ExampleEndpoints
{
    public static IEndpointRouteBuilder MapExampleEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var examples = endpoints.MapGroup("/api/hus").WithTags("Hus");

        examples.MapCreateHus();

        return endpoints;
    }
}
