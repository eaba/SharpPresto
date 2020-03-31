using System;
using System.Collections.Generic;

namespace PrestoSharp.v1
{
    public class FailureInfo
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public FailureInfo Cause { get; set; }
        public List<FailureInfo> Suppressed { get; set; }
        public List<string> Stack { get; set; }
        public ErrorLocation ErrorLocation { get; set; }
    }
}
