using Microsoft.Extensions.DependencyInjection;
using ossServer.Models;
using ossServer.Tasks;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ossServer.Controllers.Riport
{
    public class KimenoSzamlaTask : ServerTaskBase
    {
        private DateTime _teljesitesKeltetol;
        private DateTime _teljesitesKelteig;
        
        public KimenoSzamlaTask(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {

        }

        public void Setup(string sid, List<SzMT> szmt)
        {
            _sid = sid;

            _teljesitesKeltetol = (DateTime)szmt[0].Minta;
            _teljesitesKelteig = (DateTime)szmt[1].Minta;
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
                            var result = await new RiportBll().KimenoSzamlaAsync(_context, _sid,
                                _teljesitesKeltetol, _teljesitesKelteig);

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
