namespace ossServer.Tasks
{
    public class ServerTaskResultDto
    {
        public ServerTaskStates Status { get; set; } = ServerTaskStates.Queued;
        public string Error { get; set; }
    }
}
