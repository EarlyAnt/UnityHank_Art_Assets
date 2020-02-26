namespace Hank.Api
{
    public class SilenceTimeSectionFromServer
    {
        public int end_min{get;set;}
        public int start_min{get;set;}
        public int end_hour{get;set;}
        public int start_hour{get;set;}
        public int enable{get;set;}
        public string type{get;set;}
        public string weekdays{get;set;}
        public int id{get;set;}

        public SilenceTimeSectionFromServer(){}


        private SilenceTimeSectionFromServer(Builder builder)
        {
            setEnd_min(builder.getEnd_min());
            setStart_min(builder.getStart_min());
            setEnd_hour(builder.getEnd_hour());
            setStart_hour(builder.getStart_hour());
            setEnable(builder.getEnable());
            setType(builder.getType());
            setWeekdays(builder.getWeekdays());
            setId(builder.getId());
        }

        public int getEnd_min()
        {
            return end_min;
        }

        public void setEnd_min(int end_min)
        {
            this.end_min = end_min;
        }

        public int getStart_min()
        {
            return start_min;
        }

        public void setStart_min(int start_min)
        {
            this.start_min = start_min;
        }

        public int getEnd_hour()
        {
            return end_hour;
        }

        public void setEnd_hour(int end_hour)
        {
            this.end_hour = end_hour;
        }

        public int getStart_hour()
        {
            return start_hour;
        }

        public void setStart_hour(int start_hour)
        {
            this.start_hour = start_hour;
        }

        public int getEnable()
        {
            return enable;
        }

        public void setEnable(int enable)
        {
            this.enable = enable;
        }

        public string getType()
        {
            return type;
        }

        public void setType(string type)
        {
            this.type = type;
        }

        public string getWeekdays()
        {
            return weekdays;
        }

        public void setWeekdays(string weekdays)
        {
            this.weekdays = weekdays;
        }

        public int getId()
        {
            return id;
        }

        public void setId(int id)
        {
            this.id = id;
        }

        public static Builder getBuilder()
        {
            return new Builder();
        }

        public class Builder
        {
            private int end_min;
            private int start_min;
            private int end_hour;
            private int start_hour;
            private int enable;
            private string type;
            private string weekdays;
            private int id;

            public Builder()
            {
            }

            public int getEnd_min()
            {
                return end_min;
            }

            public int getStart_min()
            {
                return start_min;
            }

            public int getEnd_hour()
            {
                return end_hour;
            }

            public int getStart_hour()
            {
                return start_hour;
            }

            public int getEnable()
            {
                return enable;
            }

            public string getType()
            {
                return type;
            }

            public string getWeekdays()
            {
                return weekdays;
            }

            public int getId()
            {
                return id;
            }

            public Builder setEnd_min(int val)
            {
                end_min = val;
                return this;
            }

            public Builder setStart_min(int val)
            {
                start_min = val;
                return this;
            }

            public Builder setEnd_hour(int val)
            {
                end_hour = val;
                return this;
            }

            public Builder setStart_hour(int val)
            {
                start_hour = val;
                return this;
            }

            public Builder setEnable(int val)
            {
                enable = val;
                return this;
            }

            public Builder setType(string val)
            {
                type = val;
                return this;
            }

            public Builder setWeekdays(string val)
            {
                weekdays = val;
                return this;
            }

            public Builder setId(int val)
            {
                id = val;
                return this;
            }

            public SilenceTimeSectionFromServer build()
            {
                return new SilenceTimeSectionFromServer(this);
            }
        }
    }
}