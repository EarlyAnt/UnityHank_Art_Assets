using Gululu.Config;
using Hank;
using System.Collections.Generic;
using UnityEngine;

namespace Gululu.LocalData.Agents
{
    class LocalPetAccessoryAgent : ILocalPetAccessoryAgent
    {
        [Inject]
        public IGameDataHelper GameDataHelper { get; set; }
        [Inject]
        public IAccessoryConfig AccessoryConfig { get; set; }
        private Dictionary<string, List<string>> accessoryDatas;
        private const string DATA_KEY = "PetAccessory";

        public void LoadData()
        {
            this.accessoryDatas = this.GameDataHelper.GetObject<Dictionary<string, List<string>>>(DATA_KEY);
            if (this.accessoryDatas == null)
                this.accessoryDatas = new Dictionary<string, List<string>>();
        }

        public List<string> GetPetAccessories(string petName)
        {
            if (this.accessoryDatas.ContainsKey(petName))
            {
                return this.accessoryDatas[petName];
            }
            else
            {
                List<string> emptyList = new List<string>();
                this.accessoryDatas.Add(petName, emptyList);
                return emptyList;
            }
        }

        public List<Accessory> GetAllPetAccessories(string petName)
        {
            List<string> datas = this.GetPetAccessories(petName);
            if (datas != null && datas.Count > 0)
            {
                List<Accessory> accessories = this.AccessoryConfig.GetAllAccessories();
                if (accessories != null)
                {
                    List<Accessory> result = new List<Accessory>(accessories);
                    foreach (var accessory in accessories.ToArray())
                    {
                        if (!datas.Contains(accessory.ID.ToString()))
                            result.Remove(accessory);
                    }
                    return result;
                }
                return null;
            }
            return null;
        }

        public List<Accessory> GetAccessoriesByType(string petName, int accessoryType)
        {
            List<string> datas = this.GetPetAccessories(petName);
            if (datas != null && datas.Count > 0)
            {
                List<Accessory> accessories = this.AccessoryConfig.GetAccessoriesByTypes(new List<int> { accessoryType });
                if (accessories != null)
                {
                    List<Accessory> result = new List<Accessory>(accessories);
                    foreach (var accessory in accessories.ToArray())
                    {
                        if (!datas.Contains(accessory.ID.ToString()))
                            result.Remove(accessory);
                    }
                    return result;
                }
                return null;
            }
            return null;
        }

        public void SavePetAccessories(string petName, List<string> petAccessories)
        {
            if (this.accessoryDatas.ContainsKey(petName))
                this.accessoryDatas[petName] = petAccessories;
            else
                this.accessoryDatas.Add(petName, petAccessories);
            this.GameDataHelper.SaveObject<Dictionary<string, List<string>>>(DATA_KEY, this.accessoryDatas);
        }

        public void Clear()
        {
            this.GameDataHelper.Clear(DATA_KEY);
        }
    }
}
