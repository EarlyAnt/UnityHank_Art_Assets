namespace Gululu.LocalData.Agents
{
    public interface ILocalClockInfoAgent
    {
        ILocalDataManager localDataRepository { get; set; }

        void saveNetClockInfo(string childSn,string netClockInfo);

        void saveOtherClockInfo(string childSn,string otherClockInfo);

        string getNetClockInfo(string childSn);

        string getOtherClockInfo(string childSn);


    }
}