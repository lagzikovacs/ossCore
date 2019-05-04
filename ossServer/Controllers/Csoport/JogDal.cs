using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using System.Collections.Generic;
using System.Linq;

namespace ossServer.Controllers.Csoport
{
    public class JogDal
    {
        public static List<Models.Lehetsegesjog> Read(ossContext context, string maszk)
        {
            return context.Lehetsegesjog.AsNoTracking()
              .Where(s => s.Jog.Contains(maszk))
              .OrderBy(s => s.Jog)
              .ToList();
        }
    }
}
