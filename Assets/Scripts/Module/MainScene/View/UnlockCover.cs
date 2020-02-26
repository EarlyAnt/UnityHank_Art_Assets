using DG.Tweening;
using Hank;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hank.MainScene
{
    public class UnlockCover : MonoBehaviour
    {
        [SerializeField]
        SkeletonGraphic unlockGp;
        [SerializeField]
        RolePlayer rolePlayerSC;
        [SerializeField]
        DropWaterAni dropWaterAniSC;
        [SerializeField]
        WaterSlider waterSlider;


        private void HideOther(bool bHide)
        {
            rolePlayerSC.gameObject.SetActive(!bHide);
            dropWaterAniSC.ShowDropWater(!bHide);
            if (!bHide)
            {
                waterSlider.RefreshSlideValue();
            }
        }

        public void ShowLock(bool bShow)
        {
            gameObject.SetActive(bShow);
            HideOther(bShow);
        }

        public void LockFadeInout(bool bIn)
        {
            if (bIn)
            {
                ShowLock(bIn);
                gameObject.GetComponent<Image>().DOFade(0.9f, 2);
                unlockGp.DOFade(1, 2);
            }
            else
            {
                HideOther(false);
                gameObject.GetComponent<Image>().DOFade(0, 2);
                unlockGp.DOFade(0, 2).onComplete = delegate ()
                {
                    gameObject.SetActive(false);
                };

            }
        }

    }
}
