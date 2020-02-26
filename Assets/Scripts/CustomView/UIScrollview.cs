using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Gululu;
using Gululu.Util;
using strange.extensions.mediation.impl;

public class UIScrollview : View
{
    [Inject]
    public IPrefabRoot mPrefabRoot { get; set; }

    [Inject]
    public ILangManager mLangManager { get; set; }

    public GameObject objItemPrefab;
    RectTransform rtItemPrefab;
    ScrollRect scrollRect;
    public GameObject scrollContent;
    [HideInInspector]
    public int cellSize = 0;
    [HideInInspector]
    public delegate void VoidDelegate(GameObject go, int idx);
    public VoidDelegate updateCell;
    private GridLayoutGroup group;

    List<GameObject> localListCell = new List<GameObject>();
    List<GameObject> localListCellParent = new List<GameObject>();

    private bool isVerTical = false;
    private bool isHorizontal = false;

    public GameObject GetCellData(int i)
    {
        if (i >= 0 && i < localListCellParent.Count)
        {
            if (localListCellParent[i].transform.childCount > 0)
            {
                return localListCellParent[i].transform.GetChild(0).gameObject;
            }
        }
        return null;
    }
    public int GetCellIndex(GameObject cell)
    {
        return localListCellParent.IndexOf(cell.transform.parent.gameObject);
    }
    public float GetCellHeight()
    {
        return objItemPrefab.GetComponent<RectTransform>().rect.height;
    }

    private void PrefabLocalization()
    {
        GameObject prefab = Resources.Load<GameObject>("Localization/UI/CellInScrollView/" + objItemPrefab.name);
        if (prefab != null)
        {
            objItemPrefab = GameObject.Instantiate(prefab) as GameObject;
            mPrefabRoot.ReplaceFont(objItemPrefab);
            Text[] textAll = objItemPrefab.GetComponentsInChildren<Text>(true);
            foreach (var text in textAll)
            {
                if (text.text.Length != 0)
                {
                    if (text.text[0] == '#')
                    {
                        text.text = mLangManager.GetLanguageString(text.text.Substring(1));
                    }
                }
            }
        }
    }

    override protected void Awake()
    {
        base.Awake();
#if (UNITY_IOS || UNITY_ANDROID) && (!UNITY_EDITOR) && LOCALIZATION
        PrefabLocalization();
#endif

        scrollRect = transform.GetComponent<ScrollRect>();
        rtItemPrefab = objItemPrefab.GetComponent<RectTransform>();
        group = scrollContent.transform.GetComponent<GridLayoutGroup>();
        isVerTical = scrollRect.vertical;
        isHorizontal = scrollRect.horizontal;
        scrollRect.onValueChanged.AddListener(OnPanelMove);
    }

    // Use this for initialization
    override protected void Start()
    {
        base.Start();
        RefreshGridLayoutGroup();
    }

    public void RefreshGridLayoutGroup()
    {
        if (group != null)
        {
            RectTransform groupRect = group.GetComponent<RectTransform>();
            if (isVerTical)
            {
                nColumn = 1 + (int)((groupRect.rect.width - group.cellSize.x) / (group.cellSize.x + group.spacing.x));
                float height = (group.cellSize.y + group.spacing.y) * ((cellSize + nColumn - 1) / nColumn);
                groupRect.sizeDelta = new Vector2(groupRect.sizeDelta.x, height);
            }
            else if (isHorizontal)
            {
                nRow = 1 + (int)((groupRect.rect.height - group.cellSize.y) / (group.cellSize.y + group.spacing.y));
                float width = (group.cellSize.x + group.spacing.x) * ((cellSize + nRow - 1) / nRow);
                groupRect.sizeDelta = new Vector2(width, groupRect.sizeDelta.y);
            }
            for (int i = 0; i < localListCellParent.Count; ++i)
            {
                localListCellParent[i].SetActive(false);
            }
            bool bNeedWaitOneFrame = false;
            for (int i = 0; i < cellSize; i++)
            {
                bool bRet = false;
                AddTaskParentBar(i,out bRet);
                if(bRet)
                {
                    bNeedWaitOneFrame = true;
                }
            }
            if(bNeedWaitOneFrame)
            {
                StopCoroutine("AddTaskBarAfterOneFrame");
                StartCoroutine("AddTaskBarAfterOneFrame");
            }
            else
            {
                FillAllTaskBar();
            }
        }
    }
    int nColumn = 0;
    int nRow = 0;

