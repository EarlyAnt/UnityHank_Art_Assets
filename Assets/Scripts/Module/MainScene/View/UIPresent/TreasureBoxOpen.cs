using DG.Tweening;
using Cup.Utils.Touch;
using Gululu.Config;
using Gululu.LocalData.Agents;
using Gululu.Util;
using Hank.MainScene.UIComponent.TreasureBoxView;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hank.MainScene
{
    public class TreasureBoxOpen : MainPresentBase
    {
        [Inject]
        public IPrefabRoot PrefabRoot { get; set; }
        [Inject]
        public ILocalPetInfoAgent LocalPetInfoAgent { get; set; }
        [Inject]
        public ITreasureBoxConfig TreasureBoxConfig { get; set; }
        [Inject]
        public IItemConfig ItemConifg { get; set; }
        [Inject]
        public IMissionConfig MissionConfig { get; set; }
        [Inject]
        public IPlayerDataManager PlayerDataManager { get; set; }
        [Inject]
        public IFundDataManager FundDataManager { get; set; }
        [Inject]
        public UIStateCompleteSignal UIStateCompleteSignal { get; set; }
        [Inject]
        public CloseTreasureBoxSignal CloseTreasureBoxSignal { get; set; }
        //[SerializeField]
        //private RightTreasureBoxSpecialIdleController rightTreasure;
        [SerializeField]
        private MissionBackground missionBackground;
        [SerializeField]
        private GameObject okButton;
        [SerializeField]
        private UINimBox uiNimBox;
        [SerializeField]
        private UIPropsBox uiPropsBox;
        private int treasureBoxIndex = 0;
        private int missionId = 0;

        protected override void Start()
        {
            base.Start();
            gameObject.SetActive(false);
            this.uiNimBox.SetStatus(false);
            this.uiPropsBox.SetStatus(false);
            CloseTreasureBoxSignal.AddListener(Stop);
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
            this.uiNimBox.SetStatus(false);
            this.uiPropsBox.SetStatus(false);
        }

        protected override void OnDestroy()
        {
            CloseTreasureBoxSignal.RemoveListener(Stop);
            base.OnDestroy();
        }

        public override UIStateEnum GetStateEnum()
        {
            return UIStateEnum.eTreasureBoxOpen;
        }

        public override void ProcessState(UIState state)
        {
            this.bFinish = false;
            this.gameObject.SetActive(true);
            this.okButton.SetActive(false);
            this.treasureBoxIndex = (int)state.value0;
            this.missionId = (int)state.value1;
            this.PlayerDataManager.MissionIdOfOpenedBox = missionId;
            //this.rightTreasure.RefreshRightTreasureBoxs(this.treasureBoxIndex);
            int starID = this.MissionConfig.GetStarIDByMissionID(this.missionId);
            WorldInfo worldInfo = this.MissionConfig.GetWorldByStarID(starID);
            if (worldInfo != null)
            {
                if (worldInfo.Type == WorldInfo.WorldTypes.Normal)
                    this.OpenNimItemBox(state);
                else if (worldInfo.Type == WorldInfo.WorldTypes.Duplicate)
                    this.OpenPropsItemBox(state);
            }
            else
            {
                this.bFinish = true;
            }
        }

        private void OpenNimItemBox(UIState state)
        {
            this.uiNimBox.SetStatus(true);
            this.uiNimBox.NimImage.gameObject.SetActive(false);
            MissionInfo info = this.MissionConfig.GetMissionInfoByMissionId(missionId);
            if (info != null && this.treasureBoxIndex < info.listTreasureBox.Count)
            {
                TreasureBox treasureBox = this.TreasureBoxConfig.GetTreasureBoxById(info.listTreasureBox[this.treasureBoxIndex].BoxId);
                foreach (var treasure in treasureBox.items)
                {
                    GameObject resource = this.PrefabRoot.GetGameObject("Scenes", this.ItemConifg.GetImage(treasure.itemId));
                    if (resource && resource.GetComponent<SpriteRenderer>() != null)
                    {
                        uiNimBox.NimImage.sprite = resource.GetComponent<SpriteRenderer>().sprite;
                        Destroy(resource);
                    }
                }
            }

            this.uiNimBox.TreasureOpen.AnimationState.ClearTracks();
            this.uiNimBox.TreasureOpen.AnimationState.SetAnimation(0, "unboxing", false).Complete += delegate (Spine.TrackEntry track)
            {
                Debug.LogFormat("<><TreasureBoxOpen.OpenNimItemBox>Sound: {0}, HasModulePageOpened: {1}, InGuideView: {2}",
                                "nim_chest_open_" + LocalPetInfoAgent.getCurrentPet(), TopViewHelper.Instance.HasModulePageOpened(),
                                TopViewHelper.Instance.IsInGuideView());
                if (!TopViewHelper.Instance.HasModulePageOpened() || TopViewHelper.Instance.IsInGuideView())
                {//没有打开任何功能模块或正在引导才能播放此声音
                    Debug.LogFormat("<><TreasureBoxOpen.OpenNimItemBox>Sound: {0} ==== Playing", "nim_chest_open_" + LocalPetInfoAgent.getCurrentPet());
                    SoundPlayer.GetInstance().PlaySoundType("nim_chest_open_" + LocalPetInfoAgent.getCurrentPet());
                }
            };
            this.uiNimBox.TreasureOpen.AnimationState.AddAnimation(0, "Biological appearance", true, 0).Complete += delegate (Spine.TrackEntry track)
            {
                this.uiNimBox.NimImage.gameObject.SetActive(true);
                this.UIStateCompleteSignal.Dispatch(GetStateEnum());
                if (track.Loop)
                {
                    this.okButton.SetActive(true);
                }
            };
        }

        private void OpenPropsItemBox(UIState state)
        {
            this.uiPropsBox.SetStatus(true);
            this.uiPropsBox.SetPosition(320.0f, 0.0f);
            this.uiPropsBox.SetPosition(0.0f, 0.5f);
            this.uiPropsBox.uiPropsItems.ForEach(t => t.SetStatus(false));
            MissionInfo missionInfo = this.MissionConfig.GetMissionInfoByMissionId(missionId);
            if (missionInfo != null && this.treasureBoxIndex < missionInfo.listTreasureBox.Count)
            {
                TreasureBox treasureBox = this.TreasureBoxConfig.GetTreasureBoxById(missionInfo.listTreasureBox[this.treasureBoxIndex].BoxId);
                for (int i = 0; i < treasureBox.items.Count; i++)
                {
                    GameObject resource = this.PrefabRoot.GetGameObject("Scenes", this.ItemConifg.GetImage(treasureBox.items[i].itemId));
                    if (resource && resource.GetComponent<SpriteRenderer>() != null)
                    {
                        this.uiPropsBox.uiPropsItems[i].SetStatus(true);
                        this.uiPropsBox.uiPropsItems[i].SetContent(resource.GetComponent<SpriteRenderer>().sprite, treasureBox.items[i].count);
                        this.FundDataManager.AddProperty(treasureBox.items[i].itemId, treasureBox.items[i].count);
                        Destroy(resource);
                    }
                }
            }

            Debug.LogFormat("<><TreasureBoxOpen.OpenPropsItemBox>Sound: {0}, HasModulePageOpened: {1}, InGuideView: {2}",
                            "nim_chest_open_" + LocalPetInfoAgent.getCurrentPet(), TopViewHelper.Instance.HasModulePageOpened(),
                            TopViewHelper.Instance.IsInGuideView());
            if (!TopViewHelper.Instance.HasModulePageOpened() || TopViewHelper.Instance.IsInGuideView())
            {//没有打开任何功能模块或正在引导才能播放此声音
                Debug.LogFormat("<><TreasureBoxOpen.OpenPropsItemBox>Sound: {0} ==== Playing", "nim_chest_open_" + LocalPetInfoAgent.getCurrentPet());
                SoundPlayer.GetInstance().PlaySoundType("nim_chest_open_" + LocalPetInfoAgent.getCurrentPet());
            }

            UIStateCompleteSignal.Dispatch(GetStateEnum());
            okButton.SetActive(true);
        }

        public void PowerKeyPressUp()
        {
            if (TopViewHelper.Instance.HasModulePageOpened() || !this.okButton.activeSelf)
                return;

            UIStateCompleteSignal.Dispatch(GetStateEnum());
            Stop();
        }

        public override void Stop()
        {
            base.Stop();
            FlurryUtil.LogEvent("Nim_Chest_open_event");
            if (!TopViewHelper.Instance.HasModulePageOpened() || TopViewHelper.Instance.IsInGuideView())
            {
                SoundPlayer.GetInstance().PlaySoundType("nim_chest_get_nim");
            }

            this.missionBackground.PlayNimAnimation(treasureBoxIndex, missionId);
            bFinish = true;
            gameObject.SetActive(false);
        }
    }
}

namespace Hank.MainScene.UIComponent.TreasureBoxView
{
    //主线地图UI
    [System.Serializable]
    public class UINimBox
    {
        [SerializeField]
        private GameObject root;
        public Spine.Unity.SkeletonGraphic TreasureOpen;
        public Image NimImage;
        public void SetStatus(bool visible)
        {
            if (this.root != null)
                this.root.SetActive(visible);
        }
    }
    //副本地图UI
    [System.Serializable]
    public class UIPropsBox
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
                this.iconImage.sprite = image;

            if (this.countText != null)
                this.countText.text = count.ToString();
        }
    }
}
