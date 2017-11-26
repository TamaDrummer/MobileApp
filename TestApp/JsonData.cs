using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace TestApp
{
    class JsonData
    {
        public string jsonRawString { get; set; }


        public void SerializeJsonData(object jsonobj)
        {
            string data = JsonConvert.SerializeObject(jsonobj);
        }

        public dynamic  DeSerializeJsonData(string jsonstr)
        {
            dynamic data;
            data = JsonConvert.DeserializeObject(jsonstr);
            return data;
        }

    }
}