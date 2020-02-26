using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Gululu.Config
{
    public abstract class ConfigBase<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        private static T s_cInstance;
        public static T Instance
        {
            get
            {
                if (s_cInstance == null)
                {
                    GameObject go = new GameObject(typeof(T).Name);
                    s_cInstance = go.AddComponent<T>();
                    if(Application.isPlaying)
                    DontDestroyOnLoad(go);
                }
                return s_cInstance;
            }
        }

        protected bool m_LoadOk = false;
        public bool IsLoadedOK()
        {
            return m_LoadOk;
        }

    }

    public abstract class AbsConfigBase
    {
        protected bool m_LoadOk = false;
        public bool IsLoadedOK()
        {
            return m_LoadOk;
        }
    }

}
