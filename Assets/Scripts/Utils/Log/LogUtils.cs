using Gululu.Util;


namespace Gululu.Log
{
    class LogUtils : SingletonBase<LogUtils>{

        FileHandler flie = new FileHandler();
        public void deleteLogFlie()
        {
            flie.deleteFile();
        }

        public string readLogFile()
        {
           return flie.readLogFile();
        }
    }
}