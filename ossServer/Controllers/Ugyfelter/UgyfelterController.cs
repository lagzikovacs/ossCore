using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using ossServer.BaseResults;
using ossServer.Controllers.Ugyfel;
using ossServer.Hubs;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Threading.Tasks;

namespace ossServer.Controllers.Ugyfelter
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UgyfelterController : ControllerBase
    {
        private readonly ossContext _context;
        private readonly IHubContext<OssHub> _hubcontext;
        private readonly IConfiguration _config;

        public UgyfelterController(ossContext context, IHubContext<OssHub> hubcontext, IConfiguration config)
        {
            _context = context;
            _hubcontext = hubcontext;
            _config = config;
        }

        [HttpPost]
        public async Task<StringResult> CreateNewLink([FromQuery] string sid, [FromBody] UgyfelDto dto)
        {
            var result = new StringResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await UgyfelterBll.CreateNewLinkAsync(_context, sid, dto);

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
        public async Task<StringResult> GetLink([FromQuery] string sid, [FromBody] UgyfelDto dto)
        {
            var result = new StringResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await UgyfelterBll.GetLinkAsync(_context, sid, dto);

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
        public async Task<BaseResults.EmptyResult> ClearLink([FromQuery] string sid, [FromBody] UgyfelDto dto)
        {
            var result = new BaseResults.EmptyResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    await UgyfelterBll.ClearLinkAsync(_context, sid, dto);

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
        public async Task<UgyfelterResult> UgyfelterCheck([FromQuery] string sid, 
            [FromBody] string linkparam)
        {
            var result = new UgyfelterResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await UgyfelterBll.UgyfelterCheckAsync(_context, _hubcontext, _config, linkparam);

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