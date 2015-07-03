using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using NLog;

namespace WebApiLogging.Controllers
{
    public class ValuesController : ApiController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public async Task<HttpResponseMessage> Get()
        {
            Log.Debug("Hi there");
            return Request.CreateResponse(HttpStatusCode.OK, new[] { "one", "two" });
        }
    }

    public class LoggerDelegatingHandler : DelegatingHandler
    {
        private static readonly Logger Log = LogManager.GetLogger("RequestTracer");

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var stopwatch = new Stopwatch();

            Guid guid = Guid.NewGuid();
            NLog.Contrib.MappedDiagnosticsLogicalContext.Set("requestid", guid.ToString());
            stopwatch.Start();
            Log.Trace("Requesting:[{0}]{1}", request.Method.Method, request.RequestUri);

            var reponse = await base.SendAsync(request, cancellationToken);

            stopwatch.Stop();
            Log.Trace("Request completed. Duration:{0} StatusCode:{1}", (object) stopwatch.Elapsed, (object) reponse.StatusCode);
            return reponse;
        }
    }
}
