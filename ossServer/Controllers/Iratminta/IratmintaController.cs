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
                    result.Result = IratmintaBll.Elegedettseg(_context, sid, projektKod);

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
        public async Task<ByteArrayResult> KeszrejelentesDemasz([FromQuery] string sid, 
            [FromBody] int projektKod)
        {
            var result = new ByteArrayResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = IratmintaBll.KeszrejelentesDemasz(_context, sid, projektKod);

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
                    result.Result = IratmintaBll.KeszrejelentesElmuEmasz(_context, sid, projektKod);

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
                    result.Result = IratmintaBll.KeszrejelentesEon(_context, sid, projektKod);

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
                    result.Result = IratmintaBll.Munkalap(_context, sid, projektKod);

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
                    result.Result = IratmintaBll.SzallitasiSzerzodes(_context, sid, projektKod);

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
                    result.Result = IratmintaBll.Szerzodes(_context, sid, projektKod);

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