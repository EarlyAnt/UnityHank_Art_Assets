using System.Diagnostics;
using UnityEngine;
using System.IO;
using Gululu.Util;


namespace Gululu.Log{

    class FileHandler : IGuLogHandler{

        string LogfileName = "/Gululu.log";

        public void Handler(Recoder recoder){
            string detailLog = "";

            detailLog = recoder.time + " " + recoder.tag + " " + recoder.loglevel + " " + recoder.msg;

            HandlerLog(detailLog);
        }


        private void HandlerLog(string strLog)
        {
            string filePath = getAppLogPath();

            string sFileName = getAppLogPathName();

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            FileStream fs;
            StreamWriter sw;
            if (File.Exists(sFileName))
            {
                fs = new FileStream(sFileName, FileMode.Append, FileAccess.Write);
            }
            else
            {
                fs = new FileStream(sFileName, FileMode.Create, FileAccess.Write);
                // fs.Dispose();
            }
            sw = new StreamWriter(fs);
            sw.WriteLine(strLog);
            sw.Close();
            fs.Close();
        }

        public void deleteFile()
        {
            string logFileStr = readLogFile();
            if(logFileStr != null)
            {
                File.Delete(getAppLogPathName());
            }     
        }

        public string readLogFile()
        {
            string str = null;
            try
            {
                FileStream file = new FileStream(getAppLogPathName(), FileMode.Open);
                StreamReader reader = new StreamReader(file);
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    str += line + "\n";
                }
                reader.Close();
            }
            catch
            { 
                
            }
            return str;
        }

        public string getAppLogPath()
        {
            string filePath;


#if UNITY_EDITOR
            filePath = Application.dataPath + "/~Logs";
#else
            filePath = Application.persistentDataPath;
#endif
            return filePath;
        }

        public string getAppLogPathName()
        {
            string filePath = getAppLogPath();

            string sFileName = filePath + LogfileName;

            return sFileName;
        }
    }

}