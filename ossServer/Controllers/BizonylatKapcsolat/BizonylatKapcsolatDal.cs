using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ossServer.Controllers.BizonylatKapcsolat
{
    public class BizonylatKapcsolatDal
    {
        internal static List<Models.Bizonylatkapcsolat> Select(ossContext context, int bizonylatKod)
        {
            return context.Bizonylatkapcsolat
                .Include(r => r.IratkodNavigation).ThenInclude(r => r.IrattipuskodNavigation)
                .Where(s => s.Bizonylatkod == bizonylatKod)
                .OrderByDescending(s => s.Bizonylatkapcsolatkod).ToList();
        }

        internal static int Add(ossContext context, Models.Bizonylatkapcsolat entity)
        {
            Register.Creation(context, entity);
            context.Bizonylatkapcsolat.Add(entity);
            context.SaveChanges();

            return entity.Bizonylatkapcsolatkod;
        }

        internal static Models.Bizonylatkapcsolat Get(ossContext context, int pKey)
        {
            var result = context.Bizonylatkapcsolat
                .Include(r => r.IratkodNavigation).ThenInclude(r => r.IrattipuskodNavigation)
                .Where(s => s.Bizonylatkapcsolatkod == pKey)
                .ToList();
            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato,
                  $"{nameof(Models.Bizonylatkapcsolat.Bizonylatkapcsolatkod)}={pKey}"));
            return result.First();
        }

        internal static void Delete(ossContext context, Models.Bizonylatkapcsolat entity)
        {
            context.Bizonylatkapcsolat.Remove(entity);
            context.SaveChanges();
        }
    }
}
