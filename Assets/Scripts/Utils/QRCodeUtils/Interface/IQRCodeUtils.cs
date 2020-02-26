using Gululu.Config;
using Hank;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.Common;

public interface IQRCodeUtils
{
    /// <summary>
    /// 生成固定大小的二维码(256,256)
    /// </summary>
    /// <param name="content"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    Texture2D GenerateQRImageConstantSize(string content, int width, int height);

    /// <summary>
    /// 任意正方形
    /// </summary>
    /// <param name="content"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    Texture2D GenerateQRImageFreeSize(string content, int width, int height);

    /// <summary>
    /// 生成中心带有小图标二维码
    /// </summary>
    /// <param name="content"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="centerIcon"></param>
    /// <returns></returns>
    Texture2D GenerateQRImageWithIcon(string content, int width, int height, Texture2D centerIcon);

    /// <summary>
    /// 融合图片和二维码,得到新图片 
    /// </summary>
    /// <param name="background">底图</param>
    /// <param name="qrCode">二维码</param>
    Texture2D MixImagAndQRCode(Texture2D background, Texture2D qrCode);

    /// <summary>
    /// 保存图片
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="texture"></param>
    void SaveTextureCodeToPng(string fileName, Texture2D texture);
}