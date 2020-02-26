using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Gululu.Util
{
    public abstract class SingletonMono<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        private static T s_cInstance;
        public static T Instance
        {
            get
            {
                if (s_cInstance == null)
                {
                    try
                    {
                        GameObject go = new GameObject(typeof(T).Name);
                        s_cInstance = go.AddComponent<T>();
#if !UNITY_EDITOR
                    DontDestroyOnLoad(go);
#endif
                    }
                    catch (Exception ex)
                    {
                        GuLog.Error(string.Format("====SingletonMono.Instance: {0}", ex.Message));
                    }
                }
                return s_cInstance;
            }
        }

        void Awake()
        {
            init();
        }

        void OnDestroy()
        {
            destory();
        }

        //destory
        protected virtual void destory()
        {
            s_cInstance = null;
        }

        //init
        public virtual void init() { }
    }
}
