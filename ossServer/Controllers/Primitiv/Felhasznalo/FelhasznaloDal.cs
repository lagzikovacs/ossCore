using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Primitiv.Felhasznalo
{
    public class FelhasznaloDal
    {
        public static async Task ExistsAsync(ossContext model, Models.Felhasznalo entity)
        {
            if (await model.Felhasznalo.AnyAsync(s => s.Azonosito == entity.Azonosito))
                throw new Exception(string.Format(Messages.MarLetezoTetel, entity.Azonosito));
        }

        public static async Task<int> AddAsync(ossContext model, Models.Felhasznalo entity)
        {
            Register.Creation(model, entity);
            await model.Felhasznalo.AddAsync(entity);
            await model.SaveChangesAsync();

            return entity.Felhasznalokod;
        }

        public async static Task Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockfelhasznalo", "felhasznalokod", pKey, utoljaraModositva);
        }

        public static async Task<Models.Felhasznalo> GetAsync(ossContext model, int key)
        {
            var result = await model.Felhasznalo.Where(s => s.Felhasznalokod == key).ToListAsync();
            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato, 
                    $"{nameof(Models.Felhasznalo.Felhasznalokod)}={key}"));
            return result.First();
        }

        public static async Task<Models.Felhasznalo> GetAsync(ossContext model, string azonosito, string jelszo)
        {
            var result = await model.Felhasznalo.AsNoTracking()
              .Where(s => s.Azonosito == azonosito && s.Jelszo == jelszo && s.Statusz == "OK")
              .ToListAsync();

            if (result.Count != 1)
                throw new Exception($"Ismeretlen azonosító vagy hibás jelszó: {azonosito}!");

            return result.First();
        }

        public static async Task<List<Models.Felhasznalo>> ReadAsync(ossContext model, string maszk)
        {
            return await model.Felhasznalo.AsNoTracking()
              .Where(s => s.Nev.Contains(maszk)).OrderBy(s => s.Nev).ToListAsync();
        }

        public static async Task CheckReferencesAsync(ossContext model, int pKey)
        {
            var result = new Dictionary<string, int>();

            var n = await model.Csoportfelhasznalo.CountAsync(s => s.Felhasznalokod == pKey);
            if (n > 0)
                result.Add("CSOPORTFELHASZNALO.FELHASZNALOKOD", n);

            if (result.Count > 0)
            {
                string builder = "\r\n";
                foreach (var r in result)
                    builder += "\r\n" + r.Key + " (" + r.Value + ")";

                throw new Exception(Messages.NemTorolhetoReferenciakMiatt + builder);
            }
        }

        public static async Task DeleteAsync(ossContext model, Models.Felhasznalo entity)
        {
            model.Felhasznalo.Remove(entity);
            await model.SaveChangesAsync();
        }

        public static async Task ExistsAnotherAsync(ossContext model, Models.Felhasznalo entity)
        {
            if (await model.Felhasznalo.AnyAsync(s => s.Azonosito == entity.Azonosito && s.Felhasznalokod != entity.Felhasznalokod))
                throw new Exception(string.Format(Messages.NemMenthetoMarLetezik, entity.Azonosito));
        }

        public static async Task<int> UpdateAsync(ossContext model, Models.Felhasznalo entity)
        {
            Register.Modification(model, entity);
            await model.SaveChangesAsync();

            return entity.Felhasznalokod;
        }
    }
}
