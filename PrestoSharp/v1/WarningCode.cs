namespace PrestoSharp.v1
{
    public class WarningCode
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return Name + ":" + Code;
        }
    }
}
