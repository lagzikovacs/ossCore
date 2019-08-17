using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Particio
{
    public class ParticioDal
    {
        public static async Task<Models.Particio> GetAsync(ossContext contex)
        {
            var result = await contex.Particio
                .Where(s => s.Particiokod == contex.CurrentSession.Particiokod)
                .ToListAsync();

            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato, $"{nameof(Models.Particio.Particiokod)}={contex.CurrentSession.Particiokod}"));

            return result.First();
        }

        //csak a szerepkör kiválasztáshoz kell
        //ott kerül be a particiokod a model.Session-be
        public static Models.Particio Get(ossContext context, int key)
        {
            var result = context.Particio.Where(s => s.Particiokod == key).ToList();
            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato, $"{nameof(Models.Particio.Particiokod)}={key}"));
            return result.First();
        }

        public async static Task Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockparticio", "particiokod", pKey, utoljaraModositva);
        }

        public static int Update(ossContext model, Models.Particio entity)
        {
            Register.Modification(model, entity);
            model.SaveChanges();

            return entity.Particiokod;
        }
    }
}
