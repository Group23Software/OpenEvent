using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using OpenEvent.Web;

namespace OpenEvent.Integration.Tests
{
    public static class ClientFactory
    {
        public static HttpClient Create()
        {
            var factory = new ApplicationFactory<Startup>();
            //
            // var projectDir = Directory.GetCurrentDirectory();
            // var configPath = Path.Combine(projectDir, "appsettings.test.json");
            //
            // factory = factory.WithWebHostBuilder(builder =>
            // {
            //     builder.ConfigureAppConfiguration((context, configurationBuilder) =>
            //     {
            //         configurationBuilder.AddJsonFile(configPath);
            //     });
            // });
            
            return factory.CreateClient();
        }
    }
}