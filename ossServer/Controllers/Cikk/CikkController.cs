using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ossServer.BaseResults;
using ossServer.Models;
using ossServer.Utils;

namespace ossServer.Controllers.Cikk
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CikkController : ControllerBase
    {
        private readonly ossContext _context;

        public CikkController(ossContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<Int32Result> Add([FromQuery] string sid, [FromBody] CikkDto dto)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.InmostMessage();
                }

            return result;

            var task = new Task<Int32Result>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));
                  if (dto == null)
                      throw new ArgumentNullException(nameof(dto));

                  result.Result = new CikkBll(sid).Add(dto);
              })
            );
            task.Start();
            return await task;
        }

        [HttpPost]
        public async Task<CikkResult> CreateNew([FromQuery] string sid)
        {
            var result = new CikkResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.InmostMessage();
                }

            return result;

            var task = new Task<CikkResult>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));

                  result.Result = new List<CikkDto> { new CikkDto() };
              })
            );
            task.Start();
            return await task;
        }

        [HttpPost]
        public async Task<BaseResults.EmptyResult> Delete([FromQuery] string sid, [FromBody] CikkDto dto)
        {
            var result = new BaseResults.EmptyResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.InmostMessage();
                }

            return result;

            var task = new Task<EmptyResult>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));
                  if (dto == null)
                      throw new ArgumentNullException(nameof(dto));

                  new CikkBll(sid).Delete(dto);
              })
            );
            task.Start();
            return await task;
        }

        [HttpPost]
        public async Task<CikkResult> Get([FromQuery] string sid, [FromBody] int key)
        {
            var result = new CikkResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.InmostMessage();
                }

            return result;

            var task = new Task<CikkResult>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));

                  result.Result = new List<CikkDto> { new CikkBll(sid).Get(key) };
              })
            );
            task.Start();
            return await task;
        }

        [HttpPost]
        public async Task<CikkResult> Read([FromQuery] string sid, [FromBody] string maszk)
        {
            var result = new CikkResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.InmostMessage();
                }

            return result;

            var task = new Task<CikkResult>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));
                  if (maszk == null)
                      throw new ArgumentNullException(nameof(maszk));

                  result.Result = new CikkBll(sid).Read(maszk);
              })
            );
            task.Start();
            return await task;
        }

        [HttpPost]
        public async Task<Int32Result> Update([FromQuery] string sid, [FromBody] CikkDto dto)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.InmostMessage();
                }

            return result;

            var task = new Task<Int32Result>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));
                  if (dto == null)
                      throw new ArgumentNullException(nameof(dto));

                  result.Result = new CikkBll(sid).Update(dto);
              })
            );
            task.Start();
            return await task;
        }

        [HttpPost]
        public async Task<CikkResult> Select([FromQuery] string sid, [FromBody] CikkParam par)
        {
            var result = new CikkResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.InmostMessage();
                }

            return result;

            var task = new Task<CikkResult>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));
                  if (par == null)
                      throw new ArgumentNullException(nameof(par));

                  result.Result = new CikkBll(sid).Select(par.RekordTol, par.LapMeret, par.Fi, out var osszesRekord);
                  result.OsszesRekord = osszesRekord;
              })
            );
            task.Start();
            return await task;
        }

        [HttpPost]
        public async Task<CikkMozgasResult> Mozgas([FromQuery] string sid, [FromBody] CikkMozgasParam par)
        {
            var result = new CikkMozgasResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.InmostMessage();
                }

            return result;

            var task = new Task<CikkMozgasResult>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));
                  if (par == null)
                      throw new ArgumentNullException(nameof(par));

                  result.Result = new CikkBll(sid).Mozgas(par.CikkKod, (BizonylatTipus)par.BizonylatTipusKod);
              })
            );
            task.Start();
            return await task;
        }

        [HttpPost]
        public async Task<BaseResults.EmptyResult> ZoomCheck([FromQuery] string sid, [FromBody] CikkZoomParameter par)
        {
            var result = new BaseResults.EmptyResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.InmostMessage();
                }

            return result;

            var task = new Task<EmptyResult>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));
                  if (par == null)
                      throw new ArgumentNullException(nameof(par));

                  new CikkBll(sid).ZoomCheck(par.CikkKod, par.Cikk);
              })
            );
            task.Start();
            return await task;
        }
    }
}