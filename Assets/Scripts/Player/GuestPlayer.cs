using Gululu.Config;
using Gululu.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hank
{
    public class GuestPlayer : BasePlayer
    {
        protected override string GetTag()
        {
            return "Guest";
        }

        public void RefreshRole(string petName)
        {
            base.PetName = !string.IsNullOrEmpty(petName) ? petName : LocalPetInfoAgent.getCurrentPet();
            string newPath = RoleConfig.GetGuestPath(base.PetName);
            this.LoadPrefab(newPath);
            this.RemoveLight();//小宠物的主人fbx和客人fbx合并后，交友时，需要把客人身上的灯光关掉
        }

        private void RemoveLight()
        {
            if (this.objModel != null)
            {
                Light[] lights = this.objModel.GetComponentsInChildren<Light>();
                if (lights != null && lights.Length > 0)
                {
                    for (int i = 0; i < lights.Length; i++)
                        lights[i].enabled = false;
                }
            }
        }
    }
};

