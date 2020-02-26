using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gululu;
using System.IO;
using UnityEngine.UI;
using System;

namespace Gululu.Util
{

    public interface IAudioDownloadManager
    {
        Action<AudioClip> OnComplete { get; set; }

        //仅仅用于需要及时下载的好友头像
        void DeleteCacheFile(string fileName);
        void DownloadAudio(UrlInfo url, Action<UrlInfo, bool> downLoadResult);

        void PlayAudio(AudioSource audioSource, UrlInfo url, Action<UrlInfo, bool> downLoadResult);
        void StopCurrentReq();
    }
}
