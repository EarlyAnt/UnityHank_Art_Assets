using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gululu.LocalData.Agents
{
    class LocalUnlockedPetsAgent : ILocalUnlockedPetsAgent
    {
        [Inject]
        public ILocalDataManager LocalDataRepository { get; set; }
        private const string DATA_KEY = "UnlockedPets";

        public List<string> GetUnlockedPets()
        {
            return this.CheckPetName("GetUnlockedPets", this.LocalDataRepository.getObject<List<string>>(DATA_KEY));
        }

        public void SaveUnlockedPets(List<string> unlockedPets)
        {
            if (unlockedPets != null)
            {
                unlockedPets = this.CheckPetName("SaveUnlockedPets", unlockedPets);
                this.LocalDataRepository.saveObject<List<string>>(DATA_KEY, unlockedPets);
            }
        }

        public void Clear()
        {
            this.LocalDataRepository.removeObject(DATA_KEY);
        }

        private List<string> CheckPetName(string operate, List<string> petNames)
        {
            List<string> checkedPetNames = new List<string>();
            if (petNames != null && petNames.Count > 0)
            {
                foreach (string petName in petNames)
                {
                    checkedPetNames.Add(this.CheckPetName(operate, petName));
                }
            }
            return checkedPetNames;
        }

        private string CheckPetName(string operate, string petName)
        {
            if (petName.EndsWith("2"))
            {
                GuLog.Error(string.Format("----LocalUnlockedPetsAgent.{0}----Petname ends with '2': {1}", operate, petName));
                petName = petName.Replace("2", "");
                GuLog.Error(string.Format("----LocalUnlockedPetsAgent.{0}----Petname removed '2' {1}", operate, petName));
            }
            else
                GuLog.Debug(string.Format("----LocalUnlockedPetsAgent.{0}----Petname is correct: {1}", operate, petName));

            return petName;
        }
    }
}