    private void FillAllTaskBar()
    {
        nLastStartIndex = -1;
        nLastEndIndex = -1;
        for (int i = 0; i < cellSize; i++)
        {
            FillTaskBar(i,true);
        }
        if (nLastEndIndex == -1)
        {
            nLastEndIndex = cellSize;
        }

    }

    IEnumerator AddTaskBarAfterOneFrame()
    {
        yield return null;
        FillAllTaskBar();
    }
    public void ResetPos()
    {
        transform.GetComponent<ScrollRect>().StopMovement();
        RectTransform rect = scrollContent.GetComponent<RectTransform>();
        rect.localPosition = new Vector3(rect.localPosition.x, 0, rect.localPosition.z);


    }

    public void ScrollToIndex(int index)
    {
        if (index < 0 || index >= localListCellParent.Count)
            return;
        if(IsNeedRender(localListCellParent[index]))
        {
            return;
        }
        RectTransform selfRectTransform = scrollContent.GetComponent<RectTransform>();
        if (isVerTical)
        {

            float moveHight = (group.cellSize.y + group.spacing.y) * (index / nColumn);
            if (moveHight > 0)
                selfRectTransform.localPosition = new Vector3(selfRectTransform.localPosition.x, moveHight, selfRectTransform.localPosition.z);
        }
        else
        {
            float moveWidth = (group.cellSize.x + group.spacing.x) * (index / nRow);
            if (moveWidth > 0)
                selfRectTransform.localPosition = new Vector3(moveWidth, selfRectTransform.localPosition.y, selfRectTransform.localPosition.z);
        }
        bNeedUpdate = false;
    }

    public void scrollToBottom()
    {
        RectTransform selfRectTransform = scrollContent.GetComponent<RectTransform>();
        //scrollRect.content = selfRectTransform;
        if(isVerTical)
        {
            float moveHight = selfRectTransform.rect.height - transform.GetComponent<RectTransform>().rect.height;
            if (moveHight > 0)
                selfRectTransform.localPosition = new Vector3(selfRectTransform.localPosition.x, moveHight, selfRectTransform.localPosition.z);
        }
        else
        {
            float moveWidth = selfRectTransform.rect.width - transform.GetComponent<RectTransform>().rect.width;
            if (moveWidth > 0)
                selfRectTransform.localPosition = new Vector3(moveWidth,selfRectTransform.localPosition.y , selfRectTransform.localPosition.z);
        }
        bNeedUpdate = true;
    }

    bool bNeedUpdate = false;
    int nLastStartIndex = 0;
    int nLastEndIndex = 0;
    void Update()
    {
        if (bNeedUpdate)
        {
            bNeedUpdate = false;

            int nScrollItemCount = 0;
            if(isVerTical)
            {
                nScrollItemCount = nColumn;
            }
            else
            {
                nScrollItemCount = nRow;
            }
            int nStartIndex = nLastStartIndex - nScrollItemCount;
            int nEndIndex = nLastEndIndex+ nScrollItemCount;
            if (nEndIndex > cellSize)
            {
                nEndIndex = cellSize;
            }
            if (nStartIndex < 0)
            {
                nStartIndex = 0;
            }

            nLastStartIndex = -1;
            nLastEndIndex = -1;
            for (int i = nStartIndex; i < nEndIndex && i < localListCellParent.Count; ++i)
            {
                FillTaskBar(i,false);
            }
            if (nLastEndIndex == -1)
            {
                nLastEndIndex = nEndIndex;
            }
        }
    }

