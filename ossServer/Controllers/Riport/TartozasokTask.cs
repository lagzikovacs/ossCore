using ossServer.Models;
using ossServer.Tasks;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Riport
{
    public class TartozasokTask : ServerTaskBase
    {
        private readonly DateTime _ezenANapon;
        private readonly bool _lejart;

        public TartozasokTask(string sid, List<SzMT> szmt) : base(sid)
        {
            _ezenANapon = (DateTime)szmt[0].Minta;
            _lejart = false;
        }

        protected override Exception Run()
        {
            Exception exception = null;

            using (var _context = new ossContext())
            {
                using (var tr = _context.Database.BeginTransaction())
                    try
                    {
                        var result = new RiportBll().Tartozasok(_context, _sid,
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

            return exception;
        }
    }
}
