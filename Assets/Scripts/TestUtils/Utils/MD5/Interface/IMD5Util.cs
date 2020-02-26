using System;

public interface IMD5Util
{
    /// <summary>
    /// 生成指定文件的MD5码
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="callback">结果回调</param>
    /// <returns></returns>
    string GenerateMD5Code(string filePath, Action<bool, string> callback = null);
    /// <summary>
    /// 验证文件的MD5码
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="md5">文件的MD5码</param>
    /// <returns></returns>
    bool Verify(string filePath, string md5);
}