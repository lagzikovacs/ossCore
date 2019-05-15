using ossServer.Utils;
using System;
using System.Collections.Concurrent;

namespace ossServer.Tasks
{
    public class ServerTaskManager
    {
        private static readonly ConcurrentDictionary<string, ServerTaskBase> SessionHandlers =
            new ConcurrentDictionary<string, ServerTaskBase>();

        public static void Add(string taskId, ServerTaskBase serverTask)
        {
            if (!SessionHandlers.TryAdd(taskId, serverTask))
                throw new Exception("Tasks.Add");
        }

        public static ServerTaskBase Get(string taskId, string sid)
        {
            if (SessionHandlers.TryGetValue(taskId, out var result))
            {
                if (result.Sid != sid)
                    throw new Exception("Tasks.Get");

                return result;
            }

            throw new Exception(Messages.TaszkNemTalalhatoVagyLejart);
        }

        public static bool TryRemove(string taskId)
        {
            return SessionHandlers.TryRemove(taskId, out _);
        }
    }
}
