namespace Hank.Api
{
    public class HeartBeatBody
    {
        public SChild child{get;set;}
        public SCup cup{get;set;}
        public SGame game{get;set;}
        public SExtra extra{get;set;}

        public HeartBeatBody(){}

        private HeartBeatBody(Builder builder)
        {
            setChild(builder.getChild());
            setCup(builder.getCup());
            setGame(builder.getGame());
            setExtra(builder.getExtra());
        }

        public SChild getChild()
        {
            return child;
        }

        public void setChild(SChild child)
        {
            this.child = child;
        }

        public SCup getCup()
        {
            return cup;
        }

        public void setCup(SCup cup)
        {
            this.cup = cup;
        }

        public SGame getGame()
        {
            return game;
        }

        public void setGame(SGame game)
        {
            this.game = game;
        }

        public SExtra getExtra()
        {
            return extra;
        }

        public void setExtra(SExtra extra)
        {
            this.extra = extra;
        }

        public static Builder getBuilder()
        {
            return new Builder();
        }

        public class Builder
        {
            private SChild child;
            private SCup cup;
            private SGame game;
            private SExtra extra;

            public Builder()
            {
            }

            public SChild getChild()
            {
                return child;
            }

            public SCup getCup()
            {
                return cup;
            }

            public SGame getGame()
            {
                return game;
            }

            public SExtra getExtra()
            {
                return extra;
            }

            public Builder setChild(SChild val)
            {
                child = val;
                return this;
            }

            public Builder setCup(SCup val)
            {
                cup = val;
                return this;
            }

            public Builder setGame(SGame val)
            {
                game = val;
                return this;
            }

            public Builder setExtra(SExtra val)
            {
                extra = val;
                return this;
            }

            public HeartBeatBody build()
            {
                return new HeartBeatBody(this);
            }
        }
    }
}