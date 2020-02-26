using Gululu.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMissionHelper : SingletonBase<ShowMissionHelper>
{
    int _currShowMissionId = -1;
    public int currShowMissionId{
        get { return _currShowMissionId; }
        set { _currShowMissionId = value; }
    }

    public MeshRenderer objCurrModel;

}
