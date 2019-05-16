using ossServer.Models;
using ossServer.Tasks;
using ossServer.Utils;
using System;
using System.Collections.Generic;

namespace ossServer.Controllers.Riport
{
    public class BejovoSzamlaTask : ServerTaskBase
    {
        private readonly DateTime _teljesitesKeltetol;
        private readonly DateTime _teljesitesKelteig;

        public BejovoSzamlaTask(string sid, List<SzMT> szmt) : base(sid)
        {
            _teljesitesKeltetol = (DateTime)szmt[0].Minta;
            _teljesitesKelteig = (DateTime)szmt[1].Minta;
        }

        protected override Exception Run()
        {
            Exception exception = null;

            using (var _context = new ossContext())
            {
                using (var tr = _context.Database.BeginTransaction())
                    try
                    {
                        var result = new RiportBll().BejovoSzamla(_context, _sid,
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

            return exception;
        }
    }
}
