using Jacob_C.Web.Features.Hus.Create;

namespace Jacob_C.Web.Features.Hus;

public static class HusEndpoints
{
    public static IEndpointRouteBuilder MapHusEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var hus = endpoints.MapGroup("/api/hus").WithTags("hus");

        hus.MapCreateHus();

        return endpoints;
    }
}
