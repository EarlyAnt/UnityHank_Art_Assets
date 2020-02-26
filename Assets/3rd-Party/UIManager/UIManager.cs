using UnityEngine;
using System.Collections; 
using System.Collections.Generic;
using System.IO;

namespace UIManage
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager manager;

        //private static string Logging = "Logging";

        public static UIManager instance()
        {
            if (manager == null)
            {
                //UnityAppender.ConfigureAllLogging();
                manager = new UIManager();
            }
            return manager;
        }

        public Transform UIParent;

        public string ResourceDir = "";

        public string FirstPage = "";

        // 存放所有面板的栈    
        private Stack<UIBase> UIStack = new Stack<UIBase>();

        private Dictionary<string, UIBase> currentUIDict = new Dictionary<string, UIBase>();

        // 名字 ---- 面板的prefab
        private Dictionary<string, GameObject> UIObjectDict = new Dictionary<string, GameObject>();

        void Awake()
        {
            manager = this;
            LoadAllUIObject();
            PushUIPanel(FirstPage);
        }

        // 入栈 把界面显示出来
        public void PushUIPanel(string UIName)
        {
            if (UIStack.Count > 0)
            {
                UIBase old_topUI = UIStack.Peek();
                old_topUI.DoOnPausing();
            }

            UIBase new_topUI = GetUIBase(UIName);
            new_topUI.DoOnEntering();
            UIStack.Push(new_topUI);
        }

        private UIBase GetUIBase(string UIName)
        {
            foreach (var name in currentUIDict.Keys)
            {
                if (name == UIName)
                {
                    UIBase u = currentUIDict[UIName];
                    return u;
                }
            }

            //如果没有　就先得到面板的ｐｒｅｆａｂ
            GameObject UIPrefab = UIObjectDict[UIName];

            GameObject UIObject = GameObject.Instantiate<GameObject>(UIPrefab);
            //创建面板
            UIObject.transform.SetParent(UIParent, false);
            //UIBase.Instantiate();
            UIBase uibase = UIObject.GetComponent<UIBase>();
            currentUIDict.Add(UIName, uibase);
            return uibase;
        }

        //出栈 把界面隐藏
        public void PopUIPanel()
        {
            if (UIStack.Count == 0)
                return;
            Debug.Log(UIStack.Count);

            UIBase old_topUI = UIStack.Pop();
            old_topUI.DoOnExiting();
            Debug.Log(UIStack.Count);

            if (UIStack.Count > 0)
            {
                UIBase new_topUI = UIStack.Peek();
                new_topUI.DoOnResuming();
            }

        }

        private void LoadAllUIObject()
        {
            string path = Application.dataPath + "/Resources/Prefabs/" + ResourceDir;
            DirectoryInfo folder = new DirectoryInfo(path);
            foreach (FileInfo file in folder.GetFiles("*.prefab"))
            {
                int index = file.Name.LastIndexOf('.');
                string UIName = file.Name.Substring(0, index);
                string UIPath = "Prefabs/" + ResourceDir + "/" + UIName;
                GameObject UIObject = Resources.Load<GameObject>(UIPath);

                if (UIObject == null){
                    Debug.LogWarning("add null gameobject");
                }
                UIObjectDict.Add(UIName, UIObject);
            }
        }
    }

}