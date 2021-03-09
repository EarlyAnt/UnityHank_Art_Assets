using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
/*created by Jason 20161116
 * 浮生若梦出品
 * 作者QQ:541211225
 * Mobile:15821684699
 * v2.1
*/

[AddComponentMenu("Base/Replay")]
public class Replay : MonoBehaviour {
	[UnityEngine.Header("作者公众号“特效基地” v2.1")]
	[UnityEngine.Header("Press space to replay")]

	public bool AutoReplay = true;
	public float replayTimer = 1.5f;
	public float playSpeed = 1.0f;

	private float myTime;
	private Scene myScene;
	// Use this for initialization
	void Start () {
		
		myTime = replayTimer;
		myScene = SceneManager.GetActiveScene();

		Debug.Log("Replay!");
		//Debug.Log("Active scene is '" + myScene.name + "'."+"浮生若梦出品"+"."+"公众号“特效基地”");
	}
	void Update () {
		
		myTime -= Time.deltaTime;
		if(myTime <= 0 && AutoReplay == true){
			Jason ();
			myTime = replayTimer;
		}

		Time.timeScale = playSpeed;
		if(Input.GetKeyUp(KeyCode.Space)){
			Jason ();
		}
	}

	void Jason(){
		//for unity 5.x!!!
		SceneManager.LoadScene (myScene.name, LoadSceneMode.Single);


		//for unity 4.x!!!
		//Application.LoadLevel(Application.loadedLevel);

	}
}
