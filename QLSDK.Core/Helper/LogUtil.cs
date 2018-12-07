using log4net;
using log4net.Core;
using log4net.Appender;
using System.Windows.Forms;

namespace QLSDK.Core
{
    public class LogUtil
    {
        static LogUtil()
        {
            RollingFileAppender rfa = new RollingFileAppender();
            rfa.Name = "QLSDKFileAppender";
            rfa.File = System.IO.Path.Combine(Application.StartupPath, "Log");
            rfa.AppendToFile = true;
            rfa.RollingStyle = RollingFileAppender.RollingMode.Date;
            rfa.LockingModel = new FileAppender.MinimalLock();
            rfa.StaticLogFileName = false;
            rfa.Layout = new log4net.Layout.PatternLayout(
                "%date  %-5level  THD:%t    %message%newline");
            rfa.DatePattern = "yyyy-MM-dd\".log\"";
            rfa.ImmediateFlush = true;
            rfa.MaxSizeRollBackups = 100;
            rfa.ActivateOptions();
            rfa.Threshold = Level.Error;
            log4net.Config.BasicConfigurator.Configure(rfa);
        }
        public static ILog GetLoger(string name)
        {
            return LogManager.GetLogger(name);
        }

        internal static void SetLogLevel(string level)
        {
            if(string.IsNullOrWhiteSpace(level))
            {
                return;
            }
            var lvl = Level.Error;
            switch (level.ToLower())
            {
                case "all": lvl = Level.All; break;
                case "debug": lvl = Level.All; break;
                case "info": lvl = Level.All; break;
                case "warn": lvl = Level.All; break;
                case "error": lvl = Level.All; break;
                case "fatal": lvl = Level.All; break;
                case "off": lvl = Level.All; break;
            }
            LogManager.GetLogger("QLSDKFileAppender").Logger.Repository.Threshold = lvl;
        }
    }
}
