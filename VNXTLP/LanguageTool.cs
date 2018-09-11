using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace VNXTLP {
    static class LanguageTool {

        private static Dictionary<string, int> Counter = new Dictionary<string, int>();
        private static string CurrentProxy;
        private static DateTime BeginTime = DateTime.Now;
        private static int ReamingRequest {
            get {
                if (CurrentProxy == null)
                    CurrentProxy = string.Empty;

                if (!Counter.ContainsKey(CurrentProxy))
                    Counter[CurrentProxy] = 0;

                var Seconds = (DateTime.Now - BeginTime).TotalSeconds;
                if (Seconds > 60) {
                    BeginTime = DateTime.Now;
                    Counter = new Dictionary<string, int>();
                    return 20;
                }

                return 20 - Counter[CurrentProxy];
            }
        }
        public static Result Check(string Text, string Language, string Proxy = null) {
            CurrentProxy = Proxy;
            if (ReamingRequest <= 0) {
                throw new Exception("Too many Requests");
            }
            Counter[CurrentProxy] += 1;
            HttpWebRequest Request = WebRequest.Create("https://languagetool.org/api/v2/check") as HttpWebRequest;
            Request.Method = "POST";
            Request.ContentType = "application/x-www-form-urlencoded";
            Request.Accept = "application/json";
            if (Proxy != null)
                Request.Proxy = new WebProxy(Proxy);
            string Data = $"text={HttpUtility.UrlEncode(Text)}&enabledOnly=false&language={Language}";
            var Input = Request.GetRequestStream();
            var Temp = new MemoryStream(Encoding.UTF8.GetBytes(Data));
            Temp.CopyTo(Input);
            Temp.Close();
            Input.Close();
            var Response = Request.GetResponse();
            var Output = Response.GetResponseStream();
            Temp = new MemoryStream();
            Output.CopyTo(Temp);
            Output.Close();
            Response.Close();

            string JSON = Encoding.UTF8.GetString(Temp.ToArray());

            return new JavaScriptSerializer().Deserialize<Result>(JSON);
        }
    }

    public struct Result {
        public Software software;
        public Language language;
        public Match[] matches;
    }
    public struct Software {
        public string name;
        public string version;
        public string buildDate;
        public int apiVersion;
        public string status;
        public bool premium;
        public string premiumHint;
    }


    public struct Language {
        public string name;
        public string code;
        public DetectedLanguage detectedLanguage;
    }
    public struct DetectedLanguage {
        public string name;
        public string code;
    }

    public struct Match {
        public string message;
        public string shortMessage;
        public int offset;
        public int length;
        public String[] replacements;
        public Context context;
        public string sentense;
    }

    public struct String {
        public string value;
    }

    public struct Context {
        public string text;
        public int offset;
        public int length;
    }

    public struct Rule {
        public string id;
        public string subId;
        public string description;
        public String[] urls;
        public string issueType;
        public Category category;
    }

    public struct Category {
        public string id;
        public string name;
    }
}