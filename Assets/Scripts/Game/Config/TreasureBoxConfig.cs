using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gululu.Util;
using System.Security;
using Mono.Xml;
using System;
namespace Gululu.Config
{
    public class TreasureBoxConfig : AbsConfigBase, ITreasureBoxConfig
    {


        Dictionary<int, TreasureBox> treasureBoxInfo = new Dictionary<int, TreasureBox>();

        public void LoadTreasureBoxConfig()
        {
            if (m_LoadOk)
                return;

            treasureBoxInfo.Clear();
            ConfigLoader.Instance.LoadConfig("Config/TreasureBox.xml", Parser);
            //            StartCoroutine(_LoadTreasureBoxConfig());
        }

        private void Parser(WWW www)
        {
            m_LoadOk = true;
            if (www.isDone && (www.error == null || www.error.Length == 0))
            {
                SecurityParser xmlDoc = new SecurityParser();
                xmlDoc.LoadXml(www.text);
                ArrayList worldXmlList = xmlDoc.ToXml().Children;

                foreach (SecurityElement xeTreasureBoxs in worldXmlList)
                {
                    if (xeTreasureBoxs.Tag == "TreasureBoxes")
                    {
                        ArrayList TreasureBoxXmlList = xeTreasureBoxs.Children;
                        foreach (SecurityElement xeTreasureBox in TreasureBoxXmlList)
                        {
                            if (xeTreasureBox.Tag == "TreasureBox")
                            {
                                TreasureBox tBox = new TreasureBox();
                                tBox.treasureBoxId = Convert.ToInt32(xeTreasureBox.Attribute("Id"));
                                tBox.type = Convert.ToInt32(xeTreasureBox.Attribute("Type"));
                                ArrayList TreasureXmlList = xeTreasureBox.Children;
                                foreach (SecurityElement xeTreasure in TreasureXmlList)
                                {
                                    if (xeTreasure.Tag == "Treasure")
                                    {
                                        Treasure TreasureInfo = new Treasure();
                                        TreasureInfo.itemId = Convert.ToInt32(xeTreasure.Attribute("ItemId"));
                                        TreasureInfo.count = Convert.ToInt32(xeTreasure.Attribute("Value"));
                                        tBox.items.Add(TreasureInfo);

                                    }
                                }
                                treasureBoxInfo.Add(tBox.treasureBoxId, tBox);
                            }
                        }
                    }
                }
            }
        }

        //         IEnumerator _LoadTreasureBoxConfig()
        //         {
        //             string strPath = ResourceLoadBase.GetResourceBasePath() + "Config/TreasureBox.xml";
        //             GuLog.Debug("TreasureBoxConfigPath" + strPath);
        //             WWW www = new WWW(strPath);
        //             yield return www;
        // 
        //             Parser(www);
        // 
        //         }

        public TreasureBox GetTreasureBoxById(int treasureBoxId)
        {
            TreasureBox ret = null;
            if (treasureBoxInfo.TryGetValue(treasureBoxId, out ret))
            {
                return ret;
            }
            return ret;
        }

        public TreasureBox GetTreasureBoxByItemId(int itemId)
        {
            foreach (KeyValuePair<int, TreasureBox> kvp in this.treasureBoxInfo)
            {
                if (kvp.Value.items != null && kvp.Value.items.Exists(t => t.itemId == itemId))
                {
                    return kvp.Value;
                }
            }
            return null;
        }
    }

}
