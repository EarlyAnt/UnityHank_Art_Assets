using UnityEngine;

namespace Gululu.LocalData.Agents
{
    public class LocalCupAgent : ILocalCupAgent
    {
        [Inject]
        public ILocalDataManager localDataRepository { set; get; }

        private string cuptoken;

        public void saveCupToken(string cupSN, string token)
        {
            cuptoken = token;
            Debug.LogFormat("--------LocalCupAgent.saveCupToken: {0}, {1}, {2}", cupSN, getTokenSaveKey(cupSN), token);
            localDataRepository.saveObject<string>(getTokenSaveKey(cupSN), token);
        }

        public string getCupToken(string cupSN)
        {
            if (cuptoken == null || cuptoken == "")
            {
                cuptoken = localDataRepository.getObject<string>(getTokenSaveKey(cupSN), "");
            }
            Debug.LogFormat("--------LocalCupAgent.getCupToken: {0}, {1}, {2}", cupSN, getTokenSaveKey(cupSN), cuptoken);
            return cuptoken;
        }

        private string getTokenSaveKey(string cupSN)
        {
            return cupSN + "LOCAL_CUP_TOKEN";
        }

        private string mCupid;
        public void saveCupID(string cupSN, string cupid)
        {
            mCupid = cupid;
            localDataRepository.saveObject<string>(getCupIDSaveKey(cupSN), cupid);
        }

        public string getCupID(string cupSN)
        {
            if (mCupid == null || mCupid == "")
            {
                mCupid = localDataRepository.getObject<string>(getCupIDSaveKey(cupSN), "");
            }
            return mCupid;
        }

        private string getCupIDSaveKey(string cupSN)
        {
            return cupSN + "LOCAL_CUP_ID";
        }



    }
}