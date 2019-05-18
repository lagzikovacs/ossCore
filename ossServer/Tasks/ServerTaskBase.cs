using Microsoft.Extensions.DependencyInjection;
using ossServer.Utils;
using System;
using System.Threading;

namespace ossServer.Tasks
{
    public abstract class ServerTaskBase
    {
        protected IServiceScopeFactory _scopeFactory;
        internal string _sid { get; set; }
        internal string _tasktoken { get; }
        protected ServerTaskResult _result { get; set; }
        protected CancellationToken _cancellationtoken = new CancellationToken();
        protected bool _cancelled;

        protected ServerTaskBase(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;

            _tasktoken = Guid.NewGuid().ToString("N");
            _result = new ServerTaskResult();
        }

        public void Start()
        {
            ServerTaskManager.Add(_tasktoken, this);

            var t = System.Threading.Tasks.Task.Factory.StartNew(InternalRun, _cancellationtoken);
        }

        private void InternalRun()
        {
            lock (_result)
                _result.Status = ServerTaskStates.Running;

            var exception = Run();

            lock (_result)
            {
                if (exception == null)
                {
                    _result.Status = _cancelled ? ServerTaskStates.Cancelled : ServerTaskStates.Completed;
                }
                else
                {
                    _result.Status = ServerTaskStates.Error;
                    _result.Error = exception.InmostMessage();
                }
            }

            // Várunk 1 percet a művelet befejeztével, 
            // ennyi ideje van hogy megszerezze az eredményt a kliens.
            Thread.Sleep(new TimeSpan(0, 0, 1, 0));
            // utána eldobjuk a taskot
            ServerTaskManager.TryRemove(_tasktoken);
        }

        protected abstract Exception Run();

        public ServerTaskResult Check()
        {
            lock (_result)
            {
                switch (_result.Status)
                {
                    case ServerTaskStates.Queued:
                    case ServerTaskStates.Running:
                        break;
                    case ServerTaskStates.Error:
                    case ServerTaskStates.Completed:
                        ServerTaskManager.TryRemove(_tasktoken);
                        break;
                }

                return _result;
            }
        }

        public void Cancel()
        {
            lock (_result)
            {
                _cancelled = true;
            }
        }
    }
}
