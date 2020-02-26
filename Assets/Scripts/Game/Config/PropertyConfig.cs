using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Xml;
using Mono.Xml;
using UnityEngine;

namespace Gululu.Config
{
    public class PropertyConfig : AbsConfigBase, IPropertyConfig
    {
        private int medianPrice = 0;

        private Dictionary<int, PropertyItem> mPropertyDict = new Dictionary<int, PropertyItem>();

        public void LoadPropertyConfig()
        {
            if (m_LoadOk)
                return;
            mPropertyDict.Clear();
            ConfigLoader.Instance.LoadConfig("Config/PropertyConfig.xml", Parser);
        }

        private void Parser(WWW www)
        {
            m_LoadOk = true;
            int totalPrice = 0;
            if (www.isDone && (www.error == null || www.error.Length == 0))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(www.text);

                XmlNodeList nodeList = xmlDoc.SelectNodes("Properties/PropertyItem");

                foreach (XmlNode node in nodeList)
                {
                    PropertyItem item = new PropertyItem()
                    {
                        Number = int.Parse(node["Number"].InnerText.Trim()),
                        Id = int.Parse(node["Id"].InnerText.Trim()),
                        Type = int.Parse(node["Type"].InnerText.Trim()),
                        Name = node["Name"].InnerText.Trim(),
                        Price = int.Parse(node["Price"].InnerText.Trim()),
                        Sale = float.Parse(node["Sale"].InnerText.Trim()),
                        UnlockLevel = int.Parse(node["UnlockLevel"].InnerText.Trim()),
                        Rarity = float.Parse(node["Rarity"].InnerText.Trim()),
                        Hp = int.Parse(node["Hp"].InnerText.Trim()),
                        Exp = int.Parse(node["Exp"].InnerText.Trim()),
                        IsShow = bool.Parse(node["IsShow"].InnerText.Trim()),
                        AcquireWay = node["IsShow"].InnerText.Trim(),
                        DefaultIcon = node["DefaultIcon"].InnerText.Trim(),
                        Tag = node["Tag"].InnerText.Trim(),
                        IsNew = bool.Parse(node["IsNew"].InnerText.Trim()),
                        HighLightIcon = node["HighLightIcon"].InnerText.Trim()
                    };
                    //                    Debug.LogError("ID:"+item.Id);

                    item.IsOperation = bool.Parse(node["IsCampain"].InnerText.Trim());

                    if (item.IsOperation)
                    {
                        if (node["StartTime"].InnerText.Trim() == "NULL" || node["EndTime"].InnerText.Trim() == "NULL")
                        {
                            Debug.LogError("运营项目需要配置开始时间与结束时间");
                        }

                        item.StartTime = DateTime.Parse(node["StartTime"].InnerText.Trim());
                        item.EndTime = DateTime.Parse(node["EndTime"].InnerText.Trim());
                    }

                    item.ReactionAnim = new Dictionary<string, int>();
                    if (!string.IsNullOrEmpty(node["ReactionAnim"].InnerText.Trim()))
                    {
                        string[] animKeyValuePairs = node["ReactionAnim"].InnerText.Trim().Split(';');

                        foreach (var pair in animKeyValuePairs)
                        {
                            string[] keyValue = pair.Split(':');
                            if (keyValue.Length < 2) continue;
                            item.ReactionAnim.Add(keyValue[0], int.Parse(keyValue[1]));
                        }
                    }
                    totalPrice += item.Price;
                    mPropertyDict.Add(item.Id, item);
                }
                this.CalculateMedianPrice();//计算中位数价格
            }
        }

        public PropertyItem GetPropertyByID(int id)
        {
            PropertyItem item;
            if (mPropertyDict.TryGetValue(id, out item))
            {
                return item;
            }
            return null;
        }

        public List<PropertyItem> GetPropertyList()
        {
            if (mPropertyDict.Count > 0)
            {
                List<PropertyItem> rightList = mPropertyDict.Values.ToList();

                rightList.Sort((item, propertyItem) => item.Number.CompareTo(propertyItem.Number));
                return rightList;
            }
            return null;
        }

        public int GetMedianPrice()
        {
            return this.medianPrice;
        }

        private void CalculateMedianPrice()
        {
            if (this.mPropertyDict != null && this.mPropertyDict.Count > 0)
            {
                List<PropertyItem> items = this.mPropertyDict.Values.ToList().OrderBy(t => t.Price).ToList();
                if (items.Count % 2 == 0)
                {
                    int middleIndex = items.Count / 2 - 1;
                    float price1 = items[middleIndex].Price;
                    float price2 = items[middleIndex + 1].Price;
                    this.medianPrice = (int)((price1 + price2) / 2f);
                }
                else if (items.Count % 2 == 1)
                {
                    int middleIndex = items.Count / 2;
                    this.medianPrice = items[middleIndex].Price;
                }
            }
            else this.medianPrice = 0;
        }
    }

    public class PropertyItem
    {
        public int Number;
        public int Id;
        public int Type;
        public string Name;
        public int Price;
        public float Sale;
        public int UnlockLevel;
        public string Icon;
        public string AnimationName;
        public float Rarity;
        public int Hp;
        public int Exp;
        public bool IsShow;
        public string AcquireWay;
        public string DefaultIcon;
        public string HighLightIcon;
        public string Tag;
        public bool IsNew;
        public bool IsOperation;
        public DateTime StartTime;
        public DateTime EndTime;
        public Dictionary<string, int> ReactionAnim;
    }
}