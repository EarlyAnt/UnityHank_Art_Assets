using UnityEngine;
using System.Collections;
using Gululu.Util;
using Gululu.Config;
using strange.extensions.mediation.impl;
using Gululu;
using UnityEngine.Assertions;

namespace Hank
{
    public interface IVolumeSetting
    {
        int GetVolumeLevel();
        int LoopSetVolumeLevel(int nLevel);
        int SetHigherVolume(int nLevel);
        int SetLowerVolume(int nLevel);
        void SetCurrentVolume();
    }
}
