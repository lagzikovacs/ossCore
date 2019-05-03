using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using System.Collections.Generic;
using System.Linq;

namespace ossServer.Controllers.Primitiv.Afakulcs
{
    public class AfakulcsDal
    {
        public static List<Models.Afakulcs> Read(ossContext context, string maszk)
        {
            return context.Afakulcs.AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Afakulcs1.Contains(maszk))
              .OrderBy(s => s.Afakulcs1)
              .ToList();
        }
    }
}
