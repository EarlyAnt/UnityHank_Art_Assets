using System.Collections.Generic;
using UnityEngine;

namespace Hank
{
    public interface IAwardDataHelper
    {
        AwardDatas GetAwards(int totalCost, int awardID);
        void AddItem(AwardData awardData);
    }

    public static class AwardTypes
    {
        public const string COIN = "Coin";
        public const string FOOD = "Prop_Food";
    }

    public class AwardDatas
    {
        public List<AwardData> Datas { get; set; }

        public List<AwardData> PropDatas
        {
            get
            {
                return this.Datas != null && this.Datas.Count > 0 ? this.Datas.FindAll(t => t.IsProp == true) : null;
            }
        }

        public int Coin
        {
            get
            {
                if (this.Datas != null && this.Datas.Count > 0)
                {
                    foreach (AwardData awardData in this.Datas)
                    {
                        if (awardData.AwardType == AwardTypes.COIN)
                            return awardData.Count;
                    }
                    return 0;
                }
                else return 0;
            }
        }

        public AwardDatas()
        {
            this.Datas = new List<AwardData>();
        }

        public override string ToString()
        {
            if (this.Datas != null && this.Datas.Count > 0)
            {
                System.Text.StringBuilder strbContent = new System.Text.StringBuilder();
                foreach (var awardData in this.Datas)
                    strbContent.AppendLine(awardData.ToString());
                return strbContent.ToString();
            }
            return string.Empty;
        }
    }

    public class AwardData
    {
        public int ID { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public string Icon { get; set; }
        public string AwardType { get; set; }
        public bool IsProp { get { return !string.IsNullOrEmpty(this.AwardType) && this.AwardType.ToLower().StartsWith("prop_"); } }

        public override string ToString()
        {
            return string.Format("AwardData, ID: {0}, Price: {1}, Count: {2}, AwardType: {3}, Icon: {4}", this.ID, this.Price, this.Count, this.AwardType, this.Icon);
        }
    }
}