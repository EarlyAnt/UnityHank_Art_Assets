using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 金币框
/// </summary>
public class CoinBar : MonoBehaviour
{
    /************************************************属性与变量命名************************************************/
    [SerializeField]
    private Text coin;
    /************************************************Unity方法与事件***********************************************/

    /************************************************自 定 义 方 法************************************************/
    //设置等级边框
    public void SetCoin(int coin)
    {
        if (coin >= 0)
        {
            this.coin.text = coin.ToString();
        }
        else
        {
            Debug.LogErrorFormat("<><CoinBar.SetCoin>Parameter 'coin' is less than 0");
        }
    }
}