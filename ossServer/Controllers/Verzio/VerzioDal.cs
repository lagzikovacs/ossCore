using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using System;
using System.Threading.Tasks;

namespace ossServer.Controllers.Verzio
{
    public class VerzioDal
    {
        public static async Task<string> GetAsync(ossContext model)
        {
            var lst = await model.Verzio.AsNoTracking()
              .ToListAsync();
            if (lst.Count != 1)
                throw new Exception("A VERZIO táblában 'pontosan egy' rekordnak kell lennie!");

            return lst[0].Verzio1;
        }
    }
}
