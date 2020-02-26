using Gululu;
using Hank.Api;
using System;
using System.Collections.Generic;
using System.Text;


namespace Hank
{
    // public struct SilenceTimeSectionFromServer
    // {
    //     public int end_min;
    //     public int start_min;
    //     public int end_hour;
    //     public int start_hour;
    //     public int enable;
    //     public string type;
    //     public string weekdays;
    //     public int id;

    // }

    // public struct AllSilenceTimeSectionFromServer
    // {
    //     public List<SilenceTimeSectionFromServer> configs;
    // }

    public class SilenceTimeSection
    {
        public SilenceTimeSection()
        {
            sectionType = SectionType.Study;
            beginHour = 12;
            beginMinute = 30;
            endHour = 12;
            endMinute = 30;
            days = (int)WeekDays.Monday | (int)WeekDays.Tuesday | (int)WeekDays.Wednesday | (int)WeekDays.Thursday | (int)WeekDays.Friday;
            isOn = true;
            id = -1;
        }
        public SilenceTimeSection(SilenceTimeSection right)
        {
            sectionType = right.sectionType;
            beginHour = right.beginHour;
            beginMinute = right.beginMinute;
            endHour = right.endHour;
            endMinute = right.endMinute;
            days = right.days;
            isOn = right.isOn;
            id = right.id;
        }
        SectionType getTypeByString(string strType)
        {
            switch (strType)
            {
                case "sleep":
                    return SectionType.Sleep;
                case "school":
                    return SectionType.Study;
                default:
                    return SectionType.Other;
            }
        }
        void SetWeekDaysbyString(string strDays)
        {
            string[] strFragments = strDays.Split(',');
            foreach (var day in strFragments)
            {
                AddDay(Convert.ToInt32(day));
            }
        }

        public string GetWeekdayStringKey()
        {
            StringBuilder ret = new StringBuilder();
            for(int i = 0; i < 7; ++i)
            {
                if (IsDayActive(i))
                {
                    if (ret.Length != 0)
                    {
                        ret.Append(",");
                    }
                    ret.Append(i);
                }
            }
            return ret.ToString();

        }
        public SilenceTimeSection(SilenceTimeSectionFromServer data)
        {
            sectionType = getTypeByString(data.type);
            beginHour = data.start_hour;
            beginMinute = data.start_min;
            endHour = data.end_hour;
            endMinute = data.end_min;
            SetWeekDaysbyString(data.weekdays);
            isOn = (data.enable !=0);
            id = data.id;
        }
        public enum WeekDays
        {
            Sunday = 1,
            Monday = 1 << 1,
            Tuesday = 1 << 2,
            Wednesday = 1 << 3,
            Thursday = 1 << 4,
            Friday = 1 << 5,
            Saturday = 1 << 6,
        }
        public enum SectionType
        {
            Study = 0,
            Sleep,
            Other
        }
        public enum CrossDayType
        {
            BeginCross,
            EndCross,
            NoCross
        }
        public CrossDayType crossDayType = CrossDayType.NoCross;
        public SectionType sectionType;
        //         public string strSectionType;
//         public string GetSectionType()
//         {
//             switch (sectionType)
//             {
//                 case SectionType.Study:
//                     return LangManager.GetLanguageString("Study");
//                 case SectionType.Sleep:
//                     return LangManager.GetLanguageString("Sleep");
//                 default:
//                     return LangManager.GetLanguageString("Other");
//             }
//         }
        public string GetSectionTypeKey()
        {
            switch (sectionType)
            {
                case SectionType.Study:
                    return "study";
                case SectionType.Sleep:
                    return "sleep";
                default:
                    return "other";
            }
        }
        public int beginHour;
        public int beginMinute;
        public string GetBeginTimeString()
        {
            return beginHour.ToString() + ':' + string.Format("{0:D2}",beginMinute);
        }

        public int endHour;
        public int endMinute;
        public string GetEndTimeString()
        {
            return endHour.ToString() + ':' + string.Format("{0:D2}",endMinute);
        }

        public int days;
        public void AddDay(int day)
        {
            days |= (1 << day);
        }
        public void RemoveDay(int day)
        {
            days &= (~(1 << day));
        }
        public bool IsDayActive(int day)
        {
            return (days & (1 << day)) != 0;
        }
        public bool IsDayActive(WeekDays day)
        {
            return (days & (int)day) != 0;
        }
        public bool IsEveryDay()
        {
            return IsDayActive(WeekDays.Monday)
                && IsDayActive(WeekDays.Tuesday)
                && IsDayActive(WeekDays.Wednesday)
                && IsDayActive(WeekDays.Thursday)
                && IsDayActive(WeekDays.Friday)
                && IsDayActive(WeekDays.Saturday)
                && IsDayActive(WeekDays.Sunday)
                ;
        }
        public bool IsWorkDay()
        {
            return IsDayActive(WeekDays.Monday)
                && IsDayActive(WeekDays.Tuesday)
                && IsDayActive(WeekDays.Wednesday)
                && IsDayActive(WeekDays.Thursday)
                && IsDayActive(WeekDays.Friday)
                && (!IsDayActive(WeekDays.Saturday))
                && (!IsDayActive(WeekDays.Sunday))
                ;
        }
        public bool IsWeekEnd()
        {
            return (!IsDayActive(WeekDays.Monday))
                && (!IsDayActive(WeekDays.Tuesday))
                && (!IsDayActive(WeekDays.Wednesday))
                && (!IsDayActive(WeekDays.Thursday))
                && (!IsDayActive(WeekDays.Friday))
                && IsDayActive(WeekDays.Saturday)
                && IsDayActive(WeekDays.Sunday)
                                 ;
        }
//         public string GetWeekDaysString()
//         {
//             if (IsEveryDay())
//             {
//                 return LangManager.GetLanguageString("Everyday");
//             }
//             else if (IsWorkDay())
//             {
//                 return LangManager.GetLanguageString("Workday");
//             }
//             else if (IsWeekEnd())
//             {
//                 return LangManager.GetLanguageString("Weekend");
//             }
//             else
//             {
//                 StringBuilder ret = new StringBuilder();
//                 if (IsDayActive(WeekDays.Monday))
//                 {
//                     ret.Append(LangManager.GetLanguageString("Monday"));
//                     ret.Append(" ");
//                 }
//                 if (IsDayActive(WeekDays.Tuesday))
//                 {
//                     ret.Append(LangManager.GetLanguageString("Tuesday"));
//                     ret.Append(" ");
//                 }
//                 if (IsDayActive(WeekDays.Wednesday))
//                 {
//                     ret.Append(LangManager.GetLanguageString("Wednesday"));
//                     ret.Append(" ");
//                 }
//                 if (IsDayActive(WeekDays.Thursday))
//                 {
//                     ret.Append(LangManager.GetLanguageString("Thursday"));
//                     ret.Append(" ");
//                 }
//                 if (IsDayActive(WeekDays.Friday))
//                 {
//                     ret.Append(LangManager.GetLanguageString("Friday"));
//                     ret.Append(" ");
//                 }
//                 if (IsDayActive(WeekDays.Saturday))
//                 {
//                     ret.Append(LangManager.GetLanguageString("Saturday"));
//                     ret.Append(" ");
//                 }
//                 if (IsDayActive(WeekDays.Sunday))
//                 {
//                     ret.Append(LangManager.GetLanguageString("Sunday"));
//                     ret.Append(" ");
//                 }
//                 return ret.ToString();
//             }
//         }
        public bool isOn;

        public int id;
    }

}

