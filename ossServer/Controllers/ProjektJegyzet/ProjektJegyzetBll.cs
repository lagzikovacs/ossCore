using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ossServer.Controllers.ProjektJegyzet
{
    public class ProjektJegyzetBll
    {
        public static async Task<ProjektJegyzetDto> GetAsync(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var entity = await ProjektJegyzetDal.GetAsync(context, key);
            var result = ObjectUtils.Convert<Projektjegyzet, ProjektJegyzetDto>(entity);

            return result;
        }

        public static async Task<ProjektJegyzetDto> CreateNewAsync(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            return new ProjektJegyzetDto();
        }

        public static async Task<int> AddAsync(ossContext context, string sid, ProjektJegyzetDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var entity = ObjectUtils.Convert<ProjektJegyzetDto, Projektjegyzet>(dto);
            return await ProjektJegyzetDal.AddAsync(context, entity);
        }

        public static async Task<List<ProjektJegyzetDto>> SelectAsync(ossContext context, string sid, int projektKod)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var entities = await ProjektJegyzetDal.SelectAsync(context, projektKod);
            var result = new List<ProjektJegyzetDto>();

            foreach (var entity in entities)
            {
                var dto = ObjectUtils.Convert<Projektjegyzet, ProjektJegyzetDto>(entity);
                //dto.Teendo = entity.TeendokodNavigation.Teendo1;

                result.Add(dto);
            }

            return result;
        }

        public static async Task DeleteAsync(ossContext context, string sid, ProjektJegyzetDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            await ProjektJegyzetDal.Lock(context, dto.Projektjegyzetkod, dto.Modositva);
            var entity = await ProjektJegyzetDal.GetAsync(context, dto.Projektjegyzetkod);
            await ProjektJegyzetDal.DeleteAsync(context, entity);
        }

        public static async Task<int> UpdateAsync(ossContext context, string sid, ProjektJegyzetDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            await ProjektJegyzetDal.Lock(context, dto.Projektjegyzetkod, dto.Modositva);
            var entity = await ProjektJegyzetDal.GetAsync(context, dto.Projektjegyzetkod);
            ObjectUtils.Update(dto, entity);
            return await ProjektJegyzetDal.UpdateAsync(context, entity);
        }

        public static List<ColumnSettings> GridColumns()
        {
            return new List<ColumnSettings>
            {
                new ColumnSettings {Name="Projektjegyzetkod", Title = "Id", Type = ColumnType.INT },
                new ColumnSettings {Name="Leiras", Title = "Leirás", Type = ColumnType.STRING },
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
