using System.Collections.Generic;

namespace PrestoSharp.v1
{
    public class StageStats
    {
        public string StageId { get; set; }
        public string State { get; set; }
        public bool Done{ get; set; }
        public int Nodes{ get; set; }
        public int TotalSplits{ get; set; }
        public int QueuedSplits{ get; set; }
        public int RunningSplits{ get; set; }
        public int CompletedSplits{ get; set; }
        public long CpuTimeMillis{ get; set; }
        public long WallTimeMillis{ get; set; }
        public long ProcessedRows{ get; set; }
        public long ProcessedBytes{ get; set; }
        public List<StageStats> SubStages{ get; set; }
    }
}
