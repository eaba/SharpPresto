using System.Collections.Generic;

namespace PrestoSharp.v1
{
    public class ClientTypeSignature
    {
        public string RawType { get; set; }
        //typeArguments[]
        //literalArguments[]
        public List<ClientTypeSignatureParameter> Arguments { get; set; }
    }
}
