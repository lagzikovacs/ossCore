using Microsoft.AspNetCore.Mvc;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Threading;

namespace ossServer.Tasks
{
    public abstract class ServerTaskBase
    {
        public string TaskToken { get; }

        public string Sid { get; }

        protected CancellationToken CancellationToken = new CancellationToken();

        protected ServerTaskResultDto Result { get; set; }

        public DateTime ExpireOn { get; private set; }


        public bool Expired => ExpireOn < DateTime.Now;
        public bool Cancelled;


        protected ServerTaskBase(string sid)
        {
            Sid = sid;
            TaskToken = Guid.NewGuid().ToString("N");
        }

        public void Start()
        {
            ServerTaskManager.Add(TaskToken, this);

            System.Threading.Tasks.Task.Factory.StartNew(InternalRun, CancellationToken);
        }

        public void GiveAMinute()
        {
            ExpireOn = DateTime.Now.AddMinutes(1);
        }

        private void InternalRun()
        {
            lock (Result)
                Result.Status = ServerTaskStates.Running;

            GiveAMinute();

            var exception = Run();

            lock (Result)
            {
                if (exception == null)
                {
                    Result.Status = Cancelled ? ServerTaskStates.Cancelled : ServerTaskStates.Completed;
                }
                else
                {
                    Result.Status = ServerTaskStates.Error;
                    Result.Error = exception.InmostMessage();
                }
            }

            // Várunk 1 percet a művelet befejeztével, ennyi ideje van hogy megszerezze az eredményt a kliens.
            Thread.Sleep(new TimeSpan(0, 0, 1, 0));

            ServerTaskManager.TryRemove(TaskToken);
        }

        protected abstract Exception Run();

        public ServerTaskResultDto Check()
        {
            lock (Result)
            {
                switch (Result.Status)
                {
                    case ServerTaskStates.Queued:
                    case ServerTaskStates.Running:
                        GiveAMinute();
                        break;
                    case ServerTaskStates.Error:
                    case ServerTaskStates.Completed:
                        ServerTaskManager.TryRemove(TaskToken);
                        break;
                }
                return Result;
            }
        }

        public void Cancel()
        {
            lock (Result)
            {
                Cancelled = true;
            }
        }

        public void IsExpired()
        {
            if (Expired) //ez hiba, el kell dobni mindent
                throw new Exception("Expired!");
        }
    }
}
