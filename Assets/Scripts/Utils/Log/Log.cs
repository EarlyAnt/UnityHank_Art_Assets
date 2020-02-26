using System;
using Gululu.Log;


public class GuLog
{
    public static void Debug(string content, string tag = null)
    {
        LogManager.GetInstance().Logger(tag, content, LogLevel.Debug);
    }

    public static void Info(string content, string tag = null)
    {
        LogManager.GetInstance().Logger(tag, content, LogLevel.Info);
    }

    public static void Warning(string content, string tag = null)
    {
        LogManager.GetInstance().Logger(tag, content, LogLevel.Warning);
    }

    public static void Error(string content, string tag = null)
    {
        LogManager.GetInstance().Logger(tag, content, LogLevel.Error);
    }

    public static void Expection(Exception ex, string content, string tag = null)
    {
        string trackStr = new System.Diagnostics.StackTrace().ToString();
        content = ex.Message + "\r\n" + content + "\r\n" +  trackStr;
        LogManager.GetInstance().Logger(tag, content, LogLevel.Error);
    }

    public static void Fatal(string content, string tag = null)
    {
        LogManager.GetInstance().Logger(tag, content, LogLevel.Fatal);
    }
}


