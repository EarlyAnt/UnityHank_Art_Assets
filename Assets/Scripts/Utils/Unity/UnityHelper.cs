using UnityEngine;

public static class UnityHelper
{
    public static Transform FindDeepChild(Transform root, string childName)
    {
        Transform result = root.Find(childName);
        if (result != null)
            return result;

        for (int i = 0; i < root.childCount; i++)
        {
            result = FindDeepChild(root.GetChild(i), childName);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }

    public static void DeleteChild(Transform root, string childName)
    {
        Transform child = FindDeepChild(root, childName);
        if (child != null)
            GameObject.Destroy(child.gameObject);
    }

    public static void DeleteChild(Transform root, string nodeName, string childName)
    {
        DeleteChild(FindDeepChild(root, nodeName), childName);
    }

    public static void DeleteAllChildren(Transform root, string nodeName)
    {
        DeleteAllChildren(FindDeepChild(root, nodeName));
    }

    public static void DeleteAllChildren(Transform root)
    {
        if (root != null && root.childCount > 0)
        {
            for (int i = 0; i < root.childCount; i++)
            {
                GameObject.Destroy(root.GetChild(i).gameObject);
            }
        }
    }

    public static void SetAllChildrenVisible(Transform root, string nodeName, bool visible)
    {
        Transform child = FindDeepChild(root, nodeName);
        if (child != null) child.gameObject.SetActive(visible);
    }
}
