using System;
using BestHTTP;
using Gululu.LocalData.Agents;

namespace Gululu.net
{
    public interface IAuthenticationUtils
    {
        IUrlProvider mUrlProvider{get;set;}
        IJsonUtils mJsonUtils{get;set;}
        ILocalCupAgent mLocalCupAgent{get;set;}
        void reNewToken(Action<Result> callBack, Action<Result> errCallBack);
    }
}