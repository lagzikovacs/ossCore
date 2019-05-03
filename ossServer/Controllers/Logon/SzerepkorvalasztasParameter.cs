using System.Runtime.Serialization;

namespace ossServer.Controllers.Logon
{
    [DataContract]
    public class SzerepkorvalasztasParameter
    {
        [DataMember]
        public int ParticioKod { get; set; }

        [DataMember]
        public int CsoportKod { get; set; }
    }
}
