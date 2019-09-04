using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ossServer.Models;
using ossServer.RiportTask;
using ossServer.Utils;

namespace ossServer.Controllers.Riport
{
    public class BeszerzesTask : RiportTask.RiportTask
    {
        private DateTime _teljesitesKeltetol;
        private DateTime _teljesitesKelteig;
        private bool _reszletekIs;

        public BeszerzesTask(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {

        }

        public void Setup(string sid, List<SzMT> szmt)
        {
            _sid = sid;

            _teljesitesKeltetol = (DateTime)szmt[0].Minta;
            _teljesitesKelteig = (DateTime)szmt[1].Minta;
            _reszletekIs = bool.Parse(szmt[2].Minta.ToString());
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
                            var result = await new RiportBll().BeszerzesAsync(_context, _sid,
                                _teljesitesKeltetol, _teljesitesKelteig, _reszletekIs);

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