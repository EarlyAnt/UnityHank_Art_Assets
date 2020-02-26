using System.Collections.Generic;

namespace Gululu.LocalData.Agents
{
    public interface ILocalUnlockedPetsAgent
    {
        ILocalDataManager LocalDataRepository { get; set; }
        List<string> GetUnlockedPets();
        void SaveUnlockedPets(List<string> unlockedPets);
        void Clear();
    }
}