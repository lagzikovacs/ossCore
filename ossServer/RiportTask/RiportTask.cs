using Microsoft.Extensions.DependencyInjection;
using ossServer.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ossServer.RiportTask
{
    public abstract class RiportTask
    {
        protected IServiceScopeFactory _scopeFactory;
        internal string _sid { get; set; }
        internal string _tasktoken { get; }
        protected RiportTaskResult _result { get; set; }
        protected CancellationToken _cancellationtoken = new CancellationToken();
        protected bool _cancelled;

        protected RiportTask(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;

            _tasktoken = Guid.NewGuid().ToString("N");
            _result = new RiportTaskResult();
        }

        public void Start()
        {
            RiportTaskManager.Add(_tasktoken, this);

            var t = Task.Factory.StartNew(InternalRunAsync, _cancellationtoken);
        }

        private async Task InternalRunAsync()
        {
            lock (_result)
                _result.Status = RiportTaskStates.Running;

            var exception = await RunAsync();

            lock (_result)
            {
                if (exception == null)
                {
                    _result.Status = _cancelled ? RiportTaskStates.Cancelled : RiportTaskStates.Completed;
                }
                else
                {
                    _result.Status = RiportTaskStates.Error;
                    _result.Error = exception.InmostMessage();
                }
            }

            // Várunk 1 percet a művelet befejeztével, 
            // ennyi ideje van hogy megszerezze az eredményt a kliens.
            Thread.Sleep(new TimeSpan(0, 0, 1, 0));
            // utána eldobjuk a taskot
            RiportTaskManager.TryRemove(_tasktoken);
        }

        protected abstract Task<Exception> RunAsync();

        public RiportTaskResult Check()
        {
            lock (_result)
            {
                switch (_result.Status)
                {
                    case RiportTaskStates.Queued:
                    case RiportTaskStates.Running:
                        break;
                    case RiportTaskStates.Error:
                    case RiportTaskStates.Completed:
                        RiportTaskManager.TryRemove(_tasktoken);
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
