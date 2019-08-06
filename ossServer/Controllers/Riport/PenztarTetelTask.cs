using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ossServer.Models;
using ossServer.Tasks;
using ossServer.Utils;

namespace ossServer.Controllers.Riport
{
    public class PenztarTetelTask : ServerTaskBase
    {
        private int _penztarKod;
        private DateTime _datumTol;
        private DateTime _datumIg;

        public PenztarTetelTask(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {

        }

        public void Setup(string sid, List<SzMT> szmt)
        {
            _sid = sid;

            _penztarKod = int.Parse(szmt[0].Minta.ToString());
            _datumTol = (DateTime)szmt[1].Minta;
            _datumIg = (DateTime)szmt[2].Minta;
        }

        protected override async Task<Exception> RunAsync()
        {
            Exception exception = null;

            using (var scope = _scopeFactory.CreateScope())
            {
                using (var _context = scope.ServiceProvider.GetRequiredService<ossContext>())
                {
                    using (var tr = _context.Database.BeginTransaction())
                        try
                        {
                            var result = await new RiportBll().PenztarTetelAsync(_context, _sid,
                                _penztarKod, _datumTol, _datumIg);

                            tr.Commit();

                            lock (_result)
                            {
                                _result.Result = result;
                            }
                        }
                        catch (Exception ex)
                        {
                            tr.Rollback();
                            exception = ex;
                        }
                }
            }

            return exception;
        }
    }
}