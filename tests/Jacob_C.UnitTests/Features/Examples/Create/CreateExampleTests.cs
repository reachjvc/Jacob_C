using Jacob_C.Shared.Features.Examples;
using Jacob_C.Web.Features.Examples.Create;

namespace Jacob_C.UnitTests.Features.Examples.Create;

public sealed class CreateExampleTests
{
    [Fact]
    public void ValidateRequest_ReturnsErrors_WhenNameAndEmailAreMissing()
    {
        var errors = CreateExample.ValidateRequest(new ExampleRequest(" ", null));

        Assert.NotNull(errors);
        Assert.Contains(nameof(ExampleRequest.Name), errors.Keys);
        Assert.Contains(nameof(ExampleRequest.Email), errors.Keys);
    }
}
