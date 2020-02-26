using Gululu.Config;
using UnityEngine;
using UnityEngine.UI;

namespace Hank.DownloadPage2
{
    /// <summary>
    /// 断网提示页面
    /// </summary>
    public class TipView : PopBaseView
    {
        /************************************************属性与变量命名************************************************/

        /************************************************Unity方法与事件***********************************************/

        /************************************************自 定 义 方 法************************************************/
        //打开页面
        public override void Open(object param = null)
        {
            base.Open();
        }
        //关闭页面
        public override void Close()
        {
            base.Close(() => this.OnViewClosed(ViewResult.OK));
        }
    }
}
