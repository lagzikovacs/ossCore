using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ossServer.Controllers.ProjektKapcsolat
{
    public class ProjektKapcsolatDal
    {
        public static List<Models.Projektkapcsolat> ReadByProjektKod(ossContext context, int projektKod)
        {
            return context.Projektkapcsolat
                .Include(r => r.IratkodNavigation).ThenInclude(r => r.IrattipuskodNavigation)
                .Include(r => r.BizonylatkodNavigation)
                .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
                .Where(s => s.Projektkod == projektKod)
                .OrderByDescending(s => s.Projektkapcsolatkod).ToList();
        }

        public static List<Models.Projektkapcsolat> ReadByBizonylatKod(ossContext context, int bizonylatKod)
        {
            return context.Projektkapcsolat
                .Include(r => r.IratkodNavigation).ThenInclude(r => r.IrattipuskodNavigation)
                .Include(r => r.BizonylatkodNavigation)
                .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
                .Where(s => s.Bizonylatkod == bizonylatKod)
                .OrderByDescending(s => s.Projektkapcsolatkod).ToList();
        }

        public static List<Models.Projektkapcsolat> ReadByIratKod(ossContext context, int iratKod)
        {
            return context.Projektkapcsolat
                .Include(r => r.IratkodNavigation).ThenInclude(r => r.IrattipuskodNavigation)
                .Include(r => r.BizonylatkodNavigation)
                .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
                .Where(s => s.Iratkod == iratKod)
                .OrderByDescending(s => s.Projektkapcsolatkod).ToList();
        }

        public static int Add(ossContext context, Models.Projektkapcsolat entity)
        {
            Register.Creation(context, entity);
            context.Projektkapcsolat.Add(entity);
            context.SaveChanges();

            return entity.Projektkapcsolatkod;
        }

        public static Models.Projektkapcsolat Get(ossContext context, int pKey)
        {
            var result = context.Projektkapcsolat
                .Include(r => r.IratkodNavigation).ThenInclude(r => r.IrattipuskodNavigation)
                .Include(r => r.BizonylatkodNavigation)
                .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
                .Where(s => s.Projektkapcsolatkod == pKey)
                .ToList();
            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato,
                  $"{nameof(Projektkapcsolat.Projektkapcsolatkod)}={pKey}"));
            return result.First();
        }

        public static void Delete(ossContext context, Models.Projektkapcsolat entity)
        {
            context.Projektkapcsolat.Remove(entity);
            context.SaveChanges();
        }
    }
}
