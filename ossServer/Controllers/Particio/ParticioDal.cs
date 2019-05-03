using ossServer.Models;
using ossServer.Utils;
using System;
using System.Linq;

namespace ossServer.Controllers.Particio
{
    public class ParticioDal
    {
        public static Models.Particio Get(ossContext contex)
        {
            var result = contex.Particio.Where(s => s.Particiokod == contex.CurrentSession.Particiokod).ToList();
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
    }
}
