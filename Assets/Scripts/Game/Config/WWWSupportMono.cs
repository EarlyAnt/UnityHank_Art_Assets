using Gululu.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WWWSupportMono : SingletonMono<WWWSupportMono> {


    IEnumerator _DownloadSprite(string url, Action<WWW> wwwParser)
    {
        WWW www = new WWW(url);
        yield return www;
        wwwParser(www);
    }

    public Coroutine StartWWW(string url, Action<WWW> wwwParser)
    {
        return StartCoroutine(_DownloadSprite(url, wwwParser));
    }


    public void StopWWWCoroutine(Coroutine currCoroutine)
    {
        if (currCoroutine != null)
        {
            StopCoroutine(currCoroutine);
            currCoroutine = null;
        }
    }
}
