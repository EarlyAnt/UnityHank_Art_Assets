namespace Hank.Api
{
    public class MissionsBean
    {
        /**
         * mission_id : 1
         * mission_state : underway
         * mission_staet : ontheway
         */

        public int mission_id{get;set;}
        public string mission_state{get;set;}

        public MissionsBean(){}

        private MissionsBean(MissionsBeanBuilder builder)
        {
            setMission_id(builder.getMission_id());
            setMission_state(builder.getMission_state());
        }


        public int getMission_id()
        {
            return mission_id;
        }

        public void setMission_id(int mission_id)
        {
            this.mission_id = mission_id;
        }

        public string getMission_state()
        {
            return mission_state;
        }

        public void setMission_state(string mission_state)
        {
            this.mission_state = mission_state;
        }

        public static MissionsBeanBuilder getBuilder()
        {
            return new MissionsBeanBuilder();
        }

        public class MissionsBeanBuilder
        {
            private int mission_id;
            private string mission_state;
            public MissionsBeanBuilder()
            {
            }

            public int getMission_id() {
                return mission_id;
            }

            public string getMission_state() {
                return mission_state;
            }

            public MissionsBeanBuilder setMission_id(int val)
            {
                mission_id = val;
                return this;
            }

            public MissionsBeanBuilder setMission_state(string val)
            {
                mission_state = val;
                return this;
            }

            public MissionsBean build()
            {
                return new MissionsBean(this);
            }
        }

    }
}