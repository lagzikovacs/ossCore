using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Tasks;
using ossServer.Utils;
using System;
using System.Collections.Generic;

namespace ossServer.Controllers.BizonylatNyomtatas
{
    public class BizonylatNyomtatasTask : ServerTaskBase
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

        protected override Exception Run()
        {
            Exception exception = null;

            using (var scope = _scopeFactory.CreateScope())
            {
                var _config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var _context = scope.ServiceProvider.GetRequiredService<ossContext>();
                using (var tr = _context.Database.BeginTransaction())
                    try
                    {
                        var result = BizonylatNyomtatasBll.Nyomtatas(_config, _context, _sid,
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
