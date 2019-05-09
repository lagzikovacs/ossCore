using ossServer.Controllers.Session;
using ossServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ossServer.Controllers.Esemenynaplo
{
    public class EsemenynaploBll
    {
        public static void Bejegyzes(ossContext model, EsemenynaploBejegyzesek esemeny)
        {
            var entity = new Models.Esemenynaplo
            {
                Esemenyazonosito = (int)esemeny,
                Idopont = DateTime.Now,
                Particiokod = model.CurrentSession.Particiokod,
                Particio = model.CurrentSession.Particio,
                Csoportkod = model.CurrentSession.Csoportkod,
                Csoport = model.CurrentSession.Csoport,
                Felhasznalokod = model.CurrentSession.Felhasznalokod,
                Felhasznalo = model.CurrentSession.Felhasznalo,
                Azonosito = model.CurrentSession.Azonosito,
                Ip = model.CurrentSession.Ip,
                Host = model.CurrentSession.Host,
                Osuser = model.CurrentSession.Osuser
            };

            EsemenynaploDal.Add(model, entity);
        }

        public static List<EseBeJel> EseBeJel()
        {
            return new List<EseBeJel>
            {
                new EseBeJel(EsemenynaploBejegyzesek.Bejelentkezes, "Bejelentkezés"),
                new EseBeJel(EsemenynaploBejegyzesek.Kijelentkezes, "Kijelentkezés"),
                new EseBeJel(EsemenynaploBejegyzesek.SzerepkorValasztas, "Szerepkörválasztás"),
             };
        }

        public static string Jelentes(List<EseBeJel> eseBeJel, EsemenynaploBejegyzesek esemeny)
        {
            var e = eseBeJel.Where(s => s.Bejegyzes == esemeny).ToList();
            if (e.Count == 0)
                throw new Exception($"Ismeretlen eseménynapló bejegyzés: {esemeny}!");

            return e[0].Jelentes;
        }

        public static List<EsemenynaploDto> Select(ossContext context, string sid, int rekordTol, int lapMeret,
            int felhasznalokod, out int osszesRekord)
        {
            SessionBll.Check(context, sid);

            var qry = EsemenynaploDal.GetQuery(context, felhasznalokod);
            osszesRekord = qry.Count();
            var entities = qry.Skip(rekordTol).Take(lapMeret).ToList();
            var result = new List<EsemenynaploDto>();

            var esebejel = EseBeJel();

            foreach (var entity in entities)
            {
                var dto = new EsemenynaploDto
                {
                    Esemenynaplokod = entity.Esemenynaplokod,
                    Idopont = entity.Idopont,
                    Particio = entity.Particio,
                    Csoport = entity.Csoport,
                    Muvelet = Jelentes(esebejel, (EsemenynaploBejegyzesek)entity.Esemenyazonosito)
                };

                result.Add(dto);
            }

            return result;
        }
    }
}
