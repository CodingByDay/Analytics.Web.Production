using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dash.Log
{
    public static class Logger
    {
        public static void LogError(Type type, string message)
        {
            log4net.ILog logger = log4net.LogManager.GetLogger(type);
            logger.Error(message);
        }
        public static void LogDebug(Type type, string message)
        {
            log4net.ILog logger = log4net.LogManager.GetLogger(type);
            logger.Debug(message);
        }
        public static void LogInfo(Type type, string message)
        {
            log4net.ILog logger = log4net.LogManager.GetLogger(type);
            logger.Info(message);
        }
    }
}