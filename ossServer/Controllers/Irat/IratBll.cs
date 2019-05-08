using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Irat
{
    public class IratBll
    {
        public int Add(IratDto dto)
        {
            using (var model = OSSContext.NewContext(_sid))
                try
                {
                    model.Open(true);
                    CsoportDal.Joge(model, JogKod.IRAT);

                    var entity = ObjectUtils.Convert<IratDto, IRAT>(dto);
                    var result = IratDal.Add(model, entity);

                    if (model.Session.LOGOL)
                    {
                        EsemenynaploBll.Bejegyzes(model, EsemenynaploBejegyzesek.IratUj);
                        OssHub.Uzenet(model.Session.FELHASZNALO, EsemenynaploBejegyzesek.IratUj);
                    }

                    model.Commit();

                    return result;
                }
                catch (Exception ex)
                {
                    model.Rollback();
                    throw OSSContext.FaultException(ex);
                }
                finally
                {
                    model.Close();
                }
        }

        public IratDto CreateNew()
        {
            return new IratDto { KELETKEZETT = DateTime.Now.Date, IRANY = "Belső" };
        }

        public void Delete(IratDto dto)
        {
            using (var model = OSSContext.NewContext(_sid))
                try
                {
                    model.Open(true);
                    CsoportDal.Joge(model, JogKod.IRATMOD);

                    IratDal.Lock(model, dto.IRATKOD, dto.MODOSITVA);
                    IratDal.CheckReferences(model, dto.IRATKOD);
                    var entity = IratDal.Get(model, dto.IRATKOD);
                    IratDal.Delete(model, entity);

                    if (model.Session.LOGOL)
                    {
                        EsemenynaploBll.Bejegyzes(model, EsemenynaploBejegyzesek.IratTorlese);
                        OssHub.Uzenet(model.Session.FELHASZNALO, EsemenynaploBejegyzesek.IratTorlese);
                    }

                    model.Commit();
                }
                catch (Exception ex)
                {
                    model.Rollback();
                    throw OSSContext.FaultException(ex);
                }
                finally
                {
                    model.Close();
                }
        }

        public IratDto Get(int key)
        {
            using (var model = OSSContext.NewContext(_sid))
                try
                {
                    model.Open(true);
                    CsoportDal.Joge(model, JogKod.IRAT);

                    var entity = IratDal.Get(model, key);
                    var result = ObjectUtils.Convert<IRAT, IratDto>(entity);

                    result.IRATTIPUS = entity.IRATTIPUS.IRATTIPUS1;
                    if (entity.UGYFEL != null)
                    {
                        result.UGYFELNEV = entity.UGYFEL.NEV;
                        result.UGYFELCIM = UgyfelUtils.Cim(entity.UGYFEL);
                    }

                    if (model.Session.LOGOL)
                        EsemenynaploBll.Bejegyzes(model, EsemenynaploBejegyzesek.IratLekerdezese);

                    model.Commit();

                    return result;
                }
                catch (Exception ex)
                {
                    model.Rollback();
                    throw OSSContext.FaultException(ex);
                }
                finally
                {
                    model.Close();
                }
        }

        public static List<IratDto> Select(OSSContext model, int rekordTol, int lapMeret, List<SzMT> szmt, out int osszesRekord)
        {
            CsoportDal.Joge(model, JogKod.IRAT);

            var qry = IratDal.GetQuery(model, szmt);
            osszesRekord = qry.Count();
            var entities = qry.Skip(rekordTol).Take(lapMeret).ToList();
            var result = new List<IratDto>();

            foreach (var entity in entities)
            {
                var dto = ObjectUtils.Convert<IRAT, IratDto>(entity);

                dto.IRATTIPUS = entity.IRATTIPUS.IRATTIPUS1;
                if (entity.UGYFEL != null)
                {
                    dto.UGYFELNEV = entity.UGYFEL.NEV;
                    dto.UGYFELCIM = UgyfelUtils.Cim(entity.UGYFEL);
                }

                result.Add(dto);
            }

            if (model.Session.LOGOL)
                EsemenynaploBll.Bejegyzes(model, EsemenynaploBejegyzesek.IratLekerdezese);

            return result;
        }
        public List<IratDto> Select(int rekordTol, int lapMeret, List<SzMT> fi, out int osszesRekord)
        {
            using (var model = OSSContext.NewContext(_sid))
                try
                {
                    model.Open(true);

                    var result = Select(model, rekordTol, lapMeret, fi, out osszesRekord);

                    model.Commit();

                    return result;
                }
                catch (Exception ex)
                {
                    model.Rollback();
                    throw OSSContext.FaultException(ex);
                }
                finally
                {
                    model.Close();
                }
        }

        public int Update(IratDto dto)
        {
            using (var model = OSSContext.NewContext(_sid))
                try
                {
                    model.Open(true);
                    CsoportDal.Joge(model, JogKod.IRATMOD);

                    IratDal.Lock(model, dto.IRATKOD, dto.MODOSITVA);
                    var entity = IratDal.Get(model, dto.IRATKOD);
                    ObjectUtils.Update(dto, entity);
                    var result = IratDal.Update(model, entity);

                    if (model.Session.LOGOL)
                    {
                        EsemenynaploBll.Bejegyzes(model, EsemenynaploBejegyzesek.IratModositasa);
                        OssHub.Uzenet(model.Session.FELHASZNALO, EsemenynaploBejegyzesek.IratModositasa);
                    }

                    model.Commit();

                    return result;
                }
                catch (Exception ex)
                {
                    model.Rollback();
                    throw OSSContext.FaultException(ex);
                }
                finally
                {
                    model.Close();
                }
        }


        //sql tranzakcióban működik, kis fájlok legyenek
        internal static FajlBuf Letoltes(OSSContext model, int iratKod)
        {
            IratDal.Get(model, iratKod);
            var lstDokumentum = DokumentumDal.Select(model, iratKod);
            if (lstDokumentum.Count != 1)
                throw new Exception("Nincs pontosan egy dokumentum!");

            var entityDokumentum = DokumentumBll.Letoltes(model, lstDokumentum[0].DOKUMENTUMKOD);
            var fb = DokumentumBll.LetoltesFajl(entityDokumentum, 0, lstDokumentum[0].MERET);

            return fb;
        }
    }
}
