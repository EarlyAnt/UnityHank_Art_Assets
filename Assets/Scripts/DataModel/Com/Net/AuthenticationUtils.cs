using System;
using System.Collections.Generic;
using System.Text;
using BestHTTP;
using Cup.Utils.android;
using Gululu.LocalData.Agents;
using Hank.Api;
using UnityEngine;

namespace Gululu.net
{
    public class AuthenticationUtils : IAuthenticationUtils
    {
        [Inject]
        public IUrlProvider mUrlProvider { get; set; }

        [Inject]
        public IJsonUtils mJsonUtils { get; set; }

        [Inject]
        public ILocalCupAgent mLocalCupAgent { get; set; }

        [Inject]
        public GululuNetworkHelper mGululuNetworkHelper { get; set; }

        public void reNewToken(Action<Result> callBack, Action<Result> errCallBack)
        {
            Dictionary<string, string> body = new Dictionary<string, string>();
            body.Add("access_cup", CupBuild.getCupSn());

            HTTPRequest hTTPRequest = new HTTPRequest(new Uri(mUrlProvider.getCupTokenUrl(CupBuild.getCupSn())), HTTPMethods.Post, (request, response) =>
            {
                Debug.LogFormat("--------AuthenticationUtils.reNewToken.GotResponse--------");
                Debug.LogFormat("response.IsSuccess: {0}", response.IsSuccess);
                Debug.LogFormat("response.DataAsText: {0}", response.DataAsText);
                Debug.LogFormat("response.Message: {0}", response.Message);
                Debug.LogFormat("request.State: {0}", request.State);

                if (response.IsSuccess)
                {
                    TokenResponseData mTokenResponseData = mJsonUtils.String2Json<TokenResponseData>(response.DataAsText);
                    string token = mTokenResponseData.token;
                    mLocalCupAgent.saveCupToken(CupBuild.getCupSn(), token);
                    if (callBack != null) callBack(Result.Success(token));
                }
                else
                {
                    if (errCallBack != null) errCallBack(Result.Error());
                }
            });

            string strBody = mJsonUtils.DicToJsonStr(body);
            hTTPRequest.RawData = Encoding.UTF8.GetBytes(strBody);

            hTTPRequest.AddHeader("Gululu-Agent", mGululuNetworkHelper.GetAgent());
            hTTPRequest.AddHeader("udid", mGululuNetworkHelper.GetUdid());
            hTTPRequest.AddHeader("Accept-Language", mGululuNetworkHelper.GetAcceptLang());
            hTTPRequest.SetHeader("Content-Type", "application/json");
            hTTPRequest.Send();
        }


    }

}