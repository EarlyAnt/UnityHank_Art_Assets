using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class TimeDateUtil : MonoBehaviour
{
    /************************************************属性与变量命名************************************************/
    public Text Time;
    public Text TimeSystem;
    public Text Date;
    public Text Week;
    /************************************************Unity方法与事件***********************************************/
    void Start()
    {
        this.InvokeRepeating("UpdateTimeDate", 0f, 1f);
    }
    void Update()
    {
    }
    void OnEnable()
    {
    }
    /************************************************自 定 义 方 法************************************************/
    void UpdateTimeDate()
    {
        this.Time.text = DateTime.Now.ToString("hh:mm");
        this.TimeSystem.text = DateTime.Now.ToString("tt");
        this.Week.text = DateTime.Now.DayOfWeek.ToString().Substring(0, 3).ToUpper() + ", ";
        this.Date.text = DateTime.Now.ToString("MMM dd", CultureInfo.CreateSpecificCulture("en-GB")).ToUpper();        
        //Debug.LogFormat("----Component Test----TimeDateUtil.UpdateTimeDate: {0}, {1}, {2}", this.Time.text, this.Date.text, this.Week.text);
    }
}
