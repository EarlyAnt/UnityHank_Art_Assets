using UnityEngine;

namespace Hank.PetDress
{
    /// <summary>
    /// 网络锁定页面
    /// </summary>
    public class WifiLockView : LockView
    {
        /************************************************属性与变量命名************************************************/
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

            this.lockBox.SetStatus(true);
            SoundPlayer.GetInstance().PlaySoundInChannal("popup_no_network", this.audioPlayer, 0.2f);
        }
    }
}
