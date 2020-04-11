using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json.Linq;

namespace PrestoSharp
{
    internal class StandardTypes
    {
        static Dictionary<string, Type> _mTypeMapping = new Dictionary<string, Type>();
        static StandardTypes()
        {
            _mTypeMapping.Add(BIGINT, typeof(long));
            _mTypeMapping.Add(INTEGER, typeof(int));
            _mTypeMapping.Add(SMALLINT, typeof(short));
            _mTypeMapping.Add(TINYINT, typeof(short));
            _mTypeMapping.Add(BOOLEAN, typeof(bool));
            _mTypeMapping.Add(DATE, typeof(DateTime));
            _mTypeMapping.Add(DECIMAL, typeof(decimal));
            _mTypeMapping.Add(REAL, typeof(float));
            _mTypeMapping.Add(DOUBLE, typeof(double));
            // TODO: Map HYPER_LOG_LOG
            // TODO: Map QDIGEST
            // TODO: Map P4_HYPER_LOG_LOG
            _mTypeMapping.Add(INTERVAL_DAY_TO_SECOND, typeof(TimeSpan));
            _mTypeMapping.Add(INTERVAL_YEAR_TO_MONTH, typeof(TimeSpan));
            _mTypeMapping.Add(TIMESTAMP, typeof(DateTime));
            _mTypeMapping.Add(TIMESTAMP_WITH_TIME_ZONE, typeof(DateTime));
            _mTypeMapping.Add(TIME, typeof(DateTime));
            _mTypeMapping.Add(TIME_WITH_TIME_ZONE, typeof(DateTime));
            _mTypeMapping.Add(VARBINARY, typeof(byte[]));
            _mTypeMapping.Add(VARCHAR, typeof(string));
            _mTypeMapping.Add(CHAR, typeof(string));
            // TODO: Map ROW
            // TODO: Map ARRAY
            // TODO: Map MAP
            // TODO: Map JSON
            _mTypeMapping.Add(IPADDRESS, typeof(IPAddress));
            // TODO: Map GEOMETRY
            // TODO: Map BING_TILE

        }

        public static string BIGINT = "bigint";
        public static string INTEGER = "integer";
        public static string SMALLINT = "smallint";
        public static string TINYINT = "tinyint";
        public static string BOOLEAN = "boolean";
        public static string DATE = "date";
        public static string DECIMAL = "decimal";
        public static string REAL = "real";
        public static string DOUBLE = "double";
        public static string HYPER_LOG_LOG = "HyperLogLog";
        public static string QDIGEST = "qdigest";
        public static string P4_HYPER_LOG_LOG = "P4HyperLogLog";
        public static string INTERVAL_DAY_TO_SECOND = "interval day to second";
        public static string INTERVAL_YEAR_TO_MONTH = "interval year to month";
        public static string TIMESTAMP = "timestamp";
        public static string TIMESTAMP_WITH_TIME_ZONE = "timestamp with time zone";
        public static string TIME = "time";
        public static string TIME_WITH_TIME_ZONE = "time with time zone";
        public static string VARBINARY = "varbinary";
        public static string VARCHAR = "varchar";
        public static string CHAR = "char";
        public static string ROW = "row";
        public static string ARRAY = "array";
        public static string MAP = "map";
        public static string JSON = "json";
        public static string IPADDRESS = "ipaddress";
        public static string GEOMETRY = "Geometry";
        public static string BING_TILE = "BingTile";

        internal static Type MapType(string typeName)
        {
            if (_mTypeMapping.ContainsKey(typeName))
                return _mTypeMapping[typeName];
            return Type.Missing.GetType();
        }

        internal static object Convert(string typeName, object Obj)
        {
            if (typeName == VARBINARY)
                return System.Convert.FromBase64String((string)Obj);
            if (typeName == TIME_WITH_TIME_ZONE)
                return Helper.ParseTimeWithTimeZone((string)Obj);
            if (typeName == TIMESTAMP_WITH_TIME_ZONE)
                return Helper.ParseTimeWithTimeZone((string)Obj);
            if (typeName == INTERVAL_YEAR_TO_MONTH)
                return Helper.ParseIntervalYearToMonth((string)Obj);
            if (typeName == INTERVAL_DAY_TO_SECOND)
                return Helper.ParseIntervalDayToSecond((string)Obj);
            if (typeName == ARRAY)
                return ((JArray)Obj).ToObject<object[]>();
            if (typeName == MAP)
                return Helper.ParseDictionary((JObject)Obj);
            if (typeName == ROW)
                return Obj; // TODO: Parse Row
            if (typeName == JSON)
                return ((JToken)Obj);
            if (typeName == IPADDRESS)
                return Helper.ParseIpAddress((string)Obj);
            return System.Convert.ChangeType(Obj, MapType(typeName));
        }

    }
}
