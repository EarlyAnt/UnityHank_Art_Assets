using System.Collections.Generic;
using UnityEngine;

namespace Gululu.Config
{
    /// <summary>
    /// 字体配置文件读取接口
    /// </summary>
    public interface IFontConfig
    {
        /// <summary>
        /// 加载字体配置数据
        /// </summary>
        void LoadFontConfig();
        /// <summary>
        /// 获取字体完整名称
        /// </summary>
        /// <param name="fontShortName">字体简称</param>
        /// <returns></returns>
        string GetFontFullName(string fontShortName);
        /// <summary>
        /// 判断字体是否合法
        /// </summary>
        /// <param name="fontShortName">字体简称</param>
        /// <returns></returns>
        bool IsValid(string fontShortName);
        /// <summary>
        /// 获取所有字体的完整名称
        /// </summary>
        /// <returns></returns>
        List<string> GetAllFont();
    }
}
