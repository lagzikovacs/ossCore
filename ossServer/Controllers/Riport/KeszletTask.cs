using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ossServer.Models;
using ossServer.Tasks;
using ossServer.Utils;

namespace ossServer.Controllers.Riport
{
    public class KeszletTask : ServerTaskBase
    {
        private DateTime _idopont;

        public KeszletTask(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
            
        }

        public void Setup(string sid, List<SzMT> szmt)
        {
            _sid = sid;

            _idopont = (DateTime)szmt[0].Minta;
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
                            var result = await new RiportBll().KeszletAsync(_context, _sid, _idopont);

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