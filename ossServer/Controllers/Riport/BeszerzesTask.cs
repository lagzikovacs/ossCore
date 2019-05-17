using System;
using System.Collections.Generic;
using ossServer.Models;
using ossServer.Tasks;
using ossServer.Utils;

namespace ossServer.Controllers.Riport
{
    public class BeszerzesTask : ServerTaskBase
    {
        private readonly DateTime _teljesitesKeltetol;
        private readonly DateTime _teljesitesKelteig;
        private readonly bool _reszletekIs;

        public BeszerzesTask(string sid, List<SzMT> szmt) : base(sid)
        {
            _teljesitesKeltetol = (DateTime)szmt[0].Minta;
            _teljesitesKelteig = (DateTime)szmt[1].Minta;
            _reszletekIs = bool.Parse(szmt[2].Minta.ToString());
        }

        protected override Exception Run()
        {
            Exception exception = null;

            using (var _context = new ossContext())
            {
                using (var tr = _context.Database.BeginTransaction())
                    try
                    {
                        var result = new RiportBll().Beszerzes(_context, _sid,
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

            return exception;
        }
    }
}