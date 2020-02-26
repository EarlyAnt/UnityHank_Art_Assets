using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gululu.Util;
using System.Security;
using Mono.Xml;
using System;

namespace Gululu.Config
{
    public class ActionConfig : AbsConfigBase, IActionConfig
    {

        public class ActionInfo
        {
            public string type;
            public List<AniAndSound> animationList = new List<AniAndSound>();
        }
        public class ActionList
        {
            public Dictionary<string,ActionInfo> actionInfos = new Dictionary<string, ActionInfo>();

        }
        public Dictionary<string, ActionList> actionLists = new Dictionary<string, ActionList>();

        public void LoadActionConfig()
        {
            if (m_LoadOk)
                return;
            actionLists.Clear();
            ConfigLoader.Instance.LoadConfig("Config/ActionConfig.xml", Parser);

//             StartCoroutine(_LoadActionConfig());
        }

        private void Parser(WWW www)
        {
            m_LoadOk = true;

            if (www.isDone && (www.error == null || www.error.Length == 0))
            {
                SecurityParser xmlDoc = new SecurityParser();
                GuLog.Debug("ActionConfigPath" + www.text);

                xmlDoc.LoadXml(www.text);
                ArrayList worldXmlList = xmlDoc.ToXml().Children;

                foreach (SecurityElement xeWorld in worldXmlList)
                {
                    if (xeWorld.Tag == "Actions")
                    {
                        ArrayList roleXmlList = xeWorld.Children;
                        foreach (SecurityElement xePet in roleXmlList)
                        {
                            if (xePet.Tag == "Pet")
                            {
                                string petType = xePet.Attribute("Type");
                                ActionList actionList = new ActionList();
                                ArrayList actionXmlList = xePet.Children;
                                foreach (SecurityElement xeAction in actionXmlList)
                                {
                                    if (xeAction.Tag == "Action")
                                    {
                                        ActionInfo roleInfo = new ActionInfo();
                                        roleInfo.type = xeAction.Attribute("Type");

                                        ArrayList aniXmlList = xeAction.Children;
                                        foreach (SecurityElement xeAni in aniXmlList)
                                        {
                                            if (xeAni.Tag == "Animation")
                                            {
                                                AniAndSound aniInfo = new AniAndSound();
                                                aniInfo.AniName = xeAni.Attribute("AniName");
                                                aniInfo.SoundType = xeAni.Attribute("SoundType");
                                                roleInfo.animationList.Add(aniInfo);
                                            }
                                        }
                                        actionList.actionInfos.Add(roleInfo.type, roleInfo);
                                    }
                                }
                                actionLists.Add(petType, actionList);
                            }
                        }
                    }
                }

            }
            else
            {
                GuLog.Debug("ActionConfigPath WWW::" + www.error);
            }
        }

//         IEnumerator _LoadActionConfig()
//         {
//             string strImagePath = ResourceLoadBase.GetResourceBasePath() + "Config/ActionConfig.xml";
//             GuLog.Debug("ActionConfigPath" + strImagePath);
//             WWW www = new WWW(strImagePath);
//             yield return www;
// 
//             Parser(www);
//         }
        public AniAndSound GetAniAndSound(string petType,string actionType)
        {
            ActionList list = null;
            if(actionLists.TryGetValue(petType,out list))
            {
                ActionInfo info = null;
                if(list.actionInfos.TryGetValue(actionType,out info))
                {
                    if(info.animationList.Count > 0)
                    {
                        int random = UnityEngine.Random.Range(0, info.animationList.Count);
                        return info.animationList[random];
                    }
                }
            }
            return null;
        }
    }
}
