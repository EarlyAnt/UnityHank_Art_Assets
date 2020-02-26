namespace Gululu.LocalData.Agents
{
    public interface ILocalPetInfoAgent
    {
        ILocalDataManager localDataRepository { get; set; }

        string getCurrentPet();
        void saveCurrentPet(string currentPet);
        void Clear();

        bool ModelExists(string petName);
    }
}