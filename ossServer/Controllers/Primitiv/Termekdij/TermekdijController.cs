﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ossServer.BaseResults;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ossServer.Controllers.Primitiv.Termekdij
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TermekdijController : ControllerBase
    {
        private readonly ossContext _context;

        public TermekdijController(ossContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<Int32Result> Add([FromQuery] string sid, [FromBody] TermekdijDto dto)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = TermekdijBll.Add(_context, sid, dto);

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
        public async Task<TermekdijResult> CreateNew([FromQuery] string sid)
        {
            var result = new TermekdijResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = new List<TermekdijDto> { TermekdijBll.CreateNew(_context, sid) };

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
        public async Task<BaseResults.EmptyResult> Delete([FromQuery] string sid, [FromBody] TermekdijDto dto)
        {
            var result = new BaseResults.EmptyResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    TermekdijBll.Delete(_context, sid, dto);

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
        public async Task<TermekdijResult> Get([FromQuery] string sid, [FromBody] int key)
        {
            var result = new TermekdijResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = new List<TermekdijDto> { TermekdijBll.Get(_context, sid, key) };

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
        public async Task<TermekdijResult> Read([FromQuery] string sid, [FromBody] string maszk)
        {
            var result = new TermekdijResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = TermekdijBll.Read(_context, sid, maszk);

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
        public async Task<Int32Result> Update([FromQuery] string sid, [FromBody] TermekdijDto dto)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = TermekdijBll.Update(_context, sid, dto);

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
        public async Task<BaseResults.EmptyResult> ZoomCheck([FromQuery] string sid, [FromBody] TermekdijZoomParameter par)
        {
            var result = new BaseResults.EmptyResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    TermekdijBll.ZoomCheck(_context, sid, par.Termekdijkod, par.Termekdijkt);

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