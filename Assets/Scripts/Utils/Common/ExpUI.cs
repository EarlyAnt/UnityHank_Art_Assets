using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Gululu.Util;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class ExpUI : MonoBehaviour 
{
    private static ExpUI instance;

    [SerializeField]
    private CanvasGroup mCanvasGroup;

    [SerializeField]
    private GameObject mCoin;

    [SerializeField]
    private GameObject mExp;

    [SerializeField]
    private Text mTxtExp;

    [SerializeField]
    private Text mTxtCoin;

    private ExpUI()
    {
    }

    public static ExpUI Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = Instantiate(Resources.Load("UI/ExpUI") as GameObject);

                go.transform.localPosition = Vector3.zero;
                
                go.transform.localScale = Vector3.one;

                instance = go.GetComponent<ExpUI>();

                if (instance == null)
                {
                    instance = go.AddComponent<ExpUI>();
                }
                
                go.SetActive(false);
                
            }
            return instance;
        }
    }

    public void Show(int exp = 0,int coin = 0,float fromPosY = 0,float toPosY = 0,Transform parent = null,Action onComplete = null)
    {
        Assert.IsNotNull(parent);
        this.gameObject.transform.SetParent(parent);
        this.transform.localPosition = Vector3.zero;
        this.transform.localScale = Vector3.one;
        this.gameObject.SetActive(true);
        mCoin.SetActive(coin > 0);
        mExp.SetActive(exp > 0);

        mCanvasGroup.transform.DOLocalMoveY(fromPosY, 0.01f);
        
        int textLength = Mathf.Max(exp.ToString().Length, coin.ToString().Length);
        
        mTxtExp.text = "+" + exp.ToString().PadLeft(textLength, ' ');
        
        mTxtCoin.text = "+" + coin.ToString().PadLeft(textLength, ' ');
        
        mTxtExp.DOFade(1,0.01f);
        
        mTxtCoin.DOFade(1,0.01f);
        
        mCanvasGroup.DOFade(1f,0.01f).onComplete = () =>
        {
            mCanvasGroup.DOFade(0.5f, 0.8f).SetLoops(3);
        };

        mCanvasGroup.transform.DOLocalMoveY(toPosY, 2.4f).onComplete = delegate ()
        {
            mCanvasGroup.DOFade(0, 1.0f).onComplete = delegate ()
            {
                mCanvasGroup.transform.DOMoveY(0, 0.5f);
            };

            if (onComplete != null)
            {
                onComplete.Invoke();
                Destroy(gameObject);
            }
        };
    }
}
