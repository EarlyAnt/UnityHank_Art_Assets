using Gululu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Hank.Util
{

    public enum ChatItemType
    {
        SelfChatItem,
        OtherChatItem,
        SpeakRemindHelp,
        TotalChatItemType
    }
    public class ChatItem : BaseView
    {
        [SerializeField]
        ChatItemType type = ChatItemType.OtherChatItem;

        public ChatItemType GetChatItemType()
        {
            return type;
        }

        virtual public void ShowItem(object strChat)
        {
        }
        virtual public bool CanDestory()
        {
            return true;
        }
    }
}


