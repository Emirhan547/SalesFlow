using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.IntegrationTests.Infrastructure
{
    public abstract class IntegrationTestBase
    : IClassFixture<CustomWebApplicationFactory>
    {
        protected readonly HttpClient Client;

        protected IntegrationTestBase(
            CustomWebApplicationFactory factory)
        {
            Client = factory.CreateClient();
        }
    }
}