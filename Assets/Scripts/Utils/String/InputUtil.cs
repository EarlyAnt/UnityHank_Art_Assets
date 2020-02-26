using System.Text.RegularExpressions;

namespace Gululu.Util
{

    public class InputUtil
    {
        public static bool checkInputStrLengthTen(string inputStr)
        {
            int num = 0;
            for (int i = 0; i < inputStr.Length; i++)
            {
                if ((int)inputStr[i] > 127)
                {
                    num += 2;
                }
                else
                {
                    num += 1;
                }
            }

            if (num > 10)
            {
                return false;
            }
            return true;
        }

        public static bool checkLastSixCharacter(string inputStr)
        {
            if (inputStr.Length >= 6 && inputStr.Length <= 12)
            {
                return true;
            }
            return false;
        }

        public static bool checkLimitFourCharacter(string inputStr)
        {
            if (inputStr.Length == 4)
            {
                return true;
            }
            return false;
        }

        public static bool checkPhoneInputValid(string inputStr)
        {
            Regex r = new Regex("^[0-9]*$");
            if (r.IsMatch(inputStr) && inputStr.Length == 11)
            {
                return true;
            }
            return false;
        }

        public static bool checkYearInputValid(string inputStr)
        {
            Regex r = new Regex("^[0-9]*$");
            if (r.IsMatch(inputStr) && inputStr.Length == 4)
            {
                return true;
            }
            return false;
        }

        public static bool CheckEmailValidly(string eMail)
        {
            Regex r = new Regex("^\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$");
            if (r.IsMatch(eMail))
            {
                return true; 
            }
            else
            {
                return false;  
            }
        }

        //判断输入是否包含中文  不管你有没有输入英文,只要包含中文,就返回 true
        public static bool HasChinese(string content)
        {
            //判断是不是中文
            string regexstr = @"[\u4e00-\u9fa5]";

            if (Regex.IsMatch(content, regexstr))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //判断是不是数字
        public static bool isInterger(string str)
        {
            if (str == "")
            {
                return false;
            }
            else
            {
                foreach (char c in str)
                {
                    if (char.IsNumber(c))
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        //只允许数字或字母的判断
        public static bool isIntergerOrLetter(string content)
        {
            Regex reg1 = new Regex(@"^[A-Za-z0-9]+$");
            return reg1.IsMatch(content);
        }
    }
}