using Microsoft.AspNetCore.Mvc;
using ossServer.BaseResults;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Threading.Tasks;

namespace ossServer.Controllers.Iratminta
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IratmintaController : ControllerBase
    {
        private readonly ossContext _context;

        public IratmintaController(ossContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ByteArrayResult> Elegedettseg([FromQuery] string sid, [FromBody] int projektKod)
        {
            var result = new ByteArrayResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await IratmintaBll.ElegedettsegAsync(_context, sid, projektKod);

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
        public async Task<ByteArrayResult> KeszrejelentesMvm([FromQuery] string sid, 
            [FromBody] int projektKod)
        {
            var result = new ByteArrayResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await IratmintaBll.KeszrejelentesMvmAsync(_context, sid, projektKod);

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
        public async Task<ByteArrayResult> KeszrejelentesElmuEmasz([FromQuery] string sid, 
            [FromBody] int projektKod)
        {
            var result = new ByteArrayResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await IratmintaBll.KeszrejelentesElmuEmaszAsync(_context, sid, projektKod);

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
        public async Task<ByteArrayResult> KeszrejelentesEon([FromQuery] string sid, 
            [FromBody] int projektKod)
        {
            var result = new ByteArrayResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await IratmintaBll.KeszrejelentesEonAsync(_context, sid, projektKod);

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
        public async Task<ByteArrayResult> Munkalap([FromQuery] string sid, [FromBody] int projektKod)
        {
            var result = new ByteArrayResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await IratmintaBll.MunkalapAsync(_context, sid, projektKod);

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
        public async Task<ByteArrayResult> SzallitasiSzerzodes([FromQuery] string sid, 
            [FromBody] int projektKod)
        {
            var result = new ByteArrayResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await IratmintaBll.SzallitasiSzerzodesAsync(_context, sid, projektKod);

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
        public async Task<ByteArrayResult> FeltetelesSzerzodes([FromQuery] string sid,
            [FromBody] int projektKod)
        {
            var result = new ByteArrayResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await IratmintaBll.FeltetelesSzerzodesAsync(_context, sid, projektKod);

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
        public async Task<ByteArrayResult> OFTSzerzodes([FromQuery] string sid,
            [FromBody] int projektKod)
        {
            var result = new ByteArrayResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await IratmintaBll.OFTSzerzodesAsync(_context, sid, projektKod);

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
        public async Task<ByteArrayResult> HMKEtulajdonoshozzajarulas([FromQuery] string sid,
            [FromBody] int projektKod)
        {
            var result = new ByteArrayResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await IratmintaBll.HMKEtulajdonoshozzajarulas(_context, sid, projektKod);

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
        public async Task<ByteArrayResult> Szerzodes([FromQuery] string sid, [FromBody] int projektKod)
        {
            var result = new ByteArrayResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await IratmintaBll.SzerzodesAsync(_context, sid, projektKod);

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