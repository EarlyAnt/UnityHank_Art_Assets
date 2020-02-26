using DG.Tweening;
using Gululu.Config;
using Gululu.Util;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Hank.MainScene
{
    public class DropWaterAni : MainPresentBase
    {
        /************************************************属性与变量命名************************************************/
        [Inject]
        public IMissionConfig MissionConfig { get; set; }
        [Inject]
        public IPlayerDataManager DataManager { get; set; }
        [Inject]
        public IPrefabRoot PrefabRoot { get; set; }
        [Inject]
        public UIStateCompleteSignal uIStateCompleteSignal { get; set; }
        [SerializeField]
        Animator water;
        [SerializeField]
        Animator waterDrop;
        [SerializeField]
        Animator specialWaterDrop;
        [SerializeField]
        Animator drop;
        [SerializeField]
        Animator expShiny;
        [SerializeField]
        Slider sliderWave;
        [SerializeField]
        GameObject expBall;
        [SerializeField]
        CanvasGroup award;
        [SerializeField]
        GameObject coin;
        [SerializeField]
        Text expWord;
        [SerializeField]
        Text coinWord;
        [SerializeField]
        Transform waterLevel;

        float bdrop_pos_y;
        float sdrop_pos_x;
        float sdrop_pos_y;
        float exp_pos_y;
        float missionProcess = 0f;
        int drinkExp = 0;
        int drinkCoin = 0;
        bool bSamllDrop = false;
        /************************************************Unity方法与事件***********************************************/

        /************************************************自 定 义 方 法************************************************/
        //测试组件是否确认
        public override void TestSerializeField()
        {
            Assert.IsNotNull(water);
            Assert.IsNotNull(waterDrop);
            Assert.IsNotNull(specialWaterDrop);
            Assert.IsNotNull(drop);
            Assert.IsNotNull(expShiny);
            Assert.IsNotNull(sliderWave);
            Assert.IsNotNull(expBall);
            Assert.IsNotNull(award);
            Assert.IsNotNull(expWord);
        }
        //获取此动画的类型枚举
        public override UIStateEnum GetStateEnum()
        {
            return UIStateEnum.eDropWaterAni;
        }
        //执行动画脚本
        public override void ProcessState(UIState state)
        {
            SetFinishAndOpenScreenSave(false);
            drinkCoin = (int)state.value2;
            drinkExp = (int)state.value1;
            missionProcess = (float)state.value0;
            coin.SetActive(drinkCoin > 0);

            if (!sliderWave.gameObject.activeInHierarchy)
            {
                SetFinishAndOpenScreenSave(true);
                uIStateCompleteSignal.Dispatch(GetStateEnum());
                return;
            }
            if (sliderWave.value.Equals(missionProcess))
            {
                bSamllDrop = true;
            }

            MissionInfo missionInfo = this.MissionConfig.GetMissionInfoByMissionId(ShowMissionHelper.Instance.currShowMissionId);
            if (missionInfo == null) { this.bFinish = true; return; }

            if (string.IsNullOrEmpty(missionInfo.WaterDrop))
            {
                this.waterDrop.Play("waterDrop");
                if (this.IsNormalStatus())
                    SoundPlayer.GetInstance().PlaySoundType("water_drop_down");
            }
            else if (!string.IsNullOrEmpty(missionInfo.WaterDrop))
            {
                GameObject gameObject = this.PrefabRoot.GetObjectNoInstantiate<GameObject>("WaterDrop", missionInfo.WaterDrop);
                this.specialWaterDrop.runtimeAnimatorController = gameObject.GetComponent<Animator>().runtimeAnimatorController;
                this.StopAllCoroutines();
                this.StartCoroutine(this.CheckState());
                if (this.IsNormalStatus())
                    SoundPlayer.GetInstance().PlaySoundType(missionInfo.WaterDropAudio);
            }
        }
        //顺序1
        public void AfterWaterDrop()
        {
            DropAnimation();
            waterDrop.Play("packUp");
        }
        //顺序3
        public void AfterDropEx()
        {
            drop.Play("empty");
            drop.gameObject.transform.DOLocalMoveY(bdrop_pos_y, 0.25f);
            water.Play("spray");
        }
        //顺序4
        public void AfterSpray()
        {
            expBall.SetActive(false);
            award.gameObject.SetActive(false);
            ShowDropWater(true);
            expBall.GetComponent<Image>().DOFade(1, 0.01f);
            award.DOFade(1, 0.01f);
            expWord.GetComponent<Text>().DOFade(1, 0.01f);
            sdrop_pos_x = expBall.transform.position.x;
            sdrop_pos_y = expBall.transform.position.y;
            exp_pos_y = award.transform.position.y;
            if (missionProcess < 0.7)
            {
                expBall.transform.DOLocalMove(this.waterLevel.localPosition, 0.01f);
                award.transform.DOLocalMoveY(this.waterLevel.localPosition.y - 30, 0.01f);
            }
            WaterRiseAnimation();
        }
        //顺序2
        private void DropAnimation()
        {
            if (missionProcess > 0.84)
            {
                drop.Play("empty");
                water.Play("spray");
            }
            else
            {
                bdrop_pos_y = drop.transform.position.y;
                drop.Play("dropEx");
                drop.gameObject.transform.DOMoveY(this.waterLevel.position.y, 0.5f);
                Scale(drop.gameObject);
            }
        }
        //顺序5
        private void WaterRiseAnimation()
        {
            FlurryUtil.LogEvent("Drink_Exp_Collect_ani_View");

            if (this.IsNormalStatus())
            {
                SoundPlayer.GetInstance().PlaySoundType("water_raise");
                if (drinkCoin > 0)
                    SoundPlayer.GetInstance().PlaySoundType("gain_coin");
            }

            expBall.SetActive(true);
            Scale(expBall.gameObject);

            if (missionProcess > 0.84)
            {
                expBall.GetComponent<Image>().DOFade(1, 0.01f);
            }
            award.gameObject.SetActive(true);
            int textLength = Mathf.Max(drinkExp.ToString().Length, drinkCoin.ToString().Length);
            expWord.text = "+" + drinkExp.ToString().PadLeft(textLength, ' ');
            coinWord.text = "+" + drinkCoin.ToString().PadLeft(textLength, ' ');
            Sequence sDropSeq = DOTween.Sequence();
            sDropSeq.Append(expBall.transform.DOLocalMove(this.waterLevel.localPosition + new Vector3(-300, 240, 0), 1.5f).SetEase(Ease.OutQuad))
                    .Append(expBall.GetComponent<Image>().DOFade(0, 0.5f))
                    .Append(expBall.transform.DOLocalMove(this.waterLevel.localPosition, 0.01f));

            sDropSeq.onComplete = delegate ()
            {
                expShiny.Play("ExpShiny");
            };

            award.DOFade(0.5f, 0.8f).SetLoops(3);
            float dest_pos_y = exp_pos_y + 70;
            if (missionProcess < 0.7)
            {
                dest_pos_y = this.waterLevel.localPosition.y + 20;
            }

            award.transform.DOLocalMoveY(dest_pos_y, 2.4f).onComplete = delegate ()
            {
                award.DOFade(0, 1.0f).onComplete = delegate ()
                {
                    award.transform.DOMoveY(exp_pos_y, 0.5f);
                };
                drinkExp = 0;
                SetFinishAndOpenScreenSave(true);
                uIStateCompleteSignal.Dispatch(GetStateEnum());
            };

            sliderWave.DOValue(missionProcess, 1.0f).onComplete = delegate ()
            {
            };
        }
        //检查播放状态
        private IEnumerator CheckState()
        {
            this.specialWaterDrop.Play("Drop");
            AnimationClip[] clips = this.specialWaterDrop.runtimeAnimatorController.animationClips;
            float time = 2;
            for (int i = 0; i < clips.Length; i++)
            {
                if (clips[i].name.ToUpper() == "WATERDROP")
                {
                    time = clips[i].length;
                    break;
                }
            }
            yield return new WaitForSeconds(time);
            this.specialWaterDrop.Play("Idle");
            //GameObject.Destroy(this.specialWaterDrop.gameObject);
            //this.specialWaterDrop = null;
            this.AfterSpray();
        }
        //重设水面高度
        public void ResetWaterHeight()
        {
            sliderWave.value = 0;
        }
        //设置滴水效果是否显示
        public void ShowDropWater(bool bShow)
        {
            gameObject.SetActive(bShow);
        }
        //缩放水滴
        private void Scale(GameObject obj)
        {
            float scale = bSamllDrop ? 0.5f : 1f;
            obj.transform.localScale = new Vector3(scale, scale, scale);
        }
        //判断状态
        private bool IsNormalStatus()
        {
            return !TopViewHelper.Instance.HasModulePageOpened() || TopViewHelper.Instance.IsInGuideView();
        }
    }
}