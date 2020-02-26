using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gululu.Util;
using System.Security;
using Mono.Xml;
using System;

namespace Gululu.Config
{
    public class Item
    {
        public int ItemId;
        public int Type;
        public string Name;
        public string IconPath;
    }

    public class NimItem : Item
    {
        public string GetImage;
        public int PetType;
        public string Image;
        public string Audio;
        public string Animation;
        public string Preview;
    }

    public class PropsItem : Item
    {
        public string GetImage;
    }

    public interface IItemConfig
    {
        void LoadItemsConfig();

        Item GetItemById(int id);

        Item GetItemByName(string name);

        List<NimItem> GetAllNimItem();

        List<NimItem> GetNimItemListByPetType(int petType);

        string GetImage(int id);

        bool IsLoadedOK();
    }
}

