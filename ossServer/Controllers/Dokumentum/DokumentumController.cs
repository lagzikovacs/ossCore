using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ossServer.BaseResults;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ossServer.Controllers.Dokumentum
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DokumentumController : ControllerBase
    {
        private readonly ossContext _context;
        private readonly IConfiguration _config;

        public DokumentumController(ossContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost]
        public async Task<Int32Result> Bejegyzes([FromQuery] string sid, [FromBody] FajlBuf fajlbuf)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    var entityDokumentum = await DokumentumBll.BejegyzesAsync(_context, sid, fajlbuf);

                    tr.Commit();

                    try
                    {
                        result.Result = DokumentumBll.BejegyzesFajl(entityDokumentum);
                    }
                    catch (Exception ef)
                    {
                        result.Error = ef.InmostMessage();
                    }
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.InmostMessage();
                }

            return result;
        }

        [HttpPost]
        public async Task<BaseResults.EmptyResult> Feltoltes([FromQuery] string sid, [FromBody] FeltoltesParam par)
        {
            var result = new BaseResults.EmptyResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    var entityDokumentum = await DokumentumBll.FeltoltesAsync(_context, sid, par.DokumentumKod);

                    tr.Commit();

                    try
                    {
                        DokumentumBll.FeltoltesFajl(entityDokumentum, par.FajlBuf);
                    }
                    catch (Exception ef)
                    {
                        result.Error = ef.InmostMessage();
                    }
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.InmostMessage();
                }

            return result;
        }

        [HttpPost]
        public async Task<DokumentumResult> Get([FromQuery] string sid, [FromBody] int dokumentumKod)
        {
            var result = new DokumentumResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = new List<DokumentumDto>
                    {
                        await DokumentumBll.GetAsync(_context, sid, dokumentumKod)
                    };

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.InmostMessage();
                }

            return result;
        }

        [HttpPost]
        public async Task<LetoltesResult> Letoltes([FromQuery] string sid, [FromBody] LetoltesParam par)
        {
            var result = new LetoltesResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    var entityDokumentum = await DokumentumBll.LetoltesAsync(_context, sid, par.DokumentumKod);

                    tr.Commit();

                    try
                    {
                        result.Result = DokumentumBll.LetoltesFajl(entityDokumentum, 
                            par.KezdoPozicio, par.Olvasando);
                    }
                    catch (Exception ex)
                    {
                        result.Error = ex.InmostMessage();
                    }
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.InmostMessage();
                }

            return result;
        }

        [HttpPost]
        public async Task<ByteArrayResult> LetoltesPDF([FromQuery] string sid, [FromBody] int dokumentumKod)
        {
            var result = new ByteArrayResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    var entity = await DokumentumBll.LetoltesAsync(_context, sid, dokumentumKod);

                    tr.Commit();

                    try
                    {
                        result.Result = DokumentumBll.LetoltesPDFFajl(_config, entity);
                    }
                    catch (Exception ef)
                    {
                        result.Error = ef.InmostMessage();
                    }
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.InmostMessage();
                }

            return result;
        }

        [HttpPost]
        public async Task<DokumentumResult> Select([FromQuery] string sid, [FromBody] int iratkod)
        {
            var result = new DokumentumResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await DokumentumBll.SelectAsync(_context, sid, iratkod, false, 0);
                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.InmostMessage();
                }

            return result;
        }

        [HttpPost]
        public async Task<BaseResults.EmptyResult> Delete([FromQuery] string sid, [FromBody] DokumentumDto dto)
        {
            var result = new BaseResults.EmptyResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    await DokumentumBll.DeleteAsync(_context, sid, dto);

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.InmostMessage();
                }

            return result;
        }

        [HttpPost]
        public async Task<BaseResults.EmptyResult> Ellenorzes([FromQuery] string sid, [FromBody] int dokumentumKod)
        {
            var result = new BaseResults.EmptyResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    var entityDokumentum = await DokumentumBll.EllenorzesAsync(_context, sid, dokumentumKod);

                    tr.Commit();

                    try
                    {
                        DokumentumBll.EllenorzesFajl(entityDokumentum);
                    }
                    catch (Exception ef)
                    {
                        result.Error = ef.InmostMessage();
                    }
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.InmostMessage();
                }

            return result;
        }

        [HttpPost]
        public async Task<Int32Result> FeltoltesAngular([FromQuery] string sid, [FromBody] FajlBuf par)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    par.Hash = Crypt.MD5Hash(par.b); // a kliensen kellene készíteni...
                    par.Ext = Path.GetExtension(par.Fajlnev);
                    if (string.IsNullOrEmpty(par.Megjegyzes))
                        par.Megjegyzes = Path.GetFileNameWithoutExtension(par.Fajlnev);

                    var entityDokumentum = await DokumentumBll.BejegyzesAsync(_context, sid, par);

                    tr.Commit();

                    try
                    {
                        DokumentumBll.BejegyzesFajl(entityDokumentum);
                        DokumentumBll.FeltoltesFajl(entityDokumentum, par);

                        result.Result = entityDokumentum.Dokumentumkod;
                    }
                    catch (Exception ef)
                    {
                        result.Error = ef.InmostMessage();
                    }
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.InmostMessage();
                }

            return result;
        }

        [HttpPost]
        public async Task<ColumnSettingsResult> GetGridSettings([FromQuery] string sid)
        {
            var result = new ColumnSettingsResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = DokumentumBll.GridSettings(_context, sid);

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.InmostMessage();
                }

            return result;
        }

        [HttpPost]
        public async Task<ColumnSettingsResult> GetReszletekSettings([FromQuery] string sid)
        {
            var result = new ColumnSettingsResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = DokumentumBll.ReszletekSettings(_context, sid);

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.InmostMessage();
                }

            return result;
        }
    }
}