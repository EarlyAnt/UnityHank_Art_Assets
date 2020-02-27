using UnityEngine;
using System.Collections;
using ModelTest;
using System.Collections.Generic;

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

public enum Operations
{
    Pet = 0,
    Animation = 1,
    Dress = 2,
    Accessory = 3,
    Suit = 4,
    Sprite = 5,
    Back = 99
}

[System.Serializable]
public class DressInfo
{
    [SerializeField]
    public Transform rootObject;
    [SerializeField]
    public List<AccessoryButton> accessories;
    public AccessoryButton CurrentAccessory
    {
        get
        {
            return this.accessories[this.selectedIndex];
        }
    }
    private int selectedIndex;

    public void CollectButton()
    {
        this.accessories = new List<AccessoryButton>();
        if (this.rootObject != null && this.rootObject.childCount > 0)
        {
            for (int i = 0; i < this.rootObject.childCount; i++)
            {
                AccessoryButton accessoryButton = this.rootObject.GetChild(i).GetComponent<AccessoryButton>();
                if (accessoryButton != null)
                    this.accessories.Add(accessoryButton);
            }
        }
    }
    public void PreviousAccessory()
    {
        if (this.selectedIndex > 0)
            this.selectedIndex -= 1;
    }
    public void NextAccessory()
    {
        if (this.selectedIndex + 1 < this.accessories.Count)
            this.selectedIndex += 1;
    }
}
