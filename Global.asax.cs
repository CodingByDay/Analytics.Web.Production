using DevExpress.Web;
using System;
using System.IO;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;

namespace Dash
{
    public class Global : HttpApplication
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(Global));
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
          //  DevExpress.Web.ASPxWebControl.CallbackError += Application_Error;
            log4net.Config.XmlConfigurator.Configure();
            ASPxWebControl.CallbackError += ASPxWebControl_CallbackError;
 

        }
     
        private void ASPxWebControl_CallbackError(object sender, EventArgs e)
        {
            Exception exception = HttpContext.Current.Server.GetLastError();
            logger.Error(exception.InnerException);
         
        }

    
    }
}