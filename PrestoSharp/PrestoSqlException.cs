using System;
using PrestoSharp.v1;

namespace PrestoSharp
{
    public class PrestoSqlException : Exception
    {
        public QueryError QueryError { get; internal set; }
        internal PrestoSqlException(QueryError error) : base(error.Message)
        {
            this.QueryError = error;
        }

        internal static PrestoSqlException Create(QueryError error)
        {
            switch(error.ErrorType)
            {
                case "USER_ERROR": return new PrestoSqlUserException(error);
                case "INTERNAL_ERROR": return new PrestoSqlInternalException(error);
                case "INSUFFICIENT_RESOURCES": return new PrestoSqlInsufficientResourceException(error);
                case "EXTERNAL": return new PrestoSqlExternalException(error);
                default:
                    return new PrestoSqlException(error);
            }
        }

        public string GetMessage() { return this.QueryError.Message; }
        public string GetSqlState() { return this.QueryError.SqlState; }
        public int GetErrorCode() { return this.QueryError.ErrorCode; }
        public string GetErrorName() { return this.QueryError.ErrorName; }
        public string GetErrorType() { return this.QueryError.ErrorType; }
        public ErrorLocation GetErrorLocation() { return this.QueryError.ErrorLocation; }
        public FailureInfo GetFailureInfo() { return this.QueryError.FailureInfo; }
    }


    public class PrestoSqlUserException : PrestoSqlException
    {
        internal PrestoSqlUserException(QueryError error): base(error) { }
    }

    public class PrestoSqlInternalException : PrestoSqlException
    {
        internal PrestoSqlInternalException(QueryError error) : base(error) { }
    }

    public class PrestoSqlInsufficientResourceException : PrestoSqlException
    {
        internal PrestoSqlInsufficientResourceException(QueryError error) : base(error) { }
    }

    public class PrestoSqlExternalException : PrestoSqlException
    {
        internal PrestoSqlExternalException(QueryError error) : base(error) { }
    }
}
