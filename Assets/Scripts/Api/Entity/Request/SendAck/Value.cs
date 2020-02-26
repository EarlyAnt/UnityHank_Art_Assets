namespace Hank.Api
{
    public class ActionPostBodyValue
    {
        public string key{get;set;}
        public string value{get;set;}

        public ActionPostBodyValue(){}


        private ActionPostBodyValue(Builder builder)
        {
            setKey(builder.getKey());
            setValue(builder.getValue());
        }

        public string getKey()
        {
            return key;
        }

        public void setKey(string key)
        {
            this.key = key;
        }

        public string getValue()
        {
            return value;
        }

        public void setValue(string value)
        {
            this.value = value;
        }

        public static Builder getBuilder()
        {
            return new Builder();
        }

        public class Builder
        {
            private string key;
            private string value;

            public Builder()
            {
            }

            public string getKey()
            {
                return key;
            }

            public string getValue()
            {
                return value;
            }



            public Builder setKey(string val)
            {
                key = val;
                return this;
            }

            public Builder setValue(string val)
            {
                value = val;
                return this;
            }

            public ActionPostBodyValue build()
            {
                return new ActionPostBodyValue(this);
            }
        }
    }
}