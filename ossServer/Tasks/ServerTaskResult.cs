using ossServer.BaseResults;

namespace ossServer.Tasks
{
    public class ServerTaskResult : EmptyResult
    {
        public ServerTaskStates Status { get; set; } = ServerTaskStates.Queued;
        public byte[] Result { get; set; }
    }
}
