namespace CupSdk.Tmall.AIAudioStatusListener.Result
{
    public class AIAudioData
    {

        public string question{get;set;}
        public string domain{get;set;}
        public string intent{get;set;}
        public string spokenText{get;set;}
        public string writtenText{get;set;}

        public string extraData{get;set;}
        private int _code = 0;
        private int _type = 0;
        private int _emotion = 0;

        public int code{
            get{
                return _code;
            }
            set{
                _code = value;
            }
        }

        public int type{
            get{
                return _type;
            }
            set{
                _type = value;
            }
        }

        public int emotion{
            get{
                return _emotion;
            }
            set{
                _emotion = value;
            }
        }

    }
}