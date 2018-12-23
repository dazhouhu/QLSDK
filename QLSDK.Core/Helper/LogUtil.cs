using log4net;
using log4net.Core;
using log4net.Appender;
using System.Windows.Forms;
using log4net.Repository.Hierarchy;

namespace QLSDK.Core
{
    /// <summary>
    /// 日志处理类
    /// </summary>
    public class LogUtil
    {
        private static Level logLevel = Level.Error;
        static LogUtil()
        {
            RollingFileAppender rfa = new RollingFileAppender();
            rfa.Name = "QLSDKFileAppender";
            rfa.File = System.IO.Path.Combine(Application.StartupPath, "Log\\");
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
        /// <summary>
        /// 获取日志
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ILog GetLogger(string name)
        {
            return LogManager.GetLogger(name);
        }
        /// <summary>
        /// 设备日志等级
        /// </summary>
        /// <param name="level">日志等级：all,debug,info,warn,error,fatal,off</param>
        internal static void SetLogLevel(string level)
        {
            if(string.IsNullOrWhiteSpace(level))
            {
                return;
            }
            var logLevel = Level.Error;
            switch (level.ToLower())
            {
                case "all": logLevel = Level.All; break;
                case "debug": logLevel = Level.Debug; break;
                case "info": logLevel = Level.Info; break;
                case "warn": logLevel = Level.Warn; break;
                case "error": logLevel = Level.Error; break;
                case "fatal": logLevel = Level.Fatal; break;
                case "off": logLevel = Level.Off; break;
            }
            var hierarchy = LogManager.GetRepository() as Hierarchy;

            // Get the list of Appenders
            if (hierarchy != null)
            {
                var appenders = hierarchy.GetAppenders();

                if (appenders != null)
                {
                    foreach(var appender in appenders)
                    {
                        if (appender is AppenderSkeleton)
                        {
                            AppenderSkeleton appenderSkeleton = appender as AppenderSkeleton;

                            appenderSkeleton.Threshold = logLevel;
                        }
                    }
                }
            }
        }
    }
}
