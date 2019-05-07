using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ossServer.Controllers.Dokumentum
{
    public class DokumentumDal
    {
        public static int Add(ossContext context, Models.Dokumentum entity)
        {
            Register.Creation(context, entity);
            context.Dokumentum.Add(entity);
            context.SaveChanges();

            return entity.Dokumentumkod;
        }

        public static Models.Dokumentum Get(ossContext context, int key)
        {
            var result = context.Dokumentum
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Dokumentumkod == key)
              .ToList();
            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato, 
                    $"{nameof(Models.Dokumentum.Dokumentumkod)}={key}"));
            return result.First();
        }

        public static Models.Dokumentum GetWithVolume(ossContext context, int key)
        {
            var result = context.Dokumentum.Include(r => r.VolumekodNavigation)
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Dokumentumkod == key)
              .ToList();
            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato, 
                    $"{nameof(Models.Dokumentum.Dokumentumkod)}={key}"));
            return result.First();
        }

        public static List<int> DokumentumkodByVolume(ossContext context, int volumeKod)
        {
            return context.Dokumentum.AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod && s.Volumekod == volumeKod)
              .OrderBy(s => s.Dokumentumkod)
              .Select(s => s.Dokumentumkod)
              .ToList();
        }

        public static List<Models.Dokumentum> Select(ossContext context, int iratKod)
        {
            return context.Dokumentum
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Iratkod == iratKod)
              .OrderByDescending(s => s.Dokumentumkod)
              .ToList();
        }

        public async static void Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockdokumentum", "dokumentumkod", pKey, utoljaraModositva);
        }

        public static void Delete(ossContext context, Models.Dokumentum entity)
        {
            context.Dokumentum.Remove(entity);
            context.SaveChanges();
        }
    }
}
