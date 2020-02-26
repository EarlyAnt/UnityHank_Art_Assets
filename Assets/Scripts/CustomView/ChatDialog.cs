using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using strange.extensions.mediation.impl;
using UnityEngine.UI;
using Gululu;
using UnityEngine.Assertions;
using DG.Tweening;
using Hank.Util;

public class ChatDialog : BaseView {
    [SerializeField]
    ChatItem[] objChatItemProfabs;
    [SerializeField]
    ScrollRect scrollRect;
    [SerializeField]
    GameObject scrollContent;
    [SerializeField]
    GameObject objControl;
    [SerializeField]
    string testChat;
    [SerializeField]
    ChatItemType testChatType;

    List<ChatItem> listAllObject = new List<ChatItem>();

    float fStartYContent = 0;

    ChatItem lastChatItem = null;

    public override void TestSerializeField()
    {
        Assert.AreEqual(objChatItemProfabs.Length, (int)ChatItemType.TotalChatItemType);
        for (int i = 0; i < objChatItemProfabs.Length; ++i)
        {
            Assert.IsNotNull(objChatItemProfabs[i]);
        }
        Assert.IsNotNull(scrollRect);
        Assert.IsNotNull(scrollContent);
        Assert.IsNotNull(objControl);
    }

    // Use this for initialization
    override protected void Start () {
        base.Start();
        fStartYContent = scrollContent.GetComponent<RectTransform>().localPosition.y;
        objControl.SetActive(false);
#if UNITY_EDITOR
        objControl.SetActive(true);
#endif
    }

    public void AddChatItem(ChatItemType type,object param)
    {
        if((int)type >= objChatItemProfabs.Length)
        {
            return;
        }
        ChatItem objSelf = null;
        for (int i = 0; i < listAllObject.Count; ++i)
        {
            ChatItem obj = listAllObject[i];
            if ((!obj.gameObject.activeSelf) && obj.GetChatItemType() == type)
            {
                objSelf = obj;
                break;
            }
        }
        if (objSelf == null)
        {
            objSelf = Object.Instantiate(objChatItemProfabs[(int)type].gameObject).GetComponent<ChatItem>();
            listAllObject.Add(objSelf);
        }
        objSelf.gameObject.SetActive(true);
        objSelf.transform.parent = scrollContent.transform;
        lastChatItem = objSelf;
        objSelf.ShowItem(param);
        scrollToBottom();
    }
    public void TestChatItem()
    {
        AddChatItem(testChatType, testChat);
    }
    public void AddSelfChat(string strChat)
    {
        if(lastChatItem != null)
        {
            if (lastChatItem.GetChatItemType() == ChatItemType.SelfChatItem)
            {
                lastChatItem.ShowItem(strChat);
                return;
            }
        }

        AddChatItem(ChatItemType.SelfChatItem, strChat);
    }
    public void AddOtherChat(string strChat)
    {
        AddChatItem(ChatItemType.OtherChatItem, strChat);
    }
    bool bNeedUpdate = false;
    // Update is called once per frame
    void Update()
    {
        if(bNeedUpdate)
        {
            bNeedUpdate = false;
        }

    }

    public void scrollToBottom()
    {

        RectTransform selfRectTransform = scrollContent.GetComponent<RectTransform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(selfRectTransform);
        float moveHight = selfRectTransform.rect.height - scrollRect.GetComponent<RectTransform>().rect.height;
        if (moveHight > 0)
        {
            selfRectTransform.DOKill();
            selfRectTransform.DOLocalMoveY(moveHight + fStartYContent,0.5f).onComplete = delegate
            {                
                bNeedUpdate = true;
                List<ChatItem> noVisibleList = new List<ChatItem>();
                for (int i = 0; i < listAllObject.Count; ++i)
                {
                    ChatItem obj = listAllObject[i];
                    if (obj.gameObject.activeSelf && !IsNeedRender(obj.gameObject))
                    {
                        noVisibleList.Add(obj);
                    }
                }
                float adjustTotal = 0;
                for (int i = 0; i < noVisibleList.Count; ++i)
                {
                    ChatItem obj = noVisibleList[i];
                    float adjust = obj.GetComponent<RectTransform>().rect.height + scrollContent.GetComponent<VerticalLayoutGroup>().spacing;
                    adjustTotal += adjust;
                    if(obj.CanDestory())
                    {
                        listAllObject.Remove(obj);
                        Destroy(obj.gameObject);
                    }
                    else
                    {
                        obj.transform.parent = null;
                        obj.gameObject.SetActive(false);
                    }
                }
                selfRectTransform.localPosition = new Vector3(selfRectTransform.localPosition.x, moveHight - adjustTotal + fStartYContent, selfRectTransform.localPosition.z);
            };
        }

    }

    Vector3[] vecResult = new Vector3[4];
    private bool IsNeedRender(GameObject objParent)
    {
        objParent.GetComponent<RectTransform>().GetWorldCorners(vecResult);
        Rect parentRect = new Rect(vecResult[0].x, vecResult[0].y, vecResult[2].x - vecResult[0].x, vecResult[2].y - vecResult[0].y);
        scrollRect.GetComponent<RectTransform>().GetWorldCorners(vecResult);
        Rect rectPanel = new Rect(vecResult[0].x, vecResult[0].y, vecResult[2].x - vecResult[0].x, vecResult[2].y - vecResult[0].y);

        return rectPanel.Overlaps(parentRect);
    }

}
