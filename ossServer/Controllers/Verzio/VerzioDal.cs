using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using System;
using System.Linq;

namespace ossServer.Controllers.Verzio
{
    public class VerzioDal
    {
        public static string Get(ossContext model)
        {
            var lst = model.Verzio.AsNoTracking()
              .ToList();
            if (lst.Count != 1)
                throw new Exception("A VERZIO táblában 'pontosan egy' rekordnak kell lennie!");

            return lst[0].Verzio1;
        }
    }
}
