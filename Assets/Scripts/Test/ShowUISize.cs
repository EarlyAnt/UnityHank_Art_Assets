using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUISize : MonoBehaviour
{
    RectTransform rect;

    // Use this for initialization
    void Start()
    {
        this.rect = this.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.LogError(this.rect.sizeDelta);
    }
}
