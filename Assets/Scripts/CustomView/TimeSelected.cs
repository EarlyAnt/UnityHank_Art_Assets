using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSelected : MonoBehaviour {

    public DateControl HourControl;
    public DateControl MinuteControl;
    public Text textTitle;
    public void OnConfirm()
    {
        gameObject.SetActive(false);
        if(confirmCB != null)
        {
            confirmCB(true);
        }
    }

    public void SetInitialTime(int hour,int minute)
    {
        HourControl.InitialValue(hour);
        MinuteControl.InitialValue(minute);
    }

    public int GetHour()
    {
        return HourControl.currValue;
    }

    public int GetMinute()
    {
        return MinuteControl.currValue;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Hide()
    {
        if (confirmCB != null)
        {
            confirmCB(false);
        }
        gameObject.SetActive(false);
    }


    public delegate void ConfirmCallBack(bool bConfirm);

    ConfirmCallBack confirmCB;


    public void ShowTimeSelected(int hour, int minute,string strTileString, ConfirmCallBack cb)
    {
        textTitle.text = strTileString;
        confirmCB = cb;
        gameObject.SetActive(true);
        SetInitialTime(hour, minute);
    }
}
