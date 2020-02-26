using Gululu;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class ClockWithAmPm : BaseView {

    [SerializeField]
    GameObject timeTx, amIcon, pmIcon;

    public override void TestSerializeField()
    {
        Assert.IsNotNull(timeTx);
        Assert.IsNotNull(amIcon);
        Assert.IsNotNull(pmIcon);
    }

    protected override void Start()
    {
        base.Start();

    }
    private void OnEnable()
    {
        updateTime();
        InvokeRepeating("updateTime", 60 - DateTime.Now.Second, 60);
    }

    private void OnDisable()
    {
        CancelInvoke("updateTime");
    }


    public void updateTime()
    {
        System.Globalization.DateTimeFormatInfo dtfi = new System.Globalization.CultureInfo("en-US", false).DateTimeFormat;
        //dtfi.ShortTimePattern = "t";
        string timeStr = DateTime.Now.ToString("h:mm", dtfi);
        timeTx.GetComponent<Text>().text = timeStr;
        if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour < 12)
        {
            amIcon.SetActive(true);
            pmIcon.SetActive(false);
        }
        else
        {
            amIcon.SetActive(false);
            pmIcon.SetActive(true);
        }
    }
}
