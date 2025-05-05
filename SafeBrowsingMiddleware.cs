using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using websiteWatcher.Services;

namespace websiteWatcher
{
    public class SafeBrowsingMiddleware(SafeBrowsingService safeBrowsingService) : IFunctionsWorkerMiddleware
    {
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            var request = await context.GetHttpRequestDataAsync();

            if (!context.BindingContext.BindingData.ContainsKey("Url"))
            {
                var response = request!.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                await response.WriteStringAsync("Debes especificar el Url");
                return;
            }

            var url = context.BindingContext.BindingData["Url"]?.ToString();

            if (!IsValidUrl(url))
            {
                var response = request!.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                await response.WriteStringAsync("El Url no es válido.");
                return;
            }

            var safeCheckResult = safeBrowsingService.Check(url);

            if (safeCheckResult.HasThreat)
            {
                var response = request!.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                await response.WriteStringAsync(string.Join(" ", safeCheckResult.Threats));
                return;
            }
            else
            {
                await next(context);
            }
        }

        private bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
