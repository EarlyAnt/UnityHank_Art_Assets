using Cup.Utils.android;
using Cup.Utils.Wifi;
using DG.Tweening;
using Gululu;
using Gululu.Config;
using Gululu.LocalData.Agents;
using Gululu.net;
using Gululu.Util;
using Hank.Api;
using Hank.MainScene;
using strange.extensions.signal.impl;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Hank.PetDress
{
    public class PetDressView : BaseView
    {
        /************************************************属性与变量命名************************************************/
        #region 注入接口
        [Inject]
        public IAccessoryConfig AccessoryConfig { get; set; }
        [Inject]
        public ILocalPetInfoAgent LocalPetInfoAgent { get; set; }
        [Inject]
        public IPetAccessoryUtils PetAccessoryUtils { get; set; }
        [Inject]
        public IAssetBundleUtils AssetBundleUtils { get; set; }
        [Inject]
        public IPrefabRoot PrefabRoot { get; set; }
        [Inject]
        public ILocalChildInfoAgent LocalChildInfoAgent { get; set; }
        [Inject]
        public IPlayerDataManager PlayerDataManager { get; set; }
        [Inject]
        public IAccessoryDataManager AccessoryDataManager { get; set; }
        [Inject]
        public IFundDataManager FundDataManager { get; set; }
        [Inject]
        public IJsonUtils JsonUtils { get; set; }
        [Inject]
        public II18NConfig I18NConfig { get; set; }
        [Inject]
        public ILocalPetAccessoryAgent LocalPetAccessoryAgent { get; set; }
        [Inject]
        public IDrinkWaterUIPresentManager UIState { get; set; }
        [Inject]
        public UpdateExpAndCoinSignal UpdateExpAndCoinSignal { get; set; }
        #endregion
        #region 页面UI组件
        [SerializeField]
        private RectTransform Background;//背景图
        [SerializeField]
        private StateBar stateBar;//页面上方的等级和金币的控制条
        [SerializeField]
        private AccessoryTypes accessoryTypes;//配饰种类管理
        [SerializeField]
        private Accessories accessories;//配饰管理
        [SerializeField]
        private GameObject loadingObject;//正在加载页面
        [SerializeField]
        private QRCodeView qrCodeView;//二维码页面
        [SerializeField]
        private LevelLockView levelLockView;//等级提示页面
        [SerializeField]
        private SuitLockView suitLockView;//配饰不适用提示页面
        [SerializeField]
        private GoalLockView goalLockView;//今日饮水目标提示页面
        [SerializeField]
        private CoinLockView coinLockView;//金币不足提示页面
        [SerializeField]
        private WifiLockView wifiLockView;//无网络连接提示页面
        [SerializeField]
        private OpenLockView openLockView;//开放倒计时提示页面
        #endregion
        #region 其他变量
        private List<Accessory> accessoryList = new List<Accessory>();
        private AudioSource audioPlayer;
        private bool isBuying;
        public bool IsBuying
        {
            get
            {
                return this.isBuying;
            }

            set
            {
                this.isBuying = value;
                this.loadingObject.SetActive(value);
            }
        }
        public Signal<bool> CloseViewSignal = new Signal<bool>();
        #endregion
        /************************************************Unity方法与事件***********************************************/
        protected override void Start()
        {
            base.Start();
            this.Initialize();
        }
        protected override void OnDestroy()
        {
            if (RoleManager.Instance != null && RoleManager.Instance.CurrentRole != null)
            {
                RoleManager.Instance.CurrentRole.ResetStatus();
                RoleManager.Instance.CurrentRole.StatusCheckingEnable = true;
            }
            this.accessoryTypes.AccessoryTypeChange -= this.OnAccessoryTypeChanged;
            this.accessories.AccessoryChange -= this.OnAccessoryChanged;
            this.SetCanvasLayer(true);
            base.OnDestroy();
        }
#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                this.FundDataManager.AddCoin(this.FundDataManager.GetCoinId(), 1000);
                this.UpdateExpAndCoinSignal.Dispatch();
            }
            else if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                this.FundDataManager.ExpendCoin(this.FundDataManager.GetCoinId(), 1000);
                this.UpdateExpAndCoinSignal.Dispatch();
            }
        }
