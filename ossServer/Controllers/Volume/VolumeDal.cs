using Microsoft.EntityFrameworkCore;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;
using System.Linq;

namespace ossServer.Controllers.Volume
{
    public class VolumeDal
    {
        public static int Add(ossContext model, Models.Volume entity)
        {
            Register.Creation(model, entity);
            model.Volume.Add(entity);
            model.SaveChanges();

            return entity.Volumekod;
        }

        public static List<Models.Volume> Read(ossContext context)
        {
            return context.Volume.AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .OrderByDescending(s => s.Volumeno)
              .ToList();
        }

        public static List<Models.Volume> ReadElegSzabadHely(ossContext context, int ujFajlMerete)
        {
            var opened = KotetAllapot.Opened.ToString();

            return context.Volume
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod &&
                          s.Allapot == opened && (s.Maxmeret - s.Jelenlegimeret) > ujFajlMerete)
              .OrderByDescending(s => s.Jelenlegimeret)
              .ToList();
        }

        public static int Update(ossContext context, Models.Volume entity)
        {
            Register.Modification(context, entity);
            context.SaveChanges();

            return entity.Volumekod;
        }
    }
}
