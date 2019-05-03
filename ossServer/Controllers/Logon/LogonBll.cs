using ossServer.BaseResults;
using ossServer.Controllers.Session;
using ossServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Logon
{
    public class LogonBll
    {
        //public static string Bejelentkezes(ossContext model, string azonosito, string jelszo,
        //  string ip, string winHost, string winUser)
        //{
        //    var felhasznalo = FelhasznaloDal.Get(model, azonosito, jelszo);
        //    var sid = SessionBll.CreateNew(model, ip, winHost, winUser,
        //      felhasznalo.FELHASZNALOKOD, felhasznalo.NEV, azonosito, felhasznalo.LOGONLOG);

        //    if (model.Session.LOGOL)
        //    {
        //        EsemenynaploBll.Bejegyzes(model, EsemenynaploBejegyzesek.Bejelentkezes);
        //        OssHub.Uzenet(model.Session.FELHASZNALO, EsemenynaploBejegyzesek.Bejelentkezes);
        //    }

        //    return sid;
        //}

        //public string Bejelentkezes(string azonosito, string jelszo, string ip, string winHost, string winUser)
        //{
        //    using (var model = OSSContext.NewContext(null))
        //        try
        //        {
        //            model.Open(false);

        //            var result = Bejelentkezes(model, azonosito, jelszo, ip, winHost, winUser);

        //            model.Commit();

        //            return result;
        //        }
        //        catch (Exception ex)
        //        {
        //            model.Rollback();
        //            throw OSSContext.FaultException(ex);
        //        }
        //        finally
        //        {
        //            model.Close();
        //        }
        //}

        //public static List<CsoportDto> Szerepkorok(ossContext model)
        //{
        //    var entities = CsoportDal.GetSzerepkorok(model);
        //    var result = new List<CsoportDto>();
        //    foreach (var entity in entities)
        //    {
        //        var dto = Convert<CsoportDto>.ToNew(entity);
        //        dto.PARTICIOMEGNEVEZES = entity.PARTICIO.MEGNEVEZES;

        //        result.Add(dto);
        //    }

        //    if (model.Session.LOGOL)
        //        EsemenynaploBll.Bejegyzes(model, EsemenynaploBejegyzesek.LehetsegesSzerepkorok);

        //    return result;
        //}

        //public List<CsoportDto> Szerepkorok()
        //{
        //    using (var model = OSSContext.NewContext(_sid))
        //        try
        //        {
        //            model.Open(true, false);

        //            var result = Szerepkorok(model);

        //            model.Commit();

        //            return result;
        //        }
        //        catch (Exception ex)
        //        {
        //            model.Rollback();
        //            throw OSSContext.FaultException(ex);
        //        }
        //        finally
        //        {
        //            model.Close();
        //        }
        //}

        public static void SzerepkorValasztas_(ossContext model, string sid, int particioKod, int csoportKod)
        {
            SessionBll.UpdateRole(model, sid, particioKod, csoportKod);

            //if (model.CurrentSession.Logol)
            //    EsemenynaploBll.Bejegyzes(model, EsemenynaploBejegyzesek.SzerepkorValasztas);
        }

        public static async Task<EmptyResult> SzerepkorValasztas(ossContext context, string sid, int particioKod, int csoportKod)
        {
            var result = new EmptyResult();

            using (var tr = await context.Database.BeginTransactionAsync())
                try
                {
                    SzerepkorValasztas_(context, sid, particioKod, csoportKod);

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.Message;
                }

            return result;
        }

        public static async Task<EmptyResult> Kijelentkezes(ossContext context, string sid)
        {
            var result = new EmptyResult();

            using (var tr = await context.Database.BeginTransactionAsync())
                try
                {
                    SessionBll.Check(context, sid, false);
                    SessionBll.Delete(context, sid);

                    //if (context.CurrentSession.Logol)
                    //{
                    //    EsemenynaploBll.Bejegyzes(model, EsemenynaploBejegyzesek.Kijelentkezes);
                    //    OssHub.Uzenet(model.Session.FELHASZNALO, EsemenynaploBejegyzesek.Kijelentkezes);
                    //}

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.Message;
                }

            return result;
        }
    }
}
