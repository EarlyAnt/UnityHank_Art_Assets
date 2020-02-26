using Hank;
using Mono.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using UnityEngine;

namespace Gululu.Config
{
    /// <summary>
    /// 宝箱产出规则配置读取类
    /// </summary>
    public class AwardConfig : AbsConfigBase, IAwardConfig
    {
        /************************************************自  定  义  类************************************************/

        /************************************************属性与变量命名************************************************/
        //多语言资源配置数据字典
        private List<AwardInfo> awardInfos = new List<AwardInfo>();
        /************************************************私  有  方  法************************************************/
        //读取语言配置文件
        private void Parser(WWW www)
        {
            m_LoadOk = true;
            GuLog.Debug("AwardConfigParser WWW:" + www.error);

            if (www.isDone && (www.error == null || www.error.Length == 0))
            {
                SecurityParser xmlDoc = new SecurityParser();
                GuLog.Debug("AwardConfigParser: " + www.text);

                xmlDoc.LoadXml(www.text);
                ArrayList allNodes = xmlDoc.ToXml().Children;
                foreach (SecurityElement xeAwardConfigs in allNodes)
                {//根节点
                    if (xeAwardConfigs.Tag == "AwardConfigs")
                    {//AwardConfigs节点
                        ArrayList awardConfigNodes = xeAwardConfigs.Children;
                        foreach (SecurityElement xeAwardConfig in awardConfigNodes)
                        {//AwardConfig节点
                            if (xeAwardConfig.Tag == "AwardConfig")
                            {
                                AwardInfo awardInfo = new AwardInfo()
                                {
                                    ID = Convert.ToInt32(xeAwardConfig.Attribute("ID")),
                                };

                                ArrayList awardNodes = xeAwardConfig.Children;
                                foreach (SecurityElement xeAward in awardNodes)
                                {//ResConfig节点
                                    switch (xeAward.Tag)
                                    {
                                        case "Coin":
                                            awardInfo.Awards.Add(new CoinAward() { Percent = Convert.ToSingle(xeAward.Attribute("Percent")) });
                                            break;
                                        case "Prop":
                                            awardInfo.Awards.Add(new PropAward() { Percent = Convert.ToSingle(xeAward.Attribute("Percent")), Source = xeAward.Attribute("Source") });
                                            break;
                                    }
                                }
                                this.awardInfos.Add(awardInfo);
                            }
                        }
                    }
                }
                //GuLog.Debug(string.Format("<><AwardConfig.Parser>Award.xml: {0}", www.text));
            }
        }
        /************************************************公  共  方  法************************************************/
        /// <summary>
        /// 加载所有宝箱产出规则配置数据
        /// </summary>
        public void LoadAwardConfig()
        {
            if (m_LoadOk)
                return;
            this.awardInfos.Clear();
            ConfigLoader.Instance.LoadConfig("Config/Award.xml", Parser);
        }
        /// <summary>
        /// 获取所有宝箱产出规则
        /// </summary>
        /// <returns></returns>
        public List<AwardInfo> GetAllAwards()
        {
            return this.awardInfos;
        }
        /// <summary>
        /// 获取指定宝箱产出规则
        /// </summary>
        /// <param name="awardID"></param>
        /// <returns></returns>
        public AwardInfo GetAward(int awardID)
        {
            if (this.awardInfos != null)
                return this.awardInfos.Find(t => t.ID == awardID);
            else
                return null;
        }
    }
}
