using System;
using System.Collections.Generic;
using ossServer.Models;
using ossServer.Tasks;
using ossServer.Utils;

namespace ossServer.Controllers.Riport
{
    public class PenztarTetelTask : ServerTaskBase
    {
        private readonly int _penztarKod;
        private readonly DateTime _datumTol;
        private readonly DateTime _datumIg;

        public PenztarTetelTask(string sid, List<SzMT> szmt) : base(sid)
        {
            _penztarKod = int.Parse(szmt[0].Minta.ToString());
            _datumTol = (DateTime)szmt[1].Minta;
            _datumIg = (DateTime)szmt[2].Minta;
        }

        protected override Exception Run()
        {
            Exception exception = null;

            using (var _context = new ossContext())
            {
                using (var tr = _context.Database.BeginTransaction())
                    try
                    {
                        var result = new RiportBll().PenztarTetel(_context, _sid,
                            _penztarKod, _datumTol, _datumIg);

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