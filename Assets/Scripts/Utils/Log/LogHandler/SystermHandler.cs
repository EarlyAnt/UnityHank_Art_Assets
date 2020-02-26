using UnityEngine;


namespace Gululu.Log
{
    public class SystermHandler: IGuLogHandler
    {
        public void Handler(Recoder recoder)
        {
            string detailLog = "";

            detailLog = recoder.time + " " + recoder.tag + " " + recoder.loglevel + " " + recoder.msg;
            
//            Debug.Log(detailLog);
        }
    } 
}

