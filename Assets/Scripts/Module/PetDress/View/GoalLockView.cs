using UnityEngine;

namespace Hank.PetDress
{
    /// <summary>
    /// 每日饮水量锁定页面
    /// </summary>
    public class GoalLockView : LockView
    {
        /************************************************属性与变量命名************************************************/
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
            
            this.lockBox.SetStatus(true);
            SoundPlayer.GetInstance().PlaySoundInChannal("mall_purchase_lock", this.audioPlayer, 0.2f);
        }
    }
}
