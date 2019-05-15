namespace ossServer.Tasks
{
    public enum ServerTaskStates
    {
        Queued = 0,
        Running = 1,
        Completed = 2,
        Error = 3,
        Cancelled = 4
    }
}
