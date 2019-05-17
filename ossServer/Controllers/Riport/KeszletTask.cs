using System;
using System.Collections.Generic;
using ossServer.Models;
using ossServer.Tasks;
using ossServer.Utils;

namespace ossServer.Controllers.Riport
{
    public class KeszletTask : ServerTaskBase
    {
        private readonly DateTime _idopont;

        public KeszletTask(string sid, List<SzMT> szmt) : base(sid)
        {
            _idopont = (DateTime)szmt[0].Minta;
        }

        protected override Exception Run()
        {
            Exception exception = null;

            using (var _context = new ossContext())
            {
                using (var tr = _context.Database.BeginTransaction())
                    try
                    {
                        var result = new RiportBll().Keszlet(_context, _sid,
                            _idopont);

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