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
            using DbConnection conn = new PrestoSqlDbConnection
            {
                ConnectionString = "http://localhost:8081"
            };
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "select SequenceNr from pulsar.\"public/default\".journal WHERE PersistenceId = 'sampleActor' ORDER BY SequenceNr DESC LIMIT 1";

            using var reader = cmd.ExecuteReader();
            var rows = reader.RecordsAffected;
            if (reader.Read())
            {
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    var T = reader.GetFieldType(i);
                    var value = reader.GetValue(i);
                    _helper.WriteLine(value.ToString());
                }
            }
        }

        [Fact]
        public void Schema()
        {
            using DbConnection conn = new PrestoSqlDbConnection
            {
                ConnectionString = "http://localhost:8081"
            };
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "show tables in pulsar.\"public/default\"";

            using var reader = cmd.ExecuteReader();
            var rows = reader.RecordsAffected;
            while (reader.Read())
            {
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    var T = reader.GetFieldType(i);
                    var value = reader.GetValue(i);
                    _helper.WriteLine(value.ToString());
                }
            }
        }

        [Fact]
        public void SingleResultTest()
        {
            using DbConnection conn = new PrestoSqlDbConnection
            {
                ConnectionString = "http://localhost:8081"
            };
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "select SequenceNr from pulsar.\"public/default\".journal WHERE PersistenceId = 'sampleActor' ORDER BY SequenceNr DESC LIMIT 1";

            var v = Convert.ToInt32(cmd.ExecuteScalar());
            _helper.WriteLine(v.ToString());

            if (v <= 1)
                Assert.False(false,"Invalid return value");
        }


        [Fact]
        public void ErrorTest()
        {
            using DbConnection conn = new PrestoSqlDbConnection();
            conn.ConnectionString = "http://localhost:8081";
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"ERROR HERE!";


            try
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            var y = reader[i];
                            //_helper.WriteLine();
                        }
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
            using DbConnection conn = new PrestoSqlDbConnection
            {
                ConnectionString = "http://localhost:8081"
            };
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "select * from pulsar.\"public/default\".journal";

            using var reader = cmd.ExecuteReader();
            var rows = reader.RecordsAffected;
            while (reader.Read())
            {
                var row = new List<string>();
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    try
                    {
                        var value = reader.GetValue(i).ToString();
                        var col = reader.GetName(i);
                        row.Add($"{col} : {value}");
                    }
                    catch (Exception e)
                    {
                       
                    }
                }
                _helper.WriteLine(string.Join(" - ", row));
            }
        }

        [Fact]
        public void FilterTest()
        {
            using DbConnection conn = new PrestoSqlDbConnection
            {
                ConnectionString = "http://localhost:8081"
            };
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "select __producer_name__, __sequence_id__ from pulsar.\"public/default\".students";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var row = new List<string>();
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    try
                    {
                        var value = reader.GetValue(i).ToString();
                        var col = reader.GetName(i);
                        row.Add($"{col} : {value}");
                    }
                    catch (Exception e)
                    {

                    }
                }
                _helper.WriteLine(string.Join(" - ", row));
            }
        }
    }
}
