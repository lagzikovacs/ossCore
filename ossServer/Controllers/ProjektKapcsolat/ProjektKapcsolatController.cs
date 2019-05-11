using Microsoft.AspNetCore.Mvc;
using ossServer.BaseResults;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Threading.Tasks;

namespace ossServer.Controllers.ProjektKapcsolat
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProjektKapcsolatController : ControllerBase
    {
        private readonly ossContext _context;

        public ProjektKapcsolatController(ossContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ProjektKapcsolatResult> Get([FromQuery] string sid, 
            [FromBody] int projektkapcsolatKod)
        {
            var result = new ProjektKapcsolatResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = ProjektKapcsolatBll.Get(_context, sid, projektkapcsolatKod);

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
        public async Task<BaseResults.EmptyResult> Delete([FromQuery] string sid, 
            [FromBody] int projektKapcsolatKod)
        {
            var result = new BaseResults.EmptyResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    ProjektKapcsolatBll.Delete(_context, sid, projektKapcsolatKod);

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
        public async Task<ProjektKapcsolatResult> Select([FromQuery] string sid, 
            [FromBody] int projektKod)
        {
            var result = new ProjektKapcsolatResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = ProjektKapcsolatBll.Select(_context, sid, projektKod);

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
        public async Task<ProjektKapcsolatResult> SelectForUgyfelter([FromQuery] string sid, 
            [FromBody] int projektKod)
        {
            var result = new ProjektKapcsolatResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = ProjektKapcsolatBll.Select(_context, sid, projektKod, true);

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
        public async Task<ProjektKapcsolatResult> SelectByBizonylat([FromQuery] string sid, 
            [FromBody] int bizonylatKod)
        {
            var result = new ProjektKapcsolatResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = ProjektKapcsolatBll.SelectByBizonylat(_context, sid, bizonylatKod);

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
        public async Task<ProjektKapcsolatResult> SelectByIrat([FromQuery] string sid, 
            [FromBody] int iratKod)
        {
            var result = new ProjektKapcsolatResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = ProjektKapcsolatBll.SelectByIrat(_context, sid, iratKod);

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
        public async Task<Int32Result> AddBizonylatToProjekt([FromQuery] string sid, 
            [FromBody] ProjektKapcsolatParam par)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = ProjektKapcsolatBll.AddBizonylatToProjekt(_context, sid, 
                        par.ProjektKod, par.BizonylatKod);

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
        public async Task<Int32Result> AddIratToProjekt([FromQuery] string sid, 
            [FromBody] ProjektKapcsolatParam par)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = ProjektKapcsolatBll.AddIratToProjekt(_context, sid, 
                        par.ProjektKod, par.IratKod);

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
        public async Task<Int32Result> UjBizonylatToProjekt([FromQuery] string sid, 
            [FromBody] ProjektKapcsolatParam par)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = ProjektKapcsolatBll.UjBizonylatToProjekt(_context, sid, 
                        par.ProjektKod, par.Dto);

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