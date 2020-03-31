namespace PrestoSharp.v1
{
    public class StatementStats
    {
        public string State{ get; set; }
        public bool Queued{ get; set; }
        public bool Scheduled{ get; set; }
        public int Nodes{ get; set; }
        public int TotalSplits{ get; set; }
        public int QueuedSplits{ get; set; }
        public int RunningSplits{ get; set; }
        public int CompletedSplits{ get; set; }
        public long CpuTimeMillis{ get; set; }
        public long WallTimeMillis{ get; set; }
        public long QueuedTimeMillis{ get; set; }
        public long ElapsedTimeMillis{ get; set; }
        public long ProcessedRows{ get; set; }
        public long ProcessedBytes{ get; set; }
        public long PeakMemoryBytes{ get; set; }
        public long SpilledBytes{ get; set; }
        public StageStats RootStage{ get; set; }
    }
}
