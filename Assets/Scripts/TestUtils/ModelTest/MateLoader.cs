using Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ModelTest
{
    public class MateLoader : BaseLoader
    {
        /************************************************属性与变量命名************************************************/
        [SerializeField]
        private MateInfo topLeftMate;
        [SerializeField]
        private MateInfo bottomRightMate;
        private Dictionary<AccessoryButton, MateInfo> mates = new Dictionary<AccessoryButton, MateInfo>();
        /************************************************Unity方法与事件***********************************************/

        /************************************************自 定 义 方 法************************************************/
        public void SetMate(AccessoryButton accessoryButton)
        {
            if (accessoryButton == null)
            {
                Debug.LogError("<><MateLoader.LoadMate>Parameter 'accessoryButton' is null");
                return;
            }

            if (!accessoryButton.CanWear)
            {
                if (!this.mates.ContainsKey(accessoryButton))
                {
                    foreach (KeyValuePair<AccessoryButton, MateInfo> kvp in this.mates.ToArray())
                    {//卸载同位置的小精灵
                        if (kvp.Key.Region == accessoryButton.Region)
                            this.UnloadMate(kvp.Key);
                    }

                    this.LoadMate(accessoryButton);
                }
                else
                {
                    this.UnloadMate(accessoryButton);
                }
            }
        }
        private void LoadMate(AccessoryButton accessoryButton)
        {
            this.UnloadMate(accessoryButton);

            MateInfo mateInfo = this.GetMateInfo(accessoryButton);
            AssetBundle assetBundle = this.GetAssetBundle(accessoryButton.AB);
            Object obj = assetBundle.LoadAsset(accessoryButton.Prefab);
            mateInfo.Object = GameObject.Instantiate(obj) as GameObject;
            mateInfo.Object.name = string.Format("{0}(Clone)", accessoryButton.name);
            mateInfo.Object.transform.SetParent(mateInfo.Root, false);
            mateInfo.Object.AddComponent<FixShader>();
            mateInfo.Animator = mateInfo.Object.GetComponent<Animator>();
            if (mateInfo.Coroutine != null)
                this.StopCoroutine(mateInfo.Coroutine);
            mateInfo.Coroutine = this.StartCoroutine(this.RandomAnimation(mateInfo));
            this.mates.Add(accessoryButton, mateInfo);
        }
        private void UnloadMate(AccessoryButton accessoryButton)
        {
            if (this.mates.ContainsKey(accessoryButton))
            {
                if (this.mates[accessoryButton] != null)
                    GameObject.Destroy(this.mates[accessoryButton].Object);
                this.mates.Remove(accessoryButton);
            }

            if (assetBundles.ContainsKey(accessoryButton.AB) && assetBundles[accessoryButton.AB] != null)
                assetBundles[accessoryButton.AB].Unload(true);

            Resources.UnloadUnusedAssets();
        }
        private MateInfo GetMateInfo(AccessoryButton accessoryButton)
        {
            if (accessoryButton.Region == Regions.TopLeft)
                return this.topLeftMate;
            else
                return this.bottomRightMate;
        }
        private IEnumerator RandomAnimation(MateInfo mateInfo)
        {
            if (mateInfo.Animator == null)
                yield break;

            float time = 0;
            while (!mateInfo.StopAnimation)
            {
                time = 0;
                float interval = Random.Range(mateInfo.IntervalMin, mateInfo.IntervalMax);
                while (time < interval && !mateInfo.StopAnimation)
                {
                    time += Time.deltaTime;
                    yield return null;
                }

                if (mateInfo.Animator != null && mateInfo.Animator.runtimeAnimatorController != null && mateInfo.Animator.runtimeAnimatorController.animationClips != null)
                {
                    int state = Random.Range(1, mateInfo.Animator.runtimeAnimatorController.animationClips.Length);
                    mateInfo.Animator.SetInteger("State", state);
                    mateInfo.Animator.SetTrigger("Jump");
                }
            }
        }
    }

    [System.Serializable]
    public class MateInfo
    {
        public Transform Root;
        public Regions Region;
        public GameObject Object;
        public Animator Animator;
        [Range(0f, 60f)]
        public float IntervalMin = 10.0f;
        [Range(0f, 60f)]
        public float IntervalMax = 30.0f;
        public bool StopAnimation;
        public Coroutine Coroutine;


    }
}
