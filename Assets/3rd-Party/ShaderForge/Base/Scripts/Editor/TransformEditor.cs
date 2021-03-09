using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Transformer))]
public class TransformEditor : Editor 
{
	public override void OnInspectorGUI ()
	{
		
		var obj = (Transformer)target;
		EditorGUI.BeginChangeCheck ();
		obj.delay = EditorGUILayout.FloatField ("Delay", obj.delay);
		obj.life = EditorGUILayout.FloatField ("Life", obj.life);
		obj.transformType = (Transformer.transType)EditorGUILayout.EnumPopup("Transform Type",obj.transformType);
		switch (obj.transformType) {
		case Transformer.transType.Transform:
			obj.moveSpeed = EditorGUILayout.Vector3Field ("Move Speed", obj.moveSpeed);
			obj.rotateSpeed = EditorGUILayout.Vector3Field ("Rotate Speed", obj.rotateSpeed);
			obj.scaleSpeed = EditorGUILayout.Vector3Field ("Scale Speed", obj.scaleSpeed);
			break;
		case Transformer.transType.sinTransform:
			obj.sinRange = EditorGUILayout.Vector3Field ("Sin Range", obj.sinRange);
			obj.sinFrequency = EditorGUILayout.FloatField ("Sin Frequency",obj.sinFrequency);
			break;
		case Transformer.transType.randomTransform:
			obj.randomRange = EditorGUILayout.Vector3Field ("Random Range", obj.randomRange);
			obj.randomFrequency = EditorGUILayout.FloatField ("Random Frequency",obj.randomFrequency);
			break;
		}
		if (EditorGUI.EndChangeCheck ()) {
			EditorUtility.SetDirty (target);
		}
		//base.OnInspectorGUI ();
	}
}
