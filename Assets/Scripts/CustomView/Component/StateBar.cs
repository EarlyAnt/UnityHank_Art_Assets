using Gululu.LocalData.Agents;
using Hank;
using Hank.MainScene;
using strange.extensions.mediation.impl;
using UnityEngine;

/// <summary>
/// 状态数据页面(用户等级and金币)
/// </summary>
public class StateBar : View
{
    /************************************************属性与变量命名************************************************/
    [Inject]
    public IPlayerDataManager PlayerDataManager { get; set; }
    [Inject]
    public ILocalPetInfoAgent LocalPetInfoAgent { get; set; }
    [Inject]
    public IFundDataManager FundDataManager { get; set; }
    [Inject]
    public LevelUpSignal LevelUpSignal { get; set; }
    [Inject]
    public UpdateExpAndCoinSignal UpdateExpAndCoinSignal { get; set; }
    [SerializeField]
    private LevelBar levelBar;
    [SerializeField]
    private CoinBar coinBar;
    /************************************************Unity方法与事件***********************************************/
    protected override void OnDestroy()
    {
        this.LevelUpSignal.RemoveListener(this.SetLevel);
        this.UpdateExpAndCoinSignal.RemoveListener(this.SetExpAndCoin);
    }
    /************************************************自 定 义 方 法************************************************/
    //初始化
    public void Initialize()
    {
        this.LevelUpSignal.AddListener(this.SetLevel);
        this.UpdateExpAndCoinSignal.AddListener(this.SetExpAndCoin);
        this.SetLevelFrame();
        this.SetLevel();
        this.SetExpAndCoin();
    }
    //设置等级边框
    public void SetLevelFrame()
    {
        this.levelBar.SetFrame(this.LocalPetInfoAgent.getCurrentPet());
    }
    //设置等级
    private void SetLevel()
    {
        this.levelBar.SetLevel(this.PlayerDataManager.playerLevel);
    }
    //设置经验和金币
    private void SetExpAndCoin()
    {
        this.levelBar.SetExp(this.PlayerDataManager.GetExpPercent());
        this.coinBar.SetCoin(this.FundDataManager.GetItemCount(this.FundDataManager.GetCoinId()));
    }
}