using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

class MD5Util : IMD5Util
{
    /// <summary>
    /// 生成指定文件的MD5码
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="callback">结果回调</param>
    /// <returns></returns>
    public string GenerateMD5Code(string filePath, Action<bool, string> callback = null)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogErrorFormat("<><MD5Util.GenerateMD5Code>File not exist, '{0}'", filePath);
            if (callback != null) callback(false, "File not exist");
            return string.Empty;
        }

        try
        {
            StringBuilder sb = new StringBuilder();
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (MD5 md5 = MD5.Create())
                {
                    byte[] newBuffer = md5.ComputeHash(fileStream);
                    for (int i = 0; i < newBuffer.Length; i++)
                    {
                        sb.Append(newBuffer[i].ToString("x2"));
                    }
                }
                fileStream.Close();
            }
            if (callback != null) callback(true, "已生成");
            return sb.ToString();
        }
        catch (Exception ex)
        {
            Debug.LogErrorFormat("<><MD5Util.GenerateMD5Code>Unknown error: {0}", ex.Message);
            if (callback != null) callback(false, ex.Message);
            return string.Empty;
        }
    }
    /// <summary>
    /// 验证文件的MD5码
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="md5">文件的MD5码</param>
    /// <returns></returns>
    public bool Verify(string filePath, string md5)
    {
        string newMD5 = this.GenerateMD5Code(filePath);
        return newMD5 == md5;
    }
}