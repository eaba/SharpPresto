using System;
using System.Data;
using System.Data.Common;
using PrestoSharp.v1;

namespace PrestoSharp
{
    public class PrestoSqlDbCommand : DbCommand
    {
        public override string CommandText { get; set; }
        public override int CommandTimeout { get; set; }
        public override CommandType CommandType { get; set; }
        public override bool DesignTimeVisible { get; set; }
        public override UpdateRowSource UpdatedRowSource { get; set; }
        protected override DbConnection DbConnection { get; set; }
        protected override DbParameterCollection DbParameterCollection { get; }
        protected override DbTransaction DbTransaction { get; set; }

        internal QueryResults Result { get; set; }

        public override void Cancel()
        {
        }


        public override void Prepare()
        {
        }

        protected override DbParameter CreateDbParameter()
        {
            throw new NotImplementedException();
        }

        internal QueryResults GetNextResult()
        {
            if (Result != null && Result.NextUri != null)
            {
                Result = ((PrestoSqlDbConnection)DbConnection).GetNextResult(Result.NextUri);

                if (Result.Error != null)
                    throw PrestoSqlException.Create(Result.Error);

                return Result;
            }
            else
                return null;
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            Result = ((PrestoSqlDbConnection)DbConnection).ExecuteQuery(CommandText);

            return new PrestoSqlDbDataReader(this);
        }

        public override int ExecuteNonQuery()
        {
            var Rows = 0;

            using var Reader = ExecuteDbDataReader(CommandBehavior.SequentialAccess);
            while (Reader != null && Reader.Read())
                Rows++;
            return Rows;
        }

        public override object ExecuteScalar()
        {
            using var Reader = ExecuteDbDataReader(CommandBehavior.SequentialAccess);
            if (Reader != null && Reader.Read())
                return Reader[0];
            else
                return null;
        }
    }
}
