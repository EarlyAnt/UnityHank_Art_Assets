namespace Hank.Api
{
    public class RegisterCupBody
    {
        public string x_child_sn{get;set;}
        public string cup_hw_mac{get;set;}

        public RegisterCupBody(){}

        private RegisterCupBody(Builder builder)
        {
            setX_child_sn(builder.getX_child_sn());
            setCup_hw_mac(builder.getCup_hw_mac());
        }

        public string getX_child_sn()
        {
            return x_child_sn;
        }

        public void setX_child_sn(string x_child_sn)
        {
            this.x_child_sn = x_child_sn;
        }

        public string getCup_hw_mac()
        {
            return cup_hw_mac;
        }

        public void setCup_hw_mac(string cup_hw_mac)
        {
            this.cup_hw_mac = cup_hw_mac;
        }

        public static Builder getBuilder(){
            return new Builder();
        }


        public class Builder
        {
            private string x_child_sn;
            private string cup_hw_mac;

            public Builder()
            {
            }

            public string getX_child_sn()
            {
                return x_child_sn;
            }

            public string getCup_hw_mac()
            {
                return cup_hw_mac;
            }

            public Builder setX_child_sn(string val)
            {
                x_child_sn = val;
                return this;
            }

            public Builder setCup_hw_mac(string val)
            {
                cup_hw_mac = val;
                return this;
            }

            public RegisterCupBody build()
            {
                return new RegisterCupBody(this);
            }
        }

    }
}