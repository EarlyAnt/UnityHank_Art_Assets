namespace Hank.Api
{
    public class PlayerDataBean
    {
        /**
         * level : 12
         * exp : 10086
         * current_mission_id : 1
         * current_mission_percent : 88
         */

        public int level { get; set; }
        public int exp { get; set; }
        public int current_mission_id { get; set; }
        public int current_mission_percent { get; set; }
        public int crown_count { get; set; }
        public int starID { get; set; }
        public int sceneIndex { get; set; }
        public string purpie_Accessories { get; set; }
        public string donny_Accessories { get; set; }
        public string ninji_Accessories { get; set; }
        public string sansa_Accessories { get; set; }
        public string yoyo_Accessories { get; set; }
        public string nuo_Accessories { get; set; }

        public PlayerDataBean() { }

        private PlayerDataBean(PlayerDataBeanBuilder builder)
        {
            setLevel(builder.getLevel());
            setExp(builder.getExp());
            setCurrent_mission_id(builder.getCurrent_mission_id());
            setCurrent_mission_percent(builder.getCurrent_mission_percent());
            setCrown_count(builder.getCrown_count());
            setStarID(builder.getStarID());
            setSceneIndex(builder.getSceneIndex());
            setPurpieAccessories(builder.getPurpieAccessories());
            setDonnyAccessories(builder.getDonnyAccessories());
            setNinjiAccessories(builder.getNinjiAccessories());
            setSansaAccessories(builder.getSansaAccessories());
            setYoyoAccessories(builder.getYoyoAccessories());
            setNuoAccessories(builder.getNuoAccessories());
        }

        public int getLevel()
        {
            return level;
        }

        public void setLevel(int level)
        {
            this.level = level;
        }

        public int getExp()
        {
            return exp;
        }

        public void setExp(int exp)
        {
            this.exp = exp;
        }

        public int getCurrent_mission_id()
        {
            return current_mission_id;
        }

        public void setCurrent_mission_id(int current_mission_id)
        {
            this.current_mission_id = current_mission_id;
        }

        public int getCurrent_mission_percent()
        {
            return current_mission_percent;
        }

        public void setCurrent_mission_percent(int current_mission_percent)
        {
            this.current_mission_percent = current_mission_percent;
        }

        public int getCrown_count()
        {
            return crown_count;
        }

        public void setCrown_count(int crown_count)
        {
            this.crown_count = crown_count;
        }

        public int getStarID()
        {
            return this.starID;
        }

        public void setStarID(int starID)
        {
            this.starID = starID;
        }

        public int getSceneIndex()
        {
            return this.sceneIndex;
        }

        public void setSceneIndex(int sceneIndex)
        {
            this.sceneIndex = sceneIndex;
        }

        public string getPurpieAccessories()
        {
            return this.purpie_Accessories;
        }

        public void setPurpieAccessories(string purpieAccessories)
        {
            this.purpie_Accessories = purpieAccessories;
        }

        public string getDonnyAccessories()
        {
            return this.donny_Accessories;
        }

        public void setDonnyAccessories(string DonnyAccessories)
        {
            this.donny_Accessories = DonnyAccessories;
        }

        public string getNinjiAccessories()
        {
            return this.ninji_Accessories;
        }

        public void setNinjiAccessories(string NinjiAccessories)
        {
            this.ninji_Accessories = NinjiAccessories;
        }

        public string getSansaAccessories()
        {
            return this.sansa_Accessories;
        }

        public void setSansaAccessories(string SansaAccessories)
        {
            this.sansa_Accessories = SansaAccessories;
        }

        public string getYoyoAccessories()
        {
            return this.yoyo_Accessories;
        }

        public void setYoyoAccessories(string YoyoAccessories)
        {
            this.yoyo_Accessories = YoyoAccessories;
        }

        public string getNuoAccessories()
        {
            return this.nuo_Accessories;
        }

        public void setNuoAccessories(string NuoAccessories)
        {
            this.nuo_Accessories = NuoAccessories;
        }

        public static PlayerDataBeanBuilder getPlayerDataBeanBuilder()
        {
            return new PlayerDataBeanBuilder();
        }

        public class PlayerDataBeanBuilder
        {
            private int level;
            private int exp;
            private int current_mission_id;
            private int current_mission_percent;
            private int crown_count;
            private int starID;
            private int sceneIndex;
            private string purpie_Accessories;
            private string donny_Accessories;
            private string ninji_Accessories;
            private string sansa_Accessories;
            private string yoyo_Accessories;
            private string nuo_Accessories;

            public PlayerDataBeanBuilder()
            {
            }

            public int getLevel()
            {
                return level;
            }

            public int getExp()
            {
                return exp;
            }

            public int getCurrent_mission_id()
            {
                return current_mission_id;
            }

            public int getCurrent_mission_percent()
            {
                return current_mission_percent;
            }

            public int getCrown_count()
            {
                return crown_count;
            }

            public int getStarID()
            {
                return this.starID;
            }

            public int getSceneIndex()
            {
                return this.sceneIndex;
            }

            public string getPurpieAccessories()
            {
                return this.purpie_Accessories;
            }

            public string getDonnyAccessories()
            {
                return this.donny_Accessories;
            }

            public string getNinjiAccessories()
            {
                return this.ninji_Accessories;
            }

            public string getSansaAccessories()
            {
                return this.sansa_Accessories;
            }

            public string getYoyoAccessories()
            {
                return this.yoyo_Accessories;
            }

            public string getNuoAccessories()
            {
                return this.nuo_Accessories;
            }

            public PlayerDataBeanBuilder setLevel(int val)
            {
                level = val;
                return this;
            }

            public PlayerDataBeanBuilder setExp(int val)
            {
                exp = val;
                return this;
            }

            public PlayerDataBeanBuilder setCurrent_mission_id(int val)
            {
                current_mission_id = val;
                return this;
            }

            public PlayerDataBeanBuilder setCurrent_mission_percent(int val)
            {
                current_mission_percent = val;
                return this;
            }

            public PlayerDataBeanBuilder setCrown_count(int val)
            {
                crown_count = val;
                return this;
            }

            public PlayerDataBeanBuilder setStarID(int starID)
            {
                this.starID = starID;
                return this;
            }

            public PlayerDataBeanBuilder setSceneIndex(int sceneIndex)
            {
                this.sceneIndex = sceneIndex;
                return this;
            }

            public PlayerDataBeanBuilder setPurpieAccessories(string purpieAccessories)
            {
                this.purpie_Accessories = purpieAccessories;
                return this;
            }
            public PlayerDataBeanBuilder setDonnyAccessories(string DonnyAccessories)
            {
                this.donny_Accessories = DonnyAccessories;
                return this;
            }
            public PlayerDataBeanBuilder setNinjiAccessories(string NinjiAccessories)
            {
                this.ninji_Accessories = NinjiAccessories;
                return this;
            }
            public PlayerDataBeanBuilder setSansaAccessories(string SansaAccessories)
            {
                this.sansa_Accessories = SansaAccessories;
                return this;
            }
            public PlayerDataBeanBuilder setYoyoAccessories(string YoyoAccessories)
            {
                this.yoyo_Accessories = YoyoAccessories;
                return this;
            }
            public PlayerDataBeanBuilder setNuoAccessories(string NuoAccessories)
            {
                this.nuo_Accessories = NuoAccessories;
                return this;
            }

            public PlayerDataBean build()
            {
                return new PlayerDataBean(this);
            }
        }
    }
}