using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Ajanlatkeres
{
    public class AjanlatkeresBll
    {
        public void WebesAjanlatkeres(WebesAjanlatkeresParam par)
        {
            using (var model = OSSContext.NewContext(_sid))
                try
                {
                    model.Open(false);

                    var dto = new AjanlatkeresDto
                    {
                        PARTICIOKOD = par.PARTICIOKOD,
                        UGYNOKNEV = par.UGYNOKNEV,
                        NEV = par.Nev,
                        CIM = par.Cim,
                        EMAIL = par.Email,
                        TELEFONSZAM = par.Telefon,
                        HAVIFOGYASZTASKWH = par.HAVIFOGYASZTASKWH,
                        HAVISZAMLAFT = par.HAVISZAMLAFT,
                        NAPELEMEKTELJESITMENYEKW = par.NAPELEMEKTELJESITMENYEKW,
                        MEGJEGYZES = par.MEGJEGYZES,
                        LETREHOZTA = par.UGYNOKNEV,
                        LETREHOZVA = DateTime.Now,
                        MODOSITOTTA = par.UGYNOKNEV,
                        MODOSITVA = DateTime.Now,
                    };

                    var entity = ObjectUtils.Convert<AjanlatkeresDto, AJANLATKERES>(dto);
                    var id = AjanlatkeresDal.AddWeb(model, entity);

                    model.Commit();

                    //ügyfél
                    var uzenet = $"Tisztelt {par.Nev}!<br><br>A következő adatokkal kért tőlünk ajánlatot: <br><br>Cím: {par.Cim}<br>Email: {par.Email}<br>Telefonszám: {par.Telefon}<br><br>Hamarosan keresni fogjuk a részletek egyeztetése céljából!<br><br>www.gridsolar.hu";
                    EmailBll.Kuldes(model, par.Email, "Re: ajánlatkérés", uzenet);
                    //sales
                    uzenet = $"Hello Timi,<br><br>webes ajánlatkérés érkezett, Id: {id}.<br><br>OSS";
                    EmailBll.Kuldes(model, "sales@gridsolar.hu", "Webes ajánlatkérés", uzenet);
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

        public int Add(AjanlatkeresDto dto)
        {
            using (var model = OSSContext.NewContext(_sid))
                try
                {
                    model.Open(true);
                    CsoportDal.Joge(model, JogKod.AJANLATKERESMOD);

                    var entity = Convert<AJANLATKERES>.ToNew(dto);
                    var result = AjanlatkeresDal.Add(model, entity);
                    var ugynok = FelhasznaloDal.Get(model, model.Session.FELHASZNALOKOD);

                    model.Commit();

                    //ügyfél
                    var uzenet = $"Tisztelt {dto.NEV}!<br><br>{dto.UGYNOKNEV} ügynökünk a következő adatokkal kért ajánlatot az ön számára: <br><br>Cím: {dto.CIM}<br>Email: {dto.EMAIL}<br>Telefonszám: {dto.TELEFONSZAM}<br><br>Hamarosan keresni fogjuk a részletek egyeztetése céljából!<br><br>www.gridsolar.hu";
                    EmailBll.Kuldes(model, dto.EMAIL, "Re: ajánlatkérés", uzenet);
                    //sales
                    uzenet = $"Hello Timi,<br><br>{dto.UGYNOKNEV} ügynökünk ajánlatot kért, Id: {result}.<br><br>OSS";
                    EmailBll.Kuldes(model, "sales@gridsolar.hu", "Ügynök ajánlatkérése", uzenet);
                    //ügynök
                    uzenet = $"Tisztelt {dto.UGYNOKNEV}!<br><br>{dto.NEV} számára a következő adatokkal kért ajánlatot: <br><br>Cím: {dto.CIM}<br>Email: {dto.EMAIL}<br>Telefonszám: {dto.TELEFONSZAM}<br><br>Az ügyfelet hamarosan keresni fogjuk a részletek egyeztetése céljából!<br><br>OSS";
                    EmailBll.Kuldes(model, ugynok.EMAIL, "Re: ajánlatkérés, OSS", uzenet);

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

        public AjanlatkeresDto CreateNew()
        {
            using (var model = OSSContext.NewContext(_sid))
                try
                {
                    model.Open(true);

                    var result = new AjanlatkeresDto { UGYNOKNEV = model.Session.FELHASZNALO };

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

        public void Delete(AjanlatkeresDto dto)
        {
            using (var model = OSSContext.NewContext(_sid))
                try
                {
                    model.Open(true);
                    CsoportDal.Joge(model, JogKod.AJANLATKERESMOD);

                    AjanlatkeresDal.Lock(model, dto.AJANLATKERESKOD, dto.MODOSITVA);
                    var entity = AjanlatkeresDal.Get(model, dto.AJANLATKERESKOD);
                    AjanlatkeresDal.Delete(model, entity);

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

        public AjanlatkeresDto Get(int key)
        {
            using (var model = OSSContext.NewContext(_sid))
                try
                {
                    model.Open(true);
                    CsoportDal.Joge(model, JogKod.AJANLATKERES);

                    var entity = AjanlatkeresDal.Get(model, key);
                    var result = Convert<AjanlatkeresDto>.ToNew(entity);

                    if (model.Session.LOGOL)
                        EsemenynaploBll.Bejegyzes(model, EsemenynaploBejegyzesek.UgynokLekerdezese);

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

        public List<AjanlatkeresDto> Select(int rekordTol, int lapMeret, List<SzMT> szmt, out int osszesRekord)
        {
            using (var model = OSSContext.NewContext(_sid))
                try
                {
                    model.Open(true);
                    CsoportDal.Joge(model, JogKod.AJANLATKERES);

                    var qry = AjanlatkeresDal.GetQuery(model, szmt);
                    osszesRekord = qry.Count();
                    var entities = qry.Skip(rekordTol).Take(lapMeret).ToList();
                    var result = Convert<AjanlatkeresDto>.ToNew(entities);

                    if (model.Session.LOGOL)
                        EsemenynaploBll.Bejegyzes(model, EsemenynaploBejegyzesek.UgynokLekerdezese);

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

        public int Update(AjanlatkeresDto dto)
        {
            using (var model = OSSContext.NewContext(_sid))
                try
                {
                    model.Open(true);
                    CsoportDal.Joge(model, JogKod.AJANLATKERESMOD);

                    AjanlatkeresDal.Lock(model, dto.AJANLATKERESKOD, dto.MODOSITVA);
                    var entity = AjanlatkeresDal.Get(model, dto.AJANLATKERESKOD);
                    var result = AjanlatkeresDal.Update(model, entity);

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
    }
}
