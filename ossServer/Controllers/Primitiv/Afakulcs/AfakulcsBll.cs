using ossServer.Controllers.Session;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ossServer.Controllers.Primitiv.Afakulcs
{
    public class AfakulcsBll
    {
        public static List<AfakulcsDto> Read(ossContext context, string sid, string maszk)
        {
            SessionBll.Check(context, sid);

            var entities = AfakulcsDal.Read(context, maszk);
            return ObjectUtils.Convert<Models.Afakulcs, AfakulcsDto>(entities);
        }
    }
}
