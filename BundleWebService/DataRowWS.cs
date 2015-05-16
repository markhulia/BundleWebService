using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BundleWebService
{
    public class DataRowWS
    {
        public List<KeyValuePair> Values { get; set; }

        public DataRowWS()
        {
            Values = new List<KeyValuePair>();
        }

        public void Add(string key, object value)
        {
            Values.Add(new KeyValuePair{Key = key, Value = value});
        }
    }
}