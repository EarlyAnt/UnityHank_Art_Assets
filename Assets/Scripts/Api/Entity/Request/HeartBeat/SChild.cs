namespace Hank.Api
{
    public class SChild
    {

        public string x_child_sn{get;set;}
        public int friend_count{get;set;}
        public string language{get;set;}

        public SChild(){}

        private SChild(Builder builder)
        {
            setX_child_sn(builder.getX_child_sn());
            setFriend_count(builder.getFriend_count());
            setLanguage(builder.getLanguage());
        }

        public string getX_child_sn()
        {
            return x_child_sn;
        }

        public void setX_child_sn(string x_child_sn)
        {
            this.x_child_sn = x_child_sn;
        }

        public int getFriend_count()
        {
            return friend_count;
        }

        public void setFriend_count(int friend_count)
        {
            this.friend_count = friend_count;
        }

        public string getLanguage()
        {
            return language;
        }

        public void setLanguage(string language)
        {
            this.language = language;
        }




        public static Builder getBuilder()
        {
            return new Builder();
        }


        public class Builder
        {
            private string x_child_sn;
            private int friend_count;
            private string language;

            public Builder()
            {
            }

            public string getX_child_sn()
            {
                return x_child_sn;
            }
            public int getFriend_count()
            {
                return friend_count;
            }

            public string getLanguage()
            {
                return language;
            }


            public Builder setX_child_sn(string val)
            {
                x_child_sn = val;
                return this;
            }

            public Builder setFriend_count(int val)
            {
                friend_count = val;
                return this;
            }

            public Builder setLanguage(string val)
            {
                language = val;
                return this;
            }

            public SChild build()
            {
                return new SChild(this);
            }


        }
    }
}