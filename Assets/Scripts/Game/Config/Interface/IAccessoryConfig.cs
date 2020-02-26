using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gululu.Config
{
    /// <summary>
    /// 配饰配置接口
    /// </summary>
    public interface IAccessoryConfig
    {
        /// <summary>
        /// 加载配饰数据
        /// </summary>
        void LoadAccessoryConfig();
        /// <summary>
        /// 获取所有配饰
        /// </summary>
        /// <returns></returns>
        List<Accessory> GetAllAccessories();
        /// <summary>
        /// 获取指定类型的配饰
        /// </summary>
        /// <param name="types">类型集合</param>
        /// <returns></returns>
        List<Accessory> GetAccessoriesByTypes(List<int> types);
        /// <summary>
        /// 获取指定配饰
        /// </summary>
        /// <param name="accessoryID">配饰ID</param>
        /// <returns></returns>
        Accessory GetAccessory(int accessoryID);
    }

    public enum AccessoryTypes
    {
        Unknown = 0,
        Head = 1,
        Back = 2,
        Sides = 3,
        Suit = 4,
        SpiritUp = 5,
        SpiritDown = 6,
        Tatto = 7,
        Wallpaper = 8
    }

    #region 自定义类
    public class Accessory
    {
        public int ID { get; set; }
        public int Type { get; set; }
        public AccessoryTypes AccessoryType
        {
            get
            {
                foreach (AccessoryTypes enumItem in Enum.GetValues(typeof(AccessoryTypes)))
                {
                    if ((int)enumItem == this.Type)
                        return enumItem;
                }
                return AccessoryTypes.Unknown;
            }
        }
        public bool CanWear
        {
            get
            {
                return this.AccessoryType == AccessoryTypes.Head ||
                       this.AccessoryType == AccessoryTypes.Back ||
                       this.AccessoryType == AccessoryTypes.Sides ||
                       this.AccessoryType == AccessoryTypes.Suit;
            }
        }
        public int Index { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string AB { get; set; }
        public string Prefab { get; set; }
        public string Region { get; set; }
        public string Purchase { get; set; }
        public float Price { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }
        public bool PURPIE { get; set; }
        public bool DONNY { get; set; }
        public bool NINJI { get; set; }
        public bool SANSA { get; set; }
        public bool YOYO { get; set; }
        public bool NUO { get; set; }

        public bool Opened
        {
            get
            {
                return this.StartTime == DateTime.MaxValue || (DateTime.Now - this.StartTime).TotalSeconds >= 0;
            }
        }
        public bool Closed
        {
            get
            {
                return this.EndTime == DateTime.MaxValue || (DateTime.Now - this.EndTime).TotalSeconds >= 0;
            }
        }
        public bool Coin
        {
            get { return !string.IsNullOrEmpty(this.Purchase) && this.Purchase.ToUpper() == "COIN"; }
        }
        public bool Cash
        {
            get { return !string.IsNullOrEmpty(this.Purchase) && this.Purchase.ToUpper() == "CASH"; }
        }
        public bool Suitable(string petName)
        {
            if (string.IsNullOrEmpty(petName))
            {
                return false;
            }
            else
            {
                bool suitable = false;
                switch (petName.ToUpper())
                {
                    case "PURPIE":
                        suitable = this.PURPIE;
                        break;
                    case "DONNY":
                        suitable = this.DONNY;
                        break;
                    case "NINJI":
                        suitable = this.NINJI;
                        break;
                    case "SANSA":
                        suitable = this.SANSA;
                        break;
                    case "YOYO":
                        suitable = this.YOYO;
                        break;
                    case "NUO":
                        suitable = this.NUO;
                        break;
                }
                return suitable;
            }
        }
        public bool Exclusive(AccessoryTypes accessoryType)
        {
            if ((this.AccessoryType == AccessoryTypes.Head ||
                 this.AccessoryType == AccessoryTypes.Back ||
                 this.AccessoryType == AccessoryTypes.Sides) &&
                 accessoryType == AccessoryTypes.Suit)
                return true;//如果我是头/背/两侧，你是套装，互斥
            else if ((accessoryType == AccessoryTypes.Head ||
                      accessoryType == AccessoryTypes.Back ||
                      accessoryType == AccessoryTypes.Sides) &&
                      this.AccessoryType == AccessoryTypes.Suit)
                return true;//如果你是头/背/两侧，我是套装，互斥
            else
                return false;//否则，暂时不互斥
        }
    }
    #endregion
}
