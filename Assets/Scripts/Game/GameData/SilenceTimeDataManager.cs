using Gululu.Util;
using Hank.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using strange.extensions.mediation.impl;
using Gululu.LocalData.DataManager;
using Gululu;
using Gululu.LocalData.Agents;
using Hank.MainScene;
//using Hank.AlertBoard;

namespace Hank
{
    public class SilenceTimeDataManager : ISilenceTimeDataManager
    {
        [Inject]
        public IGameDataHelper gameDataHelper { get; set; }
        [Inject]
        public ICupClockManager mCupClockManager { get; set; }
        [Inject]
        public ILocalChildInfoAgent mLocalChildInfoAgent { get; set; }
        //[Inject]
        //public AlertBoardSignal alertBoardSignal { get; set; }

        public List<SilenceTimeSection>[] listStudyTimeSection = new List<SilenceTimeSection>[7];
        public List<SilenceTimeSection>[] listSleepTimeSection = new List<SilenceTimeSection>[7];
        public List<SilenceTimeSection>[] listOtherTimeSection = new List<SilenceTimeSection>[7];

        public List<ClocksItem> listClocksItemSection = new List<ClocksItem>();

        AllSilenceTimeSectionFromServer _allSilenceTimeSectionFromServer = AllSilenceTimeSectionFromServer.getBuilder().build();
        public AllSilenceTimeSectionFromServer allSilenceTimeSectionFromServer
        {
            get { return _allSilenceTimeSectionFromServer; }
        }
        public SilenceTimeSection GetSilenceTimeFromTime(SilenceTimeSection.SectionType type, DateTime time, out int second)
        {
            second = 0;
            List<SilenceTimeSection>[] listlist = GetSilenceTimeSectionListList(type);

            foreach (var section in listlist[(int)time.DayOfWeek])
            {
                DateTime beginTime = new DateTime(time.Year, time.Month, time.Day, section.beginHour, section.beginMinute, 0);
                DateTime endTime;
                if (section.crossDayType == SilenceTimeSection.CrossDayType.EndCross)
                {
                    DateTime dateTimeNextDay = time.AddDays(1);
                    endTime = new DateTime(dateTimeNextDay.Year, dateTimeNextDay.Month, dateTimeNextDay.Day, section.endHour, section.endMinute, 0);
                }
                else
                {
                    endTime = new DateTime(time.Year, time.Month, time.Day, section.endHour, section.endMinute, 0);
                }
                if (time > beginTime && time < endTime)
                {
                    return section;
                }
                if (time < beginTime)
                {
                    second = (int)(beginTime - time).TotalSeconds;
                    return section;
                }
            }
            return null;
        }
        [PostConstruct]

        // Use this for initialization
        public void PostConstruct()
        {
            LoadSilenceTimeSection();
        }

        // Update is called once per frame
        void Update()
        {

        }

