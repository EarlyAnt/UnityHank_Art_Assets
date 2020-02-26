using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DateSelected : MonoBehaviour {

    [SerializeField]
    DateControl YearControl;
    [SerializeField]
    DateControl MonthControl;
    [SerializeField]
    DateControl DayControl;

    public void SetInitialDate(int year,int month,int day)
    {
        YearControl.InitialValue(year);
        MonthControl.InitialValue(month);
        DayControl.InitialValue(day);
    }

    public int GetYear()
    {
        return YearControl.currValue;
    }

    public int GetMonth()
    {
        return MonthControl.currValue;
    }

    public int GetDay()
    {
        return DayControl.currValue;
    }

    // Use this for initialization
    void Start () {
//         SetInitialDate(2010, 6, 15);

    }
	
}
