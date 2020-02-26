using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Gululu.Util;

public class TopUI : MonoBehaviour 
{
    [HideInInspector]
    public delegate void VoidDelegate();
    public VoidDelegate autoCloseCallBack;

    static TopUI instance = null;

    // ui 表现
    public Image imageBg;
    public GameObject uiLoading;
    //public GameObject objNetReconnect;
	float waittingTime = 30.0f;
    Coroutine myCoroutine;
    public static TopUI Instance
    {
        get
        {
            if (instance == null)
            {
//                 GameObject gameObject = PrefabRoot.GetUniqueGameObject("UI/Common", "TopUI");
//                 if (gameObject != null)
//                 {
//                     instance = gameObject.GetComponent<TopUI>();
//                 }
            }
            return instance;
        }
    }

    static TopUI()
    {
    }

    void Awake()
    {
        uiLoading = transform.Find("loading").gameObject;
    }

	void Start () 
    {
	}

//     void ForceHide()
//     {
//         if (CallBack != null)
//             CallBack();
//         Close();
//     }
// 
//     public void ShowLoading( float fCloseTime = 0.0f ,VoidDelegate callback = null)
//     {
//         CallBack = callback;
//         gameObject.SetActive(true);
// 
//         if (fCloseTime != 0.0f)
//         {
//             waittingTime = fCloseTime;
//             StartCoroutine("HideLoadingDelay");
//         }
//         else
//         {
//             waittingTime = 30; 
//             Invoke("HideTop", waittingTime);
//         }
// 		
//     }

    IEnumerator HideLoadingDelay()
    {
        yield return new WaitForSeconds(waittingTime);
        if (autoCloseCallBack != null)
        {
            autoCloseCallBack();
        }
        Close();
    }

    public void HideTop()
    {
        autoCloseCallBack = null;
        if(myCoroutine!=null)
        {
            StopCoroutine(myCoroutine);
            myCoroutine = null;
        }
        Close();
    }

    int nTopCount = 0;

    public void Close()
    {
        --nTopCount;
        if(nTopCount == 0)
        {
            autoCloseCallBack = null;
            gameObject.SetActive(false);
        }
        else if(nTopCount < 0)
        {
            nTopCount = 0;
            GuLog.Debug("Logic error in TopUI");
        }
    }
    public void ShowTop(float fCloseTime = 30.0f,float alpha = 0.5f, VoidDelegate callbackWhenAutoClose = null)
    {
        waittingTime = fCloseTime;
        autoCloseCallBack = callbackWhenAutoClose;
        imageBg.color = new Color(imageBg.color.r, imageBg.color.g, imageBg.color.b,alpha);
        ++nTopCount;
        gameObject.SetActive(true);
        uiLoading.SetActive(true);

        if (myCoroutine != null)
        {
            StopCoroutine(myCoroutine);
        }
        myCoroutine = StartCoroutine("HideLoadingDelay");

        //objNetReconnect.SetActive(false);
    }

//     public void ShowNetReconnect()
//     {
//         gameObject.SetActive(true);
//         uiLoading.SetActive(false);
//         objNetReconnect.SetActive(true);
//     }
}
