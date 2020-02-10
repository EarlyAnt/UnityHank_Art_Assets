using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ModelTest
{
    /// <summary>
    /// 控制面板
    /// </summary>
    public class PetControl : MonoBehaviour
    {
        /************************************************属性与变量命名************************************************/
        [SerializeField]
        private PetLoader petLoader;
        /************************************************Unity方法与事件***********************************************/
        private void Start()
        {

        }
        /************************************************自 定 义 方 法************************************************/
        public void PlayAnimation(Button animationButton)
        {
            if (animationButton == null)
            {
                Debug.LogError("<><PetControl.PlayAnimation>Parameter 'animationButton' is null");
                return;
            }
            else if (this.petLoader == null || this.petLoader.PetPlayer == null)
            {
                Debug.LogError("<><PetControl.PlayAnimation>Parameter 'petLoader' or 'petLoader.PetPlayer' is null");
                return;
            }

            Type type = this.petLoader.PetPlayer.GetType();
            MethodInfo[] methodInfos = type.GetMethods();
            for (int i = 0; i < methodInfos.Length; i++)
            {
                if (methodInfos[i].Name == animationButton.name)
                {
                    methodInfos[i].Invoke(this.petLoader.PetPlayer, null);
                    break;
                }
            }
        }
    }
}
