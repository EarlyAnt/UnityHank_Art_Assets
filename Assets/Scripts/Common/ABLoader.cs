using System;
using System.Collections;
using UnityEngine;

namespace Common
{
    public class ABLoader : MonoBehaviour
    {
        public static ABLoader Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void LoadAB(string url, Action callback)
        {
            this.StartCoroutine(this.AsyncLoadAB(url, callback));
        }

        private IEnumerator AsyncLoadAB(string url, Action callback)
        {
            WWW www = WWW.LoadFromCacheOrDownload(url, 1);
            yield return www;

            if (!string.IsNullOrEmpty(www.error))
            {
                if (callback != null)
                    callback();
            }


        }
    }
}
