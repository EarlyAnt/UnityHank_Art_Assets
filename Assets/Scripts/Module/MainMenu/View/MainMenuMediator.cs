using Gululu;
using Gululu.Events;
using Gululu.Util;
using System.Collections;
using UnityEngine;

namespace Hank.MainMenu
{
    public class MainMenuMediator : BaseMediator
    {
        /************************************************属性与变量命名************************************************/
        [Inject]
        public MainMenuView MainMenuView { get; set; }
        [Inject]
        public IPrefabRoot PrefabRoot { get; set; }
        [Inject]
        public IPlayerDataManager PlayerDataManager { get; set; }
        [Inject]
        public IDrinkWaterUIPresentManager UIState { get; set; }
        private bool powerHolding = false;
        private Coroutine changeModuleCoroutine;
        /************************************************Unity方法与事件***********************************************/

        /************************************************自 定 义 方 法************************************************/
        public override void OnRegister()
        {
            this.View = TopViewHelper.ETopView.eMainMenu;
            base.OnRegister();
            this.MainMenuView.Open();
            this.MainMenuView.ShowGuide();
            FlurryUtil.LogTimeEvent("navmenu_view");
        }
        public override void OnRemove()
        {            
            base.OnRemove();
            FlurryUtil.EndTimedEvent("navmenu_view");
        }
        protected override void PowerHold()
        {
            this.powerHolding = true;
#if UNITY_EDITOR
            UIState.PushNewState(UIStateEnum.eRefreshMainByData);
#endif
        }
        protected override void PowerClick()
        {
            this.MainMenuView.SetOpenModuleIndex();
            this.SelectSubModule();
        }
        protected override void CupTiltLeft()
        {
            this.MainMenuView.HideGuide();

            if (this.changeModuleCoroutine != null)
                this.StopCoroutine(this.changeModuleCoroutine);
            this.changeModuleCoroutine = this.StartCoroutine(this.ContinueMove(true));
        }
        protected override void CupTiltRight()
        {
            this.MainMenuView.HideGuide();

            if (this.changeModuleCoroutine != null)
                this.StopCoroutine(this.changeModuleCoroutine);
            this.changeModuleCoroutine = this.StartCoroutine(this.ContinueMove(false));
        }
        protected override void CupKeepFlat()
        {
            if (this.changeModuleCoroutine != null)
                this.StopCoroutine(this.changeModuleCoroutine);
        }
        private IEnumerator ContinueMove(bool left)
        {
            while (true)
            {
                if (left)
                {
                    this.MainMenuView.MovePreviousItem();
#if UNITY_EDITOR
                    if (this.powerHolding && this.PlayerDataManager.playerLevel > 0)
                    {
                        this.PlayerDataManager.playerLevel -= 1;
                        UIState.PushNewState(UIStateEnum.eRefreshMainByData);
                    }
#endif
                }
                else
                {
                    this.MainMenuView.MoveNextItem();
#if UNITY_EDITOR
                    if (this.powerHolding && this.PlayerDataManager.playerLevel < 100)
                    {
                        this.PlayerDataManager.playerLevel += 1;
                        UIState.PushNewState(UIStateEnum.eRefreshMainByData);
                    }
#endif
                }
                yield return new WaitForSeconds(0.7f);
            }
        }
        protected override void CupShake()
        {
            FlurryUtil.LogTimeEvent("navmenu_exit_event");
            TopViewHelper.Instance.RemoveView(this.View);
            this.MainMenuView.Close(() => dispatcher.Dispatch(MainEvent.CloseMainMenu, TopViewHelper.ETopView.eMainMenu));
        }
        private void SelectSubModule()
        {
            this.UnregisterInput();
            TopViewHelper.Instance.RemoveView(this.View);//����Ҫ�ȳ�ջ����ΪMainMenu�Ķ�Ч���ӳ�View��ջ
            switch (MainMenuView.CurrentTask)
            {
                case MainMenuView.Tasks.Achievement:
                    this.dispatcher.Dispatch(MainEvent.OpenView, TopViewHelper.ETopView.eAchievement);
                    break;
                case MainMenuView.Tasks.Cup:
                    FlurryUtil.LogEvent("navmenu_system_page_click");
                    this.dispatcher.Dispatch(MainEvent.OpenView, TopViewHelper.ETopView.eSystemMenu);
                    break;
                case MainMenuView.Tasks.Home:
                    FlurryUtil.LogEvent("navmenu_home_click");
                    this.dispatcher.Dispatch(MainEvent.OpenView, TopViewHelper.ETopView.eHome);
                    break;
                case MainMenuView.Tasks.Collection:
                    FlurryUtil.LogEvent("navmenu_collection_page_click");
                    this.dispatcher.Dispatch(MainEvent.OpenView, TopViewHelper.ETopView.eCollection);
                    break;
                case MainMenuView.Tasks.Friend:
                    this.dispatcher.Dispatch(MainEvent.OpenView, TopViewHelper.ETopView.eFriend);
                    break;
                case MainMenuView.Tasks.Pet:
                    FlurryUtil.LogEvent("navmenu_store_page_click");
                    this.dispatcher.Dispatch(MainEvent.OpenView, TopViewHelper.ETopView.ePetPage);
                    break;
            }
            this.MainMenuView.Close(() => dispatcher.Dispatch(MainEvent.CloseMainMenu, TopViewHelper.ETopView.eMainMenu));
        }

    }
}