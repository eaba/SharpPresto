﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PrestoSharp
{
    internal class Helper
    {
        internal static T HttpRequest<T>(string Method, string Url, string Request = "", WebHeaderCollection Headers = null)
        {
            var http = WebRequest.Create(new Uri(Url));
            http.Method = Method;

            if (Headers != null)
                http.Headers.Add(Headers);
            http.Credentials = CredentialCache.DefaultCredentials;

            if (!string.IsNullOrEmpty(Request))
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                Byte[] bytes = encoding.GetBytes(Request);

                using (Stream newStream = http.GetRequestStream())
                {
                    newStream.Write(bytes, 0, bytes.Length);
                    newStream.Close();
                }
            }
            try
            {
                using var response = http.GetResponse();
                using var stream = response.GetResponseStream();
                if (stream != null)
                {
                    using var sr = new StreamReader(stream);
                    var str = sr.ReadToEnd();
                    return JsonConvert.DeserializeObject<T>(str);
                }
                throw new NullReferenceException();
            }
            catch (WebException ex)
            {
                if (ex.Response.ContentLength > 0)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    {
                        var sr = new StreamReader(stream);
                        throw new Exception("ERROR: " + sr.ReadToEnd());
                    }
                }
                else
                    throw new Exception("ERROR: " + ex.Message);

            }
        }

        internal static DateTime ParseTimeWithTimeZone(string Data)
        {
            int iSep = Data.IndexOf(' ');

            return DateTime.Parse(Data.Substring(0, iSep));
            // TODO: Agregar TimeZone
        }

        internal static TimeSpan ParseIntervalYearToMonth(string Data)
        {
            int iSep = Data.IndexOf('-');

            return new TimeSpan(Convert.ToInt32(Data.Substring(0, iSep)) * 365 + Convert.ToInt32(Data.Substring(iSep + 1)) * 30, 0, 0, 0);
        }

        internal static TimeSpan ParseIntervalDayToSecond(string Data)
        {
            int iSep = Data.IndexOf(' ');

            return TimeSpan.Parse(Data.Substring(iSep + 1)).Add(new TimeSpan(Convert.ToInt32(Data.Substring(0, iSep)), 0, 0, 0));
        }

        internal static Dictionary<string, object> ParseDictionary(JObject Obj)
        {
            Dictionary<string, object> List = new Dictionary<string, object>();

            foreach (JProperty Item in Obj.Properties())
                List.Add(Item.Name, Item.Value);

            return List;
        }

        internal static IPAddress ParseIpAddress(string Data)
        {
            IPAddress ip;

            if (IPAddress.TryParse(Data, out ip))
                return ip;
            else
                return null;
        }
         
    }
}
