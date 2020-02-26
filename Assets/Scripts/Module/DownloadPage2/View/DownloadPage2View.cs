using Cup.Utils.Wifi;
using Gululu;
using Gululu.Config;
using Gululu.net;
using strange.extensions.signal.impl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Hank.DownloadPage2
{
    public class DownloadPage2View : BaseView
    {
        /************************************************属性与变量命名************************************************/
        [Inject]
        public ISleepTimeManager SleepTimeManager { get; set; }//亮屏时间管理
        [Inject]
        public II18NConfig I18NConfig { get; set; }//多语言配置管理
        [Inject]
        public ISoundConfig SoundConfig { get; set; }//声音配置管理
        [Inject]
        public IAccessoryConfig AccessoryConfig { get; set; }//配饰配置
        [Inject]
        public IResourceUtils ResourceUtils { get; set; }//资源加载工具
        [Inject]
        public ICommonResourceUtils CommonResourceUtils { get; set; }//公共资源加载工具
        [Inject]
        public IHotUpdateUtils HotUpdateUtils { get; set; }//资源文件更新工具
        [SerializeField]
        private Image progressBar;//进度条
        [SerializeField]
        private Text progress;//进度文本
        [SerializeField]
        private GameObject downloadView;//下载页物体
        [SerializeField]
        private TipView tipView;//断网提示页面
        [SerializeField]
        private Text wifiStatus;//Wifi信号强度
        private bool initialized;//是否已初始化
        private bool pause = false;//是否暂停下载
        private Vector2 originPos;//进度文本框的初始位置
        private DownloadInfo downloadInfo = new DownloadInfo();//下载信息
        public Signal<bool> StopSignal = new Signal<bool>();//停止下载信号
        /************************************************Unity方法与事件***********************************************/
        protected override void Start()
        {
            base.Start();
            this.progressBar.fillAmount = 0;
            this.originPos = this.progress.rectTransform.anchoredPosition;
            this.SleepTimeManager.AddSleepStatus(SleepTimeout.NeverSleep);
            SoundPlayer.GetInstance().StopAllSoundExceptMusic();
            this.StartCoroutine(this.Download());//开启文件下载协程
            this.InvokeRepeating("CheckWifi", 0, 5f);//每5秒钟检查一次Wifi状态            
#if (!UNITY_EDITOR)
            if (this.wifiStatus != null) this.wifiStatus.gameObject.SetActive(false);
#endif
            this.ResourceUtils.IgnoreFileNotExisted = true;
        }
        protected override void OnDestroy()
        {
            this.ResourceUtils.IgnoreFileNotExisted = false;
            this.SleepTimeManager.PopSleepStatus();
            this.CancelInvoke("CheckWifi");
            base.OnDestroy();
        }
        /************************************************自 定 义 方 法************************************************/
        //点击电源键
        public void ConfirmExit()
        {
            if (this.tipView.IsOpen)
            {//断网提示页面打开时，点击电源键退出下载页
                this.StopSignal.Dispatch(false);
            }
        }
        //下载资源文件
        private IEnumerator Download()
        {
            List<AssetFile> assetFiles = this.CommonResourceUtils.GetUpdateFileList();
            if (assetFiles == null || assetFiles.Count == 0)
            {
                Debug.Log("<><DownloadPage2View.Download>No one file need to download");
                this.StopSignal.Dispatch(false);
                yield break;
            }

            UpdateRecord updateRecord = this.HotUpdateUtils.ReadUpdateRecord();
            if (assetFiles == null || assetFiles.Count == 0)
            {
                Debug.Log("<><DownloadPage2View.Download>Native update record is null");
                this.StopSignal.Dispatch(false);
                yield break;
            }

            this.downloadInfo.CompleteCount = 0;
            this.downloadInfo.TotalCount = assetFiles.Count;
            bool error = false;
            this.InvokeRepeating("CheckOverTime", 1f, 1f);
            for (int i = 0; i < assetFiles.Count; i++)
            {//逐个下载缺失的资源文件
                Debug.LogFormat("<><DownloadPage2View.Download>File: {0}", assetFiles[i]);
                yield return new WaitUntil(() => !this.pause);
                yield return this.StartCoroutine(this.ResourceUtils.LoadAsset(assetFiles[i].FullPath, (obj) =>
                                                {
                                                    LocalFileInfo localFileInfo = updateRecord.FileList.Find(t => t.File + t.MD5 == assetFiles[i].FullPath);
                                                    if (localFileInfo != null)
                                                    {
                                                        localFileInfo.File = assetFiles[i].FullPath;
                                                        localFileInfo.MD5 = assetFiles[i].MD5;
                                                        localFileInfo.Type = assetFiles[i].GetFileType();
                                                    }
                                                    else
                                                    {
                                                        updateRecord.FileList.Add(new LocalFileInfo()
                                                        {
                                                            File = assetFiles[i].FullPath,
                                                            MD5 = assetFiles[i].MD5,
                                                            Type = assetFiles[i].GetFileType()
                                                        });
                                                    }
                                                },
                                                (failureInfo) =>
                                                {
                                                    if (failureInfo != null)//中断操作类错误才做处理
                                                    {
                                                        Debug.LogErrorFormat("<><DownloadPage2View.Download>Error: {0}\n({1})", failureInfo.Message, assetFiles[i]);
                                                        if (failureInfo.Interrupt) error = true;
                                                    }
                                                }, true));
                if (error) break;//如果下载过程中遇到错误，退出下载页

                //更新进度条及进度文字
                float percent = (float)++this.downloadInfo.CompleteCount / this.downloadInfo.TotalCount;
                percent = Mathf.Clamp01(percent);
                this.progressBar.fillAmount = percent;
                this.progress.text = string.Format("{0}/{1}", this.downloadInfo.CompleteCount, this.downloadInfo.TotalCount);
                float maxLength = this.progressBar.rectTransform.rect.width - this.progress.rectTransform.rect.width / 1.5f;
                this.progress.GetComponent<RectTransform>().anchoredPosition = this.originPos + new Vector2(maxLength * percent, 0);
            }
            System.GC.Collect();
            this.CancelInvoke("CheckOverTime");//结束下载时(无论是正常结束还是遇到错误时异常结束)停止检测下载超时
            if (error)
            {
                this.ShowNoWifiPage();
            }
            else
            {
                AssetBundle.UnloadAllAssetBundles(true);
                PlayerDataManager.ResourceDownloaded = true;
                this.HotUpdateUtils.SaveUpdateRecord(updateRecord);
                this.StopSignal.Dispatch(true);
            }
        }
        //显示无网络连接页面
        private void ShowNoWifiPage()
        {
            this.pause = true;
            this.downloadView.SetActive(false);
            this.StopAllCoroutines();
            this.tipView.Open();
            this.PlaySound("download_popup_no_network");
            this.CancelInvoke();
#if !UNITY_EDITOR
            GuLog.Debug(string.Format("<><DownloadPageView.CheckWifi>Wifi connected: {0}", NativeWifiManager.Instance.IsConnected()));
#endif
        }
        //检查Wifi状态
        private void CheckWifi()
        {
#if !UNITY_EDITOR
            this.wifiStatus.text = NativeWifiManager.Instance.IsConnected() ? string.Format("Wifi:{0}", NativeWifiManager.Instance.getRssi()) : "No wifi";
            if (!NativeWifiManager.Instance.IsConnected())
            {
                this.ShowNoWifiPage();
            }
            else if (!this.initialized)
            {
                this.initialized = true;
                this.PlaySound("popup_download");
            }
#endif
        }
        //定时检测是否超时
        private void CheckOverTime()
        {
            if (this.downloadInfo.OverTime)
            {
                AssetBundle.UnloadAllAssetBundles(true);
                System.GC.Collect();
                this.ShowNoWifiPage();
                Debug.LogFormat("<><DownloadPage2View.CheckOverTime>TotalCount: {0}, CompleteCount: {1}, Duration: {2}, LastTime: {3}",
                                this.downloadInfo.CompleteCount, this.downloadInfo.TotalCount, this.downloadInfo.Duration, this.downloadInfo.LastTime);
            }
        }
        //停止播放此页面的所有音效
        protected override void StopAllSound(bool exit)
        {
            SoundPlayer.GetInstance().StopSound("popup_download");
            SoundPlayer.GetInstance().StopSound("download_popup_no_network");
        }
    }

    public class DownloadInfo
    {
        public DateTime LastTime { get; private set; }
        private bool updateOnce = false;
        private int completeCount = 0;
        public int CompleteCount
        {
            get
            {
                return this.completeCount;
            }
            set
            {
                this.completeCount = value;
                this.updateOnce = true;
                this.LastTime = DateTime.Now;
            }
        }
        public int TotalCount { get; set; }
        public int Duration { get { return (int)(DateTime.Now - this.LastTime).TotalSeconds; } }
        public bool OverTime
        {
            get
            {
                if (this.updateOnce)
                {//如果已经更新过一次下载完成数，才检测是否超时
                    return this.Duration > 60;//下载完成数的数值超过60秒没有改变，则视为超时(比如网络非常差的时候)
                }
                else return false;//否则，不视为超时
            }
        }
    }
}
