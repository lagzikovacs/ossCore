using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.PenztarTetel
{
    public class PenztarTetelBll
    {
        public static async System.Threading.Tasks.Task<PenztarTetelDto> CreateNewAsync(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PENZTAR);

            return new PenztarTetelDto { Jogcim = "Pénzfelvét bankból" };
        }

        public static async System.Threading.Tasks.Task<int> AddAsync(ossContext context, string sid, PenztarTetelDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PENZTAR);

            dto.Penztarbizonylatszam =
                context.KodGen(KodNev.Penztar.ToString() + dto.Penztarkod).ToString("00000") + "/" +
                dto.Penztarkod;

            var entity = ObjectUtils.Convert<PenztarTetelDto, Models.Penztartetel>(dto);
            return PenztarTetelDal.Add(context, entity);
        }

        public static async System.Threading.Tasks.Task<PenztarTetelDto> GetAsync(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PENZTAR);

            var entity = PenztarTetelDal.Get(context, key);
            return ObjectUtils.Convert<Models.Penztartetel, PenztarTetelDto>(entity);
        }

        public static async Task<Tuple<List<PenztarTetelDto>, int>> SelectAsync(ossContext context, string sid, int rekordTol, 
            int lapMeret, List<SzMT> szmt)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PENZTAR);

            var qry = PenztarTetelDal.GetQuery(context, szmt);
            var osszesRekord = qry.Count();
            var entities = qry.Skip(rekordTol).Take(lapMeret).ToList();
            return new Tuple<List<PenztarTetelDto>, int>(ObjectUtils.Convert<Models.Penztartetel, PenztarTetelDto>(entities), osszesRekord);
        }

        public static List<ColumnSettings> GridColumns()
        {
            return new List<ColumnSettings>
            {
                new ColumnSettings {Name="Penztarbizonylatszam", Title = "Pénztárbizonylatszám", Type = ColumnType.STRING },
                new ColumnSettings {Name="Datum", Title = "Dátum", Type = ColumnType.DATE },
                new ColumnSettings {Name="Jogcim", Title = "Jogcím", Type = ColumnType.STRING },
                new ColumnSettings {Name="Ugyfelnev", Title = "Ügyfél", Type = ColumnType.STRING },
                new ColumnSettings {Name="Bizonylatszam", Title = "Bizonylatszám", Type = ColumnType.STRING },
                new ColumnSettings {Name="Bevetel", Title = "Bevétel", Type = ColumnType.NUMBER },
                new ColumnSettings {Name="Kiadas", Title = "Kiadás", Type = ColumnType.NUMBER },
                new ColumnSettings {Name="Megjegyzes", Title = "Megjegyzés", Type = ColumnType.STRING },
            };
        }

        public static List<ColumnSettings> ReszletekColumns()
        {
            return ColumnSettingsUtil.AddIdobelyeg(GridColumns());
        }

        public static List<ColumnSettings> GridSettings(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);

            return GridColumns();
        }

        public static List<ColumnSettings> ReszletekSettings(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);

            return ReszletekColumns();
        }
    }
}
