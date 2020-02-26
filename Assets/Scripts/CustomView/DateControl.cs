using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DateControl : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public enum ItemType { _year, _month, _day, _hour, _minute, _kg, _pound }

    public ItemType _itemtype;
    public GameObject textPrefrab;
    public float heightScale = 5;
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
    int itemNum;                //子项数量
    int highlightIndex;         //高亮的子项

    [HideInInspector]
    public int currValue;

    [HideInInspector]
    public int startValue;

    [HideInInspector]
    Color itemHighColor = new Color32(255, 255, 255, 255);

    void Awake()
    {
        contentRect = transform.Find("Content").GetComponent<RectTransform>();
//         targetRec = transform.parent.Find("HighlightTarget").GetComponent<RectTransform>();
        switch (_itemtype)
        {
            case ItemType._year: InstantiateData(16, DateTime.Now.Year); break;
            case ItemType._month: InstantiateData(12, 12); break;
            case ItemType._day: InstantiateData(31, 31); break;
            case ItemType._hour: InstantiateData(24, 24); break;
            case ItemType._minute: InstantiateData(60, 60); break;
            case ItemType._kg: InstantiateData(240, 250); break;
            case ItemType._pound: InstantiateData(530, 550); break;
        }
        itemNum = transform.Find("Content").childCount - 1;
        highlightIndex = transform.Find("Content").childCount / 2;
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


        if (itemNum % 2 == 0)
            conentLimit = -(itemHeight + 5) / 2;
        else
            conentLimit = 0;
        if(contentRect)
        {
            contentRect.anchoredPosition = new Vector2(contentRect.anchoredPosition.x, conentLimit);
        }

        //         deltaX = textList[0].GetComponent<RectTransform>().sizeDelta.x;
        //         deltaY = textList[0].GetComponent<RectTransform>().sizeDelta.y;

        //         Invoke("ItemList", 0.05f);

    }
    public void InitialValue(int value)
    {
        if (contentRect == null)
            return;
        if (value > currValue)
        {
            int count = value - currValue;
            for (int i = 0; i < count; ++i)
            {
                Transform rectItem = contentRect.transform.GetChild(0);
                rectItem.transform.SetSiblingIndex(itemNum);
            }
        }
        else if(value < currValue)
        {
            int count = currValue - value;
            for (int i = 0; i < count; ++i)
            {
                Transform rectItem = contentRect.transform.GetChild(itemNum);
                rectItem.transform.SetSiblingIndex(0);
            }
        }
        currValue = value;
        HighlightAndScaleList();
    }

    /// <summary>
    /// 生成子项item
    /// </summary>
    /// <param name="itemNum">子项数量</param>
    /// <param name="dat">子项最大值</param>
    void InstantiateData(int itemNum, int dat)
    {
        startValue = dat - itemNum + 1;
        currValue = dat - itemNum / 2;
        GameObject go;
        for (int i = startValue; i <= dat; i++)
        {
            go = Instantiate(textPrefrab, contentRect);
            go.GetComponent<Text>().text = i.ToString();
            go.name = i.ToString();
/*            textList.Add(go.GetComponent<Text>());*/
//             ShowItem(true);
        }
    }

    /// <summary>
    /// 是增加或减少
    /// </summary>
    /// <param name="isIncreaseOrdecrease"></param>
    void ShowItem(bool isIncreaseOrdecrease)
    {
        itemHeight_min = -itemHeight;

        //         if (_itemtype == ItemType._day) itemHeight_max = -itemHeight * itemNum - 95;
        //         else 
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
                currValue = int.Parse(item.text);
            }
            else
            {

                float indexSc_color = Mathf.Abs(curve_color.Evaluate(a));
                //item.color = new Color(0, 0, 0, 1 - indexSc_color);
				item.color = new Color(80, 80, 80, 1 - indexSc_color);
            }

            float indexSc_scale = Mathf.Abs(curve_scale.Evaluate(a));
            item.GetComponent<RectTransform>().localScale = new Vector3(1 - indexSc_scale, 1 - indexSc_scale, 1 - indexSc_scale);

        }
//         foreach (Text item in textList)
//         {
//             float indexA = Mathf.Abs(item.GetComponent<RectTransform>().position.y - targetRec.position.y);
//             float indexSc_scale = Mathf.Abs(curve_scale.Evaluate(indexA / contentParentHeight));
//             float indexSc_color = Mathf.Abs(curve_color.Evaluate(indexA / contentParentHeight));
//             if (indexA < 15)
//             {
//                 item.color = itemColor_hig;
//                 switch (_itemtype)
//                 {
//                     case ItemType._year: _year = int.Parse(item.text); break;
//                     case ItemType._month: _month = int.Parse(item.text); break;
//                     case ItemType._day: _day = int.Parse(item.text); break;
//                     case ItemType._hour: _hour = int.Parse(item.text); break;
//                     case ItemType._minute: _minute = int.Parse(item.text); break;
//                 }
//             }
//             else item.color = new Color(0, 0, 0, 1 - indexSc_color);
// 
//             item.GetComponent<RectTransform>().localScale = new Vector3(1 - indexSc_scale, 1 - indexSc_scale, 1 - indexSc_scale);
//             //item.GetComponent<RectTransform>().sizeDelta = new Vector2(deltaX - (deltaX * indexSc), deltaY - (deltaY * indexSc));
//         }

    }


    /// <summary>
    /// 纠正Conent位置
    /// </summary>
    void UpdateEx()
    {
//         if (conentRect.anchoredPosition.y > conentLimit)
//         {
//             ShowItem(true);
//             conentRect.anchoredPosition = new Vector2(conentRect.anchoredPosition.x, conentRect.anchoredPosition.y - itemHeight);
//         }
//         if (conentRect.anchoredPosition.y < conentLimit)
//         {
//             ShowItem(false);
//             conentRect.anchoredPosition = new Vector2(conentRect.anchoredPosition.x, conentRect.anchoredPosition.y + itemHeight);
//         }
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