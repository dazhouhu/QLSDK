using log4net;
using log4net.Core;
using log4net.Appender;
using System.Windows.Forms;

namespace QLSDK.Core
{
    /// <summary>
    /// 日志处理类
    /// </summary>
    public class LogUtil
    {
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
            rfa.Threshold = Level.Debug;
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
            LogManager.GetLogger("QLSDKFileAppender").Logger.Repository.Threshold = logLevel;
        }
    }
}
