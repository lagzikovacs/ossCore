using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ossServer.Controllers.Primitiv.Felhasznalo
{
    public class FelhasznaloDal
    {
        public static void Exists(ossContext model, Models.Felhasznalo entity)
        {
            if (model.Felhasznalo.Any(s => s.Azonosito == entity.Azonosito))
                throw new Exception(string.Format(Messages.MarLetezoTetel, entity.Azonosito));
        }

        public static int Add(ossContext model, Models.Felhasznalo entity)
        {
            Register.Creation(model, entity);
            model.Felhasznalo.Add(entity);
            model.SaveChanges();

            return entity.Felhasznalokod;
        }

        public async static void Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockfelhasznalo", "felhasznalokod", pKey, utoljaraModositva);
        }

        public static Models.Felhasznalo Get(ossContext model, int key)
        {
            var result = model.Felhasznalo.Where(s => s.Felhasznalokod == key).ToList();
            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato, 
                    $"{nameof(Models.Felhasznalo.Felhasznalokod)}={key}"));
            return result.First();
        }

        public static Models.Felhasznalo Get(ossContext model, string azonosito, string jelszo)
        {
            var result = model.Felhasznalo.AsNoTracking()
              .Where(s => s.Azonosito == azonosito && s.Jelszo == jelszo && s.Statusz == "OK")
              .ToList();
            if (result.Count != 1)
                throw new Exception($"Ismeretlen azonosító vagy hibás jelszó: {azonosito}!");
            return result.First();
        }

        public static List<Models.Felhasznalo> Read(ossContext model, string maszk)
        {
            return model.Felhasznalo.AsNoTracking()
              .Where(s => s.Nev.Contains(maszk)).OrderBy(s => s.Nev).ToList();
        }

        public static void CheckReferences(ossContext model, int pKey)
        {
            var result = new Dictionary<string, int>();

            var n = model.Csoportfelhasznalo.Count(s => s.Felhasznalokod == pKey);
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

        public static void Delete(ossContext model, Models.Felhasznalo entity)
        {
            model.Felhasznalo.Remove(entity);
            model.SaveChanges();
        }

        public static void ExistsAnother(ossContext model, Models.Felhasznalo entity)
        {
            if (model.Felhasznalo.Any(s => s.Azonosito == entity.Azonosito && s.Felhasznalokod != entity.Felhasznalokod))
                throw new Exception(string.Format(Messages.NemMenthetoMarLetezik, entity.Azonosito));
        }

        public static int Update(ossContext model, Models.Felhasznalo entity)
        {
            Register.Modification(model, entity);
            model.SaveChanges();

            return entity.Felhasznalokod;
        }
    }
}
