namespace Gululu.LocalData.Agents
{
    public interface ILocalChildInfoAgent
    {
        ILocalDataManager localDataRepository { get; set; }
        IJsonUtils mJsonUtils { get; set; }
        void saveChildSN(string currentChildSN);

        string getChildSN();

        void Clear();
    }
}