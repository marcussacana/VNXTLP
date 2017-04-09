using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace VNXTLP {
    internal static class WordAPI {
        const string Refresh = "\"message\" : \"missing accessToken\"";
        const string Exipred = "\"message\" : \"demo page has expired\"";
        const string WhenStr = "when = \"";
        const string EncryptedStr = "encrypted = \"";
        const string Domain = "https://www.wordsapi.com";
        const string API = Domain + "/mashape/words/{0}/{1}?when={2}&encrypted={3}";
        const string JsonReply = "\"{0}\":[";
        private static string Encrypted = "unk";
        private static string When = "unk";
        private static void GetKeys() {
            string HTML = DownloadString(Domain);
            int IndexOf = HTML.IndexOf(WhenStr) + WhenStr.Length;
            When = GetStringAt(IndexOf, HTML);
            IndexOf = HTML.IndexOf(EncryptedStr) + EncryptedStr.Length;
            Encrypted = GetStringAt(IndexOf, HTML);
        }

        internal static string[] DownloadSynonyms(string Word) {
            return RequestArrayByType("synonyms", Word);
        }

        private static string[] RequestArrayByType(string ReqType, string Word, bool NoRetry = false) {
            if (When == "unk" || Encrypted == "unk")
                GetKeys();
            string URL = string.Format(API, Word, ReqType, When, Encrypted);
            string Response = DownloadString(URL);
            if (Response.Contains(Refresh) || Response.Contains(Exipred) && !NoRetry) {
                Encrypted = "unk";
                return RequestArrayByType(ReqType, Word, true);
            }
            else if (Response.Contains(Refresh))
                return null;
            string Tag = string.Format(JsonReply, ReqType);
            if (!Response.Contains(Tag))
                return null;
            return GetStringsAt(Response.IndexOf(Tag) + Tag.Length, Response);
        }
        private static string GetStringAt(int Pos, string content) {
            string result = string.Empty;
            while (content[Pos] != '"')
                result += content[Pos++];
            return result;
        }

        private static string[] GetStringsAt(int StartPos, string content) {
            List<string> Results = new List<string>();
            while (true) {
                string NewStr = null;
                if (content[StartPos] == ',')
                    StartPos++;
                if (content[StartPos] == '"')
                    NewStr = GetStringAt(StartPos + 1, content);
                if (string.IsNullOrWhiteSpace(NewStr))
                    break;
                else {
                    Results.Add(NewStr);
                    StartPos += NewStr.Length + 2;
                }
            }
            return Results.ToArray();
        }
        private static string DownloadString(string Url) {
            return Encoding.UTF8.GetString(DownloadData(Url));
        }
        private static byte[] DownloadData(string Url) {byte[] FC = new byte[0];
            try {
                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(Url);
                Request.UseDefaultCredentials = true;
                Request.Method = "GET";
                Request.Headers.Add("UserAgent", "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)");
                Request.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF-8");
                Request.Referer = Domain.EndsWith("/") ? Domain : Domain + "/";
                WebResponse Response = Request.GetResponse();

                using (MemoryStream Data = new MemoryStream())
                using (Stream Reader = Response.GetResponseStream()) {
                    byte[] Buffer = new byte[1024];
                    int bytesRead;
                    do {
                        bytesRead = Reader.Read(Buffer, 0, Buffer.Length);
                        Data.Write(Buffer, 0, bytesRead);
                    } while (bytesRead > 0);
                    FC = Data.ToArray();
                }
            }
            catch (Exception oEx) {
                if (oEx.GetType() == typeof(WebException)) {
                    HttpWebResponse wr = ((WebException)oEx).Response as HttpWebResponse;
                    using (MemoryStream Data = new MemoryStream())
                    using (Stream Reader = wr.GetResponseStream()) {
                        byte[] Buffer = new byte[1024];
                        int bytesRead;
                        do {
                            bytesRead = Reader.Read(Buffer, 0, Buffer.Length);
                            Data.Write(Buffer, 0, bytesRead);
                        } while (bytesRead > 0);
                        FC = Data.ToArray();
                    }
                }
            }
            return FC;
        }
    }
}
