using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//作者公众号“特效基地”
//Jason 20170616 v0.1
[AddComponentMenu("Base/LookAt")]
[ExecuteInEditMode]
public class LookAt : MonoBehaviour {

	public Transform target;
	public bool lockY;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var CamT = Camera.main.transform;
		if (target == null) {
			target = CamT;
		}
		if (lockY) {
			Vector3 dir = target.position - transform.position;
			dir.y = 0f;
			transform.rotation = Quaternion.LookRotation (dir);
		} else {
			transform.LookAt (target);
		}
	}
}
