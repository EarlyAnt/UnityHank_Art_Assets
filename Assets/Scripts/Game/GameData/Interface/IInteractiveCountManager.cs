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
    public enum InteractiveType {
        SystemSlipUpdown,
        ScreenOn,
        NoviceGuide1,
        NoviceGuide3,
        NoviceGuide4,
        NoviceGuide5,
        SystemMenuSoundGuide,
    }
    public interface IInteractiveCountManager
    {
        void Reset();

        void SaveInteractiveCount();
        void LoadInteractiveCount();

        void SetInteractive(InteractiveType interactiveId, int interactiveCount);
        void AddInteractive(InteractiveType interactiveId, int count = 1);
        bool HasInteractive(InteractiveType interactiveId, int count = 1);

        int GetInteractiveCount(InteractiveType interactiveId);

        bool IsNeedNoviceGuide();
    }
}

