using DG.Tweening;
using Gululu;
using Gululu.Config;
using Gululu.LocalData.Agents;
using Gululu.Util;
using Hank;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleManager : BaseView
{
    /************************************************属性与变量命名************************************************/
    public static RoleManager Instance { get; private set; }//????
    [Inject]
    public IRoleConfig RoleConfig { get; set; }//???????
    [Inject]
    public IPrefabRoot PrefabRoot { get; set; }//???????
    [Inject]
    public IPlayerDataManager PlayerDataManager { get; set; }
    [Inject]
    public IAccessoryConfig AccessoryConfig { get; set; }
    [Inject]
    public IAssetBundleUtils AssetBundleUtils { get; set; }
    [Inject]
    public ILocalPetInfoAgent LocalPetInfoAgent { get; set; }
    [Inject]
    public ILocalPetAccessoryAgent LocalPetAccessoryAgent { get; set; }
    public RolePlayer CurrentRole
    {
        get
        {
            if (this.role == null)
                this.role = GameObject.FindObjectOfType<RolePlayer>();
            return this.role;
        }
    }
    public List<MatePlayer> Mates
    {
        get
        {
            if (this.mates == null || this.mates.Count == 0)
            {
                MatePlayer[] matePlayers = GameObject.FindObjectsOfType<MatePlayer>();
                if (matePlayers != null && matePlayers.Length > 0)
                {
                    this.mates = new List<MatePlayer>();
                    this.mates.AddRange(matePlayers);
                }
            }
            return this.mates;
        }
    }
    [SerializeField]
    private RolePlayer role;//?????
    [SerializeField]
    private GameObject roleRoot;
    [SerializeField]
    private Transform mainView;
    [SerializeField]
    private float scaleRate = 0.16f;//???????
    private string lastPet;
    private bool destroying;
    private List<MatePlayer> mates;
    /************************************************Unity方法与事件***********************************************/
    protected override void Awake()
    {
        Instance = this;
    }
    protected override void OnDestroy()
    {
        this.destroying = true;
    }
    /************************************************自 定 义 方 法************************************************/
    public void Show(string roleName, bool hideMate = false)
    {
        this.lastPet = this.LocalPetInfoAgent.getCurrentPet();
        this.CurrentRole.RefreshRole(roleName);
        if (hideMate) this.HideMates();
        this.Show();
    }
    public void Show()
    {
        this.transform.localScale = Vector3.one;
        this.roleRoot.transform.SetParent(this.transform);
        this.roleRoot.transform.localPosition = Vector3.zero;
        this.SetChildNode(this.roleRoot.transform, new List<string>() { "moon", "sun" }, false);
        this.transform.localScale *= this.scaleRate;
        this.CurrentRole.ShowParticleEffect();
    }
    public void Hide(System.Action callback = null)
    {
        if (this.CurrentRole != null)
            this.CurrentRole.HideParticleEffect();
        this.transform.localScale = Vector3.zero;
        if (callback != null)
            callback();
    }
    public void ShowMates()
    {
        if (this.Mates != null)
            this.Mates.ForEach(t => { if (t != null) t.Show(); });
    }
    public void HideMates()
    {
        if (this.Mates != null)
            this.Mates.ForEach(t => { if (t != null) t.Hide(); });
    }
    public void Reset(bool changeRole)
    {
        if (this.destroying) return;
        this.transform.localScale = Vector3.one;
        if (changeRole) this.CurrentRole.DestoryModel();
        this.CurrentRole.RefreshRole(changeRole && !string.IsNullOrEmpty(this.lastPet) ? this.lastPet : this.LocalPetInfoAgent.getCurrentPet(), true);
        this.roleRoot.transform.SetParent(this.mainView);
        this.roleRoot.transform.localPosition = Vector3.zero;
        this.SetChildNode(this.roleRoot.transform, new List<string>() { "moon", "sun" }, true);
        this.lastPet = this.LocalPetInfoAgent.getCurrentPet();
        this.ResetMates();
        this.UnloadAllUnusedAssets();
    }
    public void ResetAccessoriesAndMates()
    {
        if (this.CurrentRole != null)
        {
            this.CurrentRole.TakeOffAllAccessories();
            this.CurrentRole.SetupPetAccessories();
        }
        this.ResetMates();
    }
    private void ResetMates()
    {
        if (this.Mates != null)
        {
            this.Mates.ForEach(t =>
            {
                if (t != null)
                    t.RefreshRole();
            });
        }
    }
    private void SetChildNode(Transform roleTransform, List<string> nodes, bool visible)
    {
        if (this.destroying) return;

        if (nodes.Exists(t => roleTransform.name.ToLower().StartsWith(t.ToLower())))
        {
            roleTransform.gameObject.SetActive(visible);
            return;
        }

        for (int i = 0; i < roleTransform.childCount; i++)
        {
            this.SetChildNode(roleTransform.GetChild(i), nodes, visible);
        }
    }
    private void UnloadAllUnusedAssets()
    {
        #region 回收小宠物资源
        List<RoleInfo> roleInfos = this.RoleConfig.GetAllRoleInfo();
        for (int i = 0; i < roleInfos.Count; i++)
        {
            if (!roleInfos[i].IsLocal && roleInfos[i].Name != this.PlayerDataManager.CurrentPet)
                this.AssetBundleUtils.UnloadAsset(roleInfos[i].AB);
        }
        #endregion

        #region 回收配饰资源
        List<string> usedAssetBundle = new List<string>();
        List<Accessory> accessories = this.AccessoryConfig.GetAllAccessories();
        List<string> wornAccessories = this.LocalPetAccessoryAgent.GetPetAccessories(this.PlayerDataManager.CurrentPet);
        for (int i = 0; i < accessories.Count; i++)
        {//统计正在使用的AssetBundle包
            if (wornAccessories.Contains(accessories[i].ID.ToString()))
                usedAssetBundle.Add(accessories[i].AB);
        }

        for (int i = 0; i < accessories.Count; i++)
        {//卸载未使用的AssetBundle包
            if (!usedAssetBundle.Contains(accessories[i].AB))
                this.AssetBundleUtils.UnloadAsset(accessories[i].AB);
        }
        #endregion

    }
}
