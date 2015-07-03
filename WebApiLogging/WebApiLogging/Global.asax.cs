using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using NLog.Contrib.LayoutRenderers;
using WebApiLogging.Controllers;

namespace WebApiLogging
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            NLog.Config.ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition("mdlc", typeof(MdlcLayoutRenderer));

            GlobalConfiguration.Configure(configuration =>
            {
                configuration.MessageHandlers.Add(new LoggerDelegatingHandler());

                WebApiConfig.Register(configuration);
            });
        }
    }
}
