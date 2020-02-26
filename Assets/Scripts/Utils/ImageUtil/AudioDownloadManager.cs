using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gululu;
using System.IO;
using UnityEngine.UI;
using Hank;
using System;

namespace Gululu.Util
{
    public class AudioDownloadManager : DownloadManagerBase, IAudioDownloadManager
    {
        public Action<AudioClip> OnComplete { get; set; }        

        override protected string ImageName2crc()
        {
            return "AudioName2crc";
        }
        override protected void ProcessCurrentReq()
        {
            string crc;
            if (GetMapImageName2crc().TryGetValue(GetCurrentRequirment().infoUrl.filename, out crc) && GetCurrentRequirment().infoUrl.crc == crc)
            {
                currCoroutine = wWWSupport.StartWWW(cachePath.GetWWWPersistentAssetsPath(
                    cachePath.GetAudioFullName(GetCurrentRequirment().infoUrl.filename)), AfterLoadAudio);
            }
            else
            {
                currCoroutine = wWWSupport.StartWWW(GetCurrentRequirment().infoUrl.url, AfterDownFromHttp);
            }

        }

        class SRequirment : SBaseRequirment
        {
            public AudioSource sourceTarget;
        }

        SRequirment GetCurrentRequirment()
        {
            return currentRequirment != null ? currentRequirment as SRequirment : null;
        }

        public void DownloadAudio(UrlInfo url, Action<UrlInfo, bool> downLoadResult)
        {
            PlayAudio(null, url, downLoadResult);
        }

        //仅仅用于需要及时下载的好友头像，
        public void PlayAudio(AudioSource audioSource, UrlInfo url, Action<UrlInfo, bool> downLoadResult)
        {
            if (url == null || url.filename == null || url.filename.Length == 0)
            {
                return;
            }
            if (GetCurrentRequirment() != null && audioSource != null && GetCurrentRequirment().sourceTarget == audioSource)
            {
                GuLog.Debug("<><AudioDownloadManager>StopCurrentReq");
                StopCurrentReq();
            }
            if (currCoroutine == null)
            {
                currentRequirment = new SRequirment
                {
                    sourceTarget = audioSource,
                    infoUrl = url,
                    downLoadResultCB = downLoadResult
                };
                GuLog.Debug("<><AudioDownloadManager>PlayAudio ProcessCurrentReq");
                ProcessCurrentReq();
            }
            else
            {
                bool bFind = false;
                if (audioSource != null)
                {
                    for (int i = 0; i < listRequirment.Count; ++i)
                    {
                        SRequirment requirment = listRequirment[i] as SRequirment;
                        if (requirment.sourceTarget == audioSource)
                        {
                            requirment.infoUrl = url;
                            requirment.downLoadResultCB = downLoadResult;
                            bFind = true;
                            break;
                        }
                    }
                }
                if (!bFind)
                {
                    listRequirment.Add(new SRequirment
                    {
                        sourceTarget = audioSource,
                        infoUrl = url,
                        downLoadResultCB = downLoadResult
                    });
                }
            }

        }

        void AfterDownFromHttp(WWW www)
        {
            currCoroutine = null;
            if (www.isDone && string.IsNullOrEmpty(www.error))
            {
                byte[] bytes = www.bytes;
                string fullName = cachePath.GetAudioFullName(GetCurrentRequirment().infoUrl.filename);
                cachePath.DeleteCacheFile(fullName);
                cachePath.WriteCacheFile(fullName, bytes);

                if (GetCurrentRequirment().sourceTarget != null)
                {
                    GetCurrentRequirment().sourceTarget.clip = www.GetAudioClip(false, true);
                    GetCurrentRequirment().sourceTarget.Play();
                    this.OnStartPlay();
                }

                GetMapImageName2crc().Remove(GetCurrentRequirment().infoUrl.filename);
                GetMapImageName2crc().Add(GetCurrentRequirment().infoUrl.filename, GetCurrentRequirment().infoUrl.crc);

                localDataRepository.saveObject<Dictionary<string, string>>(ImageName2crc(), GetMapImageName2crc());
                if (GetCurrentRequirment().downLoadResultCB != null)
                {
                    GetCurrentRequirment().downLoadResultCB(GetCurrentRequirment().infoUrl, true);
                }
                GuLog.Debug("<><AudioDownloadManager>download OK, save at: " + fullName);
            }
            else
            {
                if (GetCurrentRequirment().downLoadResultCB != null)
                {
                    GetCurrentRequirment().downLoadResultCB(GetCurrentRequirment().infoUrl, false);
                }
                GuLog.Warning("<><AudioDownloadManager>download fail: " + www.url);
            }
            ProcessNext();
        }

        private void AfterLoadAudio(WWW www)
        {
            currCoroutine = null;
            if (www.isDone && string.IsNullOrEmpty(www.error))
            {
                GuLog.Debug("<><AudioDownloadManager>AfterLoadAudio ok Start ");
                if (GetCurrentRequirment().sourceTarget != null)
                {
                    GetCurrentRequirment().sourceTarget.clip = www.GetAudioClip(false, true);
                    GetCurrentRequirment().sourceTarget.Play();
                    this.OnStartPlay();
                }
                if (GetCurrentRequirment().downLoadResultCB != null)
                {
                    GetCurrentRequirment().downLoadResultCB(GetCurrentRequirment().infoUrl, true);
                }
                GuLog.Debug("<><AudioDownloadManager>AfterLoadAudio ok: " + www.url);
                ProcessNext();
            }
            else
            {
                GuLog.Debug("<><AudioDownloadManager>AfterLoadAudio fail: " + www.error);
                currCoroutine = wWWSupport.StartWWW(GetCurrentRequirment().infoUrl.url, AfterDownFromHttp);
            }
        }

        private void OnStartPlay()
        {
            if (this.OnComplete != null)
                this.OnComplete(GetCurrentRequirment().sourceTarget.clip);
        }
    }
}
