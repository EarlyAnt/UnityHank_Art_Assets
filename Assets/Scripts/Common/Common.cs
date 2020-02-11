using UnityEngine;
using System.Collections;

public enum Regions
{
    Dummy_head,
    Dummy_wing,
    Dummy_taozhuang,
    TopLeft,
    BottomRight
}

public static class Roles
{
    public const string Purpie = "PURPIE";
    public const string Donny = "DONNY";
    public const string Ninji = "NINJI";
    public const string Sansa = "SANSA";
    public const string Yoyo = "YOYO";
    public const string Nuo = "NUO";
}

public static class Animations
{
    public const string PlayUp = "play_up_01";
    public const string PlayDown = "play_down_01";
    public const string PlayUpDown = "play_up_down_01";
    public const string EatHungry = "eat_hungry";
    public const string EatSatisfaction = "eat_satisfaction";
    public const string Idle = "idle_01";
    public const string SleepBegin = "sleep_begin_01";
    public const string SleepEnd = "sleep_end_01";
}

public static class AnimationParams
{
    public const string SleepBegin = "SleepBegin";
    public const string SleepEnd = "SleepEnd";
    public const string Hunger = "Hunger";
}
