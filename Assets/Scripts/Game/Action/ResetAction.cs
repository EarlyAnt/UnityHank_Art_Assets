using Cup.Utils.android;
using Gululu;
using Gululu.LocalData.Agents;
using Hank.Api;
using UnityEngine;

namespace Hank.Action
{
    public class ResetAction : ActionBase
    {
        [Inject]
        public IDrinkWaterUIPresentManager uiState { get; set; }
        [Inject]
        public ILocalDataManager localDataRepository { get; set; }
        [Inject]
        public ILocalChildInfoAgent mLocalChildInfoAgent { get; set; }
        [Inject]
        public ILocalPetInfoAgent mLocalPetInfoAgent { get; set; }

        HeartBeatReset value;
        public override void Init(HeartBeatActionBase actionModel)
        {
            value = actionModel as HeartBeatReset;
        }
        public override void DoAction()
        {
            localDataRepository.deleteAll(true);
            mLocalChildInfoAgent.Clear();
            mLocalPetInfoAgent.Clear();
            BuildBackAckBody(value.ack, "Reset", "OK", value.todoid);

            Loom.QueueOnMainThread(() =>
            {
                PowerManager.reboot();
            });
        }
    }
}
