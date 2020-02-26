using Gululu.Util;


namespace Gululu{
    
    public enum ServiceType
    {
        Product,
        Staging,
        dev
    }

    public class ServiceConfig
    {
        public static ServiceType service
        {
            get
            {
#if WETEST_SDK
                return ServiceType.Staging;
#else
                return ServiceType.Product; 
#endif
    }
}

        public static string defaultChildSn
        {
            get
            {
                return "32QNPYGXFEKW";
                //return "Y0RMLOWBFX2Q";
            }
        }

        public static string defaultCupHwSn
        {
            get
            {
                return "20140101541292";
                //return "22180807000217";
            }
        }

    }



}
