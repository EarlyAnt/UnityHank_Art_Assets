using System;
using LitJson;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Newtonsoft.Json;
using Gululu.Util;
using Gululu.LocalData.Agents;
using Cup.Utils.android;
using Gululu.Utils.RC4;
using System.Text;

namespace Gululu
{
    public class JsonUtils : IJsonUtils
    {
        JsonSerializerSettings settings = new JsonSerializerSettings();
            
        [Inject]
        public ILocalCupAgent mLocalCupAgent{get;set;}
        public string DicToJsonStr(IDictionary<string, string> bodyContent){
            if(bodyContent == null){
                return "";
            }
            JsonData data = new JsonData();

            foreach(KeyValuePair<string, string> info in bodyContent){
                data[info.Key] = info.Value;
            }
            String json = data.ToJson();

            return json;
        }

        public T String2Json<T>(string jsonStr){

            T json = JsonConvert.DeserializeObject<T>(jsonStr);

            return json;
        }

        public string Json2String(object json){
            settings.NullValueHandling = NullValueHandling.Ignore;
            string jsonStr = JsonConvert.SerializeObject(json
            ,settings
            );

            return jsonStr;
        }

        public JsonData JsonStr2JsonData(string josnStr) {
            JsonData data = JsonMapper.ToObject(josnStr);

            return data;
        }

        public string getToken()
        {
            string cupSn = CupBuild.getCupSn();

            string cupToken = mLocalCupAgent.getCupToken(CupBuild.getCupSn());

            Debug.Log("<><>cupSn<><>"+cupSn+"<><><><>"+cupToken);

            return mLocalCupAgent.getCupToken(CupBuild.getCupSn());
        }
    }
}