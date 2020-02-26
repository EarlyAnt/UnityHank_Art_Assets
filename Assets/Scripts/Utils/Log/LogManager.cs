using System.Collections.Generic;
using System;
using System.Diagnostics;
using UnityEngine;
using System.IO;
using Gululu.Util;
using System.Text;


namespace Gululu.Log
{
    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error,
        Fatal
    }

    public struct Recoder
    {
        public string time;
        public string tag;
        public string msg;
        public string loglevel;
    }


    public class LogManager
    {
        private static LogManager uniqueInstance;

        private HashSet<IGuLogHandler> LoggerSet;

        private LogManager(){}

        public static LogManager GetInstance()
        {
            if (uniqueInstance == null)
            {
                uniqueInstance = new LogManager();
                uniqueInstance.LoggerSet = new HashSet<IGuLogHandler>();
                
                // uniqueInstance.LoggerSet.Add(new FileHandler());

                uniqueInstance.LoggerSet.Add(new SystermHandler());

                uniqueInstance.LoggerSet.Add(new NativeLogHandler());
                
            }
            return uniqueInstance;
        }

        public void Logger(string tag, string content, LogLevel level)
        {
            if(string.IsNullOrEmpty(tag)){
                tag = "";
            }

            Recoder recoder = new Recoder
            {
                time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                tag = tag,
                msg = content,
                loglevel = getLogLevelString(level),
            };

            foreach (IGuLogHandler logger in LoggerSet)
            {
                logger.Handler(recoder);
            } 
        }

        public static string getLogLevelString(LogLevel loglevel)
        {
            if (loglevel == LogLevel.Debug)
            {
                return "[DEBUG]";
            }
            else if (loglevel == LogLevel.Info)
            {
                return "[INFO]";
            }
            else if (loglevel == LogLevel.Warning)
            {
                return "[WARNING]";
            }
            else if (loglevel == LogLevel.Error)
            {
                return "[ERROR]";
            }
            else if (loglevel == LogLevel.Fatal)
            {
                return "[FATAL]";
            }
            else
            {
                return "[DEBUG]";
            }
        }  
    } 
}
