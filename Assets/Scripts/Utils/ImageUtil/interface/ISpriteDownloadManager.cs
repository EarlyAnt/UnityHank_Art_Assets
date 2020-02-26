using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gululu;
using System.IO;
using UnityEngine.UI;
using System;

namespace Gululu.Util
{

    public interface ISpriteDownloadManager
    {

        //仅仅用于需要及时下载的好友头像，
        void FillImageWithUrl(Image target, UrlInfo url, Action<UrlInfo, bool> downLoadResult);
        void DeleteCacheFile(string fileName);
        void DownloadSprite(UrlInfo url, Action<UrlInfo, bool> downLoadResult);

        void StopCurrentReq();
    }
}
