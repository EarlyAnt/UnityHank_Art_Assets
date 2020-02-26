using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Linq;
using UnityEngine.Assertions;

namespace Gululu
{
    public class LangManager : ILangManager
    {
        [Inject]
        public ILanguageUtils languageUtils { get; set; }

        private static LangManager manager;

        public static LangManager Instance
        {
            get
            {
                if (manager == null)
                {
                    manager = new LangManager();
                    manager.ReloadXml();
                }
                return manager;
            }
        }

        public string GetLanguageString(string key){
            
            if(manager == null)
            {
                manager = this;
                ReloadXml();
            }

            return getString(key);
        }

//         public static string GetLanguageString(string key, bool isWeb = false){
//             return ;
//         }

        private Dictionary<string, string> Strings = new Dictionary<string, string>();
        GululuLanguage currLanguage = GululuLanguage.ZH_CN;

        public void Lang(GululuLanguage language){
            currLanguage = language;
            ReloadXml();
            //LoadLanguage(LanguagePathUtils.GetLanguagePath(language));
        }

        public void ReloadXml()
        {
            if (languageUtils == null)
            {
                languageUtils = new LanguageUtils();
            }
            Strings.Clear();
            LoadLanguage(languageUtils.GetLanguagePath(currLanguage));
        }
//         private void Lang (string path) {
//             string LangFilePath = path;
//             if(!File.Exists(Application.dataPath + "/Resources/" + path + ".xml")){
//                 LangFilePath = LanguagePathUtils.GetLanguagePath(GululuLanguage.ENGLISH);
//             }
//                 setLanguage(LangFilePath);
//         }
        public void LoadLanguage (string path) {
            XmlDocument xml = new XmlDocument();
            TextAsset textAsset = (TextAsset)Resources.Load(path);
            if(textAsset == null)
            {
                Assert.IsTrue(false);
                textAsset = (TextAsset)Resources.Load(languageUtils.GetLanguagePath(GululuLanguage.ENGLISH));
            }
            xml.LoadXml(textAsset.text);
     
            XmlNodeList XmlList = xml.GetElementsByTagName("languages")[0].ChildNodes;
            for (int i = 0; i < XmlList.Count; i++)
            {
                XmlElement xmlItem = (XmlElement)XmlList[i];
                string strkey = xmlItem.GetAttribute("name");
#if UNITY_EDITOR
                int index = strkey.IndexOf(AutoKeyPreFix);
                if (index != -1)
                {
                    string strNumber = strkey.Replace(AutoKeyPreFix, "");
                    int number = Convert.ToInt32(strNumber) - startIndex;
                    if (number > MaxAutoKey)
                    {
                        MaxAutoKey = number;
                    }
                }
#endif
                if(Strings.ContainsKey(strkey)){
                    Strings[strkey] = xmlItem.InnerText;
                    Debug.Log("<><>"+strkey+"<><>have more define");
                }else{
                    Strings.Add(strkey, xmlItem.InnerText);
                }


            }
        }
        
//         public void setLanguageWeb (string xmlText, string language) {
//             var xml = new XmlDocument();
//             xml.Load(new StringReader(xmlText));
//         
//             Strings = new Dictionary();
//             XmlNodeList XmlList = xml.GetElementsByTagName("languages")[0].ChildNodes;
//             for (int i = 0; i < XmlList.Count; i++)
//             {
//                 XmlElement xmlItem = (XmlElement)XmlList[i];
//                 string strkey = xmlItem.GetAttribute("name");
//                 Strings.Add(xmlItem.GetAttribute("name"), xmlItem.InnerText);
// 
//             }
//         }


        public string getString (string name) {
            if (!Strings.ContainsKey(name)) {
                GuLog.Warning("The string does not exist: " + name);
            
                return string.Empty;
            }
 
            return (string)Strings[name];
        }


#if UNITY_EDITOR
        private static LangManager managerEng;
        public static LangManager InstanceEng
        {
            get
            {
                if (managerEng == null)
                {
                    managerEng = new LangManager();
                    managerEng.Lang( GululuLanguage.ENGLISH);
                }
                return managerEng;
            }
        }

        private static LangManager managerTW;
        public static LangManager InstanceTW
        {
            get
            {
                if (managerTW == null)
                {
                    managerTW = new LangManager();
                    managerTW.Lang(GululuLanguage.ZH_TW);
                }
                return managerTW;
            }
        }
//         private static LangManager managerHK;
//         public static LangManager InstanceHK
//         {
//             get
//             {
//                 if (managerHK == null)
//                 {
//                     managerHK = new LangManager();
//                     managerHK.Lang(GululuLanguage.ZH_HK);
//                 }
//                 return managerHK;
//             }
//         }

        public static string AutoKeyPreFix = "_AutoKey";
        public int MaxAutoKey = 0;
        public int startIndex = 1000;
        public string GetKeyByChineseString(string strValue)
        {
            foreach(var pair in Strings)
            {
                if (pair.Value == strValue)
                {
                    return pair.Key;
                }
            }
            return null;
        }

        public string AddAutoValue(string strValue)
        {
            ++MaxAutoKey;
            string strKey = AutoKeyPreFix + (startIndex + MaxAutoKey).ToString();
            Strings.Add(strKey, strValue);
            return strKey;

        }

        public void RemoveUnuseString(bool[] inuse)
        {
            for(int i = 0; i < inuse.Length; ++i)
            {
                if(!inuse[i])
                {
                    string strKey = AutoKeyPreFix + (startIndex + i).ToString();
                    Strings.Remove(strKey);
                }
            }
        }

        public void SaveString()
        {
            Strings = Strings.OrderBy(o => o.Key).ToDictionary( o => o.Key, p => p.Value );
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration dec = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(dec);
            XmlElement root = xmlDoc.CreateElement("languages");
            xmlDoc.AppendChild(root);

            foreach (var pair in Strings)
            {
                XmlElement stringNode = xmlDoc.CreateElement("string");
                stringNode.SetAttribute("name", (string)pair.Key);
                stringNode.InnerText = (string)pair.Value;
                root.AppendChild(stringNode);
            }

            string path = Application.dataPath + "/Resources/" + languageUtils.GetLanguagePath(currLanguage) + ".xml";
            xmlDoc.Save(path);
        }

        public void UpdateXmlByCN()
        {
            Dictionary<string, string> cnStrings = LangManager.Instance.Strings;

            if(Strings == cnStrings)
            {
                return;
            }

            List<string> unusedKeyList = new List<string>();
            foreach(var pair in Strings)
            {
                string strKey = pair.Key;
                bool bFound = false;
                foreach(var cnPair in cnStrings)
                {
                    if(strKey == cnPair.Key)
                    {
                        bFound = true;
                        break;
                    }
                }
                if(!bFound)
                {
                    unusedKeyList.Add(strKey);
                }
            }
            foreach(var key in unusedKeyList)
            {
                Strings.Remove(key);
            }

            foreach (var cnPair in cnStrings)
            {
                string strcnKey = cnPair.Key;
                bool bFound = false;
                foreach (var pair in Strings)
                {
                    if (strcnKey == pair.Key)
                    {
                        bFound = true;
                        break;
                    }
                }
                if(!bFound)
                {
                    Strings.Add(strcnKey, cnPair.Value);
                }
            }
            SaveString();
        }

#endif
    }
}