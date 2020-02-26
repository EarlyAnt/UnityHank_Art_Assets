using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using Gululu.Config;

namespace Gululu.Util
{
    public class PrefabRoot : IPrefabRoot
    {
        [Inject]
        public ILanguageUtils languageUtils { get; set; }
        [Inject]
        public II18NConfig I18NConfig { get; set; }

        Dictionary<string, GameObject> mapImageName2Object = new Dictionary<string, GameObject>();
        GameObject fontPrefab = null;
        Font usingFont;
        Font GetCurrFont()
        {
            if (usingFont == null)
            {
                if (fontPrefab == null)
                {
                    string fontPath;
                    if (languageUtils.getCurrentLanguage() == GululuLanguage.ZH_TW)
                    {
                        fontPath = "UI/Font/DFT_HBC";
                    }
                    else
                    {
                        fontPath = "UI/Font/DFGBHBC";
                    }

                    fontPrefab = Resources.Load<GameObject>(fontPath);
                }
                if (fontPrefab)
                {
                    usingFont = fontPrefab.GetComponent<FontContainer>().currFont;
                }
            }
            return usingFont;
        }
        public void ReplaceFont(GameObject newGameObject)
        {
            Font nowfont = GetCurrFont();
            if (nowfont != null)
            {
                Text[] textAll = newGameObject.GetComponentsInChildren<Text>(true);
                foreach (var text in textAll)
                {
                    if (text.gameObject.tag != "KeepFont")
                    {
                        text.font = nowfont;
                    }
                }
            }
        }
        public GameObject GetUniqueGameObject(string strResourcePre, string name)
        {
            string strNewObjectHierachy = strResourcePre + "/" + name;


            GameObject newGameObject = null;
            if (!mapImageName2Object.TryGetValue(strNewObjectHierachy, out newGameObject))
            {
#if (UNITY_IOS || UNITY_ANDROID) && (!UNITY_EDITOR) && LOCALIZATION
                if (strResourcePre.StartsWith("UI"))
                {
                    GameObject prefab = Resources.Load<GameObject>("Localization/" + strNewObjectHierachy);
                    if (prefab != null)
                    {
                        newGameObject = GameObject.Instantiate(prefab) as GameObject;
                        ReplaceFont(newGameObject);
                        Text[] textAll = newGameObject.GetComponentsInChildren<Text>(true);
                        foreach (var text in textAll)
                        {
                            if (text.text.Length != 0)
                            {
                                if (text.text[0] == '#')
                                {
                                    text.text = LangManager.GetLanguageString(text.text.Substring(1));
                                }
                            }
                        }
                    }
                    else
                    {

                        prefab = Resources.Load(strNewObjectHierachy) as GameObject;
                        newGameObject = GameObject.Instantiate(prefab) as GameObject;
                        ReplaceFont(newGameObject);
                    }
                }
#else
                GameObject prefab = Resources.Load(strNewObjectHierachy) as GameObject;
                newGameObject = GameObject.Instantiate(prefab) as GameObject;
                ReplaceFont(newGameObject);
#endif
                if (newGameObject)
                {
                    newGameObject.name = name;
                    if (newGameObject.tag != "CanDestory")
                    {
                        GameObject.DontDestroyOnLoad(newGameObject);
                        mapImageName2Object.Add(strNewObjectHierachy, newGameObject);
                    }
                }
            }
            return newGameObject;
        }

        public GameObject GetGameObject(string strResourcePre, string name)
        {
            string strNewObjectHierachy = strResourcePre + "/" + name;

            GameObject newGameObject = null;
            GameObject prefab = Resources.Load(strNewObjectHierachy) as GameObject;
            if (prefab != null)
            {
                newGameObject = GameObject.Instantiate(prefab) as GameObject;
            }

            return newGameObject;
        }
        public T GetObjectNoInstantiate<T>(string strResourcePre, string name) where T : UnityEngine.Object
        {
            string strNewObjectHierachy = strResourcePre + "/" + name;
            if (typeof(T) == typeof(AudioClip) && this.I18NConfig != null)
                strNewObjectHierachy = strResourcePre + "/" + this.I18NConfig.GetAudioPath(name);//如果是音频文件，则根据语言去国际化配置表中查找路径
            GuLog.Debug(string.Format("----PrefabRoot.GetObjectNoInstantiate----Type: {0}, Name: {1}, ResPath: {2}, DesPath: {3}", typeof(T).Name, name, strResourcePre, strNewObjectHierachy));

            T newGameObject = Resources.Load<T>(strNewObjectHierachy) as T;
            //AudioClip prefab = Resources.Load(strNewObjectHierachy) as AudioClip;
            //if (prefab != null)
            //{
            //    newGameObject = GameObject.Instantiate(prefab) as AudioClip;
            //}

            return newGameObject;
        }


        public void persentView<T>(ref GameObject viewGameObject, Transform transform) where T : MonoBehaviour
        {
            if (viewGameObject == null)
            {
                viewGameObject = new GameObject();

                RectTransform rect = viewGameObject.AddComponent<RectTransform>();
                rect.sizeDelta = Vector2.zero;
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.one;
                rect.pivot = Vector2.one / 2;

                T t = viewGameObject.AddComponent<T>();
                viewGameObject.name = t.GetType().Name;

                viewGameObject.transform.SetParent(transform, false);
                viewGameObject.transform.localPosition = Vector3.zero;
            }
            else
            {
                viewGameObject.SetActive(false);
                viewGameObject.SetActive(true);
            }
        }

        public void hideView(ref GameObject viewGameObject)
        {
            if (viewGameObject != null)
            {
                viewGameObject.SetActive(false);
            }
        }

        public void Clear()
        {
            List<string> clearKey = new List<string>();
            foreach (var pair in mapImageName2Object)
            {
                if (pair.Value == null)
                {
                    clearKey.Add(pair.Key);
                }
            }
            foreach (var key in clearKey)
            {
                mapImageName2Object.Remove(key);
            }
        }
    }
}
