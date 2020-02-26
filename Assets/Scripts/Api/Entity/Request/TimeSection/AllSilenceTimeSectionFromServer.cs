using System.Collections.Generic;

namespace Hank.Api
{
    public class AllSilenceTimeSectionFromServer
    {
        
        public AllSilenceTimeSectionFromServer(){}
        public List<SilenceTimeSectionFromServer> configs{get;set;}

        private AllSilenceTimeSectionFromServer(Builder builder)
        {
            setConfigs(builder.getConfigs());
        }

        public List<SilenceTimeSectionFromServer> getConfigs()
        {
            return configs;
        }

        public void setConfigs(List<SilenceTimeSectionFromServer> configs)
        {
            this.configs = configs;
        }

        public static Builder getBuilder(){
            return new Builder();
        }

        public class Builder
        {
            private List<SilenceTimeSectionFromServer> configs;

            public Builder()
            {
            }

            public List<SilenceTimeSectionFromServer> getConfigs()
            {
                return configs;
            }

            public Builder setConfigs(List<SilenceTimeSectionFromServer> val)
            {
                configs = val;
                return this;
            }

            public AllSilenceTimeSectionFromServer build()
            {
                return new AllSilenceTimeSectionFromServer(this);
            }
        }
    }
}