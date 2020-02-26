using DG.Tweening;
using Gululu;
using Gululu.Events;
using strange.extensions.mediation.impl;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hank.CommingSoon
{
    public class CommingSoonMediator : BaseMediator
    {
        [Inject]
        public CommingSoonView CommingSoonView { get; set; }

        public override void OnRegister()
        {
            this.View = TopViewHelper.ETopView.eCommingSoon;
            base.OnRegister();
        }

        public override void OnRemove()
        {
            base.OnRemove();
        }

        protected override void PowerClick()
        {
            this.Close();
        }

        protected override void CupShake()
        {
            this.Close();
        }

        private void Close()
        {
            this.CommingSoonView.Close();
            dispatcher.Dispatch(MainEvent.CloseCommingSoon);
        }
    }
}