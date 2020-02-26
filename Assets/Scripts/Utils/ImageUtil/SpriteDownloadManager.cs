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
    public class SpriteDownloadManager : DownloadManagerBase,ISpriteDownloadManager
    {

        [Inject]
        public IIconManager iconManager { set; get; }

        //Dictionary<string, Sprite> mapImageName2Sprite = new Dictionary<string, Sprite>();

        override protected string ImageName2crc()
        {
            return "ImageName2crc";
        }
        override protected void ProcessCurrentReq()
        {
            string crc;
            if (GetMapImageName2crc().TryGetValue(GetCurrentRequirment().infoUrl.filename, out crc) && GetCurrentRequirment().infoUrl.crc == crc)
            {
                //Sprite outSprite = null;
                //if (mapImageName2Sprite.TryGetValue(GetCurrentRequirment().infoUrl.filename, out outSprite))
                //{
                //    if (GetCurrentRequirment().imageTarget != null)
                //    {
                //        GetCurrentRequirment().imageTarget.sprite = outSprite;
                //    }
                //    if (GetCurrentRequirment().downLoadResultCB != null)
                //    {
                //        GetCurrentRequirment().downLoadResultCB(GetCurrentRequirment().infoUrl, true);
                //    }
                //    ProcessNext();
                //}
                //else
                {
                    DefaultSprite();
                    currCoroutine = wWWSupport.StartWWW(cachePath.GetWWWPersistentAssetsPath(
                        cachePath.GetSpriteFullName(GetCurrentRequirment().infoUrl.filename)), AfterLoadSprite);
                }
            }
            else
            {
                DefaultSprite();
                currCoroutine = wWWSupport.StartWWW(GetCurrentRequirment().infoUrl.url, AfterDownFromHttp);
            }

        }

        class SRequirment : SBaseRequirment
        {
            public Image imageTarget;
        }

        SRequirment GetCurrentRequirment()
        {
            return currentRequirment != null ? currentRequirment as SRequirment : null;
        }

        public void DownloadSprite(UrlInfo url, Action<UrlInfo, bool> downLoadResult)
        {
            FillImageWithUrl(null, url, downLoadResult);
        }

        public void FillImageWithUrl(Image target, UrlInfo url, Action<UrlInfo, bool> downLoadResult)
        {
            if (url == null || url.filename == null || url.filename.Length == 0)
            {
                if(downLoadResult != null)
                {
                    downLoadResult(url, false);
                }
                return;
            }
            if(GetCurrentRequirment() != null && target != null && GetCurrentRequirment().imageTarget == target)
            {
                StopCurrentReq();
            }
            if (currCoroutine == null)
            {
                currentRequirment = new SRequirment
                {
                    imageTarget = target,
                    infoUrl = url,
                    downLoadResultCB = downLoadResult
                };
                ProcessCurrentReq();
            }
            else
            {
                bool bFind = false;
                if(target != null)
                {
                    for (int i = 0; i < listRequirment.Count; ++i)
                    {
                        SRequirment requirment = listRequirment[i] as SRequirment;
                        if (requirment.imageTarget == target)
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
                    listRequirment.Add(new SRequirment {
                        imageTarget = target,
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
                string fullName = cachePath.GetSpriteFullName(GetCurrentRequirment().infoUrl.filename);
                cachePath.DeleteCacheFile(fullName);
                cachePath.WriteCacheFile(fullName,bytes);

                Sprite loadedSprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), Vector2.zero);
                //mapImageName2Sprite.Remove(GetCurrentRequirment().infoUrl.filename);
                //mapImageName2Sprite.Add(GetCurrentRequirment().infoUrl.filename, loadedSprite);

                if (GetCurrentRequirment().imageTarget != null)
                {
                    GetCurrentRequirment().imageTarget.sprite = loadedSprite;
                }

                GetMapImageName2crc().Remove(GetCurrentRequirment().infoUrl.filename);
                GetMapImageName2crc().Add(GetCurrentRequirment().infoUrl.filename, GetCurrentRequirment().infoUrl.crc);

                localDataRepository.saveObject<Dictionary<string, string>>(ImageName2crc(), GetMapImageName2crc());
                if(GetCurrentRequirment().downLoadResultCB != null)
                {
                    GetCurrentRequirment().downLoadResultCB(GetCurrentRequirment().infoUrl,true);
                }
                GuLog.Warning("<><SpriteDownloadManager>download OK, save at:" + fullName);
            }
            else
            {
                DefaultSprite();
                if (GetCurrentRequirment().downLoadResultCB != null)
                {
                    GetCurrentRequirment().downLoadResultCB(GetCurrentRequirment().infoUrl, false);
                }
                GuLog.Warning("<><SpriteDownloadManager>download fail" + www.url);
            }
            ProcessNext();
        }

        private void AfterLoadSprite(WWW www)
        {
            currCoroutine = null;
            if (www.isDone && string.IsNullOrEmpty(www.error))
            {
                if (GetCurrentRequirment().imageTarget != null)
                {
                    Sprite loadedSprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), Vector2.zero);
                    //mapImageName2Sprite.Add(GetCurrentRequirment().infoUrl.filename, loadedSprite);
                    GetCurrentRequirment().imageTarget.sprite = loadedSprite;
                }
                if (GetCurrentRequirment().downLoadResultCB != null)
                {
                    GetCurrentRequirment().downLoadResultCB(GetCurrentRequirment().infoUrl,true);
                }
                ProcessNext();
            }
            else
            {
                currCoroutine = wWWSupport.StartWWW(GetCurrentRequirment().infoUrl.url, AfterDownFromHttp);
            }
        }

        private void DefaultSprite()
        {
            if (GetCurrentRequirment().imageTarget != null)
                GetCurrentRequirment().imageTarget.sprite = iconManager.GetIconByName("defaultSprite");
        }


    }
}
