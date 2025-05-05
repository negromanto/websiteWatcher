using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Extensions.Logging;

namespace websiteWatcher.Functions;

public class Query(ILogger<Query> logger)
{
    private const string InputQuery = @"Select w.Id, w.Url, s.Timestamp AS LastTimestamp
                                FROM dbo.Websites w
                                LEFT JOIN dbo.Snapshots s ON w.Id = s.Id
                                WHERE s.Timestamp = (select MAX(Timestamp) from dbo.Snapshots WHERE Id = w.Id)
                                AND s.Timestamp BETWEEN DATEADD(hour, -3, GETUTCDATE())
                                AND GETUTCDATE()";


    [Function(nameof(Query))]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req,
        [SqlInput(InputQuery, "WebsitesWatcher")] IReadOnlyList<dynamic> websites)
    {
        logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult(websites);
    }
}
