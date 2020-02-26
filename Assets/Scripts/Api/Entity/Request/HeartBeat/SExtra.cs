namespace Hank.Api
{
    public class SExtra
    {
        public long timestamp{get;set;}
        public int timezone{get;set;}

        public SExtra(){}

        private SExtra(Builder builder)
        {
            setTimestamp(builder.getTimestamp());
            setTimezone(builder.getTimezone());
        }

        public long getTimestamp()
        {
            return timestamp;
        }

        public void setTimestamp(long timestamp)
        {
            this.timestamp = timestamp;
        }

        public int getTimezone()
        {
            return timezone;
        }

        public void setTimezone(int timezone)
        {
            this.timezone = timezone;
        }

        public static Builder getBuilder(){
            return new Builder();
        }


        public class Builder
        {
            private long timestamp;
            private int timezone;

            public Builder()
            {
            }

            public long getTimestamp()
            {
                return timestamp;
            }

            public int getTimezone()
            {
                return timezone;
            }

            public Builder setTimestamp(long val)
            {
                timestamp = val;
                return this;
            }

            public Builder setTimezone(int val)
            {
                timezone = val;
                return this;
            }

            public SExtra build()
            {
                return new SExtra(this);
            }
        }
    }
}