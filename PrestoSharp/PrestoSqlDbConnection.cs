using System;
using System.Data;
using System.Data.Common;
using System.Net;
using PrestoSharp.v1;

namespace PrestoSharp
{
    public class PrestoSqlDbConnection : DbConnection
    {
        public PrestoSqlDbConnection()
        { }

        public PrestoSqlDbConnection(string ConnectionString)
        {
            this.ConnectionString = ConnectionString;
        }

        public sealed override string ConnectionString { get; set; }

        public override string Database { get; }

        public override string DataSource { get; }

        public override string ServerVersion { get; }

        public override ConnectionState State { get; }

        public override void ChangeDatabase(string databaseName)
        {
        }

        public override void Close()
        {
        }

        public override void Open()
        {
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand CreateDbCommand()
        {
            return new PrestoSqlDbCommand() { Connection = this };
        }

        internal QueryResults ExecuteQuery(string Query)
        {
            var headers = new WebHeaderCollection
            {
                {"Content-Type", "application/json"},
                {"Accept", "application/json"},
                {PrestoHeaders.PRESTO_USER, "sharppresto"}
            };

            return Helper.HttpRequest<QueryResults>("POST", ConnectionString + "/v1/statement", Query, headers);
        }

        internal QueryResults GetNextResult(Uri nextUri)
        {
            var headers = new WebHeaderCollection {{"Accept", "application/json"}};
            var result = Helper.HttpRequest<QueryResults>("GET", nextUri.ToString(), "", headers);
            return result;
        }

    }
}
