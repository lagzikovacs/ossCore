﻿using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Controllers.Ugyfel;
using ossServer.Enums;
using ossServer.Hubs;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Ugyfelkapcsolat
{
    public class UgyfelkapcsolatBll
    {
        public static async Task<UgyfelkapcsolatDto> CreateNewAsync(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.UGYFELEKMOD);

            return new UgyfelkapcsolatDto {};
        }

        public static async Task<int> AddAsync(ossContext context, string sid, IHubContext<OssHub> hubcontext, 
            UgyfelkapcsolatDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.UGYFELEKMOD);

            var entity = ObjectUtils.Convert<UgyfelkapcsolatDto, Models.Ugyfelkapcsolat>(dto);
            await UgyfelkapcsolatDal.ExistsAsync(context, entity);
            var result = await UgyfelkapcsolatDal.AddAsync(context, entity);

            var fromugyfel = await UgyfelBll.GetAsync(context, dto.Fromugyfelkod);

            HubUtils.Uzenet(hubcontext, context.CurrentSession.Felhasznalo,
                $"Új ügyfélkapcsolat: {fromugyfel.Nev} -> {dto.Nev}");

            return result;
        }

        public static async Task DeleteAsync(ossContext context, string sid, UgyfelkapcsolatDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.UGYFELEKMOD);

            await UgyfelkapcsolatDal.Lock(context, dto.Ugyfelkapcsolatkod, dto.Modositva);
            // CheckReferencesAsync
            var entity = await UgyfelkapcsolatDal.GetAsync(context, dto.Ugyfelkapcsolatkod);
            await UgyfelkapcsolatDal.DeleteAsync(context, entity);
        }

        private static string Cim(Models.Ugyfel entity)
        {
            var result = "";
            if (entity.HelysegkodNavigation != null)
                result = $"{entity.Iranyitoszam} {entity.HelysegkodNavigation.Helysegnev}, {entity.Kozterulet} {entity.Kozterulettipus} {entity.Hazszam}";

            return result;
        }

        private static UgyfelkapcsolatDto Calc(Models.Ugyfelkapcsolat entity, FromTo FromTo)
        {
            var result = ObjectUtils.Convert<Models.Ugyfelkapcsolat, UgyfelkapcsolatDto>(entity);

            switch (FromTo)
            {
                case FromTo.ToleIndul: // a To ügyfelek adatai kellenek
                    if (entity.TougyfelkodNavigation != null)
                    {
                        result.Nev = entity.TougyfelkodNavigation.Nev;
                        if (entity.TougyfelkodNavigation.HelysegkodNavigation != null)
                            result.Cim = Cim(entity.TougyfelkodNavigation);
                        result.Ceg = entity.TougyfelkodNavigation.Ceg;
                        result.Beosztas = entity.TougyfelkodNavigation.Beosztas;
                        result.Telefon = entity.TougyfelkodNavigation.Telefon;
                        result.Email = entity.TougyfelkodNavigation.Email;
                        if (entity.TougyfelkodNavigation.TevekenysegkodNavigation != null)
                            result.Tevekenyseg = entity.TougyfelkodNavigation.TevekenysegkodNavigation.Tevekenyseg1;
                    }
                    break;
                case FromTo.HozzaEr: // a From ügyfelek adatai kellenek
                    if (entity.FromugyfelkodNavigation != null)
                    {
                        result.Nev = entity.FromugyfelkodNavigation.Nev;
                        if (entity.FromugyfelkodNavigation.HelysegkodNavigation != null)
                            result.Cim = Cim(entity.FromugyfelkodNavigation);
                        result.Ceg = entity.FromugyfelkodNavigation.Ceg;
                        result.Beosztas = entity.FromugyfelkodNavigation.Beosztas;
                        result.Telefon = entity.FromugyfelkodNavigation.Telefon;
                        result.Email = entity.FromugyfelkodNavigation.Email;
                        if (entity.FromugyfelkodNavigation.TevekenysegkodNavigation != null)
                            result.Tevekenyseg = entity.FromugyfelkodNavigation.TevekenysegkodNavigation.Tevekenyseg1;
                    }
                    break;
            }

            return result;
        }

        public static async Task<UgyfelkapcsolatDto> GetAsync(ossContext context, string sid, UgyfelkapcsolatGetParam param)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.UGYFELEK);

            var entity = await UgyfelkapcsolatDal.GetAsync(context, param.Key);
            return Calc(entity, param.FromTo);
        }

        public static async Task<int> UpdateAsync(ossContext context, string sid, UgyfelkapcsolatDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.UGYFELEKMOD);

            await UgyfelkapcsolatDal.Lock(context, dto.Ugyfelkapcsolatkod, dto.Modositva);
            var entity = await UgyfelkapcsolatDal.GetAsync(context, dto.Ugyfelkapcsolatkod);

            ObjectUtils.Update(dto, entity);
            await UgyfelkapcsolatDal.ExistsAnotherAsync(context, entity);
            return await UgyfelkapcsolatDal.UpdateAsync(context, entity);
        }

        public static async Task<Tuple<List<UgyfelkapcsolatDto>, int>> SelectAsync(ossContext context, 
            string sid, int rekordTol, int lapMeret, int Ugyfelkod, List<SzMT> szmt, FromTo FromTo)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.UGYFELEK);

            var qry = UgyfelkapcsolatDal.GetQuery(context, Ugyfelkod, szmt, FromTo);
            var osszesRekord = await qry.CountAsync();
            var entities = await qry.Skip(rekordTol).Take(lapMeret).ToListAsync();
            var result = new List<UgyfelkapcsolatDto>();

            foreach (var entity in entities)
                result.Add(Calc(entity, FromTo));

            return new Tuple<List<UgyfelkapcsolatDto>, int>(result, osszesRekord);
        }

        public static List<ColumnSettings> GridColumns()
        {
            return new List<ColumnSettings>
            {
                new ColumnSettings {Name="Ugyfelkapcsolatkod", Title = "Id", Type = ColumnType.INT },
                new ColumnSettings {Name="Nev", Title = "Név", Type = ColumnType.STRING },
                new ColumnSettings {Name="Cim", Title = "Cím", Type = ColumnType.STRING },
                new ColumnSettings {Name="Ceg", Title = "Cég", Type = ColumnType.STRING },
                new ColumnSettings {Name="Beosztas", Title = "Beosztás", Type = ColumnType.STRING },
                new ColumnSettings {Name="Telefon", Title = "Telefon", Type = ColumnType.STRING },
                new ColumnSettings {Name="Email", Title = "Email", Type = ColumnType.STRING },
                new ColumnSettings {Name="Tevekenyseg", Title = "Tevékenység", Type = ColumnType.STRING },
                new ColumnSettings {Name="Leiras", Title = "Leírás", Type = ColumnType.STRING },
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

