using UnityEditor;

public class CreateAssetBundles
{
    [MenuItem("Assets/Collect Dependencies &C", priority = 2050)]
    static void BuildAllAssetBundles()
    {
        if (Selection.objects != null && Selection.objects.Length > 0)
        {
            int selectedObjectCount = Selection.objects.Length;
            Selection.objects = EditorUtility.CollectDependencies(Selection.objects);
            UnityEngine.Debug.LogFormat("Selected object count: {0}, all dependencies: {1}", selectedObjectCount, Selection.objects.Length);
        }
    }
}