    private GameObject AddTaskParentBar(int cellIndex,out bool bnew)
    {
        GameObject roleSir = null;
        if (cellIndex == localListCellParent.Count)
        {
            roleSir = new GameObject();
            
            localListCellParent.Add(roleSir);
            roleSir.name = "cell";
            RectTransform rect = roleSir.AddComponent<RectTransform>();
            
            rect.sizeDelta = rtItemPrefab.sizeDelta;
            bnew = true;
        }
        else
        {
            roleSir = localListCellParent[cellIndex];
            bnew = false;
        }
        roleSir.SetActive(true);
        roleSir.transform.SetParent(group.transform,false);
        roleSir.transform.localScale = rtItemPrefab.localScale;
        roleSir.transform.rotation = rtItemPrefab.rotation;

        return roleSir;
    }

    bool FillTaskBar(int cellIndex,bool forceUpdate)
    {
        GameObject objParent = localListCellParent[cellIndex];
        if (IsNeedRender(objParent))
        {
            if (nLastStartIndex == -1)
            {
                nLastStartIndex = cellIndex;
            }
            if (objParent.transform.childCount == 0)
            {
                GameObject roleSir = GetValidCellWhenMove();
                InitialCell(roleSir, objParent, cellIndex);
            }
            else if(forceUpdate)
            {
                Transform tranChild = objParent.transform.GetChild(0);
                InitialCell(tranChild.gameObject, objParent, cellIndex);
            }
        }
        else
        {
            if (nLastStartIndex != -1 && nLastEndIndex == -1)
            {
                nLastEndIndex = cellIndex + 1;
            }

            RemoveCellFromParent(objParent);
        }
        return true;

    }
    private void RemoveCellFromParent(GameObject objParent)
    {
        if (objParent.transform.childCount != 0)
        {
            Transform tranChild = objParent.transform.GetChild(0);
            tranChild.gameObject.SetActive(false);
            tranChild.transform.SetParent(scrollRect.gameObject.transform,false);
        }
    }
    GameObject GetValidCellWhenMove()
    {
        for (int i = 0; i < localListCell.Count; i++)
        {
            if (localListCell[i].transform.parent == scrollRect.gameObject.transform)
            {
                return localListCell[i];
            }
        }
        GameObject roleSir = null;
        roleSir = GameObject.Instantiate(objItemPrefab);
        roleSir.transform.localScale = objItemPrefab.transform.localScale;

        localListCell.Add(roleSir);
        return roleSir;
    }
    void OnPanelMove(Vector2 pos)
    {
        bNeedUpdate = true;
    }
    Vector3[] vecResult = new Vector3[4];
    private bool IsNeedRender(GameObject objParent)
    {
        objParent.GetComponent<RectTransform>().GetWorldCorners(vecResult);
        Rect parentRect = new Rect(vecResult[0].x, vecResult[0].y, vecResult[2].x - vecResult[0].x, vecResult[2].y - vecResult[0].y);
        gameObject.GetComponent<RectTransform>().GetWorldCorners(vecResult);
        Rect rectPanel = new Rect(vecResult[0].x, vecResult[0].y, vecResult[2].x - vecResult[0].x, vecResult[2].y - vecResult[0].y);

        return rectPanel.Overlaps(parentRect);
    }
    private void InitialCell(GameObject roleSir, GameObject objParent, int cellIndex)
    {
        roleSir.SetActive(true);
        roleSir.transform.SetParent(objParent.transform,false);
        roleSir.transform.localScale = rtItemPrefab.localScale;
        roleSir.transform.rotation = rtItemPrefab.rotation;
        roleSir.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);

        if (updateCell != null) updateCell(roleSir, cellIndex);
    }

}
