using System.Collections.Generic;

namespace Hank.Api
{
    public class HeartBeatActionBase
    {
        public string todoid;
        public HeartBeatAck ack;
    }

    public struct HeartBeatData
    {
        public string prod_name;
        public string pet_model;
        public string cup_hw_sn;

    }
    public struct HeartBeatAck
    {
        public string url;
        public string port;
        public string server;
    }

    public class HeartBeatSetValues : HeartBeatActionBase
    {
        public struct Value
        {
            public string key;
            public string value;

        }
        public List<Value> task;
    }
    public class HeartBeatGetValues : HeartBeatActionBase
    {
        public struct Value
        {
            public string key;
        }
        public List<Value> task;
    }


    public class HeartBeatSetValuesLong : HeartBeatActionBase
    {
        public class SetValuesLongTask
        {
            public string key;
            public string value;
        }
        public SetValuesLongTask task;
    }


    public class HeartBeatReset : HeartBeatActionBase
    {
    }

    public class HeartBeatReboot : HeartBeatActionBase
    {
    }

    public class HeartBeatDelete : HeartBeatActionBase
    {
        public string content;
    }

    public class HeartBeatGetValuesLong : HeartBeatActionBase
    {
        public struct GetValuesLongTask
        {
            public string port;
            public string url;
            public string server;
            public string type;
        }
        public GetValuesLongTask task;
    }



    public struct HeartBeatActions
    {
        public List<HeartBeatSetValues> set_values;
        public List<HeartBeatGetValues> get_values;
        public List<HeartBeatSetValuesLong> set_values_long;
        public List<HeartBeatGetValuesLong> get_values_long;
        public List<HeartBeatReset> reset;
        public List<HeartBeatReboot> reboot;
        public List<HeartBeatDelete> delete;
        //public List<Events> events ;
    }


    public struct HeartBeatResponse
    {
        public string status;
        public HeartBeatData data;
        public HeartBeatActions actions;
    }

    public struct HeartBeatAckResult
    {
        public string status;
    }


    public class GuEvent
    {
        public int event_id;

        public List<Res> res;
        public bool active;
        public string title;
        public int repeat_count;

        public int reward_gems;

        public string effect_time;

        public string expiry_time;
    }

    public class GuEvents
    {
        public List<GuEvent> events;
    }

    public class Res
    {

        public string type;
        public string url;
        public string crc;
        public int size;
    }

}