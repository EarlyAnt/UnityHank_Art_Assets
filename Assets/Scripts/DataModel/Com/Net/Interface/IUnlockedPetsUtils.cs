using System;
using BestHTTP;
using Gululu.LocalData.Agents;
using Hank.Api;

namespace Gululu.net
{
    public interface IUnlockedPetsUtils
    {
        IUrlProvider UrlProvider { get; set; }
        IJsonUtils JsonUtils { get; set; }
        ILocalCupAgent LocalCupAgent { get; set; }
        ILocalChildInfoAgent LocalChildInfoAgent { get; set; }
        ILocalUnlockedPetsAgent LocalUnlockedPetsAgent { get; set; }
        void SyncUnlockedPets(Action<Result> callBack = null, Action<Result> errCallBack = null);
    }
}