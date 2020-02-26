using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Hank.MainScene
{
    public class FileNameView : MonoBehaviour
    {
        /************************************************属性与变量命名************************************************/
        [SerializeField]
        private InputField txtFilePath;
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
            this.btnGenerate.onClick.AddListener(this.Format);
            this.btnClear.onClick.AddListener(this.Clear);
        }
        /************************************************自 定 义 方 法************************************************/
        private void Format()
        {
            if (!Directory.Exists(this.txtFilePath.text))
            {
                this.ShowResult(false, "路径不存在");
                this.txtFilePath.Select();
                return;
            }
            string newPath = string.Format(@"{0}\NewFiles", this.txtFilePath.text);
            if (Directory.Exists(newPath))
                Directory.Delete(newPath, true);
            Directory.CreateDirectory(newPath);

            List<FileInfo> files = new DirectoryInfo(this.txtFilePath.text).GetFiles().Where(t => t.Name.EndsWith(".png")).ToList();
            foreach (var file in files)
            {
                string newFileName = file.Name.Replace(file.Extension, "");
                if (newFileName.EndsWith("_s"))
                    newFileName = string.Format(@"{0}\{1}.png", newPath, newFileName.Replace("_s", "_big"));
                else if (newFileName.EndsWith("_us"))
                    newFileName = string.Format(@"{0}\{1}.png", newPath, newFileName.Replace("_us", "_small"));
                File.Copy(file.FullName, newFileName);
            }
        }

        private void Clear()
        {
            this.txtFilePath.text = "";
            this.txtFilePath.Select();
        }

        private void ShowResult(bool success, string content)
        {
            this.txtResult.text = content;
            this.txtResult.color = success ? Color.black : Color.red;
        }
    }
}