using ossServer.Controllers.Csoport;
using ossServer.Controllers.Dokumentum;
using ossServer.Controllers.Session;
using ossServer.Controllers.Ugyfel;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Irat
{
    public class IratBll
    {
        public static async Task<int> AddAsync(ossContext context, string sid, IratDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.IRAT);

            var entity = ObjectUtils.Convert<IratDto, Models.Irat>(dto);
            return await IratDal.AddAsync(context, entity);
        }

        public static async Task<IratDto> CreateNewAsync(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.IRAT);

            return new IratDto { Keletkezett = DateTime.Now.Date, Irany = "Belső" };
        }

        public static async Task DeleteAsync(ossContext context, string sid, IratDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.IRATMOD);

            await IratDal.Lock(context, dto.Iratkod, dto.Modositva);
            await IratDal.CheckReferencesAsync(context, dto.Iratkod);
            var entity = await IratDal.GetAsync(context, dto.Iratkod);
            await IratDal.DeleteAsync(context, entity);
        }

        private static IratDto Calc(Models.Irat entity)
        {
            var result = ObjectUtils.Convert<Models.Irat, IratDto>(entity);

            result.Irattipus = entity.IrattipuskodNavigation.Irattipus1;
            if (entity.UgyfelkodNavigation != null)
            {
                result.Ugyfelnev = entity.UgyfelkodNavigation.Nev;
                result.Ugyfelcim = UgyfelBll.Cim(entity.UgyfelkodNavigation);
            }

            return result;
        }

        public static async Task<IratDto> GetAsync(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.IRAT);

            var entity = await IratDal.GetAsync(context, key);
            return Calc(entity);
        }

        public static async Task<Tuple<List<IratDto>, int>> SelectAsync(ossContext context, string sid, int rekordTol, int lapMeret, 
            List<SzMT> szmt)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.IRAT);

            var qry = IratDal.GetQuery(context, szmt);
            var osszesRekord = qry.Count();
            var entities = qry.Skip(rekordTol).Take(lapMeret).ToList();

            var result = new List<IratDto>();
            foreach (var entity in entities)
                result.Add(Calc(entity));

            return new Tuple<List<IratDto>, int>(result, osszesRekord);
        }

        public static async Task<int> UpdateAsync(ossContext context, string sid, IratDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.IRATMOD);

            await IratDal.Lock(context, dto.Iratkod, dto.Modositva);
            var entity = await IratDal.GetAsync(context, dto.Iratkod);
            ObjectUtils.Update(dto, entity);
            return await IratDal.UpdateAsync(context, entity);
        }


        //sql tranzakcióban működik, kis fájlok legyenek
        public static async Task<FajlBuf> LetoltesAsync(ossContext context, string sid, int iratKod)
        {
            await IratDal.GetAsync(context, iratKod);
            var lstDokumentum = await DokumentumDal.SelectAsync(context, iratKod);
            if (lstDokumentum.Count != 1)
                throw new Exception("Nincs pontosan egy dokumentum!");

            var entityDokumentum = await DokumentumBll.LetoltesAsync(context, sid, lstDokumentum[0].Dokumentumkod);
            var fb = DokumentumBll.LetoltesFajl(entityDokumentum, 0, lstDokumentum[0].Meret);

            return fb;
        }

        public static List<ColumnSettings> GridColumns()
        {
            return new List<ColumnSettings>
            {
                new ColumnSettings {Name="Iratkod", Title = "Id", Type = ColumnType.INT },
                new ColumnSettings {Name="Irany", Title = "Irány", Type = ColumnType.STRING },
                new ColumnSettings {Name="Keletkezett", Title = "Keletkezett", Type = ColumnType.DATE },
                new ColumnSettings {Name="Irattipus", Title = "Irattipus", Type = ColumnType.STRING },
                new ColumnSettings {Name="Ugyfelnev", Title = "Ügyfélnév", Type = ColumnType.STRING },
                new ColumnSettings {Name="Ugyfelcim", Title = "Ügyfélcím", Type = ColumnType.STRING },
                new ColumnSettings {Name="Kuldo", Title = "Küldő", Type = ColumnType.STRING },
                new ColumnSettings {Name="Targy", Title = "Tárgy", Type = ColumnType.STRING },
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
