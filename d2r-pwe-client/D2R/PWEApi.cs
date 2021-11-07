using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace d2r_pwe_client.D2R
{
    public class PWEApiGameResult
    {
        public string GameIp { get; set; } = "0.0.0.0";
        public bool IsHotIp { get; set; }
        public bool IsValidPool { get; set; }
    }

    public class PWEApiVerificationResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class PWEApi
    {
        private const string URL = "http://ec2-18-185-241-206.eu-central-1.compute.amazonaws.com:4006";
        private const string VERSION = "1.0.0";

        public PWEApiGameResult sendGameIp(string ips, int pid, string token)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL + "/users/v2/hunt");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("authorization", string.Format("Bearer {0}", token));
                httpWebRequest.Headers.Add("x-app-version", VERSION);

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(new
                    {
                        ipAddresses = ips,
                        date = DateTime.UtcNow.ToString("o"),
                        pid = pid,
                    });

                    streamWriter.Write(json);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                dynamic data = getResponseData(httpResponse);
                return new PWEApiGameResult
                {
                    GameIp = data.ContainsKey("gameIp") ? data["gameIp"] : "0.0.0.0",
                    IsHotIp = data.ContainsKey("isHotIp") ? data["isHotIp"] : false,
                    IsValidPool = data.ContainsKey("isValidPool") ? data["isValidPool"] : false,
                };
            }
            catch (Exception e)
            {
                return new PWEApiGameResult();
            }
        }

        private dynamic getResponseData(HttpWebResponse httpResponse)
        {
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                return new JavaScriptSerializer().Deserialize<dynamic>(result);
            }
        }

        public PWEApiVerificationResult verify(int pid, string token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token)) return new PWEApiVerificationResult() { Message = "Enter token" };

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL + "/users/verify");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("authorization", string.Format("Bearer {0}", token));
                httpWebRequest.Headers.Add("x-app-version", VERSION);

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(new
                    {
                        date = DateTime.UtcNow.ToString("o"),
                        pid = pid,
                    });

                    streamWriter.Write(json);
                }


                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                dynamic data = getResponseData(httpResponse);
                return new PWEApiVerificationResult
                {
                    IsSuccess = true,
                    Message = data.ContainsKey("message") ? data["message"] : "",
                };
            }
            catch (Exception e)
            {
                try
                {
                    if (e is WebException && (e as WebException).Response is HttpWebResponse)
                    {
                        var httpResponse = (e as WebException).Response as HttpWebResponse;
                        dynamic data = getResponseData(httpResponse);
                        return new PWEApiVerificationResult
                        {
                            Message = data.ContainsKey("message") ? data["message"] : "",
                        };
                    }
                }
                catch { }

                var httpResp = (e as WebException).Response as HttpWebResponse;

                return new PWEApiVerificationResult() { Message = httpResp.StatusCode == HttpStatusCode.Unauthorized ? "Failed" : e.Message };
            }
        }
    }
}
