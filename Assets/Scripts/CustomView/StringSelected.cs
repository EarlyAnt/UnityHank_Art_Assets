using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StringSelected : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public GameObject textPrefrab;
    public float heightScale = 5;
    public bool onlyOneScroll = false;
    RectTransform contentRect;

//     RectTransform targetRec;

    Vector3 oldDragPos;

    Vector3 newDragPos;

    public AnimationCurve curve_scale;//改变大小曲线
    public AnimationCurve curve_color;//渐变效果曲线


//     List<Text> textList = new List<Text>();


    float
        itemHeight,             //子项item的高
        contentParentHeight,    //Content爸爸的高
        itemHeight_min,         //子项最小发生改变位置
        itemHeight_max,         //子项最大发生改变位置
        conentLimit,            //Conent纠正位置
        conentSpacing;          //子项间隔大小
    int itemNum = -1;                //子项数量
    int highlightIndex;         //高亮的子项


    string[] myStringValue;
    [HideInInspector]
    public string currValue {
        get
        {
            if(currIndex < myStringValue.Length)
            {
                return myStringValue[currIndex];
            }
            return "";
        }
    }
    [HideInInspector]
    public int currIndex;


    public delegate void ValueChangeCallBack(int nRet);

    ValueChangeCallBack valueChangeCB;

    [SerializeField]
    Color itemHighColor = new Color32(255, 255, 255, 255);

    void Awake()
    {
        contentRect = transform.Find("Content").GetComponent<RectTransform>();
        contentParentHeight = contentRect.parent.GetComponent<RectTransform>().sizeDelta.y;

        conentSpacing = contentRect.GetComponent<VerticalLayoutGroup>().spacing;// / 2;

        itemHeight = textPrefrab.GetComponent<Text>().rectTransform.sizeDelta.y + conentSpacing;

    }

    void OnEnable()
    {
        HighlightAndScaleList();
    }

    void Start()
    {


        //         deltaX = textList[0].GetComponent<RectTransform>().sizeDelta.x;
        //         deltaY = textList[0].GetComponent<RectTransform>().sizeDelta.y;

        //         Invoke("ItemList", 0.05f);

    }

    private int GetIndex(string stringValue)
    {
        int value = 0;
        for (int i = 0; i < myStringValue.Length; ++i)
        {
            if (myStringValue[i] == stringValue)
            {
                value = i;
                break;
            }
        }
        return value;
    }
    public void Initialize(string[] stringArray, ValueChangeCallBack callBack = null)
    {
        valueChangeCB = callBack;
        InstantiateData(stringArray);
    }
    public void SetValue(string stringValue)
    {
        int value = GetIndex(stringValue);
        if (value > currIndex)
        {
            int count = value - currIndex;
            for (int i = 0; i < count; ++i)
            {
                Transform rectItem = contentRect.transform.GetChild(0);
                rectItem.transform.SetSiblingIndex(itemNum);
            }
        }
        else if(value < currIndex)
        {
            int count = currIndex - value;
            for (int i = 0; i < count; ++i)
            {
                Transform rectItem = contentRect.transform.GetChild(itemNum);
                rectItem.transform.SetSiblingIndex(0);
            }
        }
        currIndex = value;
        if (valueChangeCB != null)
        {
            valueChangeCB(currIndex);
        }
        HighlightAndScaleList();
    }

    /// <summary>
    /// 生成子项item
    /// </summary>
    /// <param name="itemNum">子项数量</param>
    /// <param name="dat">子项最大值</param>
    void InstantiateData(string[] stringArray)
    {
        myStringValue = stringArray;
        GameObject go;
        for (int i = 0; i < stringArray.Length; i++)
        {
            go = Instantiate(textPrefrab, contentRect);
            go.GetComponent<Text>().text = stringArray[i];
            go.name = stringArray[i];
        }
        currIndex = stringArray.Length - 1;

        itemNum = stringArray.Length - 1;
        highlightIndex = (itemNum + 1) / 2;

        if (itemNum % 2 == 0)
            conentLimit = -(itemHeight + 5) / 2;
        else
            conentLimit = 0;
        if (contentRect)
        {
            contentRect.anchoredPosition = new Vector2(contentRect.anchoredPosition.x, conentLimit);
        }

    }

    /// <summary>
    /// 是增加或减少
    /// </summary>
    /// <param name="isIncreaseOrdecrease"></param>
    void ShowItem(bool isIncreaseOrdecrease)
    {
        itemHeight_min = -itemHeight;

        itemHeight_max = -itemHeight * itemNum;

        if (isIncreaseOrdecrease)
        {
            while (true)
            {
                Transform rectItem = contentRect.transform.GetChild(0);
                if (rectItem.GetComponent<RectTransform>().anchoredPosition.y > itemHeight_min)
                {
                    rectItem.transform.SetSiblingIndex(itemNum);
                }
                else
                {
                    break;
                }
            }
            Debug.Log("+");
        }
        else
        {
            while(true)
            {
                Transform rectItem = contentRect.transform.GetChild(itemNum);
                if (rectItem.GetComponent<RectTransform>().anchoredPosition.y < itemHeight_max)
                {
                    rectItem.transform.SetSiblingIndex(0);
                }
                else
                {
                    break;
                }
            }
            Debug.Log("-");
        }
    }

    /// <summary>
    /// 渐变效果，改变大小，高亮显示
    /// </summary>
    void HighlightAndScaleList()
    {
        for(int i = 0; i <= itemNum; ++i)
        {
            Text item = contentRect.transform.GetChild(i).GetComponent<Text>();
            float a = Mathf.Abs(i - highlightIndex) * heightScale / (float)(highlightIndex);
            if(a > 1f)
            {
                a = 1f;
            }
            if (i == highlightIndex)
            {
                item.color = itemHighColor;
                int nowIndex = GetIndex(item.text);
                if (currIndex != nowIndex)
                {
                    currIndex = nowIndex;
                    if(valueChangeCB != null)
                    {
                        valueChangeCB(currIndex);
                    }
                }
            }
            else
            {

                float indexSc_color = Mathf.Abs(curve_color.Evaluate(a));
                item.color = new Color(0, 0, 0, 1 - indexSc_color);
            }

            float indexSc_scale = Mathf.Abs(curve_scale.Evaluate(a));
            item.GetComponent<RectTransform>().localScale = new Vector3(1 - indexSc_scale, 1 - indexSc_scale, 1 - indexSc_scale);

        }
    }


    /// <summary>
    /// 纠正Conent位置
    /// </summary>
    void UpdateEx()
    {
    }

    /// <summary>
    /// 获取拖拽信息并改变Conent位置
    /// </summary>
    /// <param name="eventData"></param>
    void SetDraggedPosition(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(contentRect, eventData.position, eventData.pressEventCamera, out newDragPos))
        {
            newDragPos = eventData.position;
            if (Mathf.Abs(newDragPos.y - oldDragPos.y) >= itemHeight)
            {
                if (newDragPos.y > oldDragPos.y)
                {
//                    conentRect.anchoredPosition = new Vector2(conentRect.anchoredPosition.x, conentRect.anchoredPosition.y + itemHeight);
                    oldDragPos += new Vector3(0, itemHeight, 0);
                    ShowItem(true);
                    HighlightAndScaleList();
                }
                else
                {
 //                   conentRect.anchoredPosition = new Vector2(conentRect.anchoredPosition.x, conentRect.anchoredPosition.y - itemHeight);
                    oldDragPos -= new Vector3(0, itemHeight, 0);
                    ShowItem(false);
                    HighlightAndScaleList();
                }
            }
        }
    }

    /// <summary>
    /// 当开始拖拽
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        oldDragPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetDraggedPosition(eventData);
        UpdateEx();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SetDraggedPosition(eventData);
        UpdateEx();
    }
}