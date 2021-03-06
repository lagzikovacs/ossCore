﻿using ossServer.Utils;
using System;
using System.Collections.Concurrent;

namespace ossServer.RiportTask
{
    public class RiportTaskManager
    {
        private static readonly ConcurrentDictionary<string, RiportTask> SessionHandlers =
            new ConcurrentDictionary<string, RiportTask>();

        public static void Add(string taskId, RiportTask serverTask)
        {
            if (!SessionHandlers.TryAdd(taskId, serverTask))
                throw new Exception(Messages.TaszkHozzadasaNemsikerult);
        }

        public static RiportTask Get(string taskId, string sid)
        {
            if (SessionHandlers.TryGetValue(taskId, out var result))
            {
                if (result._sid != sid)
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
