using Microsoft.Extensions.DependencyInjection;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ossServer.Controllers.Kapcsolatihalo
{
    public class KapcsolatihaloTask
    {
        public IServiceScopeFactory scopeFactory;
        public string tasktoken { get; }
        public KapcsolatihaloTaskTypes tasktype { get; private set; }
        public List<KapcsolatihaloPos> pos { get; private set; }
        public string sid { get; private set; }
        public KapcsolatihaloTaskResult result { get; set; }
        public CancellationToken cancellationtoken = new CancellationToken();

        public KapcsolatihaloTask(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;

            tasktoken = Guid.NewGuid().ToString("N");
            result = new KapcsolatihaloTaskResult();
        }

        public void Setup(string sid, KapcsolatihaloTaskTypes tasktype, List<KapcsolatihaloPos> pos = null)
        {
            this.sid = sid;
            this.tasktype = tasktype;
            this.pos = pos;
        }

        public void Start()
        {
            lock (result)
            {
                if (result.status != KapcsolatihaloTaskStates.Queued)
                    throw new Exception(Messages.TaszkNemindithato);

                result.status = KapcsolatihaloTaskStates.Running;
            }

            KapcsolatihaloTaskManager.Add(tasktoken, this);
            switch (tasktype)
            {
                case KapcsolatihaloTaskTypes.Reader:
                    Task.Factory.StartNew(ReadAsync, cancellationtoken);
                    break;
                case KapcsolatihaloTaskTypes.Writer:
                    Task.Factory.StartNew(WriteAsync, cancellationtoken);
                    break;
            }
            
        }

        private async Task ReadAsync()
        {
            using (var scope = scopeFactory.CreateScope())
            {
                using (var _context = scope.ServiceProvider.GetRequiredService<ossContext>())
                {
                    using (var tr = await _context.Database.BeginTransactionAsync())
                        try
                        {
                            var r = await KapcsolatihaloBll.Read(_context, sid);

                            tr.Commit();

                            lock (result)
                            {
                                result.status = KapcsolatihaloTaskStates.Completed;
                                result.lstUgyfelkapcsolatDto = r.lstUgyfelkapcsolatDto;
                                result.lstUgyfelDto = r.lstUgyfelDto;
                            }
                        }
                        catch (Exception ex)
                        {
                            tr.Rollback();

                            lock (result)
                            {
                                result.status = KapcsolatihaloTaskStates.Error;
                                result.Error = ex.InmostMessage();
                            }
                        }
                }
            }

            // Várunk 10 percet a művelet befejeztével, 
            // ennyi ideje van hogy megszerezze az eredményt a kliens.
            Thread.Sleep(new TimeSpan(0, 0, 10, 0));
            // utána eldobjuk a taskot
            KapcsolatihaloTaskManager.TryRemove(tasktoken);
        }

        private async Task WriteAsync()
        {
            using (var scope = scopeFactory.CreateScope())
            {
                using (var _context = scope.ServiceProvider.GetRequiredService<ossContext>())
                {
                    using (var tr = await _context.Database.BeginTransactionAsync())
                        try
                        {
                            var r = await KapcsolatihaloBll.Write(_context, sid, pos);

                            tr.Commit();

                            lock (result)
                            {
                                result.status = KapcsolatihaloTaskStates.Completed;
                                result.lstUgyfelkapcsolatDto = null;
                                result.lstUgyfelDto = r.lstUgyfelDto;
                            }
                        }
                        catch (Exception ex)
                        {
                            tr.Rollback();

                            lock (result)
                            {
                                result.status = KapcsolatihaloTaskStates.Error;
                                result.Error = ex.InmostMessage();
                            }
                        }
                }
            }

            // Várunk 10 percet a művelet befejeztével, 
            // ennyi ideje van hogy megszerezze az eredményt a kliens.
            Thread.Sleep(new TimeSpan(0, 0, 10, 0));
            // utána eldobjuk a taskot
            KapcsolatihaloTaskManager.TryRemove(tasktoken);
        }

        public KapcsolatihaloTaskResult Check()
        {
            lock (result)
            {
                switch (result.status)
                {
                    case KapcsolatihaloTaskStates.Queued:
                    case KapcsolatihaloTaskStates.Running:
                        break;
                    case KapcsolatihaloTaskStates.Error:
                    case KapcsolatihaloTaskStates.Completed:
                        KapcsolatihaloTaskManager.TryRemove(tasktoken);
                        break;
                }

                return result;
            }
        }

        public void Stop()
        {

        }
    }
}