        int Comparison(SilenceTimeSection a, SilenceTimeSection b)
        {
            DateTime beginTimeA = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, a.beginHour, a.beginMinute, 0);
            DateTime beginTimeB = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, b.endHour, b.endMinute, 0);
            if (beginTimeA > beginTimeB)
            {
                return 1;
            }
            else if (beginTimeA == beginTimeB)
                return 0;
            return -1;

        }

        List<SilenceTimeSection> SortAndMergeSection(List<SilenceTimeSection> list)
        {
            if (list.Count < 2)
            {
                return list;
            }
            list.Sort(Comparison);
            List<SilenceTimeSection> retList = new List<SilenceTimeSection>();
            for (int i = 0; i < list.Count; ++i)
            {
                int mergeCount = 0;
                DateTime endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, list[i].endHour, list[i].endMinute, 0);
                for (int next = i + 1; next < list.Count; ++next)
                {
                    DateTime beginTimeNext = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, list[next].beginHour, list[next].beginMinute, 0);
                    if (beginTimeNext < endTime)
                    {
                        DateTime endTimeNext = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, list[next].endHour, list[next].endMinute, 0);
                        if (endTimeNext > endTime)
                        {
                            endTime = endTimeNext;
                        }
                        ++mergeCount;
                    }
                }
                if (mergeCount > 0)
                {
                    SilenceTimeSection newSection = new SilenceTimeSection(list[i]);
                    newSection.endHour = endTime.Hour;
                    newSection.endMinute = endTime.Minute;
                    retList.Add(newSection);
                    i += mergeCount;
                }
                else
                {
                    retList.Add(list[i]);
                }
            }
            return retList;
        }

        void SortAndMergeSection()
        {
            for (int i = 0; i < listStudyTimeSection.Length; ++i)
            {
                listStudyTimeSection[i] = SortAndMergeSection(listStudyTimeSection[i]);
            }
            for (int i = 0; i < listSleepTimeSection.Length; ++i)
            {
                listSleepTimeSection[i] = SortAndMergeSection(listSleepTimeSection[i]);
            }
            for (int i = 0; i < listOtherTimeSection.Length; ++i)
            {
                listOtherTimeSection[i] = SortAndMergeSection(listOtherTimeSection[i]);
            }
        }

        List<SilenceTimeSection>[] GetSilenceTimeSectionListList(SilenceTimeSection.SectionType type)
        {
            switch (type)
            {
                case SilenceTimeSection.SectionType.Study:
                    return listStudyTimeSection;
                case SilenceTimeSection.SectionType.Sleep:
                    return listSleepTimeSection;
                default:
                    return listOtherTimeSection;
            }
        }

        void Clear()
        {
            for (int i = 0; i < listStudyTimeSection.Length; ++i)
            {
                if (listStudyTimeSection[i] != null)
                {
                    listStudyTimeSection[i].Clear();
                }
            }
            for (int i = 0; i < listSleepTimeSection.Length; ++i)
            {
                if (listSleepTimeSection[i] != null)
                {
                    listSleepTimeSection[i].Clear();
                }
            }
            for (int i = 0; i < listOtherTimeSection.Length; ++i)
            {
                if (listOtherTimeSection[i] != null)
                {
                    listOtherTimeSection[i].Clear();
                }
            }
            listClocksItemSection.Clear();
        }

        public void SaveSilenceTimeSection(List<SilenceTimeSectionFromServer> listSilenceTimeSection)
        {
            gameDataHelper.SaveObject<List<SilenceTimeSectionFromServer>>("SilenceTimeSectionList", listSilenceTimeSection);
        }

        public void LoadSilenceTimeSection()
        {
            for (int i = 0; i < listStudyTimeSection.Length; ++i)
            {
                if (listStudyTimeSection[i] == null)
                {
                    listStudyTimeSection[i] = new List<SilenceTimeSection>();
                }
                if (listSleepTimeSection[i] == null)
                {
                    listSleepTimeSection[i] = new List<SilenceTimeSection>();
                }
                if (listOtherTimeSection[i] == null)
                {
                    listOtherTimeSection[i] = new List<SilenceTimeSection>();
                }
            }

            List<SilenceTimeSectionFromServer> listSilenceTimeSection = gameDataHelper.GetObject<List<SilenceTimeSectionFromServer>>("SilenceTimeSectionList");
            if (listSilenceTimeSection != null)
            {
                InitialSilenceTimeSection(listSilenceTimeSection, false);
            }
            else
            {
                Clear();
            }
        }

        void DumpSilenceTimeData(List<SilenceTimeSectionFromServer> listSilenceTimeSectionFromServer)
        {
            for (int i = 0; i < listSilenceTimeSectionFromServer.Count; ++i)
            {
                SilenceTimeSectionFromServer data = listSilenceTimeSectionFromServer[i];
                if (data.enable != 0)
                {
                    GuLog.Info("<><SilenceTimeData>type:" + data.type + "   enable:" + data.enable + "    begin:" + data.start_hour + ":" + data.start_min + "    end:" + data.end_hour + ":" + data.end_min + "    days:" + data.weekdays);
                }
            }
        }

        private void AddClockItem(SilenceTimeSection section, int dayIndex, bool bBegin)
        {
            ClocksItem newClocksItem = new ClocksItem
            {
                id = section.id,
                clock_type = section.GetSectionTypeKey() + (bBegin ? "_begin" : "_end"),
                clock_hour = bBegin ? section.beginHour : section.endHour,
                clock_min = bBegin ? section.beginMinute : section.endMinute,
                clock_enable = section.isOn ? 1 : 0,
                clock_weekdays = (dayIndex).ToString(),
                x_child_sn = mLocalChildInfoAgent.getChildSN()
            };
            listClocksItemSection.Add(newClocksItem);
        }

        void FillClocksItemList(List<SilenceTimeSection> listTimeSection, int dayIndex)
        {
            for (int i = 0; i < listTimeSection.Count; ++i)
            {
                SilenceTimeSection section = listTimeSection[i];
                if (section != null)
                {
                    if (section.crossDayType != SilenceTimeSection.CrossDayType.BeginCross)
                    {
                        AddClockItem(section, dayIndex, true);
                    }
                    if (section.crossDayType != SilenceTimeSection.CrossDayType.EndCross)
                    {
                        AddClockItem(section, dayIndex, false);
                    }
                }
            }
        }
        void FillClocksItemList()
        {
            for (int i = 0; i < listStudyTimeSection.Length; ++i)
            {
                FillClocksItemList(listStudyTimeSection[i], i);
            }
            for (int i = 0; i < listSleepTimeSection.Length; ++i)
            {
                FillClocksItemList(listSleepTimeSection[i], i);
            }
            for (int i = 0; i < listOtherTimeSection.Length; ++i)
            {
                FillClocksItemList(listOtherTimeSection[i], i);
            }

        }
        void DumpClocksItemSection()
        {
            for (int i = 0; i < listClocksItemSection.Count; ++i)
            {
                ClocksItem data = listClocksItemSection[i];
                if (data.clock_enable != 0)
                {
                    GuLog.Info("<><SilenceTimeData>type:" + data.clock_type + "   enable:" + data.clock_enable + "    begin:" + data.clock_hour + ":" + data.clock_min + "    days:" + data.clock_weekdays);
                }
            }
        }

        public void InitialSilenceTimeSection(List<SilenceTimeSectionFromServer> listSilenceTimeSectionFromServer, bool needSave = true)
        {
            DumpSilenceTimeData(listSilenceTimeSectionFromServer);
            SaveSilenceTimeSection(listSilenceTimeSectionFromServer);

            List<SilenceTimeSection> listSilenceTimeSection = new List<SilenceTimeSection>();
            foreach (var section in listSilenceTimeSectionFromServer)
            {
                listSilenceTimeSection.Add(new SilenceTimeSection(section));
            }
            InitialSilenceTimeSection(listSilenceTimeSection);

            FillClocksItemList();

            if (listClocksItemSection.Count > 0)
            {
                GuLog.Debug("<><SilenceTimeDataManager>listClocksItemSection[0] hour:" + listClocksItemSection[0].clock_hour
                    + "minute:" + listClocksItemSection[0].clock_min);
                DumpClocksItemSection();
            }

            mCupClockManager.setOtherClock(mLocalChildInfoAgent.getChildSN(), listClocksItemSection, () =>
            {
                GuLog.Info("<><SilenceTimeDataManager>set other alrm success !");
            }, () =>
            {
                GuLog.Error("<SilenceTimeDataManager><>set other alrm error !");
            });
        }


        void InitialSilenceTimeSection(List<SilenceTimeSection> listSilenceTimeSection)
        {
            Clear();
            foreach (var section in listSilenceTimeSection)
            {
                if (section.isOn)
                {
                    List<SilenceTimeSection>[] listlist = GetSilenceTimeSectionListList(section.sectionType);
                    for (int i = 0; i < 7; ++i)
                    {
                        if (section.IsDayActive(i))
                        {
                            DateTime beginTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, section.beginHour, section.beginMinute, 0);
                            DateTime endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, section.endHour, section.endMinute, 0);
                            if (endTime > beginTime)
                            {
                                section.crossDayType = SilenceTimeSection.CrossDayType.NoCross;
                                listlist[i].Add(section);
                            }
                            else if (endTime == beginTime)
                            {
                                continue;
                            }
                            else
                            {//cross day
                                SilenceTimeSection preDaySection = new SilenceTimeSection(section);
                                preDaySection.endHour = 0;
                                preDaySection.endMinute = 0;
                                preDaySection.crossDayType = SilenceTimeSection.CrossDayType.EndCross;
                                listlist[i].Add(preDaySection);

                                SilenceTimeSection nextDaySection = new SilenceTimeSection(section);
                                nextDaySection.beginHour = 0;
                                nextDaySection.beginMinute = 0;
                                nextDaySection.crossDayType = SilenceTimeSection.CrossDayType.BeginCross;
                                int nextDayIndex = i + 1;
                                if (nextDayIndex >= 7)
                                {
                                    nextDayIndex = 0;
                                }
                                listlist[nextDayIndex].Add(nextDaySection);
                            }
                        }
                    }
                }
            }
            SortAndMergeSection();
        }

        //test school mode
        public void test_school()
        {
            List<SilenceTimeSectionFromServer> listSilenceTimeSection = new List<SilenceTimeSectionFromServer>();

            SilenceTimeSectionFromServer silenceTimeSection = new SilenceTimeSectionFromServer();
            //silenceTimeSection.type = "school";
            silenceTimeSection.type = "sleep";
            DateTime startTime = DateTime.Now.AddMinutes(-1);
            DateTime endTime = DateTime.Now.AddMinutes(1);
            silenceTimeSection.start_hour = startTime.Hour;
            silenceTimeSection.start_min = startTime.Minute;
            silenceTimeSection.end_hour = endTime.Hour;
            silenceTimeSection.end_min = endTime.Minute;
            //silenceTimeSection.start_hour = 14;
            //silenceTimeSection.start_min = 31;
            //silenceTimeSection.end_hour = 13;
            //silenceTimeSection.end_min = 34;
            silenceTimeSection.weekdays = "0,1,2,3,4,5,6";
            silenceTimeSection.enable = 1;
            silenceTimeSection.id = 9854;
            listSilenceTimeSection.Add(silenceTimeSection);
            InitialSilenceTimeSection(listSilenceTimeSection, false);

        }

        public void test_schoolBak()
        {
            //List<SilenceTimeSection> listSilenceTimeSection = new List<SilenceTimeSection>();

            //SilenceTimeSection silenceTimeSection = new SilenceTimeSection();
            ////silenceTimeSection.sectionType = SilenceTimeSection.SectionType.Study;
            //silenceTimeSection.sectionType = SilenceTimeSection.SectionType.Sleep;
            //DateTime startTime = DateTime.Now.AddMinutes(3);
            //DateTime endTime = DateTime.Now.AddMinutes(4);
            //silenceTimeSection.beginHour = startTime.Hour;
            //silenceTimeSection.beginMinute = startTime.Minute;
            //silenceTimeSection.endHour = endTime.Hour;
            //silenceTimeSection.endMinute = endTime.Minute;
            //silenceTimeSection.days = 63;
            //silenceTimeSection.isOn = true;
            //silenceTimeSection.id = 9854;
            //listSilenceTimeSection.Add(silenceTimeSection);
            //InitialSilenceTimeSection(listSilenceTimeSection);

            List<SilenceTimeSectionFromServer> listSilenceTimeSection = new List<SilenceTimeSectionFromServer>();

            SilenceTimeSectionFromServer silenceTimeSection = new SilenceTimeSectionFromServer();
            //silenceTimeSection.type = "school";
            silenceTimeSection.type = "sleep";
            DateTime startTime = DateTime.Now.AddMinutes(3);
            DateTime endTime = DateTime.Now.AddMinutes(4);
            silenceTimeSection.start_hour = startTime.Hour;
            silenceTimeSection.start_min = startTime.Minute;
            silenceTimeSection.end_hour = endTime.Hour;
            silenceTimeSection.end_min = endTime.Minute;
            silenceTimeSection.weekdays = "0,1,2,3,4,5,6";
            silenceTimeSection.enable = 1;
            silenceTimeSection.id = 9854;
            listSilenceTimeSection.Add(silenceTimeSection);
            InitialSilenceTimeSection(listSilenceTimeSection, false);

        }

        public bool InSilenceMode(DateTime dateTime)
        {
            return this.InSleepMode(dateTime) || this.InSchoolMode(dateTime);
        }
        public bool InSleepMode(DateTime dateTime)
        {
            int seconds = 0;
            SilenceTimeSection section = this.GetSilenceTimeFromTime(SilenceTimeSection.SectionType.Sleep, DateTime.Now, out seconds);
            if (section != null && seconds == 0)
            {
                Debug.LogFormat("----SilenceTimeDataManager->InSleepMode: {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}----",
                                section.sectionType, section.beginHour, section.beginMinute,
                                section.endHour, section.endMinute, section.days, section.isOn, seconds);
                return true;
            }
            else return false;
        }
        public bool InSchoolMode(DateTime dateTime)
        {
            int seconds = 0;
            SilenceTimeSection section = this.GetSilenceTimeFromTime(SilenceTimeSection.SectionType.Study, DateTime.Now, out seconds);
            if (section != null && seconds == 0)
            {
                Debug.LogFormat("----PowerKeyLongPress->InSchoolMode: {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}----",
                                section.sectionType, section.beginHour, section.beginMinute,
                                section.endHour, section.endMinute, section.days, section.isOn, seconds);
                return true;
            }
            else return false;
        }
    }
}