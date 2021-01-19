using Microsoft.AspNetCore.Mvc;
using ossServer.BaseResults;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ossServer.Controllers.Bizonylat
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BizonylatController : ControllerBase
    {
        private readonly ossContext _context;

        public BizonylatController(ossContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<BizonylatTipusLeiroResult> BizonylatLeiro([FromQuery] string sid, 
            [FromBody] BizonylatTipus bizonylatTipus)
        {
            var result = new BizonylatTipusLeiroResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await BizonylatBll.BizonylatLeiroAsync(_context, sid, bizonylatTipus);

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
        public async Task<BizonylatComplexResult> CreateNewComplex([FromQuery] string sid)
        {
            var result = new BizonylatComplexResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = new List<BizonylatComplexDto>
                    {
                        await BizonylatBll.CreateNewComplexAsync(_context, sid)
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
        public async Task<BizonylatResult> Get([FromQuery] string sid, [FromBody] int bizonylatKod)
        {
            var result = new BizonylatResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = new List<BizonylatDto> { await BizonylatBll.GetAsync(_context, sid, bizonylatKod) };

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
        public async Task<BizonylatComplexResult> GetComplex([FromQuery] string sid, 
            [FromBody] int bizonylatKod)
        {
            var result = new BizonylatComplexResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = new List<BizonylatComplexDto> { await BizonylatBll.GetComplexAsync(_context, sid, bizonylatKod) };

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
        public async Task<BaseResults.EmptyResult> Delete([FromQuery] string sid, [FromBody] BizonylatDto dto)
        {
            var result = new BaseResults.EmptyResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    await BizonylatBll.DeleteAsync(_context, sid, dto);

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
        public async Task<BizonylatResult> Select([FromQuery] string sid, [FromBody] BizonylatParam par)
        {
            var result = new BizonylatResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    var t = await BizonylatBll.SelectAsync(_context, sid, par.RekordTol, par.LapMeret, 
                        par.BizonylatTipus, par.Fi);
                    result.Result = t.Item1;
                    result.OsszesRekord = t.Item2;

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
        public async Task<Int32Result> Save([FromQuery] string sid, [FromBody] BizonylatComplexDto par)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await BizonylatBll.SaveAsync(_context, sid, par);

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
        public async Task<Int32Result> Kibocsatas([FromQuery] string sid, 
            [FromBody] BizonylatKibocsatasParam par)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await BizonylatBll.KibocsatasAsync(_context, sid, par.Dto, par.Bizonylatszam);

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
        public async Task<Int32Result> KifizetesRendben([FromQuery] string sid, [FromBody] BizonylatDto par)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await BizonylatBll.KifizetesRendbenAsync(_context, sid, par);

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
        public async Task<Int32Result> Kiszallitva([FromQuery] string sid, [FromBody] BizonylatDto par)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await BizonylatBll.KiszallitvaAsync(_context, sid, par);

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
        public async Task<Int32Result> Storno([FromQuery] string sid, 
            [FromBody] BizonylatDto par)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await BizonylatBll.StornoAsync(_context, sid, par);

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
        public async Task<Int32Result> UjBizonylatMintaAlapjan([FromQuery] string sid, 
            [FromBody] BizonylatMintaAlapjanParam par)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await BizonylatBll.UjBizonylatMintaAlapjanAsync(_context, sid, 
                        par.BizonylatKod, par.BizonylatTipus);

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
        public async Task<BizonylatTetelResult> BizonylattetelCalc([FromQuery] string sid, 
            [FromBody] BizonylatTetelDto dto)
        {
            var result = new BizonylatTetelResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = new List<BizonylatTetelDto> { await BizonylatBll.BizonylattetelCalcAsync(_context, sid, dto) };

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
        public async Task<BizonylatTetelResult> Bruttobol([FromQuery] string sid, 
            [FromBody] BruttobolParam par)
        {
            var result = new BizonylatTetelResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = new List<BizonylatTetelDto>
                    {
                        await BizonylatBll.BruttobolAsync(_context, sid, par.dto, par.brutto)
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
        public async Task<BizonylatTetelResult> CreateNewTetel([FromQuery] string sid, 
            [FromBody] BizonylatTipus bizonylatTipus)
        {
            var result = new BizonylatTetelResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = new List<BizonylatTetelDto>
                    {
                        await BizonylatTetelBll.CreateNewAsync(_context, sid, bizonylatTipus)
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
        public async Task<BizonylatComplexResult> SumEsAfaEsTermekdij([FromQuery] string sid, 
            [FromBody] BizonylatComplexDto dto)
        {
            var result = new BizonylatComplexResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = new List<BizonylatComplexDto> { await BizonylatBll.SumEsAfaEsTermekdijAsync(_context, sid, dto) };

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
        public async Task<Int32Result> Fuvardij([FromQuery] string sid, [FromBody] FuvardijParam par)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await BizonylatBll.Fuvardij(_context, sid, par);

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
        public async Task<Int32Result> FuvardijTorles([FromQuery] string sid, [FromBody] BizonylatDto par)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await BizonylatBll.FuvardijTorles(_context, sid, par);

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
        public async Task<BaseResults.EmptyResult> ZoomCheck([FromQuery] string sid, [FromBody] BizonylatZoomParameter par)
        {
            var result = new BaseResults.EmptyResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    await BizonylatBll.ZoomCheckAsync(_context, sid, par.Bizonylatkod, par.Bizonylatszam);

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