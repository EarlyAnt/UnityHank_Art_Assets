using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hank.MainScene
{
    public class DropWaterAniCallback : MonoBehaviour
    {
        [SerializeField]
        DropWaterAni dropWaterAniSC;
        public void AfterWaterDrop()
        {
            dropWaterAniSC.AfterWaterDrop();
        }

        public void AfterDropEx()
        {
            dropWaterAniSC.AfterDropEx();
        }

        public void AfterSpray()
        {
            dropWaterAniSC.AfterSpray();
        }

    }

}
