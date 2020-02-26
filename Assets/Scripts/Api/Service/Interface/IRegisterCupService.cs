using System.Collections.Generic;
using Gululu;
using Gululu.LocalData.Agents;
using Gululu.net;

namespace Hank.Api
{
    public interface IRegisterCupService
    { 
        ServiceRegisterCupBackSignal serviceRegisterCupBackSignal { get; set; }

        ServiceRegisterCupErrBackSignal serviceRegisterCupErrBackSignal { get; set; }

        IRegisterCupModel modelRegisterCup { set; get; }

        ILocalChildInfoAgent mLocalChildInfoAgent{ set; get; }

        ILocalCupAgent mLocalCupAgent{ set; get; }

        ILocalPetInfoAgent LocalPetInfoAgent{ set; get; }

        void RegisterCup(RegisterCupBody body);
    }
}
