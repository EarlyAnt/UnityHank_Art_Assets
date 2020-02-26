using Gululu.Config;
using Gululu.net;
using Hank.Api;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Hank.PetDress
{
    /// <summary>
    /// 提示页面(解锁条件提示)
    /// </summary>
    public class QRCodeView : PopBaseView
    {
        /************************************************属性与变量命名************************************************/
        [Inject]
        public II18NConfig I18NConfig { get; set; }
        [Inject]
        public II18NUtils I18NUtils { get; set; }
        [Inject]
        public IQRCodeUtils QRCodeUtils { get; set; }
        [Inject]
        public IPetAccessoryUtils PetAccessoryUtils { get; set; }
        [SerializeField]
        private Image QRCode;
        [SerializeField, Range(0f, 5f)]
        private float delayClick = 2f;
        private DateTime beginTime;
        private OrderReceipt orderReceipt;
        private AudioSource audioPlayer;
        /************************************************Unity方法与事件***********************************************/

        /************************************************自 定 义 方 法************************************************/
        //打开页面
        public override void Open(object param = null)
        {
            base.Open();
            this.beginTime = DateTime.Now;

            if (this.audioPlayer == null)
                this.audioPlayer = this.gameObject.AddComponent<AudioSource>();

            SoundPlayer.GetInstance().StopAllSoundExceptMusic();
            SoundPlayer.GetInstance().PlaySoundInChannal("mall_purchase_qr", this.audioPlayer, 0.2f);

            if (param != null && this.QRCode != null)
            {
                this.orderReceipt = param as OrderReceipt;
                if (this.orderReceipt == null) return;
                Debug.LogFormat("<><QRCodeView.Open>orderSN: {0}", this.orderReceipt.order_sn);
                Texture2D texture = this.QRCodeUtils.GenerateQRImageFreeSize(this.orderReceipt.code_url, 256, 256);
                if (texture != null) Debug.LogFormat("<><QRCodeView.Open>Texture, width: {0}, height: {1}", texture.width, texture.height);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                if (sprite != null) Debug.LogFormat("<><QRCodeView.Open>Sprite, rect: {0}", sprite.rect);
                this.QRCode.sprite = sprite;
                this.InvokeRepeating("GetPaymentResult", 0, 2f);
            }
            RoleManager.Instance.Hide();
        }
        //关闭页面
        public override void Close()
        {
            if ((DateTime.Now - this.beginTime).TotalSeconds < this.delayClick)
                return;

            base.Close(() => this.OnViewClosed(ViewResult.Cancel));
        }
        //定时轮询付款状态
        private void GetPaymentResult()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            this.PetAccessoryUtils.GetPaymentResult(this.orderReceipt.order_sn, (response) =>
            {
                Debug.LogFormat("<><QRCodeView.GetPaymentResult>response: {0}", response);
                if (response != null && !string.IsNullOrEmpty(response.status) && response.order != null)
                {
                    string status = response.status.ToUpper();
                    if (status == "OK" && response.order.order_status == OrderInfo.ORDER_STATUS_PAID)
                    {//只有付款成功，才关闭并返回配饰页
                        Debug.LogFormat("<><QRCodeView.GetPaymentResult>Result, status: {0}, order_status: {1}", response.status, response.order.order_status);
                        base.Close(() => this.OnViewClosed(ViewResult.OK));
                    }
                }
            },
            (errorText) =>
            {
                Debug.LogErrorFormat("<><QRCodeView.GetPaymentResult>Error: {0}", errorText);
                //base.Close(() => this.OnViewClosed(ViewResult.Cancel));
            });
#endif
        }
        //当页面关闭时
        protected override void OnViewClosed(ViewResult viewResult)
        {
            this.CancelInvoke();
            RoleManager.Instance.Show();
            this.audioPlayer.Stop();
            if (this.ViewClosed != null)
            {
                this.ViewClosed(viewResult);
            }
        }
    }
}
