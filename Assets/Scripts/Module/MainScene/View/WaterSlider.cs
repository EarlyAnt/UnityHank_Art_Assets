using Hank;
using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class WaterSlider : Gululu.BaseView
{
    [Inject]
    public IPlayerDataManager dataManager { get; set; }
    [Inject]
    public IMissionDataManager missionDataManager { get; set; }

    public Image imageFillArea;
    public Image imageHandle;
    public Slider sliderWave;


    bool direction = true;
    float curAlpha = 1.0f;

    public override void TestSerializeField()
    {
        Assert.IsNotNull(imageFillArea);
        Assert.IsNotNull(imageHandle);
        Assert.IsNotNull(sliderWave);
    }


    public void RefreshSlideValue()
    {
        if(dataManager == null)
        {
            return;
        }
        if(ShowMissionHelper.Instance.currShowMissionId == dataManager.missionId)
        {
            sliderWave.value = (float)dataManager.missionPercent;
        }
        else
        {
            MissionState  state = missionDataManager.GetMisionState(ShowMissionHelper.Instance.currShowMissionId);
            if(state == MissionState.eComplete)
            {
                sliderWave.value = 1;
            }
            else
            {
                sliderWave.value = 0;
            }
        }
    }

    void transAlphaLoop()
    {
        curAlpha = (direction == true)
            ? (curAlpha > 0.5 ? curAlpha - (float)(Time.deltaTime * 0.25) : 0.5f)
            : (curAlpha < 1.0 ? curAlpha + (float)(Time.deltaTime * 0.25) : 1.0f);
        transImgAlpha(curAlpha);
        if (System.Math.Abs(curAlpha - 0.5f) < 0.01)
        {
            direction = false;
        }
        else if (System.Math.Abs(curAlpha - 1.0f) < 0.01)
        {
            direction = true;
        }
    }

    void transImgAlpha(float apl)
    {
        imageFillArea.color = new Color(255, 255, 255, apl);
        imageHandle.color = new Color(255, 255, 255, apl);
    }

    // Use this for initialization
    override protected void Start()
    {
        base.Start();
        RefreshSlideValue();
    }

    // Update is called once per frame
    void Update () {
        transAlphaLoop();

    }


}
