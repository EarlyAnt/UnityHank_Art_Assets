using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ModelTest
{
    /// <summary>
    /// 配饰按钮
    /// </summary>
    public class AccessoryButton : MonoBehaviour
    {
        /************************************************属性与变量命名************************************************/
        public enum Regions { Dummy_head, Dummy_wing, Dummy_taozhuang, TopLeft, BottomRight }
        public string AB;
        public string Prefab;
        public Regions Region;
        public string Icon;
        /************************************************Unity方法与事件***********************************************/

        /************************************************自 定 义 方 法************************************************/
        [ContextMenu("设置图标")]
        private void SetIcon()
        {
            Transform child = this.transform.Find("Icon");
            if (child == null)
            {
                Debug.LogError("<><AccessoryButton.SetIcon>Child node 'Icon' is not existed");
                return;
            }

            Image image = child.GetComponent<Image>();
            if (image != null)
                image.sprite = Resources.Load<Sprite>(string.Format("Texture/PetDress/Accessories/{0}_big", this.Icon));
        }
    }
}
