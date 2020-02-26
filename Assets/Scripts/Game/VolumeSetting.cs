using UnityEngine;
using System.Collections;
using Gululu.Util;
using Gululu.Config;
using strange.extensions.mediation.impl;
using Gululu;
using UnityEngine.Assertions;
using Cup.Utils.audio;

namespace Hank
{
    public class VolumeSetting : IVolumeSetting
    {
        int nMaxLevel = 4;
        int nMaxSoundVolume = 15;
        int[] nVolumeLevel = null;
        int voicePercent;
        bool bInitialize = false;
        void InitializeVolumeLevel()
        {
            if (!bInitialize)
            {
                bInitialize = true;
                nVolumeLevel = new int[nMaxLevel];
                for (int i = 0; i < nMaxLevel; ++i)
                {
                    nVolumeLevel[i] = (int)(nMaxSoundVolume * i / (float)(nMaxLevel - 1));
                }
                //nVolumeLevel[0] = 0;
                //nVolumeLevel[1] = (int)(nMaxSoundVolume * 0.3f);
                //nVolumeLevel[2] = (int)(nMaxSoundVolume * 0.7f);
                //nVolumeLevel[3] = nMaxSoundVolume;
#if (UNITY_ANDROID) && (!UNITY_EDITOR)
            voicePercent = AudioControl.getVolume();
            nMaxSoundVolume = AudioControl.getMaxVolume();
#else
                voicePercent = nMaxSoundVolume;
#endif
                GuLog.Debug("<><VolumeSetting> voicePercent<><>:" + voicePercent);
            }

        }
        public int GetVolumeLevel()
        {
            InitializeVolumeLevel();
            int voiceLevel = nMaxLevel - 1;
            for (int i = nMaxLevel - 1; i >= 0; --i)
            {
                if (voicePercent >= nVolumeLevel[i])
                {
                    voiceLevel = i;
                    break;
                }
            }
            return voiceLevel;
        }

        private void SetVolume(int nRet)
        {
            voicePercent = nVolumeLevel[nRet];

#if (UNITY_ANDROID) && (!UNITY_EDITOR)
	                AudioControl.setVolume(voicePercent);
#endif
            GuLog.Debug("<><VolumeSetting> setVolume<><>:" + voicePercent);
        }

        public int LoopSetVolumeLevel(int nLevel)
        {
            InitializeVolumeLevel();
            int nRet = nLevel;
            if (nLevel == nMaxLevel - 1)
            {
                nRet = 0;
            }
            else
            {
                nRet = nLevel + 1;
            }

            SetVolume(nRet);
            return nRet;
        }

        public int SetHigherVolume(int nLevel)
        {
            InitializeVolumeLevel();
            int nRet = nLevel;
            if (nLevel >= nMaxLevel - 1)
            {
                nRet = nMaxLevel - 1;
            }
            else
            {
                nRet = nLevel + 1;
            }
            SetVolume(nRet);

            return nRet;

        }
        public int SetLowerVolume(int nLevel)
        {
            InitializeVolumeLevel();
            int nRet = nLevel;
            if (nLevel <= 0)
            {
                nRet = 0;
            }
            else
            {
                nRet = nLevel - 1;
            }
            SetVolume(nRet);
            return nRet;

        }

        public void SetCurrentVolume()
        {
            int currentVolume = this.GetVolumeLevel();
            GuLog.Debug("<><VolumeSetting>SetCurrentVolume<><>: " + currentVolume);
            this.SetVolume(currentVolume);
        }
    }
}
