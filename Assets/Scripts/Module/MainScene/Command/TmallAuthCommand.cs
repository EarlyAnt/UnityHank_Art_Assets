using Gululu;
using Hank.Api;
using strange.extensions.command.impl;

namespace Hank.MainScene
{
    public class TmallAuthCommand : Command
    { 
        [Inject]
        public ITmallAuthenService mTmallAuthenService{get;set;}

        [Inject]
        public MediatorTmallAuthErrSignal mMediatorTmallAuthErrSignal{get;set;}

        [Inject]
        public MediatorTmallAuthSignal mMediatorTmallAuthSignal{get;set;}
        public override void Execute()
        {
            mTmallAuthenService.mTmallAuthBackSignal.AddListener(success);
            mTmallAuthenService.TmallAuthErrBackSignal.AddListener(error);

            mTmallAuthenService.checkAuthen();
            Retain();
        }

        public void success(TmallAuthResponse info){
            mMediatorTmallAuthSignal.Dispatch(info);

            mTmallAuthenService.mTmallAuthBackSignal.RemoveListener(success);
            mTmallAuthenService.TmallAuthErrBackSignal.RemoveListener(error);
            Release();
        }

        public void error(ResponseErroInfo errorinfo){
            mMediatorTmallAuthErrSignal.Dispatch(errorinfo);
            mTmallAuthenService.mTmallAuthBackSignal.RemoveListener(success);
            mTmallAuthenService.TmallAuthErrBackSignal.RemoveListener(error);
            Release();
        }
    }
}