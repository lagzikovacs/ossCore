using ossServer.BaseResults;

namespace ossServer.RiportTask
{
    public class RiportTaskResult : EmptyResult
    {
        public RiportTaskStates Status { get; set; } = RiportTaskStates.Queued;
        public byte[] Result { get; set; }
    }
}
