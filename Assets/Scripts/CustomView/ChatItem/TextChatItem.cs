using Gululu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Hank.Util
{
    public class TextChatItem : ChatItem
    {
        [SerializeField]
        Text textChat;
        public override void TestSerializeField()
        {
            Assert.IsNotNull(textChat);
        }

        // Use this for initialization
        override protected void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        void Update()
        {

        }
        override public void ShowItem(object strChat)
        {
            textChat.text = strChat as string;
        }
        override public bool CanDestory()
        {
            return false;
        }

    }
}

