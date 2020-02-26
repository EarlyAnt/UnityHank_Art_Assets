using Gululu.Util;
using Hank.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using strange.extensions.mediation.impl;

namespace Hank
{
    public interface ISilenceTimeDataManager
    {
        AllSilenceTimeSectionFromServer allSilenceTimeSectionFromServer { get; }
        SilenceTimeSection GetSilenceTimeFromTime(SilenceTimeSection.SectionType type, DateTime time, out int second);
        // Use this for initialization
        void SaveSilenceTimeSection(List<SilenceTimeSectionFromServer> listSilenceTimeSection);

//         void InitialSilenceTimeSection(AllSilenceTimeSectionFromServer listSilenceTimeSectionFromServer);
        void InitialSilenceTimeSection(List<SilenceTimeSectionFromServer> listSilenceTimeSectionFromServer, bool needSave = true);

        void LoadSilenceTimeSection();

        bool InSilenceMode(DateTime dateTime);
        bool InSleepMode(DateTime dateTime);
        bool InSchoolMode(DateTime dateTime);

        //test school mode
        void test_school();
        void test_schoolBak();
    }
}
