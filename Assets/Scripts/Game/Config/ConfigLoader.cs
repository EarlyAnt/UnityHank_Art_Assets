using Gululu.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigLoader : SingletonMono<ConfigLoader> {

    IEnumerator _LoadConfig(string configPath, ParserWWW wwwParser)
    {
        string sPath = ResourceLoadBase.GetResourceBasePath() + configPath;
        WWW www = new WWW(sPath);
        yield return www;

        wwwParser(www);
    }

    public delegate void ParserWWW(WWW www);

    public void LoadConfig(string configPath, ParserWWW wwwParser)
    {
        StartCoroutine(_LoadConfig(configPath,wwwParser));

    }
}
