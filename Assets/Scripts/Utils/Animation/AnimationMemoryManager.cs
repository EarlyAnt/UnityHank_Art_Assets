using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMemoryManager : MonoBehaviour
{
    /************************************************属性与变量命名************************************************/
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private List<Sprite> sprites = new List<Sprite>();
    /************************************************Unity方法与事件***********************************************/
    private void Start()
    {
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (this.spriteRenderer != null && !this.sprites.Contains(this.spriteRenderer.sprite))
        {
            this.sprites.Add(this.spriteRenderer.sprite);
            Debug.LogFormat("<><AnimationMemoryManager.Update>add sprite: {0}", this.spriteRenderer.sprite);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Animator animator = this.GetComponent<Animator>();
            if (animator != null) animator.SetTrigger("Exit");
            Debug.Log("<><AnimationMemoryManager.Update>play exit animation");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {            
            for (int i = 0; i < this.sprites.Count; i++)
            {
                Resources.UnloadAsset(this.sprites[i]);
            }
            Resources.UnloadUnusedAssets();
            GameObject.Destroy(this.gameObject);
            Debug.Log("<><AnimationMemoryManager.Update>collect memory");
        }
    }
    /************************************************自 定 义 方 法************************************************/
}
