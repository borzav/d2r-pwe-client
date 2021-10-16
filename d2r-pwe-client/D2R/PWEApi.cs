using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace d2r_pwe_client.D2R
{
    public class PWEApi
    {
        private const string URL = "http://ec2-18-185-241-206.eu-central-1.compute.amazonaws.com:4006";

        public bool sendGameIp(string ip, string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return false;
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL + "/users/hunt");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("authorization", string.Format("Bearer {0}", token));

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(new
                    {
                        ip = ip,
                    });

                    streamWriter.Write(json);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    dynamic data = new JavaScriptSerializer().Deserialize<dynamic>(result);
                    return data["isHotIp"];
                }
            }
            catch
            {
                return false;
            }
        }

        public bool verify(string token)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL + "/users/verify");
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add("authorization", string.Format("Bearer {0}", token));

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
