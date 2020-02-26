using DG.Tweening;
using Gululu.Config;
using Gululu.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Hank.PetDress
{
    /// <summary>
    /// 配饰按钮
    /// </summary>
    public class AccessoryButton : MonoBehaviour
    {
        /************************************************属性与变量命名************************************************/
        public Accessory Accessory { get; private set; }
        [SerializeField]
        private Image iconBox;//配饰图标
        [SerializeField]
        private TextLabel priceBox;//价格标签
        [SerializeField]
        private TextLabel gotBox;//已拥有标签
        [SerializeField]
        private ImageLabel cashBox;//钞票图标
        [SerializeField]
        private ImageLabel limitBox;//限制标签
        [SerializeField]
        private ImageLabel levelLockBox;//等级锁图标
        [SerializeField]
        private ImageLabel suitLockBox;//适配锁图标
        [SerializeField]
        private ImageLabel tickBox;//对勾图标
        [SerializeField, Range(1f, 2f)]
        private float sizeRate = 1.5f;
        [SerializeField, Range(0f, 2f)]
        private float lerpTime = 0.3f;
        private Sprite normalIconImage;//常态图标贴图
        private Sprite largeIconImage;//选中时图标贴图
        private bool paid;
        public bool Paid
        {
            get
            {
                return this.paid;
            }
            set
            {
                this.paid = value;
                this.gotBox.SetStatus(value);
                this.priceBox.SetStatus(!value && this.Accessory.Coin);
                this.cashBox.SetStatus(!value && this.Accessory.Cash);
                if (value) this.levelLockBox.SetStatus(false);
            }
        }//是否已购买(已拥有)
        private bool worn;
        public bool Worn
        {
            get
            {
                return this.worn;
            }
            set
            {
                this.worn = value;
                this.tickBox.SetStatus(value);
                if (value) this.levelLockBox.SetStatus(false);
            }
        }//是否已穿上
        private bool levelLlocked;
        public bool LevelLocked
        {
            get
            {
                return this.levelLlocked;
            }
            set
            {
                this.levelLlocked = value;
                this.levelLockBox.SetStatus(value && !this.suitLlocked);
                if (value || this.SuitLocked)
                {
                    this.priceBox.SetStatus(false);
                    this.cashBox.SetStatus(false);
                    this.gotBox.SetStatus(false);
                }
            }
        }//等级锁
        private bool suitLlocked;
        public bool SuitLocked
        {
            get
            {
                return this.suitLlocked;
            }
            set
            {
                this.suitLlocked = value;
                this.suitLockBox.SetStatus(value);
                if (value || this.LevelLocked)
                {
                    this.priceBox.SetStatus(false);
                    this.cashBox.SetStatus(false);
                    this.gotBox.SetStatus(false);
                }
            }
        }//适配锁
        /************************************************Unity方法与事件***********************************************/
        private void Start()
        {
            this.priceBox.Initialize(this.sizeRate, this.lerpTime);
            this.cashBox.Initialize(this.sizeRate, this.lerpTime);
            this.gotBox.Initialize(this.sizeRate, this.lerpTime);
            this.tickBox.Initialize(this.sizeRate, this.lerpTime);
            this.limitBox.Initialize(this.sizeRate, this.lerpTime);
            this.levelLockBox.Initialize(this.sizeRate, this.lerpTime);
            this.suitLockBox.Initialize(this.sizeRate, this.lerpTime);
        }
        /************************************************自 定 义 方 法************************************************/
        //初始化
        public void Initialize(Accessory accessory, Sprite normalIconImage, Sprite largeIconImage, bool levelLocked, bool suitLocked, bool paid, bool worn)
        {
            this.Accessory = accessory;
            this.normalIconImage = normalIconImage;
            this.largeIconImage = largeIconImage;

            this.limitBox.SetStatus(false);
            //this.priceBox.SetStatus(!levelLocked && !suitLocked && !this.Paid && this.Accessory.Coin);
            this.priceBox.SetText(accessory.Price.ToString());
            //this.cashBox.SetStatus(!levelLocked && !suitLocked && !this.Paid && this.Accessory.Cash);

            this.Paid = paid;
            this.Worn = worn;
            this.SuitLocked = suitLocked;
            this.LevelLocked = levelLocked;
        }
        //设置按钮状态
        public void SetStatus(bool selected, bool immediately = false)
        {
            this.iconBox.sprite = selected ? this.largeIconImage : this.normalIconImage;            
            this.priceBox.SetScale(selected ? Vector3.one * 1.5f : Vector3.one);
            this.cashBox.SetScale(selected ? Vector3.one * 1.5f : Vector3.one);
            this.gotBox.SetScale(selected ? Vector3.one * 1.5f : Vector3.one);
            this.limitBox.SetScale(selected ? Vector3.one * 1.5f : Vector3.one);
            this.tickBox.SetScale(selected ? Vector3.one * 1.5f : Vector3.one);
            this.levelLockBox.SetScale(selected ? Vector3.one * 1.5f : Vector3.one);
            this.suitLockBox.SetScale(selected ? Vector3.one * 1.5f : Vector3.one);
        }
        //设置文本
        public void SetText(II18NConfig i18nConfig)
        {
            this.gotBox.SetText(i18nConfig.GetText("已拥有"));
        }
    }
}