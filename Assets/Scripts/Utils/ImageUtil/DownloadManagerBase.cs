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
    
//     public delegate void DownLoadResult(UrlInfo infoUrl, bool bOk);

    public abstract class DownloadManagerBase 
    {

        [Inject]
        public ILocalDataManager localDataRepository { set; get; }
        [Inject]
        public ICachePath cachePath { set; get; }
        [Inject]
        public IWWWSupport wWWSupport { set; get; }

        abstract protected string ImageName2crc();
        abstract protected void ProcessCurrentReq();

        Dictionary<string, string> mapImageName2crc = null;
        protected Dictionary<string, string> GetMapImageName2crc()
        {
            if (mapImageName2crc == null)
            {
                mapImageName2crc = localDataRepository.getObject<Dictionary<string, string>>(ImageName2crc());
                if (mapImageName2crc == null)
                {
                    mapImageName2crc = new Dictionary<string, string>();
                }
            }
            return mapImageName2crc;
        }
        protected class SBaseRequirment
        {
            public UrlInfo infoUrl;
            public Action<UrlInfo, bool> downLoadResultCB;
        }
        protected List<SBaseRequirment> listRequirment = new List<SBaseRequirment>();
        protected SBaseRequirment currentRequirment = null;

        protected Coroutine currCoroutine = null;

        public void StopCurrentReq()
        {
            currentRequirment = null;
            wWWSupport.StopWWWCoroutine(currCoroutine);
            currCoroutine = null;
        }

        protected void ProcessNext()
        {
            currentRequirment = null;
            if (listRequirment.Count > 0)
            {
                currentRequirment = listRequirment[0];
                listRequirment.RemoveAt(0);
                ProcessCurrentReq();
            }

        }

        public void DeleteCacheFile(string fileName)
        {
            string crc;
            if (GetMapImageName2crc().TryGetValue(fileName, out crc))
            {
                GetMapImageName2crc().Remove(fileName);

                localDataRepository.saveObject<Dictionary<string, string>>(ImageName2crc(), GetMapImageName2crc());
                string fullName = cachePath.GetSpriteFullName(fileName);
                cachePath.DeleteCacheFile(fullName);
                if (File.Exists(fullName))
                {
                    File.Delete(fullName);
                }
            }
        }

    }
}
