﻿using ossServer.Controllers.Csoport;
using ossServer.Controllers.Particio;
using ossServer.Controllers.Session;
using ossServer.Controllers.Volume;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace ossServer.Controllers.Dokumentum
{
    public class DokumentumBll
    {
        private static readonly object LockMe = new object();

        public static DokumentumDto Get(ossContext context, string sid, int dokumentumKod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.IRAT);

            var entity = DokumentumDal.Get(context, dokumentumKod);
            return ObjectUtils.Convert<Models.Dokumentum, DokumentumDto>(entity);
        }

        public static List<DokumentumDto> Select(ossContext context, string sid, int iratKod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.IRAT);

            var entities = DokumentumDal.Select(context, iratKod);
            return ObjectUtils.Convert<Models.Dokumentum, DokumentumDto>(entities);
        }

        public static void Delete(ossContext context, string sid, DokumentumDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.IRAT);

            DokumentumDal.Lock(context, dto.Dokumentumkod, dto.Modositva);
            var entity = DokumentumDal.Get(context, dto.Dokumentumkod);
            DokumentumDal.Delete(context, entity);
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

        private static string Eleresiut(ossContext context)
        {
            var entityParticio = ParticioDal.Get(context);
            var result = entityParticio.VolumeUjvolumeEleresiut ?? throw new Exception(string.Format(Messages.ParticioHiba, "VolumeUjvolumeEleresiut"));

            if (!Directory.Exists(result))
                throw new Exception($"VolumeUjvolumeEleresiut: nem létező könyvtár: {result}!");

            return result;
        }

        private static int Meret(ossContext context)
        {
            const int minSize = 100 * 1024 * 1024;

            var entityParticio = ParticioDal.Get(context);
            var result = entityParticio.VolumeUjvolumeMaxmeret != null ?
              (int)entityParticio.VolumeUjvolumeMaxmeret : throw new Exception(string.Format(Messages.ParticioHiba, "VolumeUjvolumeMaxmeret"));

            if (result < minSize)
                throw new Exception($"VolumeUjvolumeMaxmeret: az érték legyen nagyobb, mint {minSize} - jelenleg {result}!");

            return result;
        }

        public static Models.Dokumentum Bejegyzes(ossContext context, string sid, FajlBuf fajlBuf)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.IRAT);

            Models.Volume entityVolume;
            int ujFajlMerete = fajlBuf.Meret;

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
                        Eleresiut = Eleresiut(context),
                        Maxmeret = Meret(context),
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

            var dokumentumKod = DokumentumDal.Add(context, entityDokumentum);

            return DokumentumDal.GetWithVolume(context, dokumentumKod);
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

        public static Models.Dokumentum Ellenorzes(ossContext context, string sid, int dokumentumKod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.IRAT);

            return DokumentumDal.GetWithVolume(context, dokumentumKod);
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

        public static Models.Dokumentum Feltoltes(ossContext context, string sid, int dokumentumKod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.IRAT);

            return DokumentumDal.GetWithVolume(context, dokumentumKod);
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

        public static Models.Dokumentum Letoltes(ossContext context, string sid, int dokumentumKod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.IRAT);

            return DokumentumDal.GetWithVolume(context, dokumentumKod);
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

        //sql tranzakcióban működik, kis fájlok legyenek
        public static void Feltoltes(ossContext context, string sid, FajlBuf fajlBuf)
        {
            var entityDokumentum = Bejegyzes(context, sid, fajlBuf);
            BejegyzesFajl(entityDokumentum);
            FeltoltesFajl(entityDokumentum, fajlBuf);
        }

        public static byte[] LetoltesPDF(ossContext context, string sid, int dokumentumKod)
        {
            return null;

            //SessionBll.Check(context, sid);
            //CsoportDal.Joge(context, JogKod.IRAT);

            //var entityDokumentum = Letoltes(context, dokumentumKod);
            //var ext = entityDokumentum.Ext.ToLower();
            //if (ext != ".xls" & ext != ".xlsx" & ext != ".doc" & ext != ".docx")
            //    throw new Exception($"A(z) {ext} fájlok nem konvertálhatók!");

            //context.Commit();

            //var fb = LetoltesFajl(entityDokumentum, 0, entityDokumentum.Meret);

            //ExcelFile excel;
            //Documentcontext word;
            //byte[] result = new byte[0];

            //if (ext == ".xls" | ext == ".xlsx")
            //{
            //    using (var msExcel = new MemoryStream())
            //    {
            //        msExcel.Write(fb.b, 0, fb.b.Length);
            //        msExcel.Position = 0;
            //        if (ext == ".xls")
            //            excel = ExcelFile.Load(msExcel, GemBox.Spreadsheet.LoadOptions.XlsDefault);
            //        else
            //            excel = ExcelFile.Load(msExcel, GemBox.Spreadsheet.LoadOptions.XlsxDefault);
            //    }
            //    using (var smPdf = new MemoryStream())
            //    {
            //        excel.Save(smPdf, )
            //        excel.Save(smPdf, GemBox.Spreadsheet.SaveOptions.PdfDefault);
            //        result = smPdf.ToArray();
            //    }
            //}
            //if (ext == ".doc" | ext == ".docx")
            //{
            //    using (var msWord = new MemoryStream())
            //    {
            //        msWord.Write(fb.b, 0, fb.b.Length);
            //        msWord.Position = 0;
            //        if (ext == ".doc")
            //            word = Documentcontext.Load(msWord, GemBox.Document.LoadOptions.DocDefault);
            //        else
            //            word = Documentcontext.Load(msWord, GemBox.Document.LoadOptions.DocxDefault);
            //    }
            //    using (var smPdf = new MemoryStream())
            //    {
            //        word.Save(smPdf, GemBox.Document.SaveOptions.PdfDefault);
            //        result = smPdf.ToArray();
            //    }
            //}

            //return result;
        }
    }
}
