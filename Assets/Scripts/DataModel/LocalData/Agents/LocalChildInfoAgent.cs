using UnityEngine;

namespace Gululu.LocalData.Agents
{
    public class LocalChildInfoAgent : ILocalChildInfoAgent
    {
        [Inject]
        public ILocalDataManager localDataRepository { get; set; }


        [Inject]
        public IJsonUtils mJsonUtils { get; set; }
        private string mChildSn;


        public void saveChildSN(string currentChildSN)
        {
            mChildSn = currentChildSN;
            localDataRepository.saveObject<string>(getChildSNSaveKey(), currentChildSN);
            //Debug.LogFormat("--------SaveSN--------LocalChildInfoAgent.saveChildSN: {0}", currentChildSN);
        }

        public string getChildSN()
        {
            if (mChildSn == null || mChildSn == string.Empty)
            {
//#if UNITY_EDITOR
//                saveChildSN(ServiceConfig.defaultChildSn);
//#endif
                mChildSn = localDataRepository.getObject<string>(getChildSNSaveKey(), "");
                //Debug.LogFormat("--------SaveSN--------LocalChildInfoAgent.getChildSN: {0}", mChildSn);
            }
            return mChildSn;
        }

        private string getChildSNSaveKey()
        {
            return "cup_current_childsn";
        }

        public void Clear()
        {
            mChildSn = string.Empty;
        }
    }
}