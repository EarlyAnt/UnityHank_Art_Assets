using DG.Tweening;
using Gululu.Config;
using UnityEngine;
using UnityEngine.UI;
using Hank.Data;
using Cup.Utils.Touch;
using Gululu.Util;
using Spine.Unity;
using Spine;
using UnityEngine.Assertions;
using Hank.MainScene.UIComponent.MissionCompleteView;

namespace Hank.MainScene
{
    public class MissionRetry : MainPresentBase
    {
        [Inject]
        public II18NConfig I18NConfig { get; set; }
        [Inject]
        public IPrefabRoot PrefabRoot { get; set; }
        [Inject]
        public IMissionConfig MissionConfig { get; set; }
        [SerializeField]
        private Image starImage;
        [SerializeField]
        private Image tipImage;

        protected override void Start()
        {
            base.Start();
            this.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
#if (UNITY_ANDROID) && (!UNITY_EDITOR)
            CupKeyEventHookStaticManager.addPowerKeyPressCallBack(PowerKeyPressUp);
#endif            
        }

        private void OnDisable()
        {
#if (UNITY_ANDROID) && (!UNITY_EDITOR)
            CupKeyEventHookStaticManager.removePowerKeyPressCallBack(PowerKeyPressUp);
#endif            
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override UIStateEnum GetStateEnum()
        {
            return UIStateEnum.eMissionRetry;
        }

        public override void ProcessState(UIState state)
        {
            bFinish = false;
            object missionObj = state.value0;
            int missionID = -1;
            if (missionObj == null || !int.TryParse(missionObj.ToString(), out missionID)) { this.Stop(); return; }
            WorldInfo worldInfo = this.MissionConfig.GetWorldByStarID(this.MissionConfig.GetStarIDByMissionID(missionID));
            if (worldInfo == null) { this.Stop(); return; }
            Sprite starSprite = Resources.Load<Sprite>(string.Format("Texture/WorldMap/{0}", worldInfo.Preview));
            if (starSprite == null) { this.Stop(); return; }
            Sprite tipSprite = Resources.Load<Sprite>(this.I18NConfig.GetImagePath("PlayAgain"));
            if (starSprite == null) { this.Stop(); return; }

            this.starImage.sprite = starSprite;
            this.tipImage.sprite = tipSprite;
            this.gameObject.SetActive(true);
            SoundPlayer.GetInstance().PlaySoundType("mission_complete_retry");
        }

        public void PowerKeyPressUp()
        {
            if (TopViewHelper.Instance.HasModulePageOpened())
                return;

            this.Stop();
        }

        public override void Stop()
        {
            base.Stop();
            SoundPlayer.GetInstance().StopSound("mission_complete_retry");
            this.gameObject.SetActive(false);
            this.bFinish = true;
        }
    }
}
