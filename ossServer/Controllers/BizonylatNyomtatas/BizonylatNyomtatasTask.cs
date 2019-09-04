using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ossServer.Controllers.BizonylatNyomtatas
{
    public class BizonylatNyomtatasTask : RiportTask.RiportTask
    {
        private int _bizonylatkod;
        private BizonylatNyomtatasTipus _nyomtatasTipus;

        public BizonylatNyomtatasTask(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {

        }

        public void Setup(string sid, List<SzMT> szmt)
        {
            _sid = sid;

            _bizonylatkod = SzMTUtils.GetInt(szmt, Szempont.BizonylatKod);
            _nyomtatasTipus = SzMTUtils.GetBizonylatNyomtatasTipus(szmt, Szempont.NyomtatasTipus);
        }

        protected override async Task<Exception> RunAsync()
        {
            Exception exception = null;

            using (var scope = _scopeFactory.CreateScope())
            {
                var _config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var _context = scope.ServiceProvider.GetRequiredService<ossContext>();
                using (var tr = _context.Database.BeginTransaction())
                    try
                    {
                        var result = await BizonylatNyomtatasBll.NyomtatasAsync(_config, _context, _sid,
                            _bizonylatkod, _nyomtatasTipus);

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

            return exception;
        }
    }
}
