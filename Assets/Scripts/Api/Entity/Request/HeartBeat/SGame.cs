namespace Hank.Api
{
    public class SGame
    {
        public string game_name{get;set;}
        public string game_version{get;set;}
        public string pet_model{get;set;}

        public SGame(){}

        private SGame(Builder builder)
        {
            setGame_name(builder.getGame_name());
            setGame_version(builder.getGame_version());
            setPet_model(builder.getPet_model());
        }


        public string getGame_name()
        {
            return game_name;
        }

        public void setGame_name(string game_name)
        {
            this.game_name = game_name;
        }

        public string getGame_version()
        {
            return game_version;
        }

        public void setGame_version(string game_version)
        {
            this.game_version = game_version;
        }

        public string getPet_model()
        {
            return pet_model;
        }

        public void setPet_model(string pet_model)
        {
            this.pet_model = pet_model;
        }

        public static Builder getBuilder(){
            return new Builder();
        }

        public class Builder
        {
            private string game_name;
            private string game_version;
            private string pet_model;

            public Builder()
            {
            }

            public string getGame_name()
            {
                return game_name;
            }

            public string getGame_version()
            {
                return game_version;
            }

            public string getPet_model()
            {
                return pet_model;
            }

            public Builder setGame_name(string val)
            {
                game_name = val;
                return this;
            }

            public Builder setGame_version(string val)
            {
                game_version = val;
                return this;
            }

            public Builder setPet_model(string val)
            {
                pet_model = val;
                return this;
            }

            public SGame build()
            {
                return new SGame(this);
            }
        }
    }
}