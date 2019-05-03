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
              .Where(s => s.Azonosito == azonosito && s.Jelszo == jelszo)
              .ToList();
            if (result.Count != 1)
                throw new Exception($"Ismeretlen azonosító vagy hibás jelszó: {azonosito}!");
            return result.First();
        }

        public static List<FELHASZNALO> Read(ossContext model, string maszk)
        {
            return model.FELHASZNALO.AsNoTracking()
              .Where(s => s.NEV.Contains(maszk)).OrderBy(s => s.NEV).ToList();
        }

        public static void Exists(ossContext model, FELHASZNALO entity)
        {
            if (model.FELHASZNALO.Any(s => s.AZONOSITO == entity.AZONOSITO))
                throw new Exception(string.Format(Messages.MarLetezoTetel, entity.AZONOSITO));
        }

        public static int Add(ossContext model, FELHASZNALO entity)
        {
            Register.Creation(model, entity);
            model.FELHASZNALO.Add(entity);
            model.SaveChanges();

            return entity.FELHASZNALOKOD;
        }

        public static void Lock(ossContext model, int pKey, DateTime utoljaraModositva)
        {
            if (!model.LockFELHASZNALO(pKey, utoljaraModositva))
                throw new Exception(Messages.AdatMegvaltozottNemLehetModositani);
        }

        public static void CheckReferences(ossContext model, int pKey)
        {
            var result = new Dictionary<string, int>();

            var n = model.CSOPORTFELHASZNALO.Count(s => s.FELHASZNALOKOD == pKey);
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

        public static void Delete(ossContext model, FELHASZNALO entity)
        {
            model.FELHASZNALO.Remove(entity);
            model.SaveChanges();
        }

        public static void ExistsAnother(ossContext model, FELHASZNALO entity)
        {
            if (model.FELHASZNALO.Any(s => s.AZONOSITO == entity.AZONOSITO && s.FELHASZNALOKOD != entity.FELHASZNALOKOD))
                throw new Exception(string.Format(Messages.NemMenthetoMarLetezik, entity.AZONOSITO));
        }

        public static int Update(ossContext model, FELHASZNALO entity)
        {
            Register.Modification(model, entity);
            model.SaveChanges();

            return entity.FELHASZNALOKOD;
        }
    }
}
