using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ossServer.Controllers.Csoport;
using ossServer.Controllers.Particio;
using ossServer.Controllers.Session;
using ossServer.Controllers.Volume;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ImageMagick;

namespace ossServer.Controllers.Dokumentum
{
    public class DokumentumBll
    {
        private static readonly object LockMe = new object();

        public static async Task<DokumentumDto> GetAsync(ossContext context, string sid, int dokumentumKod)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.IRAT);

            var entity = await DokumentumDal.GetAsync(context, dokumentumKod);
            return ObjectUtils.Convert<Models.Dokumentum, DokumentumDto>(entity);
        }

        public static async Task<List<DokumentumDto>> SelectAsync(ossContext context, string sid, int iratKod, bool imgprev, int imgwidth)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.IRAT);

            var entities = await DokumentumDal.SelectAsync(context, iratKod);
            var entitiesDto = new List<DokumentumDto>();
            

            foreach (var ent in entities)
            {
                //újraolvasni a volume miatt
                var doc = await DokumentumDal.GetWithVolumeAsync(context, ent.Dokumentumkod);

                var entDto = ObjectUtils.Convert<Models.Dokumentum, DokumentumDto>(ent);

                if (imgprev)
                {
                    var ext = ent.Ext.ToLower();
                    if (ext == ".jpg" | ext == ".jpeg" | ext == ".png")
                    {
                        //kell a volume
                        var fb = LetoltesFajl(doc, 0, ent.Meret);

                        using (var image = new MagickImage(fb.b))
                        {
                            image.Resize(imgwidth, imgwidth);

                            entDto.Imgprev = image.ToBase64();
                        }
                    }
                }

                entitiesDto.Add(entDto);
            }

            return entitiesDto;
        }

        public static async Task DeleteAsync(ossContext context, string sid, DokumentumDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.IRAT);

            await DokumentumDal.Lock(context, dto.Dokumentumkod, dto.Modositva);
            var entity = await DokumentumDal.GetAsync(context, dto.Dokumentumkod);
            await DokumentumDal.DeleteAsync(context, entity);
        }

        private static string GetFajlnev(Models.Dokumentum dokumentum)
        {
            var result = dokumentum.VolumekodNavigation.Eleresiut;
            if (result[result.Length - 1] != Path.DirectorySeparatorChar)
                result += Path.DirectorySeparatorChar;

            result += "VOL" + dokumentum.VolumekodNavigation.Volumeno.ToString("000") + Path.DirectorySeparatorChar +
                      "M" + dokumentum.Konyvtar.ToString("000") + Path.DirectorySeparatorChar +
                      dokumentum.Dokumentumkod.ToString("000000") + dokumentum.Ext;

            return result;
        }

        public static async Task<Models.Dokumentum> BejegyzesAsync(ossContext context, string sid, FajlBuf fajlBuf)
        {
            const int minSize = 100 * 1024 * 1024;

            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.IRAT);

            Models.Volume entityVolume;
            int ujFajlMerete = fajlBuf.Meret;

            // lock-ban nem lehet async
            var entityParticio = await ParticioDal.GetAsync(context);
            var vc = JsonConvert.DeserializeObject<VolumeConf>(entityParticio.Volume);

            var Eleresiut = vc.UjvolumeEleresiut ?? throw new Exception(string.Format(Messages.ParticioHiba, "UjvolumeEleresiut"));
            if (!Directory.Exists(Eleresiut))
                throw new Exception($"UjvolumeEleresiut: nem létező könyvtár: {Eleresiut}!");

            var Maxmeret = vc.UjvolumeMaxmeret != null ? (int)vc.UjvolumeMaxmeret : 
                throw new Exception(string.Format(Messages.ParticioHiba, "UjvolumeMaxmeret"));
            if (Maxmeret < minSize)
                throw new Exception($"UjvolumeMaxmeret: az érték legyen nagyobb, mint {minSize} - jelenleg {Maxmeret}!");

            lock (LockMe)
            {
                var lstVolume = VolumeDal.ReadElegSzabadHely(context, ujFajlMerete);

                if (lstVolume.Count > 0)
                {
                    entityVolume = lstVolume[0];

                    if (++entityVolume.Fajlokszamautolsokonyvtarban > 100)
                    {
                        ++entityVolume.Utolsokonyvtar;
                        entityVolume.Fajlokszamautolsokonyvtarban = 1;
                    }
                    entityVolume.Jelenlegimeret += ujFajlMerete;
                    //ezt lehetne okosítani...
                    if ((entityVolume.Maxmeret - entityVolume.Jelenlegimeret) < (10 * 1024 * 1024))
                        entityVolume.Allapot = KotetAllapot.Closed.ToString();
                    entityVolume.Allapotkelte = DateTime.Now;

                    VolumeDal.Update(context, entityVolume);
                }
                else
                {
                    entityVolume = new Models.Volume
                    {
                        Particiokod = (int)context.CurrentSession.Particiokod,
                        Volumeno = context.KodGen(KodNev.Volume),
                        Eleresiut = Eleresiut,
                        Maxmeret = Maxmeret,
                        Jelenlegimeret = ujFajlMerete,
                        Utolsokonyvtar = 1,
                        Fajlokszamautolsokonyvtarban = 1,
                        Allapot = KotetAllapot.Opened.ToString(),
                        Allapotkelte = DateTime.Now
                    };

                    VolumeDal.Add(context, entityVolume);
                }
            }

            var entityDokumentum = new Models.Dokumentum
            {
                Volumekod = entityVolume.Volumekod,
                Konyvtar = entityVolume.Utolsokonyvtar,
                Meret = ujFajlMerete,
                Ext = fajlBuf.Ext,
                Hash = fajlBuf.Hash,
                Iratkod = fajlBuf.IratKod,
                Megjegyzes = fajlBuf.Megjegyzes
            };

            var dokumentumKod = await DokumentumDal.AddAsync(context, entityDokumentum);

            return await DokumentumDal.GetWithVolumeAsync(context, dokumentumKod);
        }

        public static int BejegyzesFajl(Models.Dokumentum entityDokumentum)
        {
            var fajlnev = GetFajlnev(entityDokumentum);
            var konyvtar = Path.GetDirectoryName(fajlnev);
            if (!System.IO.Directory.Exists(konyvtar))
                System.IO.Directory.CreateDirectory(konyvtar);

            FileStream fs = null;
            try
            {
                fs = new FileStream(fajlnev, FileMode.Create, FileAccess.Write);
            }
            finally
            {
                fs?.Close();
            }

            return entityDokumentum.Dokumentumkod;
        }

        public static async Task<Models.Dokumentum> EllenorzesAsync(ossContext context, string sid, int dokumentumKod)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.IRAT);

            return await DokumentumDal.GetWithVolumeAsync(context, dokumentumKod);
        }

        public static void EllenorzesFajl(Models.Dokumentum entityDokumentum)
        {
            var fajlnev = GetFajlnev(entityDokumentum);
            if (!File.Exists(fajlnev))
                throw new Exception("A dokumentumhoz tartozó fájl nem érhető el!");

            var ujraHash = Crypt.fMD5Hash(fajlnev);
            if (ujraHash != entityDokumentum.Hash)
                throw new Exception("Hash hiba!");
        }

        public static async Task<Models.Dokumentum> FeltoltesAsync(ossContext context, string sid, int dokumentumKod)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.IRAT);

            return await DokumentumDal.GetWithVolumeAsync(context, dokumentumKod);
        }

        public static void FeltoltesFajl(Models.Dokumentum entityDokumentum, FajlBuf fajlBuf)
        {
            var fajlnev = GetFajlnev(entityDokumentum);
            if (!File.Exists(fajlnev))
                throw new Exception("A dokumentumhoz tartozó fájl nincs létrehozva!");

            var fi = new FileInfo(fajlnev);
            if (fi.Length + fajlBuf.b.Length > entityDokumentum.Meret)
                throw new Exception("A dokumentumhoz tartozó fájlhoz már nem illeszthető ennyi bájt...!");

            FileStream fs = null;
            try
            {
                fs = new FileStream(fajlnev, FileMode.Append, FileAccess.Write);

                fs.Write(fajlBuf.b, 0, fajlBuf.b.Length);
            }
            finally
            {
                fs?.Close();
            }
        }

        public static async Task<Models.Dokumentum> LetoltesAsync(ossContext context, string sid, int dokumentumKod)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.IRAT);

            return await DokumentumDal.GetWithVolumeAsync(context, dokumentumKod);
        }

        public static FajlBuf LetoltesFajl(Models.Dokumentum entityDokumentum, int kezdoPozicio, int olvasando)
        {
            var fajlnev = GetFajlnev(entityDokumentum);
            if (!File.Exists(fajlnev))
                throw new Exception("A dokumentumhoz tartozó fájl nem érhető el!");

            var fi = new FileInfo(fajlnev);
            if (kezdoPozicio > fi.Length - 1)
                throw new Exception("Hibás pozíció!");

            FajlBuf fajlBuf;

            FileStream fs = null;
            try
            {
                fs = new FileStream(fajlnev, FileMode.Open, FileAccess.Read);

                var olvashato = (int)fs.Length - kezdoPozicio;
                if (olvashato < olvasando)
                    olvasando = olvashato;

                var b = new byte[olvasando];

                fs.Seek(kezdoPozicio, SeekOrigin.Begin);
                fs.Read(b, 0, olvasando);

                fajlBuf = new FajlBuf
                {
                    b = b,
                    Ext = entityDokumentum.Ext,
                    Hash = entityDokumentum.Hash,
                    IratKod = entityDokumentum.Iratkod,
                    Megjegyzes = entityDokumentum.Megjegyzes
                };
            }
            finally
            {
                fs?.Close();
            }

            return fajlBuf;
        }

        public static byte[] LetoltesPDFFajl(IConfiguration config, Models.Dokumentum entityDokumentum)
        {
            var ext = entityDokumentum.Ext.ToLower();
            var fb = LetoltesFajl(entityDokumentum, 0, entityDokumentum.Meret);

            if (ext == ".pdf")
                return fb.b;
            else if (ext == ".xls" | ext == ".xlsx" | ext == ".doc" | ext == ".docx")
                return OfficeUtils.ToPdf(config, new OfficeParam { Bytes = fb.b, Ext = ext });
            else if (ext == ".jpg" | ext == ".jpeg" | ext == ".png")
            {
                using (var image = new MagickImage(fb.b))
                {
                    using (var ms = new MemoryStream())
                    {
                        image.Write(ms, MagickFormat.Pdf);
                        
                        ms.Seek(0, SeekOrigin.Begin);
                        var b = new byte[ms.Length];
                        ms.Read(b, 0, (int)ms.Length);

                        return b;
                    }
                }
            }

            throw new Exception($"A(z) {ext} fájlok nem konvertálhatók!");
        }

        //sql tranzakcióban működik, kis fájlok legyenek
        public static async Task FeltoltesAsync(ossContext context, string sid, FajlBuf fajlBuf)
        {
            var entityDokumentum = await BejegyzesAsync(context, sid, fajlBuf);
            BejegyzesFajl(entityDokumentum);
            FeltoltesFajl(entityDokumentum, fajlBuf);
        }

        public static List<ColumnSettings> GridColumns()
        {
            return new List<ColumnSettings>
            {
                new ColumnSettings {Name="Dokumentumkod", Title = "Id", Type = ColumnType.INT },
                new ColumnSettings {Name="Megjegyzes", Title = "Megjegyzés", Type = ColumnType.STRING },
                new ColumnSettings {Name="Meret", Title = "Méret", Type = ColumnType.INT },
                new ColumnSettings {Name="Ext", Title = "Ext", Type = ColumnType.STRING },
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
