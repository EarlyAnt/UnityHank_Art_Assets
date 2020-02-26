namespace Gululu.Log
{
    public class NativeLogHandler : IGuLogHandler
    {
        Cup.Utils.android.LogUtils mLogUtils;
        public void Handler(Recoder recoder)
        {
            mLogUtils = new Cup.Utils.android.LogUtils();

            string loglevel = recoder.loglevel;

            if (loglevel == "[DEBUG]")
            {
                d(recoder.tag,recoder.msg);
            }
            else if (loglevel == "[INFO]")
            {
                i(recoder.tag,recoder.msg);
            }
            else if (loglevel == "[WARNING]")
            {
                w(recoder.tag,recoder.msg);
            }
            else if (loglevel == "[ERROR]")
            {
                e(recoder.tag,recoder.msg);
            }
            else if (loglevel == "[FATAL]")
            {
                wtf(recoder.tag,recoder.msg);
            }
            else
            {
                d(recoder.tag,recoder.msg);
            }

        }

         public void d(string tag, string log)
        {
            mLogUtils.d(tag,log);
        }

        public void e(string tag, string log)
        {
            mLogUtils.e(tag,log);
        }

        public void i(string tag, string log)
        {
            mLogUtils.i(tag,log);
        }

        public void v(string tag, string log)
        {
            mLogUtils.v(tag,log);
        }

        public void w(string tag, string log)
        {
            mLogUtils.w(tag,log);
        }

        public void wtf(string tag, string log)
        {
            mLogUtils.wtf(tag,log);
        }
    }
}