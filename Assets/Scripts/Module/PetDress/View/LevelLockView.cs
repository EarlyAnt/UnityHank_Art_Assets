using Gululu.Config;
using Gululu.Util;
using UnityEngine;

namespace Hank.PetDress
{
    /// <summary>
    /// 等级锁定页面
    /// </summary>
    public class LevelLockView : LockView
    {
        /************************************************属性与变量命名************************************************/
        [Inject]
        public IPrefabRoot PrefabRoot { get; set; }
        [SerializeField]
        private TextLabel lockBox;
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
                this.lockBox.SetText("LV" + accessory.Level);
            }
            SoundPlayer.GetInstance().PlaySoundInChannal("popup_goods_level_lock", this.audioPlayer, 0.2f);
        }
    }
}
