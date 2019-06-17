using ossServer.Controllers.Primitiv.Felhasznalo;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ossServer.Controllers.Csoport
{
    public class CsoportBll
    {
        public static int Add(ossContext context, string sid, CsoportDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.CSOPORT);

            var entity = ObjectUtils.Convert<CsoportDto, Models.Csoport>(dto);
            CsoportDal.Exists(context, entity);
            return CsoportDal.Add(context, entity);
        }

        public static CsoportDto CreateNew(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.CSOPORT);

            return new CsoportDto();
        }

        public static void Delete(ossContext context, string sid, CsoportDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.CSOPORT);

            CsoportDal.Lock(context, dto.Csoportkod, dto.Modositva);
            CsoportDal.CheckReferences(context, dto.Csoportkod);
            var entity = CsoportDal.Get(context, dto.Csoportkod);
            CsoportDal.Delete(context, entity);
        }

        public static CsoportDto Get(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.CSOPORT);

            var entity = CsoportDal.Get(context, key);
            return ObjectUtils.Convert<Models.Csoport, CsoportDto>(entity);
        }

        public static List<CsoportDto> Read(ossContext context, string sid, string maszk)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.CSOPORT);

            var entities = CsoportDal.Read(context, maszk);
            var result = new List<CsoportDto>();

            foreach (var e in entities)
            {
                var r = ObjectUtils.Convert<Models.Csoport, CsoportDto>(e);
                r.Particiomegnevezes = e.ParticiokodNavigation.Megnevezes;

                result.Add(r);
            }

            return result;
        }

        public static int Update(ossContext context, string sid, CsoportDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.CSOPORT);

            CsoportDal.Lock(context, dto.Csoportkod, dto.Modositva);
            var entity = CsoportDal.Get(context, dto.Csoportkod);
            ObjectUtils.Update(dto, entity); 
            CsoportDal.ExistsAnother(context, entity);

            return CsoportDal.Update(context, entity);
        }

        public static List<FelhasznaloDto> SelectCsoportFelhasznalo(ossContext context, string sid, int csoportKod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.CSOPORT);

            var csF = CsoportDal.SelectCsoportFelhasznalo(context, csoportKod);
            var entities = FelhasznaloDal.Read(context, "");

            var result = new List<FelhasznaloDto>();

            foreach (var e in entities)
            {
                var dto = ObjectUtils.Convert<Felhasznalo, FelhasznaloDto>(e);
                dto.Csoporttag = csF.Contains(dto.Felhasznalokod);

                result.Add(dto);
            }

            return result;
        }

        public static List<LehetsegesJogDto> SelectCsoportJog(ossContext context, string sid, int csoportKod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.CSOPORT);

            var csJ = CsoportDal.SelectCsoportJog(context, csoportKod);
            var entities = JogDal.Read(context, "");

            var result = new List<LehetsegesJogDto>();

            foreach (var e in entities)
            {
                var dto = ObjectUtils.Convert<Lehetsegesjog, LehetsegesJogDto>(e);
                dto.Csoporttag = csJ.Contains(dto.Lehetsegesjogkod);

                result.Add(dto);
            }

            return result.OrderBy(s => s.Jog).ToList();
        }

        public static void CsoportFelhasznaloBeKi(ossContext context, string sid, int csoportKod, int felhasznaloKod, bool Be)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.CSOPORT);

            if (Be)
                CsoportDal.CsoportFelhasznaloBe(context, csoportKod, felhasznaloKod);
            else
                CsoportDal.CsoportFelhasznaloKi(context, csoportKod, felhasznaloKod);
        }

        public static void CsoportJogBeKi(ossContext context, string sid, int csoportKod, int lehetsegesJogKod, bool Be)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.CSOPORT);

            if (Be)
                CsoportDal.CsoportJogBe(context, csoportKod, lehetsegesJogKod);
            else
                CsoportDal.CsoportJogKi(context, csoportKod, lehetsegesJogKod);
        }

        public static List<JogKod> Jogaim(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);

            var lst = CsoportDal.Jogaim(context);

            var result = new List<JogKod>();
            foreach (var l in lst)
                result.Add((JogKod)Enum.Parse(typeof(JogKod), l));

            return result;
        }

        public static List<ColumnSettings> GridColumns()
        {
            return new List<ColumnSettings>
            {
                new ColumnSettings {Name="Csoportkod", Title = "Id", Type = ColumnType.INT },
                new ColumnSettings {Name="Csoport1", Title = "Csoport", Type = ColumnType.STRING },
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
