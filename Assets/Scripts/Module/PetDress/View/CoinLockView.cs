using UnityEngine;

namespace Hank.PetDress
{
    /// <summary>
    /// 金币锁定页面
    /// </summary>
    public class CoinLockView : LockView
    {
        /************************************************属性与变量命名************************************************/
        [Inject]
        public II18NUtils I18NUtils { get; set; }
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
            this.I18NUtils.SetText(this.lockBox.Text, "Feed_No_Coin", true);
            SoundPlayer.GetInstance().PlaySoundInChannal("no_coin", this.audioPlayer, 0.2f);

        }
    }
}
