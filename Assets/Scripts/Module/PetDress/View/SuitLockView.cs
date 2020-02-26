using Gululu.Config;
using Gululu.Util;
using UnityEngine;

namespace Hank.PetDress
{
    /// <summary>
    /// 适用锁定页面
    /// </summary>
    public class SuitLockView : LockView
    {
        /************************************************属性与变量命名************************************************/
        [Inject]
        public IPrefabRoot PrefabRoot { get; set; }
        [SerializeField]
        private ImageLabel lockBox;
        /************************************************Unity方法与事件***********************************************/
        protected override void Start()
        {
        }
        /************************************************自 定 义 方 法************************************************/
        //打开页面
        public override void Open(object param = null)
        {
            base.Open();

            Accessory accessory = param as Accessory;
            this.lockBox.SetStatus(true);
            if (accessory != null)
            {
                this.lockBox.SetIcon(this.PrefabRoot.GetObjectNoInstantiate<Sprite>("Texture/PetDress/Accessories", accessory.Icon + "_big"));
            }
            SoundPlayer.GetInstance().PlaySoundInChannal("mall_suit_not_match", this.audioPlayer, 0.2f);
        }
    }
}
