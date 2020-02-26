using Mono.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

namespace Gululu.Config
{
    /// <summary>
    /// 语言配置读取类
    /// </summary>
    public class LanConfig : AbsConfigBase, ILanConfig
    {
        /************************************************自  定  义  类************************************************/
        /// <summary>
        /// 语言枚举
        /// </summary>
        public static class Languages
        {
            /// <summary>
            /// 中文简体
            /// </summary>
            public static string ChineseSimplified { get { return "CHS"; } }
            /// <summary>
            /// 中文繁体
            /// </summary>
            public static string ChineseTraditonal { get { return "CHT"; } }
            /// <summary>
            /// 英文
            /// </summary>
            public static string English { get { return "EN"; } }
            /// <summary>
            /// 日文
            /// </summary>
            public static string Japanese { get { return "JP"; } }
        }
        /************************************************属性与变量命名************************************************/
        //语言配置数据字典
        private Dictionary<string, Language> languages = new Dictionary<string, Language>();
        /************************************************私  有  方  法************************************************/
        //读取语言配置文件
        private void Parser(WWW www)
        {
            m_LoadOk = true;
            GuLog.Debug("LanConfigPath WWW::" + www.error);

            if (www.isDone && (www.error == null || www.error.Length == 0))
            {
                SecurityParser xmlDoc = new SecurityParser();
                GuLog.Debug("LanConfigPath" + www.text);

                xmlDoc.LoadXml(www.text);
                ArrayList allNodes = xmlDoc.ToXml().Children;
                foreach (SecurityElement xeLanguages in allNodes)
                {
                    if (xeLanguages.Tag == "Languages")
                    {
                        ArrayList languageNodes = xeLanguages.Children;
                        foreach (SecurityElement xeLanguage in languageNodes)
                        {
                            if (xeLanguage.Tag == "Language")
                            {
                                Language language = new Language()
                                {
                                    Name = xeLanguage.Attribute("Name"),
                                    Text = xeLanguage.Attribute("Text"),
                                    Default = Convert.ToBoolean(xeLanguage.Attribute("Default"))
                                };
                                languages.Add(language.Name, language);
                            }
                        }
                    }
                }
            }
        }
        //获取语言实例
        private Language GetLanguage(string language)
        {
            if (this.languages != null && this.languages.Count > 0 &&
                !string.IsNullOrEmpty(language) && this.languages.ContainsKey(language))
            {
                return this.languages[language];
            }
            else return null;
        }
        /************************************************公  共  方  法************************************************/
        /// <summary>
        /// 加载语言配置数据
        /// </summary>
        public void LoadLanConfig()
        {
            if (m_LoadOk)
                return;
            this.languages.Clear();
            ConfigLoader.Instance.LoadConfig("Config/LanConfig.xml", Parser);
        }
        /// <summary>
        /// 获取语言显示文本
        /// </summary>
        /// <param name="languageShortName">语言简称(可在LanConfig.Languages类中得到枚举值</param>
        /// <returns></returns>
        public string GetLanText(string languageShortName)
        {
            Language language = this.GetLanguage(languageShortName);
            return language != null ? language.Text : null;
        }
        /// <summary>
        /// 判断指定语言是否为默认语言
        /// </summary>
        /// <param name="languageShortName">语言简称(可在LanConfig.Languages类中得到枚举值)</param>
        /// <returns></returns>
        public bool IsDefault(string languageShortName)
        {
            Language language = this.GetLanguage(languageShortName);
            return language != null ? language.Default : false;
        }
        /// <summary>
        /// 判断语言简称是否合法
        /// </summary>
        /// <param name="languageShortName"></param>
        /// <returns></returns>
        public bool IsValid(string languageShortName)
        {
            return this.languages != null && !string.IsNullOrEmpty(languageShortName) && this.languages.ContainsKey(languageShortName);
        }
        /// <summary>
        /// 获取所有语言的简称
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllLanShortName()
        {
            if (this.languages != null && this.languages.Count > 0)
            {
                List<string> allLanText = new List<string>();
                foreach (KeyValuePair<string, Language> kvp in this.languages)
                {
                    allLanText.Add(kvp.Value.Name);
                }
                return allLanText;
            }
            else return null;
        }
        /// <summary>
        /// 获取所有语言的显示文本
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllLanText()
        {
            if (this.languages != null && this.languages.Count > 0)
            {
                List<string> allLanText = new List<string>();
                foreach (KeyValuePair<string, Language> kvp in this.languages)
                {
                    allLanText.Add(kvp.Value.Text);
                }
                return allLanText;
            }
            else return null;
        }
    }
}
