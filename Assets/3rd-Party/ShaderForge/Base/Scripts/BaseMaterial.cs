using UnityEngine;
using System.Collections;

/*created by Jason 20160816
 * 浮生若梦出品
 * 作者公众号“特效基地”
 * 作者QQ:541211225
 * Mobile:15821684699
 * v1.0
*/
[AddComponentMenu("Base/BaseMaterial")]
public class BaseMaterial : MonoBehaviour {

	[UnityEngine.Header("作者公众号“特效基地”  v1.0")]
	public float life = 0f;

	[UnityEngine.Header("材质")]
	public Color _TintColor = new Color(0.5F, 0.5F, 0.5F, 0.5F);
	public AnimationCurve alphaCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(2,1));
	public Vector2 UVTile = new Vector2(1,1);
	public Vector2 UVSpeed = new Vector2(0,0);

	private Renderer myRender; 
	private Transform myTransform;

	void Start () {
		
		if(life > 0){
			Invoke ("JasonDelete",life);
		}

		myRender = this.GetComponent<Renderer> ();

	}
		
	void Update () {
		if (myRender != null) {
			Color tempColor = _TintColor;
			float curveVelue = alphaCurve.Evaluate(Time.time);
			tempColor.a = _TintColor.a * curveVelue;
			myRender.material.SetColor("_TintColor", tempColor);

		}

		if(UVTile != Vector2.one ) myRender.material.mainTextureScale = UVTile;
		if(UVSpeed != Vector2.zero ) myRender.material.mainTextureOffset += Time.deltaTime* UVSpeed;

	}
		
	void JasonDelete (){
		Destroy (this.gameObject);
	}

}