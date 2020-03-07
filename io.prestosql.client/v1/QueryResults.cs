using System;
using System.Collections.Generic;
using System.Text;

namespace io.prestosql.client.v1
{
    public class QueryResults
    {
        public String id{ get; set; }
        public Uri infoUri{ get; set; }
        public Uri partialCancelUri{ get; set; }
        public Uri nextUri{ get; set; }
        public List<Column> columns{ get; set; }
        public List<List<object>> data{ get; set; }
        public StatementStats stats{ get; set; }
        public QueryError error{ get; set; }
        public List<PrestoWarning> warnings{ get; set; }
        public String updateType{ get; set; }
        public long updateCount{ get; set; }
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
