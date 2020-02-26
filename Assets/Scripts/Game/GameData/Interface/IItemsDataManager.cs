using BestHTTP;
using Gululu;
using Gululu.LocalData.Agents;
using Gululu.net;
using Gululu.Util;
using LitJson;
using strange.extensions.mediation.impl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hank
{

    public interface IItemsDataManager
    {
        void SendAllItemsData(bool force);
        void SetValuesLong(Hank.Api.GameData datas);


        void SaveItemsData();
        void LoadItemsData();

        void SetItem(int itemId, int itemCount);
        void AddItem(int itemId, int count = 1);
        bool HasItem(int itemId, int count = 1);

        int GetItemCount(int itemId);
        List<int> GetAllItemInfo();
    }
}

