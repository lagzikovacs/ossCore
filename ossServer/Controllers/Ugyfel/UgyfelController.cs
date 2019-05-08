using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ossServer.BaseResults;
using ossServer.Models;
using ossServer.Utils;

namespace ossServer.Controllers.Ugyfel
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UgyfelController : ControllerBase
    {
        private readonly ossContext _context;

        public UgyfelController(ossContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<Int32Result> Add([FromQuery] string sid, [FromBody] UgyfelDto dto)
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

                  result.Result = new UgyfelBll(sid).Add(dto);
              })
            );
            task.Start();
            return await task;
        }

        [HttpPost]
        public async Task<UgyfelResult> CreateNew([FromQuery] string sid)
        {
            var result = new UgyfelResult();

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

            var task = new Task<UgyfelResult>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));

                  result.Result = new List<UgyfelDto> { new UgyfelBll(sid).CreateNew() };
              })
            );
            task.Start();
            return await task;
        }

        [HttpPost]
        public async Task<BaseResults.EmptyResult> Delete([FromQuery] string sid, [FromBody] UgyfelDto dto)
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

                  new UgyfelBll(sid).Delete(dto);
              })
            );
            task.Start();
            return await task;
        }

        [HttpPost]
        public async Task<UgyfelResult> Get([FromQuery] string sid, [FromBody] int key)
        {
            var result = new UgyfelResult();

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

            var task = new Task<UgyfelResult>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));

                  result.Result = new List<UgyfelDto> { new UgyfelBll(sid).Get(key) };
              })
            );
            task.Start();
            return await task;
        }

        [HttpPost]
        public async Task<UgyfelResult> Read([FromQuery] string sid, [FromBody] string maszk)
        {
            var result = new UgyfelResult();

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

            var task = new Task<UgyfelResult>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));
                  if (maszk == null)
                      throw new ArgumentNullException(nameof(maszk));

                  result.Result = new UgyfelBll(sid).Read(maszk);
              })
            );
            task.Start();
            return await task;
        }

        [HttpPost]
        public async Task<Int32Result> Update([FromQuery] string sid, [FromBody] UgyfelDto dto)
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

                  result.Result = new UgyfelBll(sid).Update(dto);
              })
            );
            task.Start();
            return await task;
        }

        [HttpPost]
        public async Task<UgyfelResult> Select([FromQuery] string sid, [FromBody] UgyfelParam par)
        {
            var result = new UgyfelResult();

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

            var task = new Task<UgyfelResult>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));
                  if (par == null)
                      throw new ArgumentNullException(nameof(par));

                  result.Result = new UgyfelBll(sid).Select(par.RekordTol, par.LapMeret, par.Fi, out var osszesRekord);
                  result.OsszesRekord = osszesRekord;
              })
            );
            task.Start();
            return await task;
        }

        [HttpPost]
        public async Task<BaseResults.EmptyResult> ZoomCheck([FromQuery] string sid, [FromBody] UgyfelZoomParameter par)
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

                  new UgyfelBll(sid).ZoomCheck(par.Ugyfelkod, par.Ugyfelnev);
              })
            );
            task.Start();
            return await task;
        }
    }
}