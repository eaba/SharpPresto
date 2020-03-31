using System;
using System.Collections.Generic;

namespace PrestoSharp.v1
{
    public class QueryResults
    {
        public String Id{ get; set; }
        public Uri InfoUri{ get; set; }
        public Uri PartialCancelUri{ get; set; }
        public Uri NextUri{ get; set; }
        public List<Column> Columns{ get; set; }
        public List<List<object>> Data{ get; set; }
        public StatementStats Stats{ get; set; }
        public QueryError Error{ get; set; }
        public List<PrestoWarning> Warnings{ get; set; }
        public String UpdateType{ get; set; }
        public long UpdateCount{ get; set; }
        /*
         * public virtual Id {get;}
		public virtual InfoUri {get;}
		public virtual PartialCancelUri {get;}
		public virtual NextUri {get;}
		private readonly IList<Column> columns;
		private readonly IEnumerable<IList<object>> data;
		public virtual Stats {get;}
		public virtual Exception {get;}
		private readonly IList<PrestoWarning> warnings;
		public virtual UpdateType {get;}
		private readonly long? updateCount;
         */
    }
}
