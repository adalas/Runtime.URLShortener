using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace Runtime.URLShortener.FunctionalTests.Web.Controllers{
    public class URLControllerTest:IClassFixture<WebTestFixture>
    {
        public HttpClient Client {get;}
        public URLControllerTest(WebTestFixture factory)
        {
            Client = factory.CreateClient(
                new WebApplicationFactoryClientOptions{
                    AllowAutoRedirect = false
                    }
                );
        }

        [Fact]
        public async Task URLGetError()
        {
            var response = await Client.GetAsync("/URL/1232");
            Assert.False(response.IsSuccessStatusCode);
        }
    }
}