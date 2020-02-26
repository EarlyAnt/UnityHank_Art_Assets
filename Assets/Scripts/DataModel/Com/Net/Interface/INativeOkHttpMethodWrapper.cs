using System;

namespace Gululu
{
    public interface INativeOkHttpMethodWrapper
    {
        void post(string url, string headerStr, string body, Action<string> result,Action<ResponseErroInfo> faile);

        void put(string url, string headerStr, string body, Action<string> result,Action<ResponseErroInfo> faile);

        void get(string url, string headerStr, Action<string> result,Action<ResponseErroInfo> faile);

        void delete(string url, string headerStr, Action<string> result,Action<ResponseErroInfo> faile);

        void delete(string url, string headerStr, string body, Action<string> result,Action<ResponseErroInfo> faile);
    }
}