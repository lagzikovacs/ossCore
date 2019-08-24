using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ossServer.BaseResults;
using ossServer.Models;
using ossServer.Utils;

namespace ossServer.Controllers.Ugyfelkapcsolat
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UgyfelkapcsolatController : ControllerBase
    {
        private readonly ossContext _context;

        public UgyfelkapcsolatController(ossContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<UgyfelkapcsolatResult> CreateNew([FromQuery] string sid)
        {
            var result = new UgyfelkapcsolatResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = new List<UgyfelkapcsolatDto> { await UgyfelkapcsolatBll.CreateNewAsync(_context, sid) };

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
        public async Task<Int32Result> Add([FromQuery] string sid, [FromBody] UgyfelkapcsolatDto dto)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await UgyfelkapcsolatBll.AddAsync(_context, sid, dto);

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
        public async Task<BaseResults.EmptyResult> Delete([FromQuery] string sid, [FromBody] UgyfelkapcsolatDto dto)
        {
            var result = new BaseResults.EmptyResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    await UgyfelkapcsolatBll.DeleteAsync(_context, sid, dto);

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.InmostMessage();
                }

            return result;
        }




        //[HttpPost]
        //public async Task<UgyfelkapcsolatResult> Get([FromQuery] string sid, [FromBody] int key)
        //{
        //    var result = new UgyfelkapcsolatResult();

        //    using (var tr = await _context.Database.BeginTransactionAsync())
        //        try
        //        {
        //            result.Result = new List<UgyfelkapcsolatDto> { await UgyfelkapcsolatBll.GetAsync(_context, sid, key) };

        //            tr.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            tr.Rollback();
        //            result.Error = ex.InmostMessage();
        //        }

        //    return result;
        //}









        //[HttpPost]
        //public async Task<UgyfelResult> Read([FromQuery] string sid, [FromBody] string maszk)
        //{
        //    var result = new UgyfelResult();

        //    using (var tr = await _context.Database.BeginTransactionAsync())
        //        try
        //        {
        //            result.Result = await UgyfelBll.ReadAsync(_context, sid, maszk);

        //            tr.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            tr.Rollback();
        //            result.Error = ex.InmostMessage();
        //        }

        //    return result;
        //}

        //[HttpPost]
        //public async Task<Int32Result> Update([FromQuery] string sid, [FromBody] UgyfelDto dto)
        //{
        //    var result = new Int32Result();

        //    using (var tr = await _context.Database.BeginTransactionAsync())
        //        try
        //        {
        //            result.Result = await UgyfelBll.UpdateAsync(_context, sid, dto);

        //            tr.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            tr.Rollback();
        //            result.Error = ex.InmostMessage();
        //        }

        //    return result;
        //}

        //[HttpPost]
        //public async Task<UgyfelResult> Select([FromQuery] string sid, [FromBody] UgyfelParam par)
        //{
        //    var result = new UgyfelResult();

        //    using (var tr = await _context.Database.BeginTransactionAsync())
        //        try
        //        {
        //            var t = await UgyfelBll.SelectAsync(_context, sid, par.RekordTol, par.LapMeret,
        //                par.Csoport, par.Fi);
        //            result.Result = t.Item1;
        //            result.OsszesRekord = t.Item2;

        //            tr.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            tr.Rollback();
        //            result.Error = ex.InmostMessage();
        //        }

        //    return result;
        //}

        //[HttpPost]
        //public async Task<BaseResults.EmptyResult> ZoomCheck([FromQuery] string sid, [FromBody] UgyfelZoomParameter par)
        //{
        //    var result = new BaseResults.EmptyResult();

        //    using (var tr = await _context.Database.BeginTransactionAsync())
        //        try
        //        {
        //            await UgyfelBll.ZoomCheckAsync(_context, sid, par.Ugyfelkod, par.Ugyfelnev);

        //            tr.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            tr.Rollback();
        //            result.Error = ex.InmostMessage();
        //        }

        //    return result;
        //}

        //[HttpPost]
        //public async Task<StringResult> vCard([FromQuery] string sid, [FromBody] int key)
        //{
        //    var result = new StringResult();

        //    using (var tr = await _context.Database.BeginTransactionAsync())
        //        try
        //        {
        //            result.Result = await UgyfelBll.vCardAsync(_context, sid, key);

        //            tr.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            tr.Rollback();
        //            result.Error = ex.InmostMessage();
        //        }

        //    return result;
        //}

        //[HttpPost]
        //public async Task<ColumnSettingsResult> GetGridSettings([FromQuery] string sid)
        //{
        //    var result = new ColumnSettingsResult();

        //    using (var tr = await _context.Database.BeginTransactionAsync())
        //        try
        //        {
        //            result.Result = UgyfelBll.GridSettings(_context, sid);

        //            tr.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            tr.Rollback();
        //            result.Error = ex.InmostMessage();
        //        }

        //    return result;
        //}

        //[HttpPost]
        //public async Task<ColumnSettingsResult> GetReszletekSettings([FromQuery] string sid)
        //{
        //    var result = new ColumnSettingsResult();

        //    using (var tr = await _context.Database.BeginTransactionAsync())
        //        try
        //        {
        //            result.Result = UgyfelBll.ReszletekSettings(_context, sid);

        //            tr.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            tr.Rollback();
        //            result.Error = ex.InmostMessage();
        //        }

        //    return result;
        //}
    }
}