using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Text))]
public class ScrollText : MonoBehaviour {

    Text m_TxtMsg;//跑马灯text.
    [SerializeField]
    float speed = 200f;
    [SerializeField]
    int loop = 3;

    //Queue<string> m_MsgQueue;//灯队列.
    string strConcent;
    //Font m_Font;
    float beginX;
    float width;
    bool isScrolling = false;//判断当前text中的跑马灯是否跑完.
    private void Start()
    {
        Init();
    }
    public void Init()
    {
        m_TxtMsg = GetComponent<Text>();
        beginX = m_TxtMsg.transform.position.x;
        width = m_TxtMsg.gameObject.GetComponent<RectTransform>().sizeDelta.x;
        //         m_MsgQueue = new Queue<string>();
    }
    /// <summary>
    /// 添加跑马灯信息.
    /// </summary>
    /// <param name="msg"></param>
    public void AddMessage(string msg)
    {
        strConcent = msg;
        //m_MsgQueue.Enqueue(msg);
    }

    Coroutine myCoroutine = null;
    public void StartScrolling()
    {
//         if (isScrolling) return;

        StopCoroutine();
        myCoroutine = StartCoroutine(Scrolling());
    }
    public void StopCoroutine()
    {
        if(myCoroutine != null)
        {
            StopCoroutine(myCoroutine);
            myCoroutine = null;
        }
    }
    public IEnumerator Scrolling()
    {
        //while (m_MsgQueue.Count > 0)
        {
            //string msg = m_MsgQueue.Dequeue();
            if(strConcent != null && strConcent.Length > 0)
            {
                m_TxtMsg.text = strConcent;
            }
            float txtWidth = m_TxtMsg.preferredWidth;//文本自身的长度.
            m_TxtMsg.alignment = TextAnchor.MiddleLeft;
            Vector3 pos = m_TxtMsg.rectTransform.localPosition;
            float distance = txtWidth + width;
            float duration = distance / speed;
            isScrolling = true;
            int count = loop;
            while (count-- > 0)
            {
                m_TxtMsg.rectTransform.localPosition = new Vector3(beginX, pos.y, pos.z);
                m_TxtMsg.rectTransform.DOLocalMoveX(beginX - distance, duration).SetEase(Ease.Linear);
                yield return new WaitForSeconds(duration);
            }
            isScrolling = false;
            myCoroutine = null;

        }
    }

    public void StartPingPong()
    {
        StopCoroutine();
        myCoroutine = StartCoroutine(PingPong());

    }

    public IEnumerator PingPong()
    {
        bool btoLeft = true;
        //while (m_MsgQueue.Count > 0)
        {
            //string msg = m_MsgQueue.Dequeue();
            if (strConcent != null && strConcent.Length > 0)
            {
                m_TxtMsg.text = strConcent;
            }
            float txtWidth = m_TxtMsg.preferredWidth;//文本自身的长度.
            if (txtWidth < width)
            {
                m_TxtMsg.alignment = TextAnchor.MiddleCenter;
                myCoroutine = null;
            }
            else
            {
                m_TxtMsg.alignment = TextAnchor.MiddleLeft;
                Vector3 pos = m_TxtMsg.rectTransform.localPosition;
                float distance = txtWidth - width;
                float duration = distance / speed;
                isScrolling = true;
                while (true)
                {
                    if (btoLeft)
                    {
                        btoLeft = false;
                        m_TxtMsg.rectTransform.localPosition = new Vector3(beginX, pos.y, pos.z);
                        m_TxtMsg.rectTransform.DOLocalMoveX(beginX - distance, duration).SetEase(Ease.Linear);
                    }
                    else
                    {
                        btoLeft = true;
                        m_TxtMsg.rectTransform.localPosition = new Vector3(beginX - distance, pos.y, pos.z);
                        m_TxtMsg.rectTransform.DOLocalMoveX(beginX, duration).SetEase(Ease.Linear);
                    }
                    yield return new WaitForSeconds(duration);
                }
            }
        }
    }
}
