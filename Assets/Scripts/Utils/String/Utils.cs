namespace Gululu.Util
{
    public class StringUtils
    {
        public static bool is5GWifi(string ssid){
            return ssid.Contains("5G");
        }
    }
}