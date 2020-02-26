using Gululu.Config;
using Gululu.Util;
using UnityEngine;

namespace Hank.PetDress
{
    /// <summary>
    /// 未开放锁定页面
    /// </summary>
    public class OpenLockView : LockView
    {
        /************************************************属性与变量命名************************************************/
        [Inject]
        public IPrefabRoot PrefabRoot { get; set; }
        [SerializeField]
        private ImageLabel lockBox;
        [SerializeField]
        private TimerView timerView;
        /************************************************Unity方法与事件***********************************************/
        protected override void Start()
        {
            this.timerView.OnCompleted = () => this.Close();
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
                this.timerView.StartTimer(accessory.StartTime);
            }
            SoundPlayer.GetInstance().PlaySoundInChannal("mall_lock_time", this.audioPlayer, 0.2f);
        }
        //关闭页面
        public override void Close()
        {
            this.timerView.StopTimer();
            base.Close();
        }
    }
}
