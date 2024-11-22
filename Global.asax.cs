using DevExpress.Web;
using Sentry;
using Sentry.AspNet;
using System;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;

namespace Dash
{
    public class Global : HttpApplication
    {
        private log4net.ILog logger = log4net.LogManager.GetLogger(typeof(Global));
        private IDisposable _sentry;

        private void Application_Start(object sender, EventArgs e)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            SentrySdk.Init(options =>
            {
                options.Dsn = "https://703f764922d98c2d12ff882016effbfc@o4507304617836544.ingest.de.sentry.io/4508262301761616";
                // When configuring for the first time, to see what the SDK is doing:
                options.Debug = true;

                // Set traces_sample_rate to 1.0 to capture 100% of transactions for performance monitoring.
                // We recommend adjusting this value in production.
                options.TracesSampleRate = 1.0;

                // If you also installed the Sentry.EntityFramework package
                options.AddAspNet();
            });

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

        // Global error catcher
        protected void Application_Error() => Server.CaptureLastError();

        protected void Application_BeginRequest()
        {
            Context.StartSentryTransaction();
        }

        protected void Application_EndRequest()
        {
            Context.FinishSentryTransaction();
        }

        protected void Application_End()
        {
            // Flushes out events before shutting down.
            _sentry?.Dispose();
        }
    }
}