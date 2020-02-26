using Cup.Utils.CupMotion;
using Cup.Utils.Touch;
using Cup.Utils.Water;
using CupSdk.BaseSdk.Touch;
using Gululu;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace Hank.MainScene
{
    public class HeartBeatView : View
    {
        // Update is called once per frame
        void Update()
        {
            if(Time.deltaTime > 0.3f)
            {
                GuLog.Info("<><HeartBeatView>Long Frame:" + Time.deltaTime.ToString());
            }
        }
    }

}
