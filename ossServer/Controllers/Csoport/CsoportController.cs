using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ossServer.BaseResults;
using ossServer.Controllers.Primitiv.Felhasznalo;
using ossServer.Models;
using ossServer.Utils;

namespace ossServer.Controllers.Csoport
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CsoportController : ControllerBase
    {
        private readonly ossContext _context;

        public CsoportController(ossContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<Int32Result> Add([FromQuery] string sid, [FromBody] CsoportDto dto)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = CsoportBll.Add(_context, sid, dto);

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
        public async Task<CsoportResult> CreateNew([FromQuery] string sid)
        {
            var result = new CsoportResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = new List<CsoportDto> { new CsoportDto() };

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
            [FromBody] CsoportDto dto)
        {
            var result = new BaseResults.EmptyResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    CsoportBll.Delete(_context, sid, dto);

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
        public async Task<CsoportResult> Get([FromQuery] string sid, [FromBody] int key)
        {
            var result = new CsoportResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = new List<CsoportDto> { CsoportBll.Get(_context, sid, key) };

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
        public async Task<CsoportResult> Read([FromQuery] string sid, [FromBody] string maszk)
        {
            var result = new CsoportResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = CsoportBll.Read(_context, sid, maszk);

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
        public async Task<Int32Result> Update([FromQuery] string sid, [FromBody] CsoportDto dto)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = CsoportBll.Update(_context, sid, dto);

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
        public async Task<FelhasznaloResult> SelectCsoportFelhasznalo([FromQuery] string sid, 
            [FromBody] int csoportKod)
        {
            var result = new FelhasznaloResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = CsoportBll.SelectCsoportFelhasznalo(_context, sid, csoportKod);

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
        public async Task<LehetsegesJogResult> SelectCsoportJog([FromQuery] string sid, 
            [FromBody] int csoportKod)
        {
            var result = new LehetsegesJogResult();


            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = CsoportBll.SelectCsoportJog(_context, sid, csoportKod);

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
        public async Task<BaseResults.EmptyResult> CsoportFelhasznaloBeKi([FromQuery] string sid, 
            [FromBody] CsoportFelhasznaloParameter par)
        {
            var result = new BaseResults.EmptyResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    CsoportBll.CsoportFelhasznaloBeKi(_context, sid, 
                        par.CsoportKod, par.FelhasznaloKod, par.Be);

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
        public async Task<BaseResults.EmptyResult> CsoportJogBeKi([FromQuery] string sid, 
            [FromBody] CsoportJogParameter par)
        {
            var result = new BaseResults.EmptyResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    CsoportBll.CsoportJogBeKi(_context, sid, 
                        par.CsoportKod, par.LehetsegesJogKod, par.Be);

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
        public async Task<JogaimResult> Jogaim([FromQuery] string sid)
        {
            var result = new JogaimResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = CsoportBll.Jogaim(_context, sid);

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