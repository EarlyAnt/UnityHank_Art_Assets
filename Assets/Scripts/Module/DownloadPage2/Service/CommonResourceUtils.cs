using Assets.Script.Global;
using Cup.Utils.Wifi;
using Gululu.Config;
using Gululu.net;
using Hank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class CommonResourceUtils : ICommonResourceUtils
{
    [Inject]
    public II18NConfig I18NConfig { get; set; }//多语言配置管理
    [Inject]
    public ISoundConfig SoundConfig { get; set; }//Sound配置
    [Inject]
    public IRoleConfig RoleConfig { get; set; }//Role配置
    [Inject]
    public IAccessoryConfig AccessoryConfig { get; set; }//Accessory配置
    [Inject]
    public IResourceUtils ResourceUtils { get; set; }//资源工具
    [Inject]
    public IHotUpdateUtils HotUpdateUtils { get; set; }
    private List<AssetFile> updateFileList = null;

    /// <summary>
    /// 检查资源文件
    /// </summary>
    /// <param name="callback">检查结果回调(true-有文件需要下载，false-无文件需要下载或有错误)</param>
    public void CheckResources(Action<bool> callback)
    {
        /*
         * 第1步 从服务器获取资源文件更新数据
         * 第2步 将获取到的N条更新数据整合(distinct)，得到文件列表A
         * 第3步 将本地文件列表B与文件列表A合并，得到最终的文件列表C，并保存下来
         */

#if (UNITY_ANDROID) && (!UNITY_EDITOR)
        this.HotUpdateUtils.GetUpdateInfo((updateInfos) =>
        {
            if (updateInfos != null && !string.IsNullOrEmpty(updateInfos.status) && updateInfos.status.ToUpper() == "OK")
            {
                Debug.LogFormat("<><CommonResourceUtils.CheckResources>updateInfo status: {0}, url: {1}", updateInfos.status, updateInfos.url_prefix);
                this.ResourceUtils.SetServerPath(updateInfos.url_prefix);
                List<AssetFile> updateFileList = this.GetUpdateFiles(updateInfos);
                if (updateFileList != null) this.updateFileList = updateFileList;
                if (callback != null) callback(this.updateFileList != null && this.updateFileList.Count > 0);
            }
            else
            {
                Debug.LogError("<><CommonResourceUtils.CheckResources>Response data 'updateInfos' is null");
            }
        }, (errorText) =>
        {

        });
#elif (UNITY_EDITOR)
        List<AssetFile> updateFileList = this.GetUpdateFiles(null);
        if (updateFileList != null) this.updateFileList = updateFileList;
        if (callback != null) callback(this.updateFileList != null && this.updateFileList.Count > 0);
#endif
    }
    /// <summary>
    /// 获取需要更新的文件列表
    /// </summary>
    /// <returns></returns>
    public List<AssetFile> GetUpdateFileList()
    {
        if ((this.updateFileList == null || this.updateFileList.Count == 0) && 
            NativeWifiManager.Instance.IsConnected())
        {
            this.CheckResources(null);
            Debug.Log("<><CommonResourceUtils.GetUpdateFileList>Check resources again");
        }
        return this.updateFileList;
    }
    /// <summary>
    /// 获取根据本地各个配置文件统计出来的所有资源文件的列表
    /// </summary>
    /// <returns></returns>
    public List<AssetFile> GetAllFileList()
    {
        List<AssetFile> assetFiles = new List<AssetFile>();
        #region 添加AssetBundleManifest文件
        assetFiles.Add(new AssetFile() { Path = "Android", FullPath = "Model/Android", MD5 = "" });
        #endregion
        #region 统计AudioClip文件
        List<SoundInfo> allSoundInfos = this.SoundConfig.GetAllSoundInfos();
        if (allSoundInfos != null && allSoundInfos.Count > 0)
        {
            foreach (SoundInfo soundInfo in allSoundInfos)
            {
                if (soundInfo.Storage.ToUpper() == "REMOTE")
                {
                    foreach (SoundClip soundClip in soundInfo.sounds)
                    {
                        string path = this.I18NConfig.GetAudioPath(soundClip.SoundPath);
                        assetFiles.Add(new AssetFile() { Path = path, FullPath = string.Format("Audio/{0}/{1}.mp3", soundInfo.Module, path), MD5 = "" });
                    }
                }
            }
        }
        #endregion
        #region 统计小宠物的AssetBundle文件
        foreach (RoleInfo roleInfo in this.RoleConfig.GetAllRoleInfo())
        {
            if (!roleInfo.IsLocal)
                assetFiles.Add(new AssetFile() { Path = roleInfo.AB, FullPath = string.Format("Model/{0}.ab", roleInfo.AB), MD5 = "" });
        }
        #endregion
        #region 统计配饰和小精灵等的AssetBundle文件
        assetFiles.Add(new AssetFile() { Path = "Android", FullPath = "Model/Android", MD5 = "" });
        foreach (Accessory accessory in this.AccessoryConfig.GetAllAccessories())
        {
            assetFiles.Add(new AssetFile() { Path = accessory.AB, FullPath = string.Format("Model/{0}.ab", accessory.AB), MD5 = "" });
        }
        #endregion
        return assetFiles;
    }
    /// <summary>
    /// 整合服务器更新数据与本地更新数据，并返回计算后的确实需要更新的文件列表
    /// </summary>
    /// <param name="updateInfos">服务器更新数据</param>
    /// <returns></returns>
    private List<AssetFile> GetUpdateFiles(UpdateInfos updateInfos)
    {
        try
        {
            #region 本地数据校验
            UpdateRecord updateRecord = this.HotUpdateUtils.ReadUpdateRecord();
            if (updateRecord == null) return null;//如果获取不到本地更新记录，包括默认记录，返回null
            Debug.LogFormat("<><CommonResourceUtils.GetUpdateFiles>Step1->UpdateRecord, apkVersionCode: {0}, resVersionCode: {1}, timeStamp: {2}",
                            updateRecord.ApkVersionCode, updateRecord.ResVersionCode, updateRecord.TimeStamp);
            #endregion
            #region 服务器数据筛选
            List<UpdateFileInfo> severFileList = new List<UpdateFileInfo>();//结果数据集
#if (UNITY_ANDROID) && (!UNITY_EDITOR)
            if (updateInfos != null && updateInfos.res_list != null)
            {
                List<UpdateInfo> updateInfoList = updateInfos.res_list.OrderByDescending(t => t.res_ver_code).ToList();//按照资源版本号倒叙排列历次更新数据
                foreach (var updateInfo in updateInfoList)
                {//遍历历次更新数据，并且只关注apk版本号和资源版本号符合要求的数据
                    Debug.LogFormat("<><CommonResourceUtils.GetUpdateFiles>0 - updateInfo: {0}", updateInfo);
                    if (updateInfo.apk_ver_code > AppInfo.Instance.VersionCode ||
                        updateInfo.res_ver_code <= updateRecord.ResVersionCode)
                        continue;

                    foreach (var updateFileInfo in updateInfo.update_files)
                    {//遍历某个更新数据中的文件列表，因为已经做过倒叙排列处理，所以这里只记录结果数据集中没有的文件信息
                        if (!severFileList.Exists(t => t.file == this.GetShortName(updateFileInfo.file)))
                        {
                            Debug.LogFormat("<><CommonResourceUtils.GetUpdateFiles>1 - server file name: {0}", updateFileInfo.file);
                            updateFileInfo.file = this.GetShortName(updateFileInfo.file);
                            severFileList.Add(updateFileInfo);
                            Debug.LogFormat("<><CommonResourceUtils.GetUpdateFiles>2 - formatted file name: {0}", updateFileInfo.file);
                        }
                    }
                    Debug.LogFormat("<><CommonResourceUtils.GetUpdateFiles>3 - version file count: {0}", updateInfo.update_files.Count);
                }
            }
            Debug.LogFormat("<><CommonResourceUtils.GetUpdateFiles>Step2->SeverFileList, count: {0}", severFileList.Count);
#endif
            #endregion
            #region 服务器数据与本地数据整合
            List<AssetFile> updateFileList = this.GetAllFileList();
            if (severFileList.Count > 0)
            {
                foreach (var serverFile in severFileList)
                {
                    AssetFile localFile = updateFileList.Find(t => t.Path == serverFile.file);
                    if (localFile == null)
                    {
                        updateFileList.Add(new AssetFile()
                        {
                            Path = serverFile.file,
                            FullPath = serverFile.file + serverFile.md5,
                            MD5 = serverFile.md5,
                            HasNewVersion = true
                        });
                    }
                    else
                    {
                        localFile.FullPath = serverFile.file + serverFile.md5;
                        localFile.MD5 = serverFile.md5;
                        localFile.HasNewVersion = true;
                    }
                }
            }
            Debug.LogFormat("<><CommonResourceUtils.GetUpdateFiles>Step3->UpdateFileList, count: {0}", updateFileList.Count);
            #endregion
            #region 检查本地是否存在文件
            DateTime beginTime = DateTime.Now;
            if (updateFileList != null && updateFileList.Count > 0)
            {
                AssetFile[] assetFiles = updateFileList.ToArray();
                foreach (var assetFile in assetFiles)
                {
                    if (!assetFile.HasNewVersion && this.ResourceUtils.FileExisted(assetFile.FullPath))
                        updateFileList.Remove(assetFile);//服务器上没有新版本文件，且本地已经有此文件，则不需要下载(从列表中移除)
                }
            }
            Debug.LogFormat("<><CommonResourceUtils.GetUpdateFiles>Step4->Check file exist, left file count: {0}", updateFileList.Count);
            Debug.LogFormat("<><CommonResourceUtils.GetUpdateFiles>Check file exist, take time: {0} seconds", (DateTime.Now - beginTime).TotalSeconds);
            return updateFileList;
            #endregion
        }
        catch (Exception ex)
        {
            Debug.LogErrorFormat("<><CommonResourceUtils.GetUpdateFiles>Error: {0}", ex.Message);
            return null;
        }
    }
    /// <summary>
    /// 获取短文件名(去掉服务器文件名的根目录)
    /// </summary>
    /// <param name="serverFileName"></param>
    /// <returns></returns>
    private string GetShortName(string serverFileName)
    {
        return !string.IsNullOrEmpty(serverFileName) ? serverFileName.Replace(this.ResourceUtils.SERVER_ROOT_PATH, "") : "";
    }
}

