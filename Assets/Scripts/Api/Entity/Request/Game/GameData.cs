using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Hank.Api
{
    public class GameData
    {

        /**
         * timestamp : 1234567890
         * missions : [{"mission_id":1,"mission_state":"underway"},{"mission_id":2,"mission_staet":"ontheway"}]
         * playerData : {"level":12,"exp":10086,"current_mission_id":1,"current_mission_percent":88}
         */



        public int timestamp{get;set;}
        public PlayerDataBean playerData{get;set;}

        public List<MissionsBean> missions{get;set;}

        public List<ItemsDataBean> itemsData { get; set; }

        public GameData(){
            
        }

        private GameData(GameDataBuilder builder)
        {
            setTimestamp(builder.getTimestamp());
            setPlayerData(builder.getPlayerData());
            setMissions(builder.getMissions());
            setItemsData(builder.getItemsData());
        }


        public int getTimestamp()
        {
            return timestamp;
        }

        public void setTimestamp(int timestamp)
        {
            this.timestamp = timestamp;
        }

        public PlayerDataBean getPlayerData()
        {
            return playerData;
        }

        public void setPlayerData(PlayerDataBean playerData)
        {

            this.playerData = playerData;
            
        }

        public List<MissionsBean> getMissions()
        {
            return missions;
        }

        public void setMissions(List<MissionsBean> missions)
        {
            this.missions = missions;
           
        }

        public List<ItemsDataBean> getItemsData()
        {
            return itemsData;
        }

        public void setItemsData(List<ItemsDataBean> itemsData)
        {
            this.itemsData = itemsData;

        }


        public static GameDataBuilder getGameDataBuilder()
        {
            return new GameDataBuilder();
        }

        public class GameDataBuilder
        {
            private int timestamp;
            private PlayerDataBean playerData;
            private List<MissionsBean> missions;
            private List<ItemsDataBean> itemsData;

            public GameDataBuilder()
            {
            }

            public int getTimestamp()
            {
                return timestamp;
            }

            public PlayerDataBean getPlayerData()
            {
                return playerData;
            }


            public GameDataBuilder setTimestamp(int val)
            {
                timestamp = val;
                return this;
            }

            public GameDataBuilder setPlayerData(PlayerDataBean val)
            {
                playerData = val;
                return this;
            }

            public List<MissionsBean> getMissions()
            {
                return missions;
            }
            public GameDataBuilder setMissions(List<MissionsBean> val)
            {
                missions = val;
                return this;
            }

            public List<ItemsDataBean> getItemsData()
            {
                return itemsData;
            }
            public GameDataBuilder setItemsData(List<ItemsDataBean> val)
            {
                itemsData = val;
                return this;
            }

            public GameData build()
            {
                return new GameData(this);
            }
        }
    }

}