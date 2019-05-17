using System;
using System.Collections.Generic;
using ossServer.Models;
using ossServer.Tasks;
using ossServer.Utils;

namespace ossServer.Controllers.Riport
{
    public class ProjektTask : ServerTaskBase
    {
        private readonly int _kod;
        private readonly string _nev;

        public ProjektTask(string sid, List<SzMT> szmt) : base(sid)
        {
            _kod = int.Parse(szmt[0].Minta.ToString());
            _nev = (string)szmt[1].Minta;
        }

        protected override Exception Run()
        {
            Exception exception = null;

            using (var _context = new ossContext())
            {
                using (var tr = _context.Database.BeginTransaction())
                    try
                    {
                        var result = new RiportBll().Projekt(_context, _sid,
                            _kod, _nev);

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