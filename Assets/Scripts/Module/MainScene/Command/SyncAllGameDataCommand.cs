using Gululu.Util;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using UnityEngine;
namespace Hank.MainScene
{
    public class SyncAllGameDataCommand : Command
    {
        [Inject]
        public IPlayerDataManager dataManager { get; set; }
        [Inject]
        public IMissionDataManager missionDataManager { get; set; }
        [Inject]
        public IItemsDataManager itemsDataManager { get; set; }

        [Inject] public IFundDataManager fundDataManager { get; set; }

        public override void Execute()
        {
            missionDataManager.SendAllMissionData(true);
            dataManager.TrySendPlayerData2Server();
            itemsDataManager.SendAllItemsData(true);
            fundDataManager.SyncData2Server();
        }
    }
}