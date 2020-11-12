using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpineManager : MonoBehaviour
{
    [SerializeField]
    private List<SpineNode> spineList;
    [SerializeField]
    private Text tip;
    [SerializeField]
    private Nodes node = Nodes.Prefab;
    private int spineIndex;
    private enum Nodes { Prefab = 0, AssetBundle = 1 }

    private void Start()
    {
        this.tip.text = this.node.ToString();
        this.ShowSpine();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            this.node = Nodes.Prefab;
            this.tip.text = this.node.ToString();
            this.ShowSpine();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            this.node = Nodes.AssetBundle;
            this.tip.text = this.node.ToString();
            this.ShowSpine();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.spineIndex = (this.spineIndex - 1 + this.spineList.Count) % this.spineList.Count;
            this.ShowSpine();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            this.spineIndex = (this.spineIndex + 1) % this.spineList.Count;
            this.ShowSpine();
        }
    }

    private void ShowSpine()
    {
        if (this.spineList == null || this.spineList.Count == 0)
            return;

        this.spineList.ForEach(t => t.RootNode.SetActive(false));
        this.spineList[this.spineIndex].PrefabNode.SetActive(this.node == Nodes.Prefab);
        this.spineList[this.spineIndex].AssetBundleNode.SetActive(this.node == Nodes.AssetBundle);
        this.spineList[this.spineIndex].RootNode.SetActive(true);
    }

    //[ContextMenu("搜集Spine动画节点")]
    //private void CollectNodes()
    //{
    //    GameObject[] rootNodes = GameObject.FindGameObjectsWithTag("SpineRoot");
    //    if (rootNodes == null || rootNodes.Length == 0) return;

    //    this.spineList = new List<SpineNode>();
    //    for (int i = 0; i < rootNodes.Length; i++)
    //    {
    //        Transform prefabNode = rootNodes[i].transform.Find(string.Format("{0}_Test", rootNodes[i].name));
    //        Transform assetBundleNode = rootNodes[i].transform.Find(rootNodes[i].name);
    //        this.spineList.Add(new SpineNode() { RootNode = rootNodes[i], PrefabNode = prefabNode.gameObject, AssetBundleNode = assetBundleNode.gameObject });
    //    }
    //}
}

[System.Serializable]
public class SpineNode
{
    public GameObject RootNode;
    public GameObject PrefabNode;
    public GameObject AssetBundleNode;
}

