using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonPlayer : MonoBehaviour {

    public SkeletonGraphic SkeletonGraphic;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            string startAnimation = this.SkeletonGraphic.startingAnimation;
            if (string.IsNullOrEmpty(startAnimation) ||
                startAnimation == "none")
            {
                this.SkeletonGraphic.gameObject.SetActive(false);
                this.SkeletonGraphic.startingAnimation = "animation01";
                this.SkeletonGraphic.gameObject.SetActive(false);
            }
            else
            {
                this.SkeletonGraphic.gameObject.SetActive(false);
                startAnimation = startAnimation == "animation01" ? "animation02" : "animation01";
                this.SkeletonGraphic.startingAnimation = startAnimation;
                this.SkeletonGraphic.gameObject.SetActive(true);
            }
        }
    }
}
