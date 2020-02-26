﻿using System;
using UnityEngine;

public interface IAssetBundleUtils
{
    /// <summary>
    /// 从AssetBundle包中加载所需要的物体
    /// </summary>
    /// <param name="assetBundleName">AssetBundle包的名称</param>
    /// <param name="prefabName">预制体的名称</param>
    /// <param name="success">成功时的回调</param>
    /// <param name="failure">失败时的回调</param>
    void LoadAssetAsync(string assetBundleName, string prefabName, Action<GameObject> success, Action<string> failure = null);
    /// <summary>
    /// 停止加载AssetBundle及其中的物体
    /// </summary>
    /// <param name="assetBundleName">AssetBundle包的名称</param>
    void StopLoadAsset(string assetBundleName);
    /// <summary>
    /// 卸载AssetBundle包
    /// </summary>
    /// <param name="assetBundleName">AssetBundle包的名称</param>
    void UnloadAsset(string assetBundleName);
}