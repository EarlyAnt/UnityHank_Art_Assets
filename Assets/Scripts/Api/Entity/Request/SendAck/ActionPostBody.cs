using System.Collections.Generic;

namespace Hank.Api
{
    public class ActionPostBody
    {
        public string msg{get;set;}
        public string status{get;set;}
        public string cup_hw_sn{get;set;}
        public string todoid{get;set;}
        public List<ActionPostBodyValue> values{get;set;}

        public ActionPostBody(){}

        private ActionPostBody(Builder builder)
        {
            setMsg(builder.getMsg());
            setStatus(builder.getStatus());
            setCup_hw_sn(builder.getCup_hw_sn());
            setTodoid(builder.getTodoid());
            setValues(builder.getValues());
        }

        public string getMsg()
        {
            return msg;
        }

        public void setMsg(string msg)
        {
            this.msg = msg;
        }

        public string getStatus()
        {
            return status;
        }

        public void setStatus(string status)
        {
            this.status = status;
        }

        public string getCup_hw_sn()
        {
            return cup_hw_sn;
        }

        public void setCup_hw_sn(string cup_hw_sn)
        {
            this.cup_hw_sn = cup_hw_sn;
        }

        public string getTodoid()
        {
            return todoid;
        }

        public void setTodoid(string todoid)
        {
            this.todoid = todoid;
        }

        public List<ActionPostBodyValue> getValues()
        {
            return values;
        }

        public void setValues(List<ActionPostBodyValue> values)
        {
            this.values = values;
        }

        public static Builder getBuilder(){
            return new Builder();
        }
        public class Builder
        {
            private string msg;
            private string status;
            private string cup_hw_sn;
            private string todoid;
            private List<ActionPostBodyValue> values;

            public Builder()
            {
            }

            public string getMsg()
            {
                return msg;
            }

            public string getStatus()
            {
                return status;
            }

            public string getCup_hw_sn()
            {
                return cup_hw_sn;
            }

            public string getTodoid()
            {
                return todoid;
            }

            public List<ActionPostBodyValue> getValues()
            {
                return values;
            }

            public Builder setMsg(string val)
            {
                msg = val;
                return this;
            }

            public Builder setStatus(string val)
            {
                status = val;
                return this;
            }

            public Builder setCup_hw_sn(string val)
            {
                cup_hw_sn = val;
                return this;
            }

            public Builder setTodoid(string val)
            {
                todoid = val;
                return this;
            }

            public Builder setValues(List<ActionPostBodyValue> val)
            {
                values = val;
                return this;
            }

            public ActionPostBody build()
            {
                return new ActionPostBody(this);
            }
        }
    }
}