using System;
using System.Collections.Generic;
using System.Data.Common;
using Xunit;
using Xunit.Abstractions;

namespace PrestoSharp.test
{
    //https://github.com/apache/pulsar/issues/6501
    //http://pulsar.apache.org/docs/en/sql-getting-started/
    //https://pulsar.apache.org/docs/en/sql-rest-api/
    //docker run -it -p 6650:6650 -p 8080:8080 -p 8081:8081 --mount source=pulsardata,target=/pulsar/data --mount source=pulsarconf,target=/pulsar/conf apachepulsar/pulsar:2.5.0 bin/pulsar standalone sql-worker function-worker
    public class UnitTest1
    {
        private ITestOutputHelper _helper;

        public UnitTest1(ITestOutputHelper Helper)
        {
            _helper = Helper;
        }

        [Fact]
        public void FirstTest()
        {
            using DbConnection Conn = new PrestoSqlDbConnection
            {
                ConnectionString = "http://52.177.28.96:8081"
            };
            Conn.Open();

            using var Cmd = Conn.CreateCommand();
            Cmd.CommandText = "show catalogs";

            using var Reader = Cmd.ExecuteReader();
            if (Reader.Read())
            {
                for (var i = 0; i < Reader.FieldCount; i++)
                {
                    var T = Reader.GetFieldType(i);
                    var Value = Reader.GetValue(i);
                    _helper.WriteLine(Value.ToString());
                }
            }
        }

        [Fact]
        public void Schema()
        {
            using DbConnection Conn = new PrestoSqlDbConnection
            {
                ConnectionString = "http://52.177.28.96:8081"
            };
            Conn.Open();

            using var Cmd = Conn.CreateCommand();
            Cmd.CommandText = "show tables in pulsar.\"public/default\"";

            using var Reader = Cmd.ExecuteReader();
            while (Reader.Read())
            {
                for (var i = 0; i < Reader.FieldCount; i++)
                {
                    var T = Reader.GetFieldType(i);
                    var Value = Reader.GetValue(i);
                    _helper.WriteLine(Value.ToString());
                }
            }
        }

        [Fact]
        public void SingleResultTest()
        {
            using DbConnection Conn = new PrestoSqlDbConnection
            {
                ConnectionString = "http://52.177.28.96:8081"
            };
            Conn.Open();

            using var Cmd = Conn.CreateCommand();
            Cmd.CommandText = "SELECT COUNT(*) FROM system.runtime.nodes";

            var v = Convert.ToInt32(Cmd.ExecuteScalar());

            if (v <= 1)
                Assert.False(false,"Invalid return value");
        }


        [Fact]
        public void ErrorTest()
        {
            using DbConnection Conn = new PrestoSqlDbConnection();
            Conn.ConnectionString = "http://localhost:8080";
            Conn.Open();

            using var Cmd = Conn.CreateCommand();
            Cmd.CommandText = @"ERROR HERE!";


            try
            {
                using (var Reader = Cmd.ExecuteReader())
                {
                    while (Reader.Read())
                    {
                        for (var i = 0; i < Reader.FieldCount; i++)
                            Console.Write(Reader[i]);

                    }
                }

                Assert.False(false);
            }
            catch (PrestoSqlException ex)
            {

            }
        }


        [Fact]
        public void SecondTest()
        {
            using DbConnection Conn = new PrestoSqlDbConnection
            {
                ConnectionString = "http://52.177.28.96:8081"
            };
            Conn.Open();

            using var Cmd = Conn.CreateCommand();
            Cmd.CommandText = "select * from pulsar.\"public/default\".students";

            using var Reader = Cmd.ExecuteReader();
            while (Reader.Read())
            {
                var row = new List<string>();
                for (var i = 0; i < Reader.FieldCount; i++)
                {
                    var T = Reader.GetFieldType(i);
                    var Value = Reader.GetValue(i).ToString();
                    var col = Reader.GetName(i);
                    row.Add($"{col}:{Value}");
                }
                _helper.WriteLine(string.Join(" - ", row));
            }
        }
    }
}
