namespace Hank.Api
{
    public class SCup
    {
        public string cup_hw_mac{get;set;}
        public int battery{get;set;}
        public bool charge{get;set;}
        public int capability{get;set;}

        public SCup(){}
        private SCup(Builder builder)
        {
            setCup_hw_mac(builder.getCup_hw_mac());
            setBattery(builder.getBattery());
            setCharge(builder.isCharge());
            setCapability(builder.getCapability());
        }

        public string getCup_hw_mac()
        {
            return cup_hw_mac;
        }

        public void setCup_hw_mac(string cup_hw_mac)
        {
            this.cup_hw_mac = cup_hw_mac;
        }

        public int getBattery()
        {
            return battery;
        }

        public void setBattery(int battery)
        {
            this.battery = battery;
        }

        public bool isCharge()
        {
            return charge;
        }

        public void setCharge(bool charge)
        {
            this.charge = charge;
        }

        public int getCapability()
        {
            return capability;
        }

        public void setCapability(int capability)
        {
            this.capability = capability;
        }

        public static Builder getBuilder()
        {
            return new Builder();
        }
 
        public class Builder
        {
            private string cup_hw_mac;
            private int battery;
            private bool charge;
            private int capability;

            public Builder()
            {
            }

            public string getCup_hw_mac()
            {
                return cup_hw_mac;
            }

            public int getBattery()
            {
                return battery;
            }

            public bool isCharge()
            {
                return charge;
            }

            public int getCapability()
            {
                return capability;
            }


            public Builder setCup_hw_mac(string val)
            {
                cup_hw_mac = val;
                return this;
            }

            public Builder setBattery(int val)
            {
                battery = val;
                return this;
            }

            public Builder setCharge(bool val)
            {
                charge = val;
                return this;
            }

            public Builder setCapability(int val)
            {
                capability = val;
                return this;
            }

            public SCup build()
            {
                return new SCup(this);
            }
        }
    }
}