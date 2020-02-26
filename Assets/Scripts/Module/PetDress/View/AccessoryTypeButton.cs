using Gululu.Config;
using Gululu.Util;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hank.PetDress
{
    /// <summary>
    /// 配饰类型按钮
    /// </summary>
    public class AccessoryTypeButton : MonoBehaviour
    {
        /************************************************属性与变量命名************************************************/
        public int AccessoryType { get; private set; }//场景编码
        [SerializeField]
        private Image borderBox;//边框
        [SerializeField]
        private Image iconBox;//图标
        [SerializeField]
        private Sprite normalBorderImage;//常态边框贴图
        [SerializeField]
        private Sprite largeBorderImage;//选中时边框贴图
        private Sprite normalIconImage;//常态图标贴图
        private Sprite largeIconImage;//选中时图标贴图
        /************************************************Unity方法与事件***********************************************/

        /************************************************自 定 义 方 法************************************************/
        //初始化
        public void Initialize(int accessoryType, Sprite normalIconImage, Sprite largeIconImage)
        {
            this.AccessoryType = accessoryType;
            this.normalIconImage = normalIconImage;
            this.largeIconImage = largeIconImage;
        }
        //设置按钮状态
        public void SetStatus(bool selected)
        {
            this.borderBox.sprite = selected ? this.largeBorderImage : this.normalBorderImage;
            this.iconBox.sprite = selected ? this.largeIconImage : this.normalIconImage;
        }
    }
}