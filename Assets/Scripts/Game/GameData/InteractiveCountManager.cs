using BestHTTP;
using Cup.Utils.android;
using Gululu;
using Gululu.Config;
using Gululu.LocalData.Agents;
using Gululu.net;
using Gululu.Util;
using Hank.Api;
using Hank.MainScene;
using LitJson;
using strange.extensions.mediation.impl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hank
{
    public class InteractiveCountManager : IInteractiveCountManager
    {
        [Inject]
        public ILocalDataManager localDataRepository { get; set; }
        [Inject]
        public ILocalChildInfoAgent mLocalChildInfoAgent { get; set; }

        public const string interactivesKey = "InteractiveCount";


        Dictionary<InteractiveType, int> interactivesInfo = new Dictionary<InteractiveType, int>();

        struct InteractiveInfo
        {
            public InteractiveType interactiveId;
            public int interactiveCount;
        }

        [PostConstruct]
        public void PostConstruct()
        {
            LoadInteractiveCount();
        }


        public int GetInteractiveCount(InteractiveType interactiveId)
        {
            int interactiveCount;
            if (interactivesInfo.TryGetValue(interactiveId, out interactiveCount))
            {
                return interactiveCount;
            }
            return 0;

        }


        public void SetInteractive(InteractiveType interactiveId,int interactiveCount)
        {
            int count = 0;
            if (interactivesInfo.TryGetValue(interactiveId, out count))
            {
                interactivesInfo.Remove(interactiveId);
            }

            if(interactiveCount > 0)
            {
                interactivesInfo.Add(interactiveId, interactiveCount);
            }
            SaveInteractiveCount();
        }

        public void AddInteractive(InteractiveType interactiveId, int interactiveCount = 1)
        {
            int count = 0;
            if (interactivesInfo.TryGetValue(interactiveId, out count))
            {
                interactivesInfo.Remove(interactiveId);
            }
            int newCount = count;
            if(int.MaxValue - interactiveCount > count)
            {
                newCount = interactiveCount + count;
            }
            interactivesInfo.Add(interactiveId, newCount);
            SaveInteractiveCount();
        }


        private List<InteractiveInfo> Convert2List()
        {
            List<InteractiveInfo> listInfos = new List<InteractiveInfo>();
            foreach (var pair in interactivesInfo)
            {
                listInfos.Add(new InteractiveInfo
                {
                    interactiveId = pair.Key,
                    interactiveCount = pair.Value
                });
            }
            return listInfos;
        }

        public void SaveInteractiveCount()
        {
            List<InteractiveInfo> listInfos = Convert2List();

            localDataRepository.saveSpecialObject<List<InteractiveInfo>>(interactivesKey, listInfos);

        }
        public void LoadInteractiveCount()
        {
            interactivesInfo.Clear();
            List<InteractiveInfo> listInfos = null;
            try
            {
                listInfos = localDataRepository.getSpecialObject<List<InteractiveInfo>>(interactivesKey);
            }
            catch (Exception e)
            {
                GuLog.Info("<><InteractiveCountManager>LoadInteractiveCount error:" + e.ToString());
            }
            if (listInfos != null)
            {
                foreach (var info in listInfos)
                {
                    interactivesInfo.Add(info.interactiveId, info.interactiveCount);
                    GuLog.Debug("<><InteractiveCountManager>interactiveId:" + info.interactiveId + "    Count:" + info.interactiveCount.ToString());
                }
            }
        }

        public bool HasInteractive(InteractiveType interactiveId, int interactiveCount = 1)
        {
            int count = 0;
            if (interactivesInfo.TryGetValue(interactiveId, out count))
            {
                return count >= interactiveCount;
            }
            return false;
        }

        public void Reset()
        {
            interactivesInfo.Clear();
            SaveInteractiveCount();
        }
        public bool IsNeedNoviceGuide()
        {
            return false;
            return (!HasInteractive(InteractiveType.NoviceGuide1))
                || (!HasInteractive(InteractiveType.NoviceGuide3))
                || (!HasInteractive(InteractiveType.NoviceGuide4))
                || (!HasInteractive(InteractiveType.NoviceGuide5));
        }
    }
}

