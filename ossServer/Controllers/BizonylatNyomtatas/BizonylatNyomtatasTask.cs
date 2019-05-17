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
        private readonly int _bizonylatkod;
        private readonly BizonylatNyomtatasTipus _nyomtatasTipus;

        public BizonylatNyomtatasTask(string sid, List<SzMT> szmt) : base(sid)
        {
            _bizonylatkod = SzMTUtils.GetInt(szmt, Szempont.BizonylatKod);
            _nyomtatasTipus = SzMTUtils.GetBizonylatNyomtatasTipus(szmt, Szempont.NyomtatasTipus);
        }

        protected override Exception Run()
        {
            Exception exception = null;

            using (var _context = new ossContext())
            {
                using (var tr = _context.Database.BeginTransaction())
                    try
                    {
                        var result = BizonylatNyomtatasBll.Nyomtatas(_context, _sid, 
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
