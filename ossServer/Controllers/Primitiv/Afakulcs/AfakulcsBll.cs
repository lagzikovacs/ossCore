using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    //    if (sid == null)
                    //        throw new ArgumentNullException(nameof(sid));

                    var entities = AfakulcsDal.Read(context, maszk);
                    result.Result = ObjectUtils.Convert<Models.Afakulcs, AfakulcsDto>(entities);

                    //result.Result = new List<AfakulcsDto>();
                    //foreach (var e in entities)
                    //    result.Result.Add(ObjectUtils.Convert<Models.Afakulcs, AfakulcsDto>(e));

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
