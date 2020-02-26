using Gululu.Util;
using Mono.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using UnityEngine;

namespace Gululu.Config
{
    public class ItemConfig : AbsConfigBase, IItemConfig
    {

        private Dictionary<int, Item> missionInfo = new Dictionary<int, Item>();
        private Dictionary<int, List<NimItem>> animalInfo = new Dictionary<int, List<NimItem>>();


        public void LoadItemsConfig()
        {
            if (m_LoadOk)
                return;
            missionInfo.Clear();
            ConfigLoader.Instance.LoadConfig("Config/Item.xml", Parser);
            //             StartCoroutine(_LoadItemsConfig());
        }

        private void Parser(WWW www)
        {
            m_LoadOk = true;

            if (www.isDone && (www.error == null || www.error.Length == 0))
            {
                SecurityParser xmlDoc = new SecurityParser();
                xmlDoc.LoadXml(www.text);
                ArrayList worldXmlList = xmlDoc.ToXml().Children;

                foreach (SecurityElement xeWorld in worldXmlList)
                {
                    if (xeWorld.Tag == "Items")
                    {
                        ArrayList sceneXmlList = xeWorld.Children;
                        foreach (SecurityElement xeScene in sceneXmlList)
                        {
                            if (xeScene.Tag == "Item")
                            {
                                Item sceneInfo = null;
                                int type = Convert.ToInt32(xeScene.Attribute("Type"));
                                switch (type)
                                {
                                    case 4:
                                        {
                                            NimItem nimInfo = new NimItem();
                                            nimInfo.ItemId = Convert.ToInt32(xeScene.Attribute("Id"));
                                            nimInfo.Type = type;
                                            nimInfo.Name = xeScene.Attribute("Name");
                                            nimInfo.GetImage = xeScene.Attribute("getImage");

                                            nimInfo.PetType = int.Parse(xeScene.Attribute("PetType"));
                                            nimInfo.Image = xeScene.Attribute("Image");
                                            nimInfo.Audio = xeScene.Attribute("Audio");
                                            nimInfo.Animation = xeScene.Attribute("Animation");
                                            nimInfo.Preview = xeScene.Attribute("Preview");
                                            sceneInfo = nimInfo;
                                        }
                                        break;
                                    case 6:
                                        {
                                            PropsItem propsItem = new PropsItem();
                                            propsItem.ItemId = Convert.ToInt32(xeScene.Attribute("Id"));
                                            propsItem.Type = type;
                                            propsItem.Name = xeScene.Attribute("Name");
                                            propsItem.GetImage = xeScene.Attribute("getImage");
                                            sceneInfo = propsItem;
                                        }
                                        break;
                                    default:
                                        {
                                            sceneInfo = new Item();
                                            sceneInfo.ItemId = Convert.ToInt32(xeScene.Attribute("Id"));
                                            sceneInfo.Type = type;
                                            sceneInfo.Name = xeScene.Attribute("Name");
                                        }
                                        break;
                                }
                                missionInfo.Add(sceneInfo.ItemId, sceneInfo);

                            }
                        }
                    }
                }

            }
        }

        IEnumerator _LoadItemsConfig()
        {
            string strImagePath = ResourceLoadBase.GetResourceBasePath() + "Config/Item.xml";
            GuLog.Debug("ItemsConfigPath" + strImagePath);
            WWW www = new WWW(strImagePath);
            yield return www;

            Parser(www);

        }

        public Item GetItemById(int id)
        {
            Item item = null;
            if (missionInfo.TryGetValue(id, out item))
            {
                return item;
            }
            return null;
        }

        public Item GetItemByName(string name)
        {
            Item item = null;
            List<KeyValuePair<int, Item>> list = this.missionInfo.ToList();
            KeyValuePair<int, Item>? kvp = list.Find(t => t.Value.Name == name);
            if (kvp != null)
            {
                return kvp.Value.Value;
            }
            return null;
        }

        public string GetImage(int id)
        {
            Item item = GetItemById(id);
            if (item == null)
                return string.Empty;

            string image = string.Empty;
            switch (item.Type)
            {
                case 4:
                    NimItem nimInfo = item as NimItem;
                    if (nimInfo != null) image = nimInfo.GetImage;
                    break;
                case 6:
                    PropsItem propsItem = item as PropsItem;
                    if (propsItem != null) image = propsItem.GetImage;
                    break;
            }
            return image;
        }

        public List<NimItem> GetAllNimItem()
        {
            List<NimItem> nimItems = new List<NimItem>();
            if (this.missionInfo != null && this.missionInfo.Count > 0)
            {
                foreach (KeyValuePair<int, Item> kvp in this.missionInfo)
                {
                    NimItem nimItem = kvp.Value as NimItem;
                    if (nimItem != null && nimItem.Type == 4)
                    {
                        nimItems.Add(nimItem);
                    }
                }
            }
            return nimItems;
        }

        public List<NimItem> GetNimItemListByPetType(int petType)
        {
            if (!this.animalInfo.ContainsKey(petType))
                this.animalInfo.Add(petType, new List<NimItem>());

            if (this.animalInfo[petType].Count == 0)
            {
                foreach (KeyValuePair<int, Item> kvp in this.missionInfo)
                {
                    NimItem nimItem = kvp.Value as NimItem;
                    if (nimItem != null && nimItem.PetType == petType)
                        this.animalInfo[petType].Add(nimItem);
                }
            }
            return this.animalInfo[petType];
        }
    }
}