#endif
        /************************************************自 定 义 方 法************************************************/
        //初始化
        private void Initialize()
        {
            if (this.audioPlayer == null)
                this.audioPlayer = this.gameObject.AddComponent<AudioSource>();

            this.FadeIn();
            this.SetCanvasLayer(false);
            RoleManager.Instance.CurrentRole.StatusCheckingEnable = false;
            RoleManager.Instance.CurrentRole.Hide();
            RoleManager.Instance.CurrentRole.Show();
            RoleManager.Instance.Show();
            if (RoleManager.Instance.Mates != null)
                RoleManager.Instance.Mates.ForEach(t => t.HideParticleEffect());
            this.accessories.Initialize(this.PrefabRoot, this.PlayerDataManager, this.AccessoryDataManager, this.LocalPetAccessoryAgent, this.I18NConfig);
            this.accessoryTypes.AccessoryTypeChange += this.OnAccessoryTypeChanged;
            this.accessories.AccessoryChange += this.OnAccessoryChanged;
            this.accessoryTypes.Initialize(this.PrefabRoot);
            this.qrCodeView.ViewClosed += this.OnQRCodeiewClosed;
            this.stateBar.Initialize();
            this.InvokeRepeating("GetPaidItems", 0, 1f);
            this.LocalPetAccessoryAgent.LoadData();
            SoundPlayer.GetInstance().StopAllSoundExceptMusic();
            SoundPlayer.GetInstance().PlayMusic("mall_bgm");
            this.PlaySound("mall_welcome");
            this.SyncData();
        }
        //入场动画
        private void FadeIn()
        {
            this.accessoryTypes.RootObject.alpha = 0;
            this.accessories.RootObject.alpha = 0;
            this.stateBar.GetComponent<CanvasGroup>().alpha = 0;
            this.Background.DOLocalMoveY(0, 1f).onComplete = () =>
            {
                this.stateBar.GetComponent<CanvasGroup>().DOFade(1, 0.5f).onComplete = () =>
                {
                    this.accessoryTypes.RootObject.DOFade(1, 0.5f).onComplete = () =>
                    {
                        this.accessories.RootObject.DOFade(1, 0.5f);
                    };
                };
            };
        }        
        //关闭页面
        public override void Close(System.Action callback)
        {
            if (this.suitLockView.IsOpen)
            {//关闭配饰不适用提示页面
                this.suitLockView.Close();
            }
            else if (this.levelLockView.IsOpen)
            {//关闭等级不够提示页面
                this.levelLockView.Close();
            }
            else if (this.goalLockView.IsOpen)
            {//关闭饮水目标未达标提示页面
                this.goalLockView.Close();
            }
            else if (this.coinLockView.IsOpen)
            {//关闭金币不足提示页面
                this.coinLockView.Close();
            }
            else if (this.wifiLockView.IsOpen)
            {//关闭无网络连接提示页面
                this.wifiLockView.Close();
            }
            else if (this.qrCodeView.IsOpen)
            {//关闭二维码页面
                this.CancelInvoke();
                this.qrCodeView.Close();
            }
            else
            {//关闭换装页
                base.Close(callback);
            }
        }
        //设置页面快照
        public override void SetViewSnapShot()
        {
            ViewSnapShot.Instance.SetViewSnapShot(this.Background.GetComponent<Image>().sprite, ViewSnapShot.Directions.Down);
        }
        //上一个商店(配饰种类)
        public void PreAccessoryType()
        {
            if (!this.SubViewOpened())
            {
                this.accessoryTypes.Previous();
            }
        }
        //下一个商店(配饰种类)
        public void NextAccessoryType()
        {
            if (!this.SubViewOpened())
            {
                this.accessoryTypes.Next();
            }
        }
        //上一个配饰
        public void PreAccessory()
        {
            if (!this.SubViewOpened())
            {
                this.accessories.Previous();
            }
        }
        //下一个配饰
        public void NextAccessory()
        {
            if (!this.SubViewOpened())
            {
                this.accessories.Next();
            }
        }
        //点击确认
        public void Confirm()
        {
            if (this.accessoryTypes.BackButtonSelected)
            {//退出换装页
                this.CloseViewSignal.Dispatch(true);
            }
            else if (!this.accessories.HasItems)
            {//如果当前商店中无配饰
                return;
            }
            else if (!this.accessories.CurrentButton.Accessory.Suitable(this.PlayerDataManager.CurrentPet) && !this.suitLockView.IsOpen)
            {//显示配饰不适用提示页面
                this.audioPlayer.Stop();
                this.suitLockView.Open(this.accessories.CurrentButton.Accessory);
                FlurryUtil.LogEventWithParam("mall_scene_lock_suitable", "accessory_name", this.accessories.CurrentButton.Accessory.Name);
            }
            else if (this.suitLockView.IsOpen)
            {//关闭配饰不适用提示页面
                this.suitLockView.Close();
            }
            else if (this.accessories.CurrentButton.Paid && !this.accessories.CurrentButton.Worn)
            {//已拥有：穿上当前配饰
                this.PutOn(this.accessories.CurrentButton.Accessory);
                this.PlaySound("mall_put_on_accessory");
                FlurryUtil.LogEventWithParam("mall_scene_change", "accessory_name", this.accessories.CurrentButton.Accessory.Name);
            }
            else if (this.accessories.CurrentButton.Paid && this.accessories.CurrentButton.Worn)
            {//已拥有：脱掉当前配饰
                this.TakeOff(this.accessories.CurrentButton.Accessory);
                this.PlaySound(this.accessories.CurrentButton.Accessory.CanWear ? "mall_take_off_accessory" : "mall_spirit_out");
            }
            else if (this.accessories.CurrentButton.Accessory.Level > this.PlayerDataManager.playerLevel && !this.levelLockView.IsOpen)
            {//显示等级不够提示页面
                this.audioPlayer.Stop();
                this.levelLockView.Open(this.accessories.CurrentButton.Accessory);
                FlurryUtil.LogEventWithParam("mall_scene_lock_level", "accessory_name", this.accessories.CurrentButton.Accessory.Name);
            }
            else if (this.levelLockView.IsOpen)
            {//关闭等级不够提示页面
                this.levelLockView.Close();
            }
            else if (!this.accessories.CurrentButton.Accessory.Opened && !this.openLockView.IsOpen)
            {//显示未开放提示页面
                this.audioPlayer.Stop();
                this.openLockView.Open(this.accessories.CurrentButton.Accessory);
            }
            else if (this.openLockView.IsOpen)
            {//关闭未开放提示页面
                this.openLockView.Close();
            }
            else if (this.goalLockView.IsOpen)
            {//关闭饮水目标未达标提示页面
                this.goalLockView.Close();
            }
            else if (this.coinLockView.IsOpen)
            {//关闭金币不足提示页面
                this.coinLockView.Close();
            }
            else if (this.wifiLockView.IsOpen)
            {//关闭无网络连接提示页面
                this.wifiLockView.Close();
            }
            else if (this.qrCodeView.IsOpen)
            {//关闭二维码页面
                this.qrCodeView.Close();
            }
            else if (!this.accessories.CurrentButton.Paid && this.accessories.CurrentButton.Accessory.Coin)
            {//金币购买的配饰：钱够→扣钱→配饰进包裹→记录穿戴数据→配饰图标状态更新
                if (this.FundDataManager.GetItemCount(this.FundDataManager.GetCoinId()) >= this.accessories.CurrentButton.Accessory.Price)
                {
                    this.accessories.CurrentButton.Paid = true;
                    this.AccessoryDataManager.AddItem(this.accessories.CurrentButton.Accessory.ID);//配饰添加到包裹
                    this.FundDataManager.ExpendCoin(this.FundDataManager.GetCoinId(), (int)this.accessories.CurrentButton.Accessory.Price);//扣钱                    
                    SoundPlayer.GetInstance().StopAllSoundExceptMusic();
                    SoundPlayer.GetInstance().PlaySoundInChannal("mall_purchase_qr_complete", this.audioPlayer, 0.2f);
                    this.PutOn(this.accessories.CurrentButton.Accessory);
                    ExpUI.Instance.Show(this.accessories.CurrentButton.Accessory.Exp, 0, -25, 25, GameObject.FindGameObjectWithTag("ExpUILayer").transform);
                    this.PlayerDataManager.exp += this.accessories.CurrentButton.Accessory.Exp;
                    this.UIState.PushNewState(UIStateEnum.eExpIncrease, this.PlayerDataManager.GetExpPercent());
                    this.UpdateExpAndCoinSignal.Dispatch();
                    FlurryUtil.LogEventWithParam("mall_scene_purchase", "accessory_name", this.accessories.CurrentButton.Accessory.Name);
                }
                else if (!this.coinLockView.IsOpen)
                {//显示金币不足提示页面
                    this.audioPlayer.Stop();
                    this.coinLockView.Open();
                }
            }
            else if (!this.accessories.CurrentButton.Paid && this.accessories.CurrentButton.Accessory.Cash && !this.qrCodeView.IsOpen)
            {//货币购买的配饰：向服务器发送请求，获取二维码并打开页面显示
                if (this.PlayerDataManager.currIntake < this.PlayerDataManager.GetDailyGoal() && !this.goalLockView.IsOpen)
                {//显示饮水目标未达标提示页面
                    this.audioPlayer.Stop();
                    this.goalLockView.Open(this.accessories.CurrentButton.Accessory);
                    FlurryUtil.LogEventWithParam("mall_scene_lock_goal", "accessory_name", this.accessories.CurrentButton.Accessory.Name);
                }
#if !UNITY_EDITOR && UNITY_ANDROID
                else if (!NativeWifiManager.Instance.IsConnected() && !this.wifiLockView.IsOpen)
                {//显示无网络连接提示页面
                    this.audioPlayer.Stop();
                    this.wifiLockView.Open();
                }
#else
                else if (!this.wifiLockView.IsOpen)
                {//显示无网络连接提示页面
                    this.audioPlayer.Stop();
                    this.wifiLockView.Open();
                }
#endif
                else
                {
#if !UNITY_EDITOR && UNITY_ANDROID
                    this.IsBuying = true;
                    PetAccessoryOrder order = new PetAccessoryOrder()
                    {
                        items = new List<PetAccessory>() { new PetAccessory() { sn = this.accessories.CurrentButton.Accessory.ID.ToString(), count = 1 } },
                        remark = string.Format("CupSn:{0}, ChildSn:{1}", CupBuild.getCupSn(), this.LocalChildInfoAgent.getChildSN())
                    };

                    this.PetAccessoryUtils.BuyPetAccessories(order, (result) =>
                    {
                        this.IsBuying = false;
                        OrderReceipt orderReceipt = this.JsonUtils.String2Json<OrderReceipt>(result.info);
                        if (orderReceipt != null && !string.IsNullOrEmpty(orderReceipt.status) && orderReceipt.status.ToUpper() == "OK")
                        {
                            this.audioPlayer.Stop();
                            this.qrCodeView.Open(orderReceipt);
                            FlurryUtil.LogEventWithParam("mall_scene_qr", "accessory_name", this.accessories.CurrentButton.Accessory.Name);
                        }
                        else
                            Debug.LogError("<><PetPageView.Confirm>orderReceipt is null");
                    },
                    (error) =>
                    {
                        this.IsBuying = false;
                        Debug.LogErrorFormat("<><PetPageView.Confirm>BuyPetAccessories, error: {0}", error.info);
                    });
#else
                    this.audioPlayer.Stop();
                    this.qrCodeView.Open(null);
#endif
                }
            }
        }
        //穿上配饰
        private void PutOn(Accessory accessory, bool saveData = true)
        {
            if (accessory == null || !accessory.Suitable(this.PlayerDataManager.CurrentPet))
                return;

            if (RoleManager.Instance.CurrentRole != null && accessory.CanWear)
            {
                if (accessory.Exclusive(Gululu.Config.AccessoryTypes.Suit))//当前配饰与套装互斥，脱掉套装
                    RoleManager.Instance.CurrentRole.TakeOffPetAccessoryByTypes(new List<int> { (int)Gululu.Config.AccessoryTypes.Suit });
                else if (accessory.AccessoryType == Gululu.Config.AccessoryTypes.Suit)//当前配饰是套装，脱掉与套装互斥的配饰
                    RoleManager.Instance.CurrentRole.TakeOffPetAccessoryByTypes(new List<int> { (int)Gululu.Config.AccessoryTypes.Head, (int)Gululu.Config.AccessoryTypes.Back, (int)Gululu.Config.AccessoryTypes.Suit });

                RoleManager.Instance.CurrentRole.SetupPetAccessory(accessory);
                RoleManager.Instance.CurrentRole.HideParticleEffect();
            }
            else if ((accessory.AccessoryType == Gululu.Config.AccessoryTypes.SpiritUp ||
                      accessory.AccessoryType == Gululu.Config.AccessoryTypes.SpiritDown) &&
                     RoleManager.Instance.Mates != null)
            {
                RoleManager.Instance.Mates.ForEach(t =>
                {
                    if (t != null && t.IsNative(accessory.Region))
                    {
                        t.RefreshRole(accessory);
                        t.HideParticleEffect();
                    }
                });
            }
            this.CheckBuffer(accessory);

            if (saveData)
            {
                this.SaveData(true, accessory);
                if (accessory.CanWear)
                    RoleManager.Instance.CurrentRole.Play("display01");
            }
        }
        //脱下配饰
        private void TakeOff(Accessory accessory, bool saveData = true)
        {
            if (accessory == null) return;

            if (RoleManager.Instance.CurrentRole != null && accessory.CanWear)
            {
                RoleManager.Instance.CurrentRole.TakeOffPetAccessory(accessory);
            }
            else if ((accessory.AccessoryType == Gululu.Config.AccessoryTypes.SpiritUp ||
                      accessory.AccessoryType == Gululu.Config.AccessoryTypes.SpiritDown) &&
                     RoleManager.Instance.Mates != null)
            {
                RoleManager.Instance.Mates.ForEach(t =>
                {
                    if (t != null && t.IsNative(accessory.Region))
                        t.DestoryModel();
                });
            }

            if (saveData)
                this.SaveData(false, accessory);
        }
        //保存数据
        private void SaveData(bool toWear, Accessory accessory)
        {
            List<string> petAccessories = this.LocalPetAccessoryAgent.GetPetAccessories(this.PlayerDataManager.CurrentPet);//记录小宠物身上的配饰的数据
            if (toWear && !petAccessories.Contains(accessory.ID.ToString()))
            {
                petAccessories.Add(accessory.ID.ToString());//记录当前穿上的配饰数据
                List<Accessory> accessories = this.LocalPetAccessoryAgent.GetAccessoriesByType(this.PlayerDataManager.CurrentPet, accessory.Type);
                if (accessories != null && accessories.Count > 0)
                {//检查并移除掉与当前配饰相同类型的配饰
                    for (int i = 0; i < accessories.Count; i++)
                    {
                        if (accessories[i].ID != accessory.ID && petAccessories.Contains(accessories[i].ID.ToString()))
                            petAccessories.Remove(accessories[i].ID.ToString());
                    }
                }

                accessories = this.LocalPetAccessoryAgent.GetAllPetAccessories(this.PlayerDataManager.CurrentPet);
                if (accessories != null && accessories.Count > 0)
                {//要检查并移除掉与当前配饰互斥的配饰
                    for (int i = 0; i < accessories.Count; i++)
                    {
                        if (accessories[i].ID != accessory.ID && accessory.Exclusive(accessories[i].AccessoryType))
                            petAccessories.Remove(accessories[i].ID.ToString());
                    }
                }
            }
            else if (!toWear)
            {
                petAccessories.Remove(accessory.ID.ToString());
            }
            this.LocalPetAccessoryAgent.SavePetAccessories(this.PlayerDataManager.CurrentPet, petAccessories);
            this.PlayerDataManager.TrySendPlayerData2Server();
            this.accessories.PutOnOrTakeOff(toWear);//刷新配饰图标
        }
        //轮询所有已购买的配饰
        private void GetPaidItems()
        {
            if (this.qrCodeView.IsOpen) return;
#if !UNITY_EDITOR && UNITY_ANDROID
            this.PetAccessoryUtils.GetPaidItems((getPaidItemsResponse) =>
            {
                if (getPaidItemsResponse != null)
                {
                    this.accessories.SetPaid(getPaidItemsResponse.acc_list.ToList(), (accessory) =>
                    {
                        if (accessory != null)
                        {
                            Debug.LogFormat("<><PetDressView.GetPaidItems>Got a new accessory: {0}_{1}", accessory.ID, accessory.Name);
                            this.PlayerDataManager.exp += accessory.Exp;
                            this.UIState.PushNewState(UIStateEnum.eExpIncrease, this.PlayerDataManager.GetExpPercent());
                            this.UpdateExpAndCoinSignal.Dispatch();
                            FlurryUtil.LogEventWithParam("mall_scene_purchase_rmb", "accessory_name", accessory.Name);
                        }
                    });
                }
            },
            (errorText) =>
            {
                Debug.LogErrorFormat("<><PetDressView.GetPaidItems>Error: {0}", errorText);
            });
#endif
        }
        //检查是否有子页面打开
        private bool SubViewOpened()
        {
            return this.levelLockView.IsOpen || this.suitLockView.IsOpen || this.goalLockView.IsOpen || this.coinLockView.IsOpen || this.wifiLockView.IsOpen || this.qrCodeView.IsOpen;
        }
        //当二维码页面关闭时
        private void OnQRCodeiewClosed(ViewResult viewResult)
        {
            if (viewResult == ViewResult.OK)
            {
                this.accessories.CurrentButton.Paid = true;
                this.AccessoryDataManager.AddItem(this.accessories.CurrentButton.Accessory.ID);//配饰添加到包裹
                SoundPlayer.GetInstance().StopAllSoundExceptMusic();
                SoundPlayer.GetInstance().PlaySoundInChannal("mall_purchase_qr_complete", this.audioPlayer, 0.2f);
                this.PutOn(this.accessories.CurrentButton.Accessory);
                ExpUI.Instance.Show(this.accessories.CurrentButton.Accessory.Exp, 0, -25, 25, GameObject.FindGameObjectWithTag("ExpUILayer").transform);
                this.PlayerDataManager.exp += this.accessories.CurrentButton.Accessory.Exp;
                this.UIState.PushNewState(UIStateEnum.eExpIncrease, this.PlayerDataManager.GetExpPercent());
                this.UpdateExpAndCoinSignal.Dispatch();
                FlurryUtil.LogEventWithParam("mall_scene_purchase_rmb", "accessory_name", this.accessories.CurrentButton.Accessory.Name);
            }
        }
        //当配置类别变化时
        private void OnAccessoryTypeChanged(int typeIndex, List<int> accessoryTypes)
        {
            this.accessories.LoadData(typeIndex, this.AccessoryConfig.GetAccessoriesByTypes(accessoryTypes));
            FlurryUtil.LogEvent("mall_scene_switch_type_event");
        }
        //当配饰变化时
        private void OnAccessoryChanged(Accessory currentAccessory, Accessory lastAccessory)
        {
            RoleManager.Instance.ResetAccessoriesAndMates();
            RoleManager.Instance.CurrentRole.HideParticleEffect();
            RoleManager.Instance.Mates.ForEach(t => t.HideParticleEffect());
            //不管当前是什么配饰，都需要先脱掉与其相同类型或部位的配饰
            List<Accessory> accessories = this.LocalPetAccessoryAgent.GetAccessoriesByType(this.PlayerDataManager.CurrentPet, currentAccessory.Type);
            if (accessories != null && accessories.Count > 0)
                foreach (var accessory in accessories.ToArray())
                    this.TakeOff(accessory, false);

            if (currentAccessory.Suitable(this.PlayerDataManager.CurrentPet))
                this.PutOn(currentAccessory, false);
            FlurryUtil.LogEvent("mall_scene_switch_item_event");
        }
        //停止所有音频
        protected override void StopAllSound(bool exit)
        {
            if (exit) SoundPlayer.GetInstance().StopMusic();
            SoundPlayer.GetInstance().StopSound("mall_welcome");
            SoundPlayer.GetInstance().StopSound("mall_change_item");
            SoundPlayer.GetInstance().StopSound("mall_put_on_accessory");
            SoundPlayer.GetInstance().StopSound("mall_take_off_accessory");
            SoundPlayer.GetInstance().StopSound("mall_spirit_in");
            SoundPlayer.GetInstance().StopSound("mall_spirit_out");
            if (this.audioPlayer != null) this.audioPlayer.Stop();
        }
        //同步数据(上传之前没有上传的配饰和小精灵)
        private void SyncData()
        {
            List<Accessory> accessories = this.LocalPetAccessoryAgent.GetAllPetAccessories(this.PlayerDataManager.CurrentPet);
            if (accessories != null && accessories.Count > 0)
            {//检查并移除掉与当前配饰相同类型的配饰
                for (int i = 0; i < accessories.Count; i++)
                {
                    if (!this.AccessoryDataManager.HasItem(accessories[i].ID))
                        this.AccessoryDataManager.AddItem(accessories[i].ID);
                }
            }
        }
        //检查缓存(保证配饰和小宠物的缓存数量不超过10个)
        private void CheckBuffer(Accessory accessory)
        {
            if (this.accessoryList.Count >= 10)
            {
                List<string> usedAssetBundle = new List<string>();
                List<Accessory> accessories = this.AccessoryConfig.GetAllAccessories();
                List<string> wornAccessories = this.LocalPetAccessoryAgent.GetPetAccessories(this.PlayerDataManager.CurrentPet);
                for (int i = 0; i < accessories.Count; i++)
                {//统计正在使用的AssetBundle包
                    if (wornAccessories.Contains(accessories[i].ID.ToString()))
                        usedAssetBundle.Add(accessories[i].AB);
                }
                if (!usedAssetBundle.Contains(accessory.AB))
                    usedAssetBundle.Add(accessory.AB);//新穿上的配饰的AssetBundle包也要算上

                Accessory[] accessoryArray = accessoryList.ToArray();
                for (int i = 0; i < accessoryArray.Length; i++)
                {
                    if (!usedAssetBundle.Contains(accessoryArray[i].Name))
                    {
                        this.AssetBundleUtils.UnloadAsset(accessoryArray[i].AB);
                        this.accessoryList.Remove(accessoryArray[i]);
                        Debug.LogFormat("<><PetDressView.CheckBuffer>Remove one accessory, {0}, {1}", accessoryArray[i].AB, accessoryArray[i].Prefab);
                        break;
                    }
                }
            }

            if (!this.accessoryList.Contains(accessory))
            {
                this.accessoryList.Add(accessory);
                Debug.LogFormat("<><PetDressView.CheckBuffer>Add new accessory, {0}, {1}", accessory.AB, accessory.Prefab);
            }
        }
    }

    /// <summary>
    /// 商店类型
    /// </summary>
    public static class AccessoryStores
    {
        public const int ACCESSORY = 1;
        public const int SUIT = 2;
        public const int SPIRIT = 3;
    }

    /// <summary>
    /// 配饰种类(也是商店)
    /// </summary>
    [System.Serializable]
    public class AccessoryTypes
    {
        public CanvasGroup RootObject;
        [SerializeField]
        private DockView dockView;//导航按钮动效
        [SerializeField]
        private AccessoryTypeButton buttonPrefab;//按钮预制体
        [SerializeField]
        private Sprite normalBackImage;//常态返回图片
        [SerializeField]
        private Sprite largeBackImage;//选中返回图片
        [SerializeField]
        private List<AccessoryTypeButton> buttons;//场景按钮
        private PageCounter pageCounter;//页码组件
        private IPrefabRoot prefabRoot;
        private List<int> stores = new List<int>() { 1, 2, 3 };//商店1-帽子，翅膀，两侧；商店2-套装；商店3-左上角精灵，左下角精灵
        private Dictionary<int, string> typeIcons = new Dictionary<int, string>();
        private static int lastIndex;
        public bool BackButtonSelected//是否选中返回按钮
        {
            get
            {
                if (this.buttons != null && this.pageCounter != null && this.buttons.Count > 0 &&
                    this.pageCounter.ItemIndex + 1 == this.buttons.Count)
                    return true;
                else
                    return false;
            }
        }
        public AccessoryTypeButton CurrentButton//当前选中的配置种类按钮
        {
            get
            {
                if (this.buttons != null && this.pageCounter != null && this.buttons.Count > 0 &&
                    this.pageCounter.ItemIndex + 1 < this.buttons.Count)
                    return this.buttons[this.pageCounter.ItemIndex];
                else
                    return null;
            }
        }
        public System.Action<int, List<int>> AccessoryTypeChange { get; set; }
        //初始化类型数据
        public void Initialize(IPrefabRoot prefabRoot)
        {
            this.typeIcons = new Dictionary<int, string>();
            this.typeIcons.Add(1, "acc");
            this.typeIcons.Add(2, "suit");
            this.typeIcons.Add(3, "elf");

            this.prefabRoot = prefabRoot;
            this.LoadData();
            this.ChangeButton();
        }
        //上一个配饰种类
        public void Previous()
        {
            this.pageCounter.PreItem();
            this.ChangeButton();
        }
        //下一个配饰种类
        public void Next()
        {
            this.pageCounter.NextItem();
            this.ChangeButton();
        }
        //加载数据
        private void LoadData()
        {
            this.dockView.ClearAllChild();
            this.buttons = new List<AccessoryTypeButton>();
            this.pageCounter = new PageCounter();
            for (int i = 0; i < this.stores.Count; i++)
            {
                AccessoryTypeButton typeButton = GameObject.Instantiate(this.buttonPrefab, this.dockView.transform);
                typeButton.name = string.Format("Scene_{0}", this.stores[i]);
                typeButton.Initialize(this.stores[i],
                                       prefabRoot.GetObjectNoInstantiate<Sprite>("Texture/PetDress/Types", string.Format("{0}_small", this.typeIcons[this.stores[i]])),
                                       prefabRoot.GetObjectNoInstantiate<Sprite>("Texture/PetDress/Types", string.Format("{0}_big", this.typeIcons[this.stores[i]])));

                this.buttons.Add(typeButton);
                this.dockView.AddChild(typeButton.GetComponent<RectTransform>());
            }

            AccessoryTypeButton backButton = GameObject.Instantiate(this.buttonPrefab, this.dockView.transform);
            backButton.name = "Back";
            backButton.Initialize(-1, this.normalBackImage, this.largeBackImage);
            this.buttons.Add(backButton);
            this.dockView.AddChild(backButton.GetComponent<RectTransform>());
            this.dockView.UpdateItemPosition();
            this.pageCounter.Reset(this.buttons.Count, 6);
            this.pageCounter.Locate(lastIndex);
        }
        //切换场景
        private void ChangeButton()
        {
            if (!this.BackButtonSelected)
                lastIndex = this.pageCounter.ItemIndex;

            for (int i = 0; i < this.buttons.Count; i++)
            {
                this.buttons[i].SetStatus(i == this.pageCounter.ItemIndex);
            }
            this.dockView.Select(this.pageCounter.ItemIndex);

            List<int> types = new List<int> { 0 };
            if (!this.BackButtonSelected && this.CurrentButton != null)
            {
                switch (this.CurrentButton.AccessoryType)
                {
                    case AccessoryStores.ACCESSORY:
                        types = new List<int> { 1, 2, 3 };
                        break;
                    case AccessoryStores.SUIT:
                        types = new List<int> { 4 };
                        break;
                    case AccessoryStores.SPIRIT:
                        types = new List<int> { 5, 6 };
                        break;
                }
            }
            this.OnAccessoryTypeChanged(lastIndex, types);
        }
        //当配置类别变化时
        private void OnAccessoryTypeChanged(int typeIndex, List<int> accessoryTypes)
        {
            if (this.AccessoryTypeChange != null)
                this.AccessoryTypeChange(typeIndex, accessoryTypes);
        }
    }

    /// <summary>
    /// 配饰
    /// </summary>
    [System.Serializable]
    public class Accessories
    {
        public CanvasGroup RootObject;
        [SerializeField]
        private BubbleView bubbleView;
        [SerializeField]
        private AccessoryButton buttonPrefab;//按钮预制体
        [SerializeField]
        private List<AccessoryButton> buttons;
        private PageCounter pageCounter;//页码组件
        private IPrefabRoot prefabRoot;
        private IPlayerDataManager playerDataManager;
        private IAccessoryDataManager accessoryDataManager;
        private ILocalPetAccessoryAgent localPetAccessoryAgent;
        private II18NConfig i18nConfig;
        private int typeIndex;
        private AccessoryButton lastButton;
        private static Dictionary<int, int> lastIndex = new Dictionary<int, int>();
        public AccessoryButton CurrentButton//当前选中的场景按钮
        {
            get
            {
                if (this.buttons != null && this.pageCounter != null && this.buttons.Count > 0 &&
                    this.pageCounter.ItemIndex < this.buttons.Count)
                    return this.buttons[this.pageCounter.ItemIndex];
                else
                    return null;
            }
        }
        public bool HasItems//当前商店中是否有配饰
        {
            get { return this.buttons != null && this.buttons.Count > 0; }
        }
        public System.Action<Accessory, Accessory> AccessoryChange { get; set; }
        //初始化类型数据
        public void Initialize(IPrefabRoot prefabRoot, IPlayerDataManager playerDataManager, IAccessoryDataManager accessoryDataManager, ILocalPetAccessoryAgent localPetAccessoryAgent, II18NConfig i18nConfig)
        {
            this.prefabRoot = prefabRoot;
            this.playerDataManager = playerDataManager;
            this.accessoryDataManager = accessoryDataManager;
            this.localPetAccessoryAgent = localPetAccessoryAgent;
            this.i18nConfig = i18nConfig;
        }
        //上一个场景
        public void Previous()
        {
            if (this.pageCounter.ItemIndex > 0)
            {
                this.lastButton = this.CurrentButton;
                this.pageCounter.PreItem();
                this.ChangeButton();
                SoundPlayer.GetInstance().PlaySoundType("mall_change_item");
            }
        }
        //下一个场景
        public void Next()
        {
            if (this.pageCounter.ItemIndex + 1 < this.pageCounter.ItemCount)
            {
                this.lastButton = this.CurrentButton;
                this.pageCounter.NextItem();
                this.ChangeButton();
                SoundPlayer.GetInstance().PlaySoundType("mall_change_item");
            }
        }
        //设置已购买配饰的图标的状态
        public void SetPaid(List<string> paidItemList, System.Action<Accessory> paidCallback)
        {
            if (paidItemList != null && paidItemList.Count > 0 && this.buttons != null && this.buttons.Count > 0)
                this.buttons.ForEach(t =>
                {
                    if (t.Accessory.Cash && paidItemList.Contains(t.Accessory.ID.ToString()))
                    {
                        t.Paid = true;//设置已购买状态
                        if (!this.accessoryDataManager.HasItem(t.Accessory.ID))
                        {//如果此配饰已购买，且本地数据中未添加，则记录数据
                            this.accessoryDataManager.AddItem(t.Accessory.ID);
                            if (paidCallback != null) paidCallback(t.Accessory);
                        }
                    }
                });
        }
        //加载数据
        public void LoadData(int typeIndex, List<Accessory> accessories)
        {
            this.typeIndex = typeIndex;
            this.bubbleView.ClearAllChild();
            this.buttons = new List<AccessoryButton>();
            List<string> petAccessories = this.localPetAccessoryAgent.GetPetAccessories(this.playerDataManager.CurrentPet);
            if (accessories != null && accessories.Count > 0)
            {
                this.pageCounter = new PageCounter();
                for (int i = 0; i < accessories.Count; i++)
                {
                    if (accessories[i].Cash && this.playerDataManager.GetLanguage().ToUpper() != LanConfig.Languages.ChineseSimplified)
                        continue;

                    AccessoryButton accessoryButton = GameObject.Instantiate(this.buttonPrefab, this.bubbleView.transform);
                    accessoryButton.name = string.Format("Accessory_{0}", accessories[i].ID);
                    accessoryButton.Initialize(accessories[i],
                                               prefabRoot.GetObjectNoInstantiate<Sprite>("Texture/PetDress/Accessories", string.Format("{0}_small", accessories[i].Icon)),
                                               prefabRoot.GetObjectNoInstantiate<Sprite>("Texture/PetDress/Accessories", string.Format("{0}_big", accessories[i].Icon)),
                                               this.playerDataManager.playerLevel < accessories[i].Level, !accessories[i].Suitable(this.playerDataManager.CurrentPet),
                                               this.accessoryDataManager.HasItem(accessories[i].ID), petAccessories.Contains(accessories[i].ID.ToString()));
                    accessoryButton.SetText(this.i18nConfig);
                    this.buttons.Add(accessoryButton);
                    this.bubbleView.AddChild(accessoryButton.GetComponent<RectTransform>());
                }
                this.bubbleView.UpdateItemPosition();
                this.pageCounter.Reset(this.buttons.Count, this.buttons.Count);
                this.pageCounter.Locate(lastIndex.ContainsKey(typeIndex) ? lastIndex[typeIndex] : 0);
                this.ChangeButton(true);
            }
        }
        //穿上或脱下某个配饰
        public void PutOnOrTakeOff(bool toWear)
        {
            if (this.CurrentButton != null && this.CurrentButton.Accessory != null)
            {
                if (toWear && this.buttons != null)
                {
                    this.buttons.ForEach(t =>
                    {
                        if (t.Accessory != null && t.Accessory.AccessoryType == this.CurrentButton.Accessory.AccessoryType)//穿上某个配饰的同时，仅脱下同类型的其他配饰
                            t.Worn = false;
                    });
                    this.CurrentButton.Worn = true;
                }
                else if (!toWear)
                {
                    this.CurrentButton.Worn = false;
                }
            }
        }
        //切换场景
        private void ChangeButton(bool immediately = false)
        {
            if (lastIndex.ContainsKey(this.typeIndex))
                lastIndex[this.typeIndex] = this.pageCounter.ItemIndex;
            else
                lastIndex.Add(this.typeIndex, this.pageCounter.ItemIndex);

            for (int i = 0; i < this.buttons.Count; i++)
            {
                this.buttons[i].SetStatus(i == this.pageCounter.ItemIndex, immediately);
            }
            this.bubbleView.Select(this.pageCounter.ItemIndex, immediately);

            this.OnAccessoryChanged();
        }
        //当配饰变化时
        private void OnAccessoryChanged()
        {
            if (this.AccessoryChange != null)
                this.AccessoryChange(this.CurrentButton.Accessory, this.lastButton != null ? this.lastButton.Accessory : null);
        }
    }
}
