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

class QRCodeUtils : IQRCodeUtils
{
    /// <summary>
    /// 生成固定大小的二维码(256,256)
    /// </summary>
    /// <param name="content"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public Texture2D GenerateQRImageConstantSize(string content, int width, int height)
    {
        // 编码成color32
        EncodingOptions options = null;
        BarcodeWriter writer = new BarcodeWriter();
        options = new EncodingOptions
        {
            Width = width,
            Height = height,
            Margin = 1,
        };
        options.Hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");
        writer.Format = BarcodeFormat.QR_CODE;
        writer.Options = options;
        Color32[] colors = writer.Write(content);
        // 转成texture2d
        Texture2D texture = new Texture2D(width, height);
        texture.SetPixels32(colors);
        texture.Apply();
        return texture;
    }

    /// <summary>
    /// 任意正方形
    /// </summary>
    /// <param name="content"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public Texture2D GenerateQRImageFreeSize(string content, int width, int height)
    {
        // 编码成color32
        MultiFormatWriter writer = new MultiFormatWriter();
        Dictionary<EncodeHintType, object> hints = new Dictionary<EncodeHintType, object>();
        hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");
        hints.Add(EncodeHintType.MARGIN, 1);
        hints.Add(EncodeHintType.ERROR_CORRECTION, ZXing.QrCode.Internal.ErrorCorrectionLevel.M);
        BitMatrix bitMatrix = writer.encode(content, BarcodeFormat.QR_CODE, width, height, hints);
        // 转成texture2d
        int w = bitMatrix.Width;
        int h = bitMatrix.Height;
        Debug.LogFormat("w={0},h={1}", w, h);
        Texture2D texture = new Texture2D(w, h);
        for (int x = 0; x < h; x++)
        {
            for (int y = 0; y < w; y++)
            {
                if (bitMatrix[x, y])
                {
                    //可在此改颜色
                    texture.SetPixel(y, x, Color.black);
                }
                else
                {
                    texture.SetPixel(y, x, Color.white);
                }
            }
        }
        texture.Apply();
        return texture;
    }

    /// <summary>
    /// 生成中心带有小图标二维码
    /// </summary>
    /// <param name="content"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="centerIcon"></param>
    /// <returns></returns>
    public Texture2D GenerateQRImageWithIcon(string content, int width, int height, Texture2D centerIcon)
    {
        // 编码成color32
        MultiFormatWriter writer = new MultiFormatWriter();
        Dictionary<EncodeHintType, object> hints = new Dictionary<EncodeHintType, object>();
        hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");
        hints.Add(EncodeHintType.MARGIN, 1);
        hints.Add(EncodeHintType.ERROR_CORRECTION, ZXing.QrCode.Internal.ErrorCorrectionLevel.H);
        BitMatrix bitMatrix = writer.encode(content, BarcodeFormat.QR_CODE, width, height, hints);
        // 转成texture2d
        int w = bitMatrix.Width;
        int h = bitMatrix.Height;
        Texture2D texture = new Texture2D(w, h);
        for (int x = 0; x < h; x++)
        {
            for (int y = 0; y < w; y++)
            {
                if (bitMatrix[x, y])
                {
                    texture.SetPixel(y, x, Color.black);
                }
                else
                {
                    texture.SetPixel(y, x, Color.white);
                }
            }
        }
        // 添加小图
        int halfWidth = texture.width / 2;
        int halfHeight = texture.height / 2;
        int halfWidthOfIcon = centerIcon.width / 2;
        int halfHeightOfIcon = centerIcon.height / 2;
        int centerOffsetX = 0;
        int centerOffsetY = 0;
        for (int x = 0; x < h; x++)
        {
            for (int y = 0; y < w; y++)
            {
                centerOffsetX = x - halfWidth;
                centerOffsetY = y - halfHeight;
                if (Mathf.Abs(centerOffsetX) <= halfWidthOfIcon && Mathf.Abs(centerOffsetY) <= halfHeightOfIcon)
                {
                    texture.SetPixel(x, y, centerIcon.GetPixel(centerOffsetX + halfWidthOfIcon, centerOffsetY + halfHeightOfIcon));
                }
            }
        }
        texture.Apply();
        return texture;
    }

    /// <summary>
    /// 融合图片和二维码,得到新图片 
    /// </summary>
    /// <param name="background">底图</param>
    /// <param name="qrCode">二维码</param>
    public Texture2D MixImagAndQRCode(Texture2D background, Texture2D qrCode)
    {
        Texture2D newTexture = GameObject.Instantiate(background) as Texture2D; ;
        Vector2 uv = new Vector2((background.width - qrCode.width) / background.width, (background.height - qrCode.height) / background.height);
        for (int i = 0; i < qrCode.width; i++)
        {
            for (int j = 0; j < qrCode.height; j++)
            {
                float w = uv.x * background.width - qrCode.width + i;
                float h = uv.y * background.height - qrCode.height + j;
                //从底图图片中获取到（w，h）位置的像素
                Color baseColor = newTexture.GetPixel((int)w, (int)h);
                //从二维码图片中获取到（i，j）位置的像素
                Color codeColor = qrCode.GetPixel(i, j);
                //融合
                newTexture.SetPixel((int)w, (int)h, codeColor);
            }
        }
        newTexture.Apply();
        return newTexture;
    }

    /// <summary>
    /// 保存图片
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="texture"></param>
    public void SaveTextureCodeToPng(string fileName, Texture2D texture)
    {
        byte[] bytes = texture.EncodeToJPG();
        string folderUrl = Path.GetDirectoryName(fileName);
        if (Directory.Exists(folderUrl))
        {
            Directory.CreateDirectory(folderUrl);
        }
        File.WriteAllBytes(fileName, bytes);
    }
}