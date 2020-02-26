using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeightSelected : MonoBehaviour {

    [SerializeField]
    DateControl KGControl;
    [SerializeField]
    DateControl PoundControl;

    [SerializeField]
    GameObject objKg,objPound;
    [SerializeField]
    GameObject objKgButton, objPoundButton;

    //     [SerializeField]
    //     StringSelected UnitOfWeightSelected;

    string[] stringUnitOfWeight =
    {
        "kg",
        "lbs",
    };
    int currUnitOfWeightIndex = 0; 

    public string GetCurrUnitOfWeight()
    {
        if(currUnitOfWeightIndex < stringUnitOfWeight.Length)
        {
            return stringUnitOfWeight[currUnitOfWeightIndex];
        }

        return "";
    }
    public int GetKG()
    {
        return KGControl.currValue;
    }

    public int GetPound()
    {
        return PoundControl.currValue;
    }

    void Change2Kg(GameObject go)
    {
        UnitOfWeightChange(0, true);
    }
    void Change2Pound(GameObject go)
    {
        UnitOfWeightChange(1, true);
    }

    public void UnitOfWeightChange(int i, bool syncValue = false)
    {
        currUnitOfWeightIndex = i;
        if (currUnitOfWeightIndex == 0)
        {
            objKg.SetActive(true);
            objPound.SetActive(false);
            if (syncValue)
            {
                KGControl.InitialValue((int)(GetPound() * 0.45359237f));
            }
        }
        else
        {
            objKg.SetActive(false);
            objPound.SetActive(true);
            if (syncValue)
            {
                PoundControl.InitialValue((int)(GetKG() * 2.20462262f));
            }
        }
    }

    public void SetInitialDate(int kg,int pound, int unit)
    {
        KGControl.InitialValue(kg);
        PoundControl.InitialValue(pound);
        UnitOfWeightChange(unit);

        //         UnitOfWeightSelected.SetValue(stringUnitOfWeight[currUnitOfWeightIndex]);
    }

    private void Start()
    {
        objKg.SetActive(true);
        objPound.SetActive(false);

        EventTriggerListener.Get(objKgButton).onUp = Change2Kg;
        EventTriggerListener.Get(objPoundButton).onUp = Change2Pound;


        //        UnitOfWeightSelected.Initialize(stringUnitOfWeight, UnitOfWeightChange);
    }
}
