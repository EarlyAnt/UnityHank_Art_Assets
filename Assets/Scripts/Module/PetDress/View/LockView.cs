using UnityEngine;

namespace Hank.PetDress
{
    /// <summary>
    /// 等级锁定页面
    /// </summary>
    public abstract class LockView : PopBaseView
    {
        /************************************************属性与变量命名************************************************/
        [SerializeField]
        protected AudioSource audioPlayer;
        /************************************************Unity方法与事件***********************************************/
        protected override void Start()
        {
        }
        /************************************************自 定 义 方 法************************************************/
        //打开页面
        public override void Open(object param = null)
        {
            base.Open();

            if (this.audioPlayer == null)
                this.audioPlayer = this.gameObject.AddComponent<AudioSource>();

            RoleManager.Instance.Hide();
            SoundPlayer.GetInstance().StopAllSoundExceptMusic();
        }
        //关闭页面
        public override void Close()
        {
            base.Close(() => this.OnViewClosed(ViewResult.OK));
        }
        //当页面关闭时
        protected override void OnViewClosed(ViewResult viewResult)
        {
            this.audioPlayer.Stop();
            RoleManager.Instance.Show();
            base.OnViewClosed(viewResult);
        }
    }
}
