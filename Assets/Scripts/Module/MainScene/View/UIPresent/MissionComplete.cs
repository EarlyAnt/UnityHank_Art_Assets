using System.Collections;
using System.Collections.Generic;
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
    public class MissionComplete : MainPresentBase
    {
        [Inject]
        public IMissionConfig MissionConfig { get; set; }
        [Inject]
        public IPrefabRoot PrefabRoot { get; set; }
        [SerializeField]
        private Spine.Unity.SkeletonGraphic missionComplete;
        [SerializeField]
        private UINormalMission uiNormalMission;
        [SerializeField]
        private UIDuplicateMission uiDuplicateMission;
        private bool is_loop = false;
        private bool can_hide = false;
        private WorldInfo.WorldTypes worldTypes;

        protected override void Start()
        {
            base.Start();
            this.gameObject.SetActive(false);
            this.uiNormalMission.SetStatus(false);
            this.uiDuplicateMission.SetStatus(false);
            this.missionComplete.AnimationState.Complete += delegate (TrackEntry trackEntry)
            {
                if (trackEntry.trackIndex == 1)
                {
                    is_loop = true;
                    missionComplete.AnimationState.ClearTracks();
                    missionComplete.AnimationState.SetAnimation(2, "mission_completed_loop", true);
                }
            };
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
            SoundPlayer.GetInstance().StopMusic();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override UIStateEnum GetStateEnum()
        {
            return UIStateEnum.eMissionComplete;
        }

        public override void ProcessState(UIState state)
        {
            bFinish = false;
            int starID = this.MissionConfig.GetStarIDByMissionID((int)state.value0);
            WorldInfo worldInfo = this.MissionConfig.GetWorldByStarID(starID);
            if (worldInfo != null)
            {
                this.missionComplete.gameObject.SetActive(true);
                this.missionComplete.Skeleton.SetSlotsToSetupPose();
                this.missionComplete.AnimationState.ClearTracks();
                this.missionComplete.AnimationState.SetAnimation(1, "mission_completed_start", false);

                this.worldTypes = worldInfo.Type;
                if (worldInfo.Type == WorldInfo.WorldTypes.Normal)
                    this.NormalMissionComplete(state);
                else if (worldInfo.Type == WorldInfo.WorldTypes.Duplicate)
                    this.DuplicateMissionComplete(state);
            }
            else
            {
                this.bFinish = true;
            }
        }

        private void NormalMissionComplete(UIState state)
        {
            int exp = (int)state.value1;
            int coin = (int)state.value2;
            this.uiNormalMission.ExpText.text = exp.ToString();
            this.uiNormalMission.CoinText.text = coin.ToString();

            MissionInfo info = MissionConfig.GetMissionInfoByMissionId((int)state.value0);
            if (info != null)
                this.uiNormalMission.PetImage.sprite = this.PrefabRoot.GetObjectNoInstantiate<Sprite>("Texture/Animals", info.NimIcon);
            this.gameObject.SetActive(true);
            this.uiNormalMission.SetPosition(320f, 0.0f);
            SoundPlayer.GetInstance().PlaySoundType("mission_complete_init");
        }

        private void DuplicateMissionComplete(UIState state)
        {
            this.uiDuplicateMission.uiPropsItems.ForEach(t => t.SetStatus(false));
            this.uiDuplicateMission.uiPropsItems[0].SetStatus(true);
            this.uiDuplicateMission.uiPropsItems[0].SetContent(this.uiDuplicateMission.CoinIcon, (int)state.value2);
            this.uiDuplicateMission.uiPropsItems[1].SetStatus(true);            
            this.uiDuplicateMission.uiPropsItems[1].SetContent(this.uiDuplicateMission.ExpIcon, (int)state.value1);

            this.gameObject.SetActive(true);
            this.uiDuplicateMission.SetPosition(320.0f, 0.0f);
            SoundPlayer.GetInstance().PlaySoundType("mission_complete_init");
        }

        public void PowerKeyPressUp()
        {
            if (TopViewHelper.Instance.HasModulePageOpened())
                return;

            FlurryUtil.LogEvent("Mission_Finish_Collectionpage_Click");
            if (is_loop)
            {
                this.missionComplete.AnimationState.ClearTracks();
                this.missionComplete.gameObject.SetActive(false);
                if (this.worldTypes == WorldInfo.WorldTypes.Normal)
                {
                    this.uiNormalMission.SetStatus(true);
                    this.uiNormalMission.RectTransform.DOLocalMoveY(0f, 0.5f).onComplete = delegate ()
                    {
                        this.is_loop = false;
                        this.can_hide = true;
                    };
                }
                else if (this.worldTypes == WorldInfo.WorldTypes.Duplicate)
                {
                    this.uiDuplicateMission.SetStatus(true);
                    this.uiDuplicateMission.RectTransform.DOLocalMoveY(0f, 0.5f).onComplete = delegate ()
                    {
                        this.is_loop = false;
                        this.can_hide = true;
                    };
                }
                SoundPlayer.GetInstance().PlaySoundType("mission_complete_board_fly_in");

            }
            if (this.can_hide)
            {
                this.can_hide = false;
                this.bFinish = true;
                this.gameObject.SetActive(false);
                this.uiNormalMission.SetStatus(false);
                this.uiDuplicateMission.SetStatus(false);
            }
        }

        public override void Stop()
        {
            base.Stop();
            this.gameObject.SetActive(false);
        }
    }
}

namespace Hank.MainScene.UIComponent.MissionCompleteView
{
    //主线地图UI
    [System.Serializable]
    public class UINormalMission
    {
        [SerializeField]
        private GameObject root;
        public RectTransform RectTransform
        {
            get
            {
                return this.root != null ? this.root.GetComponent<RectTransform>() : null;
            }
        }
        public Image PetImage;
        public Text ExpText, CoinText;
        public void SetStatus(bool visible)
        {
            if (this.root != null)
                this.root.SetActive(visible);
        }
        public void SetPosition(float endValue, float duration)
        {
            if (this.RectTransform != null)
                this.RectTransform.DOLocalMoveY(endValue, duration);
        }
    }
    //副本地图UI
    [System.Serializable]
    public class UIDuplicateMission
    {
        [SerializeField]
        private GameObject root;
        public Sprite ExpIcon;
        public Sprite CoinIcon;
        public RectTransform RectTransform
        {
            get
            {
                return this.root != null ? this.root.GetComponent<RectTransform>() : null;
            }
        }
        public List<UIPropsItem> uiPropsItems;
        public void SetStatus(bool visible)
        {
            if (this.root != null)
                this.root.SetActive(visible);
        }
        public void SetPosition(float endValue, float duration)
        {
            if (this.RectTransform != null)
                this.RectTransform.DOLocalMoveY(endValue, duration);
        }
    }
    //副本地图-道具条目
    [System.Serializable]
    public class UIPropsItem
    {
        [SerializeField]
        private GameObject root;
        [SerializeField]
        private Image iconImage;
        [SerializeField]
        private Text countText;
        public void SetStatus(bool visible)
        {
            if (this.root != null)
                this.root.SetActive(visible);
        }
        public void SetContent(Sprite image, int count)
        {
            if (this.iconImage != null)
            {
                this.iconImage.sprite = image;
                this.iconImage.SetNativeSize();
            }

            if (this.countText != null)
                this.countText.text = count.ToString();
        }
    }
}
