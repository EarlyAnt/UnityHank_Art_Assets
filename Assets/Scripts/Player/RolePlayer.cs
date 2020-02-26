using Cup.Utils.Screen;
using Gululu.Config;
using Hank.MainScene;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hank
{
    public class RolePlayer : PetPlayer
    {
        public static class BodyPart
        {
            public const string HEAD = "Dummy_head";
            public const string WING = "Dummy_wing";
            public const string SUIT = "Dummy_taozhuang";
        }

        [Inject]
        public ISilenceTimeDataManager SilenceTimeDataManager { get; set; }
        [Inject]
        public RolePlayerPlayActionSignal rolePlayerPlayActionSignal { get; set; }
        public bool StatusCheckingEnable { get; set; }
        private bool isHunger;
        private bool isIdle;

        protected override void Start()
        {
            base.Start();
        }
        protected override void OnDestroy()
        {
            ScreenManager.removeListener(this.OnScreenStatusChanged);
            rolePlayerPlayActionSignal.RemoveListener(Play);
            this.CancelInvoke();
            base.OnDestroy();
        }

        protected override void Initialize()
        {
            if (this.createDefaultRole)
                RefreshRole();
            rolePlayerPlayActionSignal.AddListener(Play);
            this.InvokeRepeating("CheckStatus", 1f, 1f);
            ScreenManager.addListener(this.OnScreenStatusChanged);
        }
        protected override string GetTag()
        {
            return "Host";
        }
        public override void Show()
        {
            base.Show();
            this.ResetStatus();
        }
        public override void ResetStatus()
        {
            this.isIdle = false;
            this.isHunger = false;
        }
        private void CheckStatus()
        {
            if (this.StatusCheckingEnable && this.animator != null && this.gameObject.activeInHierarchy &&
                !TopViewHelper.Instance.IsOpenedView(TopViewHelper.ETopView.eNoviceGuide))
            {
                if (this.PlayerDataManager.IsPetHunger && !this.isHunger)
                {
                    this.isIdle = false;
                    this.isHunger = true;
                    this.animator.SetBool("Hunger", true);
                    if (Time.time > 8 && !TopViewHelper.Instance.HasModulePageOpened() && !this.SilenceTimeDataManager.InSilenceMode(DateTime.Now))
                        SoundPlayer.GetInstance().PlayRoleSound("pet_hungry_tips");
                }
                else if (!this.PlayerDataManager.IsPetHunger && !this.isIdle)
                {
                    this.isIdle = true;
                    this.isHunger = false;
                    this.animator.SetBool("Hunger", false);
                }
            }
        }
        private void OnScreenStatusChanged(string status)
        {
#if (UNITY_ANDROID) && (!UNITY_EDITOR)
            switch (status)
            {
                case "ACTION_SCREEN_ON":
                if (this.StatusCheckingEnable && this.PlayerDataManager.IsPetHunger && 
                    !TopViewHelper.Instance.IsOpenedView(TopViewHelper.ETopView.eNoviceGuide) && 
                    !this.SilenceTimeDataManager.InSilenceMode(DateTime.Now))
                    SoundPlayer.GetInstance().PlayRoleSound("pet_hungry_tips");
                break;
            }
            Debug.LogFormat("<><RolePlayer.OnScreenStatusChanged>Status: {0}", status);
#endif
        }
    }
}

