using Microsoft.EntityFrameworkCore;
using ossServer.Enums;
using ossServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ossServer.Controllers.Riport
{
    public class RiportDal
    {
        public static List<int> KimenoSzamlakBizonylatkodok(ossContext context, 
            DateTime teljesitesKeltetol, DateTime teljesitesKelteig)
        {
            var pluszEgyNap = teljesitesKelteig.AddDays(1);

            var qry = context.Bizonylat.AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Bizonylattipuskod == (int)BizonylatTipus.Szamla)
              .Where(s => s.Teljesiteskelte >= teljesitesKeltetol && s.Teljesiteskelte < pluszEgyNap)
              .OrderBy(s => s.Bizonylatszam)
              .Select(s => s.Bizonylatkod);

            var result = qry.ToList();

            var qry1 = context.Bizonylat.AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Bizonylattipuskod == (int)BizonylatTipus.ElolegSzamla)
              .Where(s => s.Teljesiteskelte >= teljesitesKeltetol && s.Teljesiteskelte < pluszEgyNap)
              .OrderBy(s => s.Bizonylatszam)
              .Select(s => s.Bizonylatkod);

            result.AddRange(qry1.ToList());

            return result;
        }

        public static List<int> BejovoSzamlakBizonylatkodok(ossContext context, 
            DateTime teljesitesKeltetol, DateTime teljesitesKelteig)
        {
            var pluszEgyNap = teljesitesKelteig.AddDays(1);

            return context.Bizonylat.AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Bizonylattipuskod == (int)BizonylatTipus.BejovoSzamla)
              .Where(s => s.Teljesiteskelte >= teljesitesKeltetol && s.Teljesiteskelte < pluszEgyNap)
              .OrderBy(s => s.Bizonylatszam)
              .Select(s => s.Bizonylatkod)
              .ToList();
        }

        public static List<Models.Bizonylat> BizonylatRiporttetelek(ossContext context, 
            List<int> bizonylatkodok)
        {
            var temp = context.Bizonylat.AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => bizonylatkodok.Contains(s.Bizonylatkod)).ToList();

            var result = new List<Models.Bizonylat>();

            //sorbarendezés
            for (var i = 0; i < bizonylatkodok.Count; i++)
                result.Add(temp.Single(s => s.Bizonylatkod == bizonylatkodok[i]));

            return result;
        }


        public static List<int> KovetelesekBizonylatkodok(ossContext context, 
            DateTime ezenANapon, bool lejart)
        {
            var ezenANaponPluszEgyNap = ezenANapon.AddDays(1); // Az sql-es < reláció miatt (óra/perc/sec belekalkulálása)

            var qry = context.Bizonylat.AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Bizonylattipuskod == (int)BizonylatTipus.DijBekero ||
                          s.Bizonylattipuskod == (int)BizonylatTipus.ElolegSzamla ||
                          s.Bizonylattipuskod == (int)BizonylatTipus.Szamla)
              .Where(s => s.Fizetesimod != "Készpénz" &&
                          s.Ezstornozo == false &&
                          s.Ezstornozott == false &&
                          s.Bizonylatkelte < ezenANaponPluszEgyNap &&
                          s.Kifizetesrendben != true)
              .Where(s =>
                s.Kifizetes.Count(k => k.Datum < ezenANaponPluszEgyNap) == 0 || //ha nincs kifizetés, nem értékeli ki tovább
                s.Kifizetes.Where(k => k.Datum < ezenANaponPluszEgyNap).Sum(k => k.Osszeg) != s.Brutto);

            if (lejart)
                qry = qry.Where(s =>
                  s.Fizetesihatarido < ezenANapon); // Itt direkt a kapott dátummal dolgozunk (nem az egy nappal korábbival)

            qry = qry.OrderBy(s => s.Bizonylatszam);

            return qry.Select(s => s.Bizonylatkod).ToList();
        }

        public static List<int> TartozasokBizonylatkodok(ossContext context, 
            DateTime ezenANapon, bool lejart)
        {
            var ezenANaponPluszEgyNap = ezenANapon.AddDays(1); // Az sql-es < reláció miatt (óra/perc/sec belekalkulálása)

            var qry = context.Bizonylat.AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Bizonylattipuskod == (int)BizonylatTipus.BejovoSzamla)
              .Where(s => (s.Fizetesimod == "Átutalás" || s.Fizetesimod == "Előreutalás") &&
                          s.Bizonylatkelte < ezenANaponPluszEgyNap &&
                          s.Kifizetesrendben != true)
              .Where(s => s.Kifizetes.Count(k => k.Datum < ezenANaponPluszEgyNap) == 0 ||
                          s.Kifizetes.Where(k => k.Datum < ezenANaponPluszEgyNap).Sum(k => k.Osszeg) != s.Brutto);

            if (lejart)
                qry = qry.Where(s =>
                  s.Fizetesihatarido < ezenANapon); // Itt direkt a kapott dátummal dolgozunk (nem az egy nappal korábbival)

            qry = qry.OrderBy(s => s.Fizetesihatarido).ThenBy(s => s.Ugyfelnev);

            return qry.Select(s => s.Bizonylatkod).ToList();
        }

        public static List<KovetelesTartozasRiporttetelDto> KovetelesekTartozasokRiporttetelek(ossContext context,
            List<int> bizonylatkodok, DateTime ezenANapon)
        {
            var temp = context.Bizonylat.AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => bizonylatkodok.Contains(s.Bizonylatkod))
              .Select(s => new KovetelesTartozasRiporttetelDto
              {
                  Bizonylatkod = s.Bizonylatkod,
                  Afa = s.Afa,
                  Bizonylatkelte = s.Bizonylatkelte,
                  Bizonylatszam = s.Bizonylatszam,
                  Brutto = s.Brutto,
                  Fizetesihatarido = s.Fizetesihatarido,
                  Fizetesimod = s.Fizetesimod,
                  Netto = s.Netto,
                  Penznem = s.Penznem,
                  Teljesiteskelte = s.Teljesiteskelte,
                  Ugyfelnev = s.Ugyfelnev,
                  Arfolyam = s.Arfolyam
              })
              .ToList();

            var result = new List<KovetelesTartozasRiporttetelDto>();

            var ezenANaponPluszEgyNap = ezenANapon.AddDays(1); // Az sql-es < reláció miatt (óra/perc/sec belekalkulálása)

            for (var i = 0; i < bizonylatkodok.Count; ++i)
            {
                var bizonylatkod = bizonylatkodok[i];
                var dto = temp.Single(s => s.Bizonylatkod == bizonylatkod);

                var qry = context.Kifizetes.AsNoTracking()
                  .Where(s => s.Bizonylatkod == bizonylatkod & s.Datum < ezenANaponPluszEgyNap)
                  .Select(s => s.Osszeg);

                dto.Megfizetve = qry.Any() ? qry.Sum() : 0;

                result.Add(dto);
            }

            return result;
        }

        public static List<BeszerzesRiporttetelDto> BeszerzesRiporttetelek(ossContext context,
          DateTime teljesitesKelteTol, DateTime teljesitesKelteIg)
        {
            teljesitesKelteIg = teljesitesKelteIg.AddDays(1);

            return context.Bizonylattetel.AsNoTracking()
              .Include(r => r.BizonylatkodNavigation)
              .Where(s => s.BizonylatkodNavigation.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.BizonylatkodNavigation.Bizonylattipuskod == (int)BizonylatTipus.BejovoSzamla)
              .Where(s => s.BizonylatkodNavigation.Teljesiteskelte >= teljesitesKelteTol &&
                          s.BizonylatkodNavigation.Teljesiteskelte < teljesitesKelteIg)
              .GroupBy(s => new { s.Megnevezes, s.Me })
              .Select(t => new BeszerzesRiporttetelDto
              {
                  Megnevezes = t.Key.Megnevezes,
                  Mennyiseg = t.Sum(u => u.Mennyiseg),
                  Me = t.Key.Me,
                  Nettoft = t.Sum(u => u.Netto * u.BizonylatkodNavigation.Arfolyam),
                  BizonylatFej = t.Select(u => new BeszerzesRiporttetelBizonylatDto
                  {
                      Bizonylatszam = u.BizonylatkodNavigation.Bizonylatszam,
                      Bizonylatkelte = u.BizonylatkodNavigation.Bizonylatkelte,
                      Teljesiteskelte = u.BizonylatkodNavigation.Teljesiteskelte,
                      Ugyfelnev = u.BizonylatkodNavigation.Ugyfelnev
                  }).OrderBy(o => o.Teljesiteskelte)
              })
              .ToList();
        }

        public static List<KeszletDto> KeszletErtekNelkul(ossContext context, DateTime ezenIdopontig)
        {
            ezenIdopontig = ezenIdopontig.AddDays(1);

            var qry = context.Bizonylattetel.AsNoTracking()
              .Include(r => r.BizonylatkodNavigation)
              .Include(r => r.CikkkodNavigation).ThenInclude(r => r.MekodNavigation)
              .Where(s => s.BizonylatkodNavigation.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.CikkkodNavigation.Keszletetkepez)
              .Where(s => s.BizonylatkodNavigation.Bizonylattipuskod == (int)BizonylatTipus.Szallito ||
                          s.BizonylatkodNavigation.Bizonylattipuskod == (int)BizonylatTipus.BejovoSzamla)
              .Where(s => s.BizonylatkodNavigation.Teljesiteskelte < ezenIdopontig)
              .GroupBy(s => new { s.Cikkkod, s.CikkkodNavigation.Megnevezes, s.CikkkodNavigation.MekodNavigation.Me })
              .Select(t => new KeszletDto
              {
                  Cikkkod = t.Key.Cikkkod,
                  Cikk = t.Key.Megnevezes,
                  Me = t.Key.Me,
                  Keszlet = t.Sum(s => s.BizonylatkodNavigation.Bizonylattipuskod == (int)BizonylatTipus.BejovoSzamla ? s.Mennyiseg : 0) -
                          t.Sum(s => s.BizonylatkodNavigation.Bizonylattipuskod == (int)BizonylatTipus.Szallito ? s.Mennyiseg : 0)
              });

            return qry.ToList().Where(s => s.Keszlet != 0).OrderBy(s => s.Cikk).ToList();
        }

        //a készlet az utoljára beérkezett tételekből áll
        public static void KeszletErteke(ossContext context, KeszletDto dto, DateTime ezenIdopontig)
        {
            ezenIdopontig = ezenIdopontig.AddDays(1);

            var rekordTol = 0;
            const int lapMeret = 10;

            var qry = context.Bizonylattetel.AsNoTracking()
              .Include(r => r.BizonylatkodNavigation)
              .Where(s => s.BizonylatkodNavigation.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.BizonylatkodNavigation.Bizonylattipuskod == (int)BizonylatTipus.BejovoSzamla)
              .Where(s => s.Cikkkod == dto.Cikkkod && s.BizonylatkodNavigation.Teljesiteskelte < ezenIdopontig)
              .OrderByDescending(s => s.BizonylatkodNavigation.Teljesiteskelte)
              .ThenByDescending(s => s.Bizonylattetelkod)
              .Select(s => new
              {
                  s.BizonylatkodNavigation.Teljesiteskelte,
                  s.BizonylatkodNavigation.Penznem,
                  s.BizonylatkodNavigation.Arfolyam,
                  s.Mennyiseg,
                  s.Egysegar
              });

            var osszes = qry.Count();
            if (osszes > 0)
            {
                var levonando = dto.Keszlet;

                while (rekordTol < osszes && levonando > 0)
                {
                    var lst = qry.Skip(rekordTol).Take(lapMeret).ToList();

                    if (rekordTol == 0)
                    {
                        dto.Utolsobevet = lst[0].Teljesiteskelte;
                        dto.Utolsoar = lst[0].Egysegar;
                        dto.Utolsoarpenzneme = lst[0].Penznem;
                        dto.Utolsoarforintban = lst[0].Egysegar * lst[0].Arfolyam;
                        dto.Beszerzesekszama = osszes;
                    }

                    for (var i = 0; i < lst.Count; i++)
                    {
                        var levonhato = levonando;
                        if (lst[i].Mennyiseg < levonhato)
                            levonhato = lst[i].Mennyiseg;
                        levonando -= levonhato;

                        dto.Keszletertek += levonhato * lst[i].Egysegar * lst[i].Arfolyam;

                        if (levonando == 0)
                            break;
                    }

                    rekordTol += lapMeret;
                }
            }
        }

        public static List<Models.Penztartetel> PenztarTetel(ossContext context, int penztarKod, DateTime datumTol,
          DateTime datumIg)
        {
            return context.Penztartetel.AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Penztarkod == penztarKod && s.Datum >= datumTol && s.Datum <= datumIg)
              .OrderByDescending(s => s.Penztarbizonylatszam)
              .ToList();
        }

        public static List<Models.Projekt> Projekt(ossContext context, int statusz)
        {
            var qry = context.Projekt.AsNoTracking()
                .Include(r => r.UgyfelkodNavigation).ThenInclude(r => r.HelysegkodNavigation)
                .Where(s => s.Particiokod == context.CurrentSession.Particiokod);

            if (statusz != 0)
                qry = qry.Where(s => s.Statusz == statusz);

            return qry.OrderByDescending(s => s.Projektkod).ToList();
        }
    }
}
