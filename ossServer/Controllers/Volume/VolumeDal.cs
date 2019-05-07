using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using System.Collections.Generic;
using System.Linq;

namespace ossServer.Controllers.Volume
{
    public class VolumeDal
    {
        internal static List<Models.Volume> Read(ossContext context)
        {
            return context.Volume.AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .OrderByDescending(s => s.Volumeno)
              .ToList();
        }

        internal static List<int> DokumentumkodByVolume(ossContext context, int volumeKod)
        {
            return context.Dokumentum.AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod && s.Volumekod == volumeKod)
              .OrderBy(s => s.Dokumentumkod)
              .Select(s => s.Dokumentumkod)
              .ToList();
        }
    }
}
