using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gululu.Util;
using System.Security;
using Mono.Xml;
using System;
namespace Gululu.Config
{
    public class Treasure
    {
        public int itemId;
        public int count;
    }

    [Serializable]
    public class TreasureInfo
    {
        public int ItemID;
        public Sprite Image;
        public AudioClip Audio;
        public GameObject Animation;
    }

    public class TreasureBox
    {
        public int treasureBoxId;
        public int type;
        public List<Treasure> items = new List<Treasure>();
    }

    public interface ITreasureBoxConfig
    {

        void LoadTreasureBoxConfig();

        TreasureBox GetTreasureBoxById(int treasureBoxId);

        TreasureBox GetTreasureBoxByItemId(int itemId);

        bool IsLoadedOK();

    }
}
