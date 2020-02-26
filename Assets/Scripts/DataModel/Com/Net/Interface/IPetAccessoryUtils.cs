using Hank.Api;
using System;

namespace Gululu.net
{
    public interface IPetAccessoryUtils
    {
        void BuyPetAccessories(PetAccessoryOrder petAccessoryOrder, Action<Result> callBack = null, Action<Result> errCallBack = null);

        void GetPaymentResult(string orderSN, Action<GetPaymentResultResponse> callBack = null, Action<string> errCallBack = null);

        void GetPaidItems(Action<GetPaidItemsResponse> callBack = null, Action<string> errCallBack = null);
    }
}