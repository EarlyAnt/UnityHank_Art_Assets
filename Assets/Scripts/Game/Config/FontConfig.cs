using Mono.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using UnityEngine;

namespace Gululu.Config
{
    /// <summary>
    /// 语言配置读取类
    /// </summary>
    public class FontConfig : AbsConfigBase, IFontConfig
    {
        /************************************************属性与变量命名************************************************/
        public const string DEFAULT_FONT = "Arial";
        //语言配置数据字典
        private Dictionary<string, string> fonts = new Dictionary<string, string>();
        /************************************************私  有  方  法************************************************/
        //读取语言配置文件
        private void Parser(WWW www)
        {
            m_LoadOk = true;
            GuLog.Debug("FontConfig WWW::" + www.error);

            if (www.isDone && (www.error == null || www.error.Length == 0))
            {
                SecurityParser xmlDoc = new SecurityParser();
                GuLog.Debug("FontConfig" + www.text);

                xmlDoc.LoadXml(www.text);
                ArrayList allNodes = xmlDoc.ToXml().Children;
                foreach (SecurityElement xeLanguages in allNodes)
                {
                    if (xeLanguages.Tag == "Fonts")
                    {
                        ArrayList languageNodes = xeLanguages.Children;
                        foreach (SecurityElement xeLanguage in languageNodes)
                        {
                            if (xeLanguage.Tag == "Font")
                            {
                                fonts.Add(xeLanguage.Attribute("Name"), xeLanguage.Attribute("FullName"));
                            }
                        }
                    }
                }
            }
        }
        /************************************************公  共  方  法************************************************/
        /// <summary>
        /// 加载字体配置数据
        /// </summary>
        public void LoadFontConfig()
        {
            if (m_LoadOk)
                return;
            this.fonts.Clear();
            ConfigLoader.Instance.LoadConfig("Config/FontConfig.xml", Parser);
        }
        /// <summary>
        /// 获取字体完整名称
        /// </summary>
        /// <param name="fontShortName">字体简称</param>
        /// <returns>如果配置项中能找到简称对应的字体则返回该字体全程，否则返回Unity默认字体Arial</returns>
        public string GetFontFullName(string fontShortName)
        {
            return this.fonts != null && this.fonts.ContainsKey(fontShortName) ? this.fonts[fontShortName] : DEFAULT_FONT;
        }
        /// <summary>
        /// 判断字体是否合法
        /// </summary>
        /// <param name="fontShortName">字体简称</param>
        /// <returns></returns>
        public bool IsValid(string fontShortName)
        {
            return this.fonts != null && this.fonts.ContainsKey(fontShortName);
        }
        /// <summary>
        /// 获取所有字体的完整名称
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllFont()
        {
            return this.fonts != null && this.fonts.Count > 0 ? this.fonts.Values.ToList() : null;
        }
    }
}
