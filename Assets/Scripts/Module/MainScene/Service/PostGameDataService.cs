using LitJson;
using System.Collections.Generic;
using Gululu;
using Gululu.LocalData.Agents;
using Cup.Utils.android;
using BestHTTP;
using Hank.Api;

namespace Hank.MainScene
{
    public class PostGameDataService : BaseService, IPostGameDataService
    {
        [Inject]
        public ServicePostGameDataBackSignal servicePostGameDataBackSignal { get; set; }

        [Inject]
        public ServicePostGameDataErrBackSignal servicePostGameDataErrBackSignal { get; set; }

        [Inject]
        public IPostGameDataModel modelPostGameData { set; get; }
		
		[Inject]
        public ILocalChildInfoAgent mLocalChildInfoAgent { get; set; }

        public void PostGameData(string paras)
        {
            string prod_name = AppData.GetAppName();

		    string x_child_sn = mLocalChildInfoAgent.getChildSN();
            if (x_child_sn == string.Empty)
            {
                return;
            }

            string x_cup_sn = CupBuild.getCupSn();
            GuLog.Info("PostGameData:" + paras);
			mNativeOkHttpMethodWrapper.post(mUrlProvider.GameDataUrl(x_child_sn, prod_name, x_cup_sn), "", paras,(r) => {
                
                PostGameDataResponse info = mJsonUtils.String2Json<PostGameDataResponse>(r);

                modelPostGameData.responsePostGameData = info;

                servicePostGameDataBackSignal.Dispatch(modelPostGameData.responsePostGameData);

            }, (e) => {
                servicePostGameDataErrBackSignal.Dispatch(e);
            });

        }
    }
}