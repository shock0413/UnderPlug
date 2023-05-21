using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hansero
{

    /// <summary>
    /// Log를 제어하는 매니저 Class
    /// </summary>
    public class LogManager
    {
        /// <summary>
        /// 로그 타입
        /// </summary>
        public enum LogType { Trace, Action, Info, Fatal, Debug, Warn, Error }

        /// <summary>
        /// Log 등록시의 이벤트 핸들러
        /// </summary>
        public delegate void OnLogEventHandler(LogType logType, string msg);
        /// <summary>
        /// Log 등록시의 이벤트
        /// </summary>
        public event OnLogEventHandler OnLogEvent = delegate { };

        private static ILog log = log4net.LogManager.GetLogger("Program");

        //Custom Log Level
        private static readonly Level traceLevel = new Level(10, "Trace");
        private static readonly Level actionLevel = new Level(11, "Action");

        static void Main(string[] args)
        {
            Console.ReadLine();
        }

        /// <summary>
        /// 로그를 기입하기 위한 매니저
        /// </summary>
        /// <param name="isShowClassName">로그가 발생한 Class명 표시 여부</param>
        /// <param name="isShowMethod">로그가 발생한 Method명 표시 여부</param>
        public LogManager(bool isShowClassName, bool isShowMethod)
        {
            var repository = log4net.LogManager.GetRepository();
            if(repository.Configured)
            {
                return;
            }
            repository.Configured = true;

            repository.LevelMap.Add(traceLevel);
            repository.LevelMap.Add(actionLevel);

            string consolePattern = "%d{yyyy-MM-dd HH:mm:ss} [%thread]";
            
            if(isShowClassName)
            {
                consolePattern += " [%C]";
            }
            if(isShowMethod)
            {
                consolePattern += " [%M]";
            }
                
            consolePattern += " %level - %message%newline";

            // 컬러 콘솔 로그 패턴 설정
            var appender = new ColoredConsoleAppender
            {
                Threshold = Level.All,
                Layout = new PatternLayout(consolePattern),
            };
            appender.AddMapping(new ColoredConsoleAppender.LevelColors
            {
                Level = Level.Debug,
                ForeColor = ColoredConsoleAppender.Colors.White
                    | ColoredConsoleAppender.Colors.HighIntensity
            });
            appender.AddMapping(new ColoredConsoleAppender.LevelColors
            {
                Level = Level.Info,
                ForeColor = ColoredConsoleAppender.Colors.Green
                    | ColoredConsoleAppender.Colors.HighIntensity
            });
            appender.AddMapping(new ColoredConsoleAppender.LevelColors
            {
                Level = Level.Warn,
                ForeColor = ColoredConsoleAppender.Colors.Purple
                    | ColoredConsoleAppender.Colors.HighIntensity
            });
            appender.AddMapping(new ColoredConsoleAppender.LevelColors
            {
                Level = Level.Error,
                ForeColor = ColoredConsoleAppender.Colors.Red
                    | ColoredConsoleAppender.Colors.HighIntensity
            });
            appender.AddMapping(new ColoredConsoleAppender.LevelColors
            {
                Level = Level.Fatal,
                ForeColor = ColoredConsoleAppender.Colors.White
                    | ColoredConsoleAppender.Colors.HighIntensity,
                BackColor = ColoredConsoleAppender.Colors.Red
            });
            appender.AddMapping(new ColoredConsoleAppender.LevelColors
            {
                Level = actionLevel,
                ForeColor = ColoredConsoleAppender.Colors.Yellow
                    | ColoredConsoleAppender.Colors.HighIntensity
            });
            appender.ActivateOptions();
            BasicConfigurator.Configure(appender);

            // 파일 로그 패턴 설정
            var rollingAppender = new RollingFileAppender();
            rollingAppender.Name = "RollingFile";

            // 시스템이 기동되면 파일을 추가해서 할 것인가? 새로 작성할 것인가?
            rollingAppender.AppendToFile = true;

            rollingAppender.DatePattern = "yyyy년MM월dd일.lo\\g";
            // 로그 파일 설정
            rollingAppender.File = @"Log\";
            rollingAppender.StaticLogFileName = false;
            
            // 파일 단위 날짜 또는 파일 사이즈로 설정.
            rollingAppender.RollingStyle = RollingFileAppender.RollingMode.Date;

            string rollingPattern = "%d [%t] ";
            if (isShowClassName)
            {
                rollingPattern += "[%C]";
            }
            if (isShowMethod)
            {
                rollingPattern += "[%M]";
            }
            rollingPattern += "%-5p %c - %m%n";

            rollingAppender.Layout = new PatternLayout(rollingPattern);
            var hierarchy = (Hierarchy)repository;
            hierarchy.Root.AddAppender(rollingAppender);
            rollingAppender.ActivateOptions();
             
            hierarchy.Root.Level = log4net.Core.Level.All;

        }

        /// <summary>
        /// 시스템의 동작을 추적하기 위한 Log를 남길 시 사용.
        /// </summary>
        /// <param name="msg">표시할 메시지</param>
        public void Trace(string msg)
        {
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, traceLevel, msg, null);
            OnLogEvent(LogType.Trace, msg);
        }

        /// <summary>
        /// 사용자 또는 외부의 제어 및 동작에 대한 Log를 남길 시 사용.
        /// </summary>
        /// <param name="msg">표시할 메시지</param>
        public void Action(string msg)
        {
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, actionLevel, msg, null);
            OnLogEvent(LogType.Action, msg);
        }

        /// <summary>
        /// 정보를 Log에 남길 시 사용.
        /// </summary>
        /// <param name="msg">표시할 메시지</param>
        public void Info(string msg)
        {
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, Level.Info, msg, null);
            OnLogEvent(LogType.Info, msg);
        }

        /// <summary>
        /// 치명적인 에러 발생시 남길 Log에 사용.
        /// </summary>
        /// <param name="msg">표시할 메시지</param>
        public void Fatal (string msg)
        {
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, Level.Fatal, msg, null);
            OnLogEvent(LogType.Fatal, msg);
        }

        /// <summary>
        /// 디버깅시 Log 출력시 사용.
        /// </summary>
        /// <param name="msg">표시할 메시지</param>
        public void Debug(string msg)
        {
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, Level.Debug, msg, null);
            OnLogEvent(LogType.Debug, msg);
        }

        /// <summary>
        /// 경고 Log 출력시 사용.
        /// </summary>
        /// <param name="msg">표시할 메시지</param>
        public void Warn(string msg)
        {
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, Level.Warn, msg, null);
            OnLogEvent(LogType.Warn, msg);
        }

        /// <summary>
        /// 에러 메시지 출력시 사용.
        /// </summary>
        /// <param name="msg">표시할 메시지</param>
        public void Error(string msg)
        {
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, Level.Error, msg, null);
            OnLogEvent(LogType.Error, msg);
        }
    }
}
