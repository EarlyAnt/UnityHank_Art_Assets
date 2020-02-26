namespace Gululu
{
    public class ResponseErroInfo
    {
        public static ResponseErroInfo GetErrorInfo(int ErrorCode,string ErrorInfo){
            GuLog.Debug(ErrorInfo);
            return new ResponseErroInfo(ErrorCode,ErrorInfo);
        }
        
        private ResponseErroInfo(int ErrorCode,string ErrorInfo){
            this.ErrorCode = ErrorCode;
            this.ErrorInfo = ErrorInfo;

        }
        public int ErrorCode;

        public string ErrorInfo;
    }
}