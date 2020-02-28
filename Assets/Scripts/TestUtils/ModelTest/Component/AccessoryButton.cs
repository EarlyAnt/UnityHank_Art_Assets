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
        public string AB;
        public string Prefab;
        public Regions Region;
        public string Icon;
        public bool CanWear;
        private Image imageBox;
        public Image ImageBox
        {
            get
            {
                if (this.imageBox == null)
                {
                    Transform child = this.transform.Find("Icon");
                    if (child == null)
                    {
                        Debug.LogError("<><AccessoryButton.ImageBox>Child node 'Icon' is not existed");
                        return null;
                    }
                    this.imageBox = child.GetComponent<Image>();
                }
                return this.imageBox;
            }
        }
        /************************************************Unity方法与事件***********************************************/

        /************************************************自 定 义 方 法************************************************/
        [ContextMenu("设置图标")]
        private void SetIcon()
        {
            if (this.ImageBox != null)
                this.ImageBox.sprite = Resources.Load<Sprite>(string.Format("Texture/PetDress/Accessories/{0}_big", this.Icon));
        }
    }
}
