using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gululu.Log;


public class ScreenHandler : MonoBehaviour, IGuLogHandler 
{
    static List<string> mLines = new List<string>();
    static List<string> mWriteTxt = new List<string>();
    private string outpath;

    public void Handler(Recoder recoder)
    {
        string detailLog = "";

        detailLog = recoder.time + " " + recoder.tag + " " + recoder.loglevel + " " + recoder.msg;
        
        mWriteTxt.Add(detailLog);
    }
 
    void Update () 
    {
        //因为写入文件的操作必须在主线程中完成，所以在Update中哦给你写入文件。
        if(mWriteTxt.Count > 0)
        {
            string[] temp = mWriteTxt.ToArray();
            foreach(string t in temp)
            {
                using(StreamWriter writer = new StreamWriter(outpath, true, Encoding.UTF8))
                {
                    writer.WriteLine(t);
                }
                mWriteTxt.Remove(t);
            }
        }
    }
 
    void OnGUI()
    {
        GUI.color = Color.red;
        for (int i = 0, imax = mLines.Count; i < imax; ++i)
        {
            GUILayout.Label(mLines[i]);
        }
    }
}
