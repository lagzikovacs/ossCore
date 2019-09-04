using ossServer.BaseResults;
using ossServer.RiportTask;

namespace ossServer.Controllers.Riport
{
    public class RiportResult : EmptyResult
    {
        public RiportTaskStates Status { get; set; } = RiportTaskStates.Queued;
        public byte[] Riport { get; set; }
    }
}
