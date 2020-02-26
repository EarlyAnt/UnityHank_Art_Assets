using Mono.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using UnityEngine;

namespace Gululu.Config
{
    public class RoleConfig : AbsConfigBase, IRoleConfig
    {
        private Dictionary<string, RoleInfo> roleInfos = new Dictionary<string, RoleInfo>();

        public void LoadRoleConfig()
        {
            if (m_LoadOk)
                return;
            roleInfos.Clear();
            ConfigLoader.Instance.LoadConfig("Config/RoleConfig.xml", Parser);
            //            StartCoroutine(_LoadRoleConfig());
        }

        private void Parser(WWW www)
        {
            m_LoadOk = true;
            GuLog.Debug("RoleConfigPath WWW::" + www.error);

            if (www.isDone && (www.error == null || www.error.Length == 0))
            {
                SecurityParser xmlDoc = new SecurityParser();
                GuLog.Debug("RoleConfigPath" + www.text);

                xmlDoc.LoadXml(www.text);
                ArrayList worldXmlList = xmlDoc.ToXml().Children;

                foreach (SecurityElement xeWorld in worldXmlList)
                {
                    if (xeWorld.Tag == "Roles")
                    {

                        ArrayList roleXmlList = xeWorld.Children;
                        foreach (SecurityElement xeRole in roleXmlList)
                        {
                            if (xeRole.Tag == "Role")
                            {
                                RoleInfo roleInfo = new RoleInfo();
                                roleInfo.roleId = Convert.ToInt32(xeRole.Attribute("Id"));
                                roleInfo.Name = xeRole.Attribute("Name");
                                roleInfo.ChineseName = xeRole.Attribute("ChineseName");
                                roleInfo.Storage = xeRole.Attribute("Storage");
                                roleInfo.AB = xeRole.Attribute("AB");
                                roleInfo.ModelPath = xeRole.Attribute("ModelPath");
                                roleInfo.GuestPath = xeRole.Attribute("GuestPath");

                                roleInfos.Add(roleInfo.Name, roleInfo);
                            }
                        }
                    }
                }

            }
        }

        //         IEnumerator _LoadRoleConfig()
        //         {
        //             string strImagePath = ResourceLoadBase.GetResourceBasePath() + "Config/RoleConfig.xml";
        //             GuLog.Debug("RoleConfigPath" + strImagePath);
        //             WWW www = new WWW(strImagePath);
        //             yield return www;
        // 
        //             Parser(www);
        //         }

        public RoleInfo GetRoleInfo(string strName)
        {
            RoleInfo info = null;
            if (roleInfos.TryGetValue(strName, out info))
            {
                return info;
            }
            return null;
        }

        public string GetModelPath(string strName)
        {

            RoleInfo info = GetRoleInfo(strName);
            if (info != null)
            {
                return info.ModelPath;
            }
            return string.Empty;
        }

        public string GetGuestPath(string strName)
        {

            RoleInfo info = GetRoleInfo(strName);
            if (info != null)
            {
                return info.GuestPath;
            }
            return string.Empty;
        }

        public string GetChineseName(string strName)
        {
            RoleInfo info = GetRoleInfo(strName);
            if (info != null)
            {
                return info.ChineseName;
            }
            return string.Empty;
        }

        public List<string> GetAllRoleNames()
        {
            return this.roleInfos != null && this.roleInfos.Count > 0 ? this.roleInfos.Keys.ToList() : null;
        }

        public List<RoleInfo> GetAllRoleInfo()
        {
            return this.roleInfos != null && this.roleInfos.Count > 0 ? this.roleInfos.Values.ToList() : null;
        }
    }
}
