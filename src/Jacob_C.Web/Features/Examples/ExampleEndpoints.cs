using Jacob_C.Web.Features.Examples.Create;
using Jacob_C.Web.Features.Examples.Delete;
using Jacob_C.Web.Features.Examples.Read;
using Jacob_C.Web.Features.Examples.Update;
using Microsoft.AspNetCore.Routing;

namespace Jacob_C.Web.Features.Examples;

public static class ExampleEndpoints
{
    public static IEndpointRouteBuilder MapExampleEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var examples = endpoints.MapGroup("/api/examples");

        examples.MapCreateExample();
        examples.MapReadExamples();
        examples.MapUpdateExample();
        examples.MapDeleteExample();

        return endpoints;
    }
}
