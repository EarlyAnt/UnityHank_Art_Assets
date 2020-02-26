using Spine.Unity;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SkeletonUtils : MonoBehaviour
{
    /************************************************属性与变量命名************************************************/

    /************************************************Unity方法与事件***********************************************/
    void Awake()
    {
    }
    void Start()
    {
    }
    void Update()
    {
    }
    /************************************************自 定 义 方 法************************************************/
    [ContextMenu("1-克隆Skeleton物体")]
    private void CloneSkeletonObject()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 21; j++)
            {
                string childName = string.Format("SkeletonGraphic (nim_{0}_{1})", (i + 1).ToString().PadLeft(2, '0'), (j + 1).ToString().PadLeft(2, '0'));
                GameObject childObject = GameObject.Find(childName);
                if (childObject != null)
                {
                    GameObject newChildObject = GameObject.Instantiate(childObject, childObject.transform.parent);

                    childObject.name = string.Format("SkeletonGraphic_nim_{0}_{1}_1", (i + 1).ToString().PadLeft(2, '0'), (j + 1).ToString().PadLeft(2, '0'));
                    newChildObject.name = string.Format("SkeletonGraphic_nim_{0}_{1}_2", (i + 1).ToString().PadLeft(2, '0'), (j + 1).ToString().PadLeft(2, '0'));

                    SkeletonGraphic sgOfChild = childObject.GetComponent<SkeletonGraphic>();
                    if (sgOfChild != null) sgOfChild.startingAnimation = "animation01";
                    SkeletonGraphic sgOfNewChild = newChildObject.GetComponent<SkeletonGraphic>();
                    if (sgOfNewChild != null) sgOfNewChild.startingAnimation = "animation02";
                }
            }
        }
    }
    [ContextMenu("2-生成父物体")]
    private void CreateParentObject()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 21; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    GameObject newObject = new GameObject();
                    newObject.name = string.Format("Skeleton_nim_{0}_{1}_{2}", (i + 1).ToString().PadLeft(2, '0'), (j + 1).ToString().PadLeft(2, '0'), k + 1);
                    newObject.transform.SetParent(this.transform);
                    newObject.transform.localPosition = Vector3.zero;
                    newObject.transform.localRotation = Quaternion.identity;
                    newObject.AddComponent<RectTransform>().sizeDelta = new Vector2() { x = 240, y = 320 };
                }
            }
        }
    }
    [ContextMenu("3-设置层级关系")]
    private void SetParent()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 21; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    string parentObjectName = string.Format("Skeleton_nim_{0}_{1}_{2}", (i + 1).ToString().PadLeft(2, '0'), (j + 1).ToString().PadLeft(2, '0'), k + 1);
                    GameObject parentObject = GameObject.Find(parentObjectName);
                    string childObjectName = string.Format("SkeletonGraphic_nim_{0}_{1}_{2}", (i + 1).ToString().PadLeft(2, '0'), (j + 1).ToString().PadLeft(2, '0'), k + 1);
                    GameObject childObject = GameObject.Find(childObjectName);
                    if (parentObject != null && childObject != null)
                    {
                        childObject.transform.SetParent(parentObject.transform);
                        GameObject imageObject = new GameObject();
                        imageObject.name = "Image";
                        imageObject.transform.SetParent(parentObject.transform);
                        imageObject.transform.localPosition = Vector3.zero;
                        imageObject.transform.localRotation = Quaternion.identity;
                        imageObject.AddComponent<RectTransform>().sizeDelta = new Vector2() { x = 240, y = 320 };
                        imageObject.AddComponent<Image>();
                        imageObject.transform.SetAsFirstSibling();
                    }
                }
            }
        }
    }
    [ContextMenu("4-重新设置父物体名称")]
    private void RenameParentObject()
    {
        int itemId = 40001;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 21; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    string objectName = string.Format("Skeleton_nim_{0}_{1}_{2}", (i + 1).ToString().PadLeft(2, '0'), (j + 1).ToString().PadLeft(2, '0'), k + 1);
                    GameObject newObject = GameObject.Find(objectName);
                    newObject.name = itemId.ToString();
                    itemId += 1;
                }
            }
        }
    }
}
