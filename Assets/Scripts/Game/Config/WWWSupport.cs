using Gululu.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WWWSupport : IWWWSupport
{

    public Coroutine StartWWW(string url, Action<WWW> wwwParser)
    {
        return WWWSupportMono.Instance.StartWWW(url, wwwParser);
    }


    public void StopWWWCoroutine(Coroutine currCoroutine)
    {
        WWWSupportMono.Instance.StopWWWCoroutine(currCoroutine);
    }
}
