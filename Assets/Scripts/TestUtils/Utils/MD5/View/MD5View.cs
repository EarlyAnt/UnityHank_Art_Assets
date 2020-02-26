using UnityEngine;
using UnityEngine.UI;

namespace Hank.MainScene
{
    public class MD5View : MonoBehaviour
    {
        /************************************************属性与变量命名************************************************/
        [SerializeField]
        private InputField txtFilePath;
        [SerializeField]
        private InputField txtMD5Code;
        [SerializeField]
        private Text txtResult;
        [SerializeField]
        private Button btnGenerate;
        [SerializeField]
        private Button btnClear;
        private IMD5Util md5Util;
        /************************************************Unity方法与事件***********************************************/
        private void Start()
        {
            this.md5Util = new MD5Util();
            this.btnGenerate.onClick.AddListener(this.Generate);
            this.btnClear.onClick.AddListener(this.Clear);
        }
        /************************************************自 定 义 方 法************************************************/
        private void Generate()
        {
            this.txtMD5Code.text = this.md5Util.GenerateMD5Code(this.txtFilePath.text, (success, message) =>
            {
                this.txtResult.text = message;
                this.txtResult.color = success ? Color.black : Color.red;
            });
        }

        private void Clear()
        {
            this.txtFilePath.text = "";
            this.txtMD5Code.text = "";
        }
    }
}