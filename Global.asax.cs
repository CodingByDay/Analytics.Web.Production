using DevExpress.Web;
using System;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;

namespace Dash
{
    public class Global : HttpApplication
    {
        private log4net.ILog logger = log4net.LogManager.GetLogger(typeof(Global));

        private void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            // DevExpress.Web.ASPxWebControl.CallbackError += Application_Error;
            log4net.Config.XmlConfigurator.Configure();
            ASPxWebControl.CallbackError += ASPxWebControl_CallbackError;
        }

        private void ASPxWebControl_CallbackError(object sender, EventArgs e)
        {
            Exception exception = HttpContext.Current.Server.GetLastError();

            if (exception.Message.Contains("ProdajaKomercialist"))
            {
                return;
            }

            logger.Error(exception.InnerException);
        }
    }
}