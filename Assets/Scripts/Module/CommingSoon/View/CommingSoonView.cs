using Gululu;
using UnityEngine;

namespace Hank.CommingSoon
{
    public class CommingSoonView : BaseView
    {
        [Inject]
        public II18NUtils I18NUtils { get; set; }
        [SerializeField]
        Spine.Unity.SkeletonGraphic spine;

        protected override void Start()
        {
            base.Start();
            this.I18NUtils.SetAnimation(this.spine, "CommingSoon");
            SoundPlayer.GetInstance().PlaySoundType("comming_soon");
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
