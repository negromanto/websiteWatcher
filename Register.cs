using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Extensions.Logging;

namespace websiteWatcher;

public class Register(ILogger<Register> logger)
{
    [Function(nameof(Register))]
    [SqlOutput("dbo.Websites", "WebsitesWatcher")]
    public async Task<Website> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
    {
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

        var options = new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        };

        Website newWebsite = JsonSerializer.Deserialize<Website>(requestBody, options);
        newWebsite.Id = Guid.NewGuid();

        //logger.LogInformation("C# HTTP trigger function processed a request.");
        return newWebsite;
    }
}

public class Website
{
    public Guid Id { get; set; }
    public string Url { get; set; }
    public string XPathExpression { get; set; }

}
