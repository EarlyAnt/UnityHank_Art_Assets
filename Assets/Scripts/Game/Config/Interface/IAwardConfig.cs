using System.Collections.Generic;
using System.Linq;

namespace Gululu.Config
{
    /// <summary>
    /// 配饰配置接口
    /// </summary>
    public interface IAwardConfig
    {
        /// <summary>
        /// 加载配饰数据
        /// </summary>
        void LoadAwardConfig();
        /// <summary>
        /// 获取所有配饰
        /// </summary>
        /// <returns></returns>
        List<AwardInfo> GetAllAwards();
        /// <summary>
        /// 获取指定配饰
        /// </summary>
        /// <param name="awardID">配饰ID</param>
        /// <returns></returns>
        AwardInfo GetAward(int awardID);
    }

    #region 自定义类
    public static class AwardSources
    {
        public const string FOOD = "Food";
        public const string MATERIAL = "Material";
    }

    public class AwardInfo
    {
        public int ID { get; set; }
        public List<Award> Awards { get; set; }

        public AwardInfo()
        {
            this.Awards = new List<Award>();
        }
        public override string ToString()
        {
            System.Text.StringBuilder strbContent = new System.Text.StringBuilder();
            strbContent.AppendFormat("AwardInfo, ID: {0}, ", this.ID);
            if (this.Awards.Count > 0)
            {
                foreach (var award in this.Awards)
                    strbContent.AppendFormat("[{0}], ", award);
            }

            if (strbContent.Length > 2)
                strbContent = strbContent.Remove(strbContent.Length - 2, 2);

            return strbContent.ToString();
        }
    }

    public abstract class Award
    {
        public float Percent { get; set; }
    }

    public class CoinAward : Award
    {
        public override string ToString()
        {
            return string.Format("CoinAward, Percent: {0}", this.Percent);
        }
    }

    public class PropAward : Award
    {
        public string Source { get; set; }
        public bool FoodAward
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Source))
                {
                    string[] parts = this.Source.Split(',');
                    if (parts != null && parts.Length > 0 && parts.ToList().Exists(t => !string.IsNullOrEmpty(t) && t.ToLower() == AwardSources.FOOD.ToLower()))
                        return true;
                    else
                        return false;

                }
                else return false;
            }
        }
        public bool MaterialAward
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Source))
                {
                    string[] parts = this.Source.Split(',');
                    if (parts != null && parts.Length > 0 && parts.ToList().Exists(t => !string.IsNullOrEmpty(t) && t.ToLower() == AwardSources.MATERIAL.ToLower()))
                        return true;
                    else
                        return false;

                }
                else return false;
            }
        }

        public override string ToString()
        {
            return string.Format("PropAward, Percent: {0}, Source: {1}, FoodAward: {2}, MaterialAward: {3}", this.Percent, this.Source, this.FoodAward, this.MaterialAward);
        }
    }

    #endregion
}
