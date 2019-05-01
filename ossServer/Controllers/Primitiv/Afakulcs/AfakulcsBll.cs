using ossServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Primitiv.Afakulcs
{
    public class AfakulcsBll
    {
        private readonly ossContext _context;
        private readonly string _sid;

        public AfakulcsBll(ossContext context, string sid)
        {
            _context = context;
            _sid = sid;
        }

        public async Task<AfaKulcsResult> Read(string maszk)
        {
            var result = new AfaKulcsResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    //    if (sid == null)
                    //        throw new ArgumentNullException(nameof(sid));

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.Message;
                }

            return result;
        }
    }
}
