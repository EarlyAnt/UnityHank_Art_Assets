using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearUnusedResource : MonoBehaviour {

    void ClearUnusedResourceRepeat()
    {
        Resources.UnloadUnusedAssets();
    }

    // Use this for initialization
    void Start () {
        InvokeRepeating("ClearUnusedResourceRepeat", 60, 60);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
