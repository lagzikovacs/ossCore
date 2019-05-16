using ossServer.BaseResults;
using ossServer.Tasks;

namespace ossServer.Controllers.Riport
{
    public class RiportResult : EmptyResult
    {
        public ServerTaskStates Status { get; set; } = ServerTaskStates.Queued;
        public byte[] Riport { get; set; }
    }
}
