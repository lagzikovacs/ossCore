using ossServer.Utils;
using System;
using System.Collections.Concurrent;

namespace ossServer.Controllers.Kapcsolatihalo
{
    public class KapcsolatihaloTaskManager
    {
        private static readonly ConcurrentDictionary<string, KapcsolatihaloTask> SessionHandlers =
            new ConcurrentDictionary<string, KapcsolatihaloTask>();

        public static void Add(string taskId, KapcsolatihaloTask serverTask)
        {
            if (!SessionHandlers.TryAdd(taskId, serverTask))
                throw new Exception(Messages.TaszkHozzadasaNemsikerult);
        }

        public static KapcsolatihaloTask Get(string taskId, string sid)
        {
            if (SessionHandlers.TryGetValue(taskId, out var result))
            {
                if (result.sid != sid)
                    throw new Exception(Messages.TaszkNemferhetHozza);

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
