namespace Gululu.LocalData.Agents
{
    public class LocalClockInfoAgent : ILocalClockInfoAgent
    {
        [Inject]
        public ILocalDataManager localDataRepository{ get;set;}

        public string getNetClockInfo(string childSn)
        {
            return localDataRepository.getObject<string>(getNetClockInfokey(childSn),"");
        }

        public string getOtherClockInfo(string childSn)
        {
            return localDataRepository.getObject<string>(getOtherClockInfoKey(childSn),"");
        }

        public void saveNetClockInfo(string childSn, string netClockInfo)
        {
            localDataRepository.saveObject<string>(getNetClockInfokey(childSn),netClockInfo);
        }

        public void saveOtherClockInfo(string childSn, string otherClockInfo)
        {
            localDataRepository.saveObject<string>(getOtherClockInfoKey(childSn),otherClockInfo);
        }

        private string getNetClockInfokey(string childSn){
            return childSn+"NET_CLOCK_INFO_KEY";
        }

        private string getOtherClockInfoKey(string childSn){
            return childSn + "OTHER_CLOCK_INFO_KEY";
        }
    }
}