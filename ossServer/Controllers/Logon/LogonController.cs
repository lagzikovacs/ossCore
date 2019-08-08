using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ossServer.BaseResults;
using ossServer.Hubs;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Threading.Tasks;

namespace ossServer.Controllers.Logon
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LogonController : ControllerBase
    {
        private readonly ossContext _context;
        private readonly IHubContext<OssHub> _hubcontext;

        public LogonController(ossContext context, IHubContext<OssHub> hubcontext)
        {
            _context = context;
            _hubcontext = hubcontext;
        }

        [HttpPost]
        public async Task<StringResult> Bejelentkezes([FromBody] LogonParameter par)
        {
            var result = new StringResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await LogonBll.BejelentkezesAsync(_context, _hubcontext, par.Azonosito, par.Jelszo, 
                        par.Ip, par.WinHost, par.WinUser);

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
        public async Task<SzerepkorokResult> Szerepkorok([FromQuery] string sid)
        {
            var result = new SzerepkorokResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await LogonBll.SzerepkorokAsync(_context, sid);

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
        public async Task<BaseResults.EmptyResult> Szerepkorvalasztas([FromQuery] string sid, [FromBody] SzerepkorvalasztasParameter par)
        {
            var result = new BaseResults.EmptyResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    await LogonBll.SzerepkorValasztasAsync(_context, sid, par.ParticioKod, par.CsoportKod);

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
        public async Task<BaseResults.EmptyResult> Kijelentkezes([FromQuery] string sid)
        {
            var result = new BaseResults.EmptyResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    await LogonBll.KijelentkezesAsync(_context, sid);

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