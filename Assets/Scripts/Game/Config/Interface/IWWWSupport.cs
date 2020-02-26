using Gululu.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public interface IWWWSupport {

    Coroutine StartWWW(string url, Action<WWW> wwwParser);


    void StopWWWCoroutine(Coroutine currCoroutine);
}
