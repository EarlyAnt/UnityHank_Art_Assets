using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class UIBase : MonoBehaviour   
{
    public string UIName = "";

    protected CanvasGroup canvasGroup;

    protected virtual void Start() { canvasGroup = GetComponent<CanvasGroup>(); }

    public virtual void DoOnEntering() { }   

    public virtual void DoOnPausing() { }      

    public virtual void DoOnResuming() { }   

    public virtual void DoOnExiting() { }  

}

