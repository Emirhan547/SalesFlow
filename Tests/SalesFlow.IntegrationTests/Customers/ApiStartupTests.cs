using FluentAssertions;
using SalesFlow.IntegrationTests.Infrastructure;
using Xunit;

namespace SalesFlow.IntegrationTests.Customers;

public class ApiStartupTests
    : IntegrationTestBase
{
    public ApiStartupTests(
        CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Api_Should_Start()
    {
        var response = await Client.GetAsync("/");

        response.Should().NotBeNull();
    }
}