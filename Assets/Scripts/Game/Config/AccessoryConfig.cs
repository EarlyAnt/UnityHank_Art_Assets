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
    /// 配饰配置读取类
    /// </summary>
    public class AccessoryConfig : AbsConfigBase, IAccessoryConfig
    {
        /************************************************自  定  义  类************************************************/

        /************************************************属性与变量命名************************************************/
        //多语言资源配置数据字典
        private List<Accessory> accessories = new List<Accessory>();
        /************************************************私  有  方  法************************************************/
        //读取语言配置文件
        private void Parser(WWW www)
        {
            m_LoadOk = true;
            GuLog.Debug("AccessoryConfigParser WWW:" + www.error);

            if (www.isDone && (www.error == null || www.error.Length == 0))
            {
                SecurityParser xmlDoc = new SecurityParser();
                GuLog.Debug("AccessoryConfigParser: " + www.text);

                xmlDoc.LoadXml(www.text);
                ArrayList allNodes = xmlDoc.ToXml().Children;
                foreach (SecurityElement xeResConfigs in allNodes)
                {//根节点
                    if (xeResConfigs.Tag == "Accessories")
                    {//Accessories点
                        ArrayList accessoryConfigNodes = xeResConfigs.Children;
                        foreach (SecurityElement xeAccessory in accessoryConfigNodes)
                        {//Accessory节点
                            if (xeAccessory.Tag == "Accessory")
                            {
                                Accessory accessory = new Accessory()
                                {
                                    ID = Convert.ToInt32(xeAccessory.Attribute("ID")),
                                    Type = Convert.ToInt32(xeAccessory.Attribute("Type")),
                                    Index = Convert.ToInt32(xeAccessory.Attribute("Index")),
                                    Name = xeAccessory.Attribute("Name"),
                                    Icon = xeAccessory.Attribute("Icon"),
                                    AB = xeAccessory.Attribute("AB"),
                                    Prefab = xeAccessory.Attribute("Prefab"),
                                    Region = xeAccessory.Attribute("Region"),
                                    Purchase = xeAccessory.Attribute("Purchase"),
                                    Price = Convert.ToSingle(xeAccessory.Attribute("Price")),
                                    StartTime = DateTime.MaxValue,
                                    EndTime = DateTime.MaxValue,
                                    Level = Convert.ToInt32(xeAccessory.Attribute("Level")),
                                    Exp = Convert.ToInt32(xeAccessory.Attribute("Exp")),
                                    PURPIE = xeAccessory.Attribute("PURPIE") == "1",
                                    DONNY = xeAccessory.Attribute("DONNY") == "1",
                                    NINJI = xeAccessory.Attribute("NINJI") == "1",
                                    SANSA = xeAccessory.Attribute("SANSA") == "1",
                                    YOYO = xeAccessory.Attribute("YOYO") == "1",
                                    NUO = xeAccessory.Attribute("NUO") == "1"
                                };

                                DateTime dateTime = DateTime.MaxValue;
                                if (DateTime.TryParse(xeAccessory.Attribute("StartTime"), out dateTime))
                                    accessory.StartTime = dateTime;
                                if (DateTime.TryParse(xeAccessory.Attribute("EndTime"), out dateTime))
                                    accessory.EndTime = dateTime;

                                this.accessories.Add(accessory);
                            }
                        }
                    }
                }
                //GuLog.Debug(string.Format("<><AccessoryConfig.Parser>Accessory.xml: {0}", www.text));
            }
        }
        /************************************************公  共  方  法************************************************/
        /// <summary>
        /// 加载配饰数据
        /// </summary>
        public void LoadAccessoryConfig()
        {
            if (m_LoadOk)
                return;
            this.accessories.Clear();
            ConfigLoader.Instance.LoadConfig("Config/Accessory.xml", Parser);
        }
        /// <summary>
        /// 获取所有配饰
        /// </summary>
        /// <returns></returns>
        public List<Accessory> GetAllAccessories()
        {
            return this.accessories;
        }
        /// <summary>
        /// 获取指定类型的配饰
        /// </summary>
        /// <param name="types">类型集合</param>
        /// <returns></returns>
        public List<Accessory> GetAccessoriesByTypes(List<int> types)
        {
            if (this.accessories != null && types != null)
                return this.accessories.FindAll(t => types.Contains(t.Type)).OrderBy(t => t.Index).ToList();
            else
                return null;
        }
        /// <summary>
        /// 获取指定配饰
        /// </summary>
        /// <param name="accessoryID">配饰ID</param>
        /// <returns></returns>
        public Accessory GetAccessory(int accessoryID)
        {
            if (this.accessories != null)
                return this.accessories.Find(t => t.ID == accessoryID);
            else
                return null;
        }
    }
}
