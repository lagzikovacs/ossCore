using Microsoft.EntityFrameworkCore;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Csoport
{
    public class CsoportDal
    {
        public static void Exists(ossContext context, Models.Csoport entity)
        {
            if (context.Csoport.Any(s => s.Particiokod == entity.Particiokod && s.Csoport1 == entity.Csoport1))
                throw new Exception(string.Format(Messages.MarLetezoTetel, entity.Csoport1));
        }

        public static int Add(ossContext context, Models.Csoport entity)
        {
            Register.Creation(context, entity);
            context.Csoport.Add(entity);
            context.SaveChanges();

            return entity.Csoportkod;
        }

        public async static Task Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockcsoport", "csoportkod", pKey, utoljaraModositva);
        }

        public static Models.Csoport Get(ossContext context, int pKey)
        {
            var result = context.Csoport
              .Where(s => s.Csoportkod == pKey)
              .ToList();
            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato, $"{nameof(Models.Csoport.Csoportkod)}={pKey}"));
            return result.First();
        }

        public static void CheckReferences(ossContext context, int pKey)
        {
            var result = new Dictionary<string, int>();

            var n = context.Csoportfelhasznalo.Count(s => s.Csoportkod == pKey);
            if (n > 0)
                result.Add("CSOPORTFELHASZNALO.CSOPORTKOD", n);

            n = context.Csoportjog.Count(s => s.Csoportkod == pKey);
            if (n > 0)
                result.Add("CSOPORTJOG.CSOPORTKOD", n);

            if (result.Count > 0)
            {
                string builder = "\r\n";
                foreach (var r in result)
                    builder += "\r\n" + r.Key + " (" + r.Value + ")";

                throw new Exception(Messages.NemTorolhetoReferenciakMiatt + builder);
            }
        }

        public static void Delete(ossContext context, Models.Csoport entity)
        {
            context.Csoport.Remove(entity);
            context.SaveChanges();
        }

        public static void ExistsAnother(ossContext context, Models.Csoport entity)
        {
            if (context.Csoport.Any(s =>
              s.Particiokod == entity.Particiokod && s.Csoport1 == entity.Csoport1 && s.Csoportkod != entity.Csoportkod))
                throw new Exception(string.Format(Messages.NemMenthetoMarLetezik, entity.Csoport1));
        }

        public static int Update(ossContext context, Models.Csoport entity)
        {
            Register.Modification(context, entity);
            context.SaveChanges();

            return entity.Csoportkod;
        }

        public static List<Models.Csoport> Read(ossContext context, string maszk)
        {
            return context.Csoport.Include(r => r.ParticiokodNavigation).AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod && s.Csoport1.Contains(maszk))
              .OrderBy(s => s.Csoport1).ToList();
        }


        public static List<Models.Csoport> GetSzerepkorok(ossContext context)
        {
            var qryCsf = context.Csoportfelhasznalo
              .Where(s => s.Felhasznalokod == context.CurrentSession.Felhasznalokod)
              .Select(s => s.Csoportkod);

            return context.Csoport.Include(r => r.ParticiokodNavigation)
              .Where(s => qryCsf.Contains(s.Csoportkod))
              .OrderBy(s => s.ParticiokodNavigation.Megnevezes)
              .ThenBy(s => s.Csoport1)
              .ToList();
        }

        public static void CheckSzerepkor(ossContext context, int particioKod, int csoportKod)
        {
            var qryCsf = context.Csoportfelhasznalo
              .Where(s => s.Particiokod == particioKod)
              .Where(s => s.Csoportkod == csoportKod)
              .Where(s => s.Felhasznalokod == context.CurrentSession.Felhasznalokod);

            if (qryCsf.ToList().Count != 1)
                throw new Exception("A kiválasztott szerepkör nem létezik!");
        }

        public static int Joge(ossContext context, string jogKod)
        {
            return context.Csoportjog
              .Include(r => r.LehetsegesjogkodNavigation)
              .Where(s => s.Csoportkod == context.CurrentSession.Csoportkod)
              .Count(s => s.LehetsegesjogkodNavigation.Jogkod == jogKod);
        }

        public static void Joge(ossContext context, JogKod jogKod)
        {
            // TODO upgrade után kivenni
            if (jogKod == JogKod.BIZONYLAT)
                return;

            if (Joge(context, jogKod.ToString()) == 0)
                throw new Exception("Hm... acces denied!");
        }

        public static List<string> Jogaim(ossContext context)
        {
            return context.Csoportjog
                .Include(r => r.LehetsegesjogkodNavigation)
                .Where(s => s.Csoportkod == context.CurrentSession.Csoportkod)
                .Select(s => s.LehetsegesjogkodNavigation.Jogkod)
                .ToList();
        }

        public static List<int> SelectCsoportFelhasznalo(ossContext context, int csoportKod)
        {
            return context.Csoportfelhasznalo
              .Where(s => s.Csoportkod == csoportKod)
              .Select(s => s.Felhasznalokod)
              .ToList();
        }

        public static List<int> SelectCsoportJog(ossContext context, int csoportKod)
        {
            return context.Csoportjog
              .Where(s => s.Csoportkod == csoportKod)
              .Select(s => s.Lehetsegesjogkod)
              .ToList();
        }

        public static void CsoportFelhasznaloBe(ossContext context, int particioKod, int csoportKod, int felhasznaloKod)
        {
            var entity = new Csoportfelhasznalo
            {
                Particiokod = particioKod,
                Csoportkod = csoportKod,
                Felhasznalokod = felhasznaloKod
            };

            Register.Creation(context, entity);

            context.Csoportfelhasznalo.Add(entity);
            context.SaveChanges();
        }

        public static void CsoportFelhasznaloBe(ossContext context, int csoportKod, int felhasznaloKod)
        {
            var entity = new Csoportfelhasznalo
            {
                Csoportkod = csoportKod,
                Felhasznalokod = felhasznaloKod
            };

            Register.Creation(context, entity);

            context.Csoportfelhasznalo.Add(entity);
            context.SaveChanges();
        }

        public static void CsoportFelhasznaloKi(ossContext context, int csoportKod, int felhasznaloKod)
        {
            var lst = context.Csoportfelhasznalo
              .Where(s => s.Csoportkod == csoportKod && s.Felhasznalokod == felhasznaloKod)
              .ToList();
            if (lst.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato,
                  $"{nameof(Csoportfelhasznalo.Csoportkod)}={csoportKod}, {nameof(Csoportfelhasznalo.Felhasznalokod)}={felhasznaloKod}"));

            context.Csoportfelhasznalo.Remove(lst[0]);
            context.SaveChanges();
        }

        public static void CsoportJogBe(ossContext context, int csoportKod, int lehetsegesJogKod)
        {
            var entity = new Csoportjog
            {
                Csoportkod = csoportKod,
                Lehetsegesjogkod = lehetsegesJogKod
            };

            Register.Creation(context, entity);

            context.Csoportjog.Add(entity);
            context.SaveChanges();
        }

        public static void CsoportJogKi(ossContext context, int csoportKod, int lehetsegesJogKod)
        {
            var lst = context.Csoportjog
              .Where(s => s.Csoportkod == csoportKod && s.Lehetsegesjogkod == lehetsegesJogKod)
              .ToList();
            if (lst.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato,
                  $"{nameof(Csoportjog.Csoportkod)}={csoportKod}, {nameof(Csoportjog.Lehetsegesjogkod)}={lehetsegesJogKod}"));

            context.Csoportjog.Remove(lst[0]);
            context.SaveChanges();
        }
    }
}
