using Gululu;
using strange.extensions.command.impl;
using System.Collections.Generic;

namespace Hank.MainScene
{
    public class PostGameDataCommand : Command
    {
        [Inject]
        public IPostGameDataService getPostGameDataService { get; set; }

        [Inject]
        public MediatorPostGameDataErrSignal mediatorPostGameDataErrSignal{ get; set; }

        [Inject]
        public MediatorPostGameDataSignal mediatorPostGameDataSignal { get; set; }

        [Inject]
        public string parms { set; get; }

        public override void Execute()
        {
            getPostGameDataService.servicePostGameDataBackSignal.AddListener(PostGameDataResult);
            getPostGameDataService.servicePostGameDataErrBackSignal.AddListener(PostGameDataResultErr);

            getPostGameDataService.PostGameData(parms);            

            Retain();
        }

        public void PostGameDataResult(PostGameDataResponse result){
            getPostGameDataService.servicePostGameDataBackSignal.RemoveListener(PostGameDataResult);
            getPostGameDataService.servicePostGameDataErrBackSignal.RemoveListener(PostGameDataResultErr);

            mediatorPostGameDataSignal.Dispatch(result);
			Release();
        }

        public void PostGameDataResultErr(ResponseErroInfo result)
        {
            getPostGameDataService.servicePostGameDataBackSignal.RemoveListener(PostGameDataResult);
            getPostGameDataService.servicePostGameDataErrBackSignal.RemoveListener(PostGameDataResultErr);
            mediatorPostGameDataErrSignal.Dispatch(result);
			Release();
        }
    }
}
/*
            injectionBinder.Bind<IPostGameDataModel>().To<PostGameDataModel>().ToSingleton();
            injectionBinder.Bind<IPostGameDataService>().To<PostGameDataService>().ToSingleton();
            injectionBinder.Bind<ServicePostGameDataBackSignal>().ToSingleton();
            injectionBinder.Bind<ServicePostGameDataErrBackSignal>().ToSingleton();
            injectionBinder.Bind<MediatorPostGameDataSignal>().ToSingleton();
            injectionBinder.Bind<MediatorPostGameDataErrSignal>().ToSingleton();
            commandBinder.Bind<StartPostGameDataCommandSignal>().To<PostGameDataCommand>();
*/