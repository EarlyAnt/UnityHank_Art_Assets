namespace Gululu.LocalData.Agents
{
    public interface ILocalCupAgent
    {
        ILocalDataManager localDataRepository { get; set; }
        void saveCupToken(string cupSN, string token);

        string getCupToken(string cupSN);
        void saveCupID(string cupSN, string cupid);

        string getCupID(string cupSN);

    }
}