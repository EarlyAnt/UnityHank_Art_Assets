using Gululu.Config;
using System.Collections.Generic;

namespace Gululu.LocalData.Agents
{
    public interface ILocalPetAccessoryAgent
    {
        void LoadData();
        List<string> GetPetAccessories(string petName);
        List<Accessory> GetAllPetAccessories(string petName);
        List<Accessory> GetAccessoriesByType(string petName, int accessoryType);
        void SavePetAccessories(string petName, List<string> petAccessories);
        void Clear();
    }
}