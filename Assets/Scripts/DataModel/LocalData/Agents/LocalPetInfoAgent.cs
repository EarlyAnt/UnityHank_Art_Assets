using Gululu;
using Gululu.Config;
using UnityEngine;

namespace Gululu.LocalData.Agents
{
    public class LocalPetInfoAgent : ILocalPetInfoAgent
    {
        [Inject]
        public IRoleConfig RoleConfig { get; set; }
        [Inject]
        public ILocalDataManager localDataRepository { get; set; }
        [Inject]
        public IResourceUtils ResourceUtils { get; set; }

        private string currentPetName;

        public string getCurrentPet()
        {
            if (currentPetName == null || currentPetName == string.Empty)
            {
                currentPetName = localDataRepository.getObject<string>(getCurrentPetKey(), "NINJI");
            }

            this.currentPetName = this.CheckPetName("getCurrentPet", this.currentPetName);
            return currentPetName;
        }

        public void saveCurrentPet(string currentPet)
        {
            this.currentPetName = this.CheckPetName("saveCurrentPet", currentPet);
            localDataRepository.saveObject<string>(getCurrentPetKey(), currentPet);
        }

        private string CheckPetName(string operate, string petName)
        {
            if (petName.EndsWith("2"))
            {
                GuLog.Error(string.Format("----LocalPetInfoAgent.{0}----Petname ends with '2': {1}", operate, petName));
                petName = petName.Replace("2", "");
                GuLog.Error(string.Format("----LocalPetInfoAgent.{0}----Petname removed '2' {1}", operate, petName));
            }
            else
                GuLog.Debug(string.Format("----LocalPetInfoAgent.{0}----Petname is correct: {1}", operate, petName));

            return petName;
        }

        private string getCurrentPetKey()
        {
            return "CUP_CURRENT_PET";
        }

        public void Clear()
        {
            currentPetName = string.Empty;
            localDataRepository.removeObject(this.getCurrentPetKey());
        }

        public bool ModelExists(string petName)
        {
            RoleInfo roleInfo = this.RoleConfig.GetRoleInfo(petName);
            bool existed = roleInfo != null && !roleInfo.IsLocal &&
                           this.ResourceUtils.FileExisted(string.Format("Model/{0}.ab",  roleInfo.AB));
            return existed;
        }
    }
}