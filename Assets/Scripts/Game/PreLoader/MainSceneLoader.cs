using Cup.Utils.android;
using Gululu;
using Gululu.Config;
using Gululu.LocalData.Agents;
using Gululu.LocalData.DataManager;
using Gululu.Util;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Hank.PreLoad
{
    public class MainSceneLoader : BaseView
    {
        /************************************************属性与变量命名************************************************/
        #region 注入接口
        [Inject]
        public ITreasureBoxConfig TreasureBoxConfig { get; set; }
        [Inject]
        public ITaskConfig TaskConifg { get; set; }
        [Inject]
        public IItemConfig ItemConifg { get; set; }
        [Inject]
        public IMissionConfig MissionConifg { get; set; }
        [Inject]
        public IRoleConfig RoleConifg { get; set; }
        [Inject]
        public ILanConfig LanConfig { get; set; }
        [Inject]
        public IFontConfig FontConfig { get; set; }
        [Inject]
        public II18NConfig I18NConfig { get; set; }
        [Inject]
        public IAccessoryConfig AccessoryConfig { get; set; }
        [Inject]
        public IAwardConfig AwardConfig { get; set; }
        [Inject]
        public IActionConfig ActionConifg { get; set; }
        [Inject]
        public ISoundConfig SoundConfig { get; set; }
        [Inject]
        public ILevelConfig LevelConifg { get; set; }
        [Inject]
        public IPropertyConfig propertyConfig { get; set; }
        [Inject]
        public IIconManager iconManager { get; set; }
        [Inject]
        public IHeartbeatManager mHeartbeatManager { get; set; }
        [Inject(name = "UploadFriendHeartbeat")]
        public IAbsDoHeartbeatAction mUploadFreindHeatbeatAction { set; get; }
        [Inject(name = "IntakeHeartbeat")]
        public IAbsDoHeartbeatAction mUploadintakeHeatbeatAction { set; get; }
        [Inject]
        public IHeartbeatActionManager mHeartbearActionManager { set; get; }
        [Inject]
        public IPlayerDataManager PlayerDataManager { get; set; }
        [Inject]
        public ILocalChildInfoAgent LocalChildInfoAgent { get; set; }
        [Inject]
        public BuglyUtil BuglyUtil { set; get; }
        #endregion
        #region 页面UI组件
        [SerializeField]
        Image imageProgress;
        [SerializeField]
        GameObject objProgress;
        [SerializeField]
        VideoPlayer videoPlayer;
        #endregion
        #region 其他变量
        private bool bMovieEnd = false;
        private int displayProgress = 0;
        private string sceneName = "";
        #endregion        
        /************************************************Unity方法与事件***********************************************/
        protected override void Start()
        {
            base.Start();

            videoPlayer.loopPointReached += (VideoPlayer source) =>
            {
                bMovieEnd = true;
            };

            BuglyUtil.Init();
            videoPlayer.Prepare();
            MissionConifg.LoadMissionConfig();
            propertyConfig.LoadPropertyConfig();
            TaskConifg.LoadTaskData();
            ItemConifg.LoadItemsConfig();
            TreasureBoxConfig.LoadTreasureBoxConfig();
            iconManager.OnlyLoadSelf();
            RoleConifg.LoadRoleConfig();
            LanConfig.LoadLanConfig();
            FontConfig.LoadFontConfig();
            I18NConfig.LoadResConfig();
            AccessoryConfig.LoadAccessoryConfig();
            AwardConfig.LoadAwardConfig();
            ActionConifg.LoadActionConfig();
            LevelConifg.LoadLevelConfig();
            SoundConfig.LoadSoundConfig();
            imageProgress.fillAmount = 0;
            GuLog.Debug("<><Preload>Load Begin!");
            StartCoroutine(StartLoading());
#if CLEAR_DATA
            LocalDataManager.getInstance().deleteAll();
#endif
            mHeartbearActionManager.addEventListener(mUploadFreindHeatbeatAction);
            mHeartbearActionManager.addEventListener(mUploadintakeHeatbeatAction);
            mHeartbeatManager.setHeartbeatListener(() =>
            {
                GuLog.Debug("Preload start do heart beat");
                mHeartbearActionManager.startHeartbeat();
            });
        }
        /************************************************自 定 义 方 法************************************************/
        //开始加载
        private IEnumerator StartLoading()
        {
            yield return this.StartCoroutine(this.LoadConfigs());
            yield return this.StartCoroutine(this.CheckStatus());
            yield return this.StartCoroutine(this.LoadScene());
        }
        //加载配置数据
        private IEnumerator LoadConfigs()
        {
            yield return new WaitUntil(() => MissionConifg.IsLoadedOK()); RefreshProgress();
            yield return new WaitUntil(() => TaskConifg.IsLoadedOK()); RefreshProgress();
            yield return new WaitUntil(() => ItemConifg.IsLoadedOK()); RefreshProgress();
            yield return new WaitUntil(() => TreasureBoxConfig.IsLoadedOK()); RefreshProgress();
            yield return new WaitUntil(() => RoleConifg.IsLoadedOK()); RefreshProgress();
            yield return new WaitUntil(() => ActionConifg.IsLoadedOK()); RefreshProgress();
            yield return new WaitUntil(() => SoundConfig.IsLoadedOK()); RefreshProgress();
            yield return new WaitUntil(() => LevelConifg.IsLoadedOK()); RefreshProgress();
            yield return new WaitUntil(() => PlayerDataManager.Init()); RefreshProgress();
            Debug.Log("<><MainSceneLoader.LoadConfigs>all configs loaded");
        }
        //检查语言状态
        private IEnumerator CheckStatus()
        {
            string currentChildSN = LocalChildInfoAgent.getChildSN();
            Debug.LogFormat("<><MainSceneLoader.CheckStatus>currentChildSN: {0}", currentChildSN);
            if (!string.IsNullOrEmpty(currentChildSN) || this.PlayerDataManager.LanguageInitialized())
            {//配对过的，或设置过语言的杯子不再显示语言设置页，直接进入主场景MainScene
                string defaultLanguage = this.PlayerDataManager.GetLanguage();
                Debug.LogFormat("<><MainSceneMediator.CheckStatus>defaultLanguage_1: {0}", defaultLanguage);
                if (!this.LanConfig.IsValid(defaultLanguage))
                    this.PlayerDataManager.SaveLanguage(Gululu.Config.LanConfig.Languages.ChineseSimplified);//设置默认语言
                Debug.LogFormat("<><MainSceneLoader.CheckStatus>defaultLanguage_2: {0}", this.PlayerDataManager.GetLanguage());
                this.sceneName = "MainScene";
            }
            else
            {
                this.sceneName = "LanguageScene";
            }
            Debug.LogFormat("<><MainSceneLoader.CheckStatus>load scene: {0}", this.sceneName);
            yield return null;
        }
        //加载场景
        private IEnumerator LoadScene()
        {
            int loadSceneProgress = 0;
            AsyncOperation op = SceneManager.LoadSceneAsync(this.sceneName);
            op.allowSceneActivation = false;
            while (op.progress < 0.9f)
            {
                loadSceneProgress = (int)(op.progress * 100);
                Debug.LogFormat("<><MainSceneLoader.LoadScene>loading......[{0}]", op.progress.ToString());
                while (displayProgress < loadSceneProgress)
                {
                    ++displayProgress;
                    imageProgress.fillAmount = displayProgress / (float)100;
                    yield return new WaitForEndOfFrame();
                }
            }

            loadSceneProgress = 100;
            while (this.displayProgress < loadSceneProgress)
            {
                GuLog.Debug("<><MainSceneLoader.LoadScene>In loading!");
                this.displayProgress += 3;
                imageProgress.fillAmount = this.displayProgress / (float)100;
                yield return new WaitForEndOfFrame();
            }
            FlurryUtil.LogEvent("Hank_Openani_View");
            GuLog.Debug("<><Preload>LoadOk!");
            this.objProgress.SetActive(false);
            this.videoPlayer.Play();
            this.bMovieEnd = false;
            yield return new WaitUntil(() => bMovieEnd);
            op.allowSceneActivation = true;
        }
        //更新进度条
        private void RefreshProgress()
        {
            this.displayProgress += 3;
            this.imageProgress.fillAmount = this.displayProgress / (float)100;
        }
        //字段检查
        public override void TestSerializeField()
        {
            Assert.IsNotNull(imageProgress);
        }

    }
}
