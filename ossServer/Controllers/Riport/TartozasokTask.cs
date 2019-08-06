using Microsoft.Extensions.DependencyInjection;
using ossServer.Models;
using ossServer.Tasks;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ossServer.Controllers.Riport
{
    public class TartozasokTask : ServerTaskBase
    {
        private DateTime _ezenANapon;
        private bool _lejart;

        public TartozasokTask(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {

        }

        public void Setup(string sid, List<SzMT> szmt)
        {
            _sid = sid;

            _ezenANapon = (DateTime)szmt[0].Minta;
            _lejart = false;
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
                            var result = await new RiportBll().TartozasokAsync(_context, _sid,
                                _ezenANapon, _lejart);

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
