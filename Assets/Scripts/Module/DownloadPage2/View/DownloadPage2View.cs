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
        /************************************************�������������************************************************/
        [Inject]
        public ISleepTimeManager SleepTimeManager { get; set; }//����ʱ�����
        [Inject]
        public II18NConfig I18NConfig { get; set; }//���������ù���
        [Inject]
        public ISoundConfig SoundConfig { get; set; }//�������ù���
        [Inject]
        public IAccessoryConfig AccessoryConfig { get; set; }//��������
        [Inject]
        public IResourceUtils ResourceUtils { get; set; }//��Դ���ع���
        [Inject]
        public ICommonResourceUtils CommonResourceUtils { get; set; }//������Դ���ع���
        [Inject]
        public IHotUpdateUtils HotUpdateUtils { get; set; }//��Դ�ļ����¹���
        [SerializeField]
        private Image progressBar;//������
        [SerializeField]
        private Text progress;//�����ı�
        [SerializeField]
        private GameObject downloadView;//����ҳ����
        [SerializeField]
        private TipView tipView;//������ʾҳ��
        [SerializeField]
        private Text wifiStatus;//Wifi�ź�ǿ��
        private bool initialized;//�Ƿ��ѳ�ʼ��
        private bool pause = false;//�Ƿ���ͣ����
        private Vector2 originPos;//�����ı���ĳ�ʼλ��
        private DownloadInfo downloadInfo = new DownloadInfo();//������Ϣ
        public Signal<bool> StopSignal = new Signal<bool>();//ֹͣ�����ź�
        /************************************************Unity�������¼�***********************************************/
        protected override void Start()
        {
            base.Start();
            this.progressBar.fillAmount = 0;
            this.originPos = this.progress.rectTransform.anchoredPosition;
            this.SleepTimeManager.AddSleepStatus(SleepTimeout.NeverSleep);
            SoundPlayer.GetInstance().StopAllSoundExceptMusic();
            this.StartCoroutine(this.Download());//�����ļ�����Э��
            this.InvokeRepeating("CheckWifi", 0, 5f);//ÿ5���Ӽ��һ��Wifi״̬            
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
        /************************************************�� �� �� �� ��************************************************/
        //�����Դ��
        public void ConfirmExit()
        {
            if (this.tipView.IsOpen)
            {//������ʾҳ���ʱ�������Դ���˳�����ҳ
                this.StopSignal.Dispatch(false);
            }
        }
        //������Դ�ļ�
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
            {//�������ȱʧ����Դ�ļ�
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
                                                    if (failureInfo != null)//�жϲ���������������
                                                    {
                                                        Debug.LogErrorFormat("<><DownloadPage2View.Download>Error: {0}\n({1})", failureInfo.Message, assetFiles[i]);
                                                        if (failureInfo.Interrupt) error = true;
                                                    }
                                                }, true));
                if (error) break;//������ع��������������˳�����ҳ

                //���½���������������
                float percent = (float)++this.downloadInfo.CompleteCount / this.downloadInfo.TotalCount;
                percent = Mathf.Clamp01(percent);
                this.progressBar.fillAmount = percent;
                this.progress.text = string.Format("{0}/{1}", this.downloadInfo.CompleteCount, this.downloadInfo.TotalCount);
                float maxLength = this.progressBar.rectTransform.rect.width - this.progress.rectTransform.rect.width / 1.5f;
                this.progress.GetComponent<RectTransform>().anchoredPosition = this.originPos + new Vector2(maxLength * percent, 0);
            }
            System.GC.Collect();
            this.CancelInvoke("CheckOverTime");//��������ʱ(��������������������������ʱ�쳣����)ֹͣ������س�ʱ
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
        //��ʾ����������ҳ��
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
        //���Wifi״̬
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
        //��ʱ����Ƿ�ʱ
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
        //ֹͣ���Ŵ�ҳ���������Ч
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
                {//����Ѿ����¹�һ��������������ż���Ƿ�ʱ
                    return this.Duration > 60;//�������������ֵ����60��û�иı䣬����Ϊ��ʱ(��������ǳ����ʱ��)
                }
                else return false;//���򣬲���Ϊ��ʱ
            }
        }
    }
}
