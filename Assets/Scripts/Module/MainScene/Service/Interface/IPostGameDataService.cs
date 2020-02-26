using System.Collections.Generic;
namespace Hank.MainScene
{
    public interface IPostGameDataService
    { 
        ServicePostGameDataBackSignal servicePostGameDataBackSignal { get; set; }

        ServicePostGameDataErrBackSignal servicePostGameDataErrBackSignal { get; set; }

        IPostGameDataModel modelPostGameData { set; get; }

        void PostGameData( string paras);
    }
}
