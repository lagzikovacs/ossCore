using ossServer.Controllers.Session;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Threading.Tasks;

namespace ossServer.Controllers.Primitiv.Afakulcs
{
    public class AfakulcsBll
    {
        public static async Task<AfaKulcsResult> Read(ossContext context, string sid, string maszk)
        {
            var result = new AfaKulcsResult();

            using (var tr = await context.Database.BeginTransactionAsync())
                try
                {
                    SessionBll.Check(context, sid);

                    var entities = AfakulcsDal.Read(context, maszk);
                    result.Result = ObjectUtils.Convert<Models.Afakulcs, AfakulcsDto>(entities);

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
