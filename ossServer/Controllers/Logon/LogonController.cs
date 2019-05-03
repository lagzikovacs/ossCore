using Microsoft.AspNetCore.Mvc;
using ossServer.BaseResults;
using ossServer.Models;
using System;
using System.Threading.Tasks;

namespace ossServer.Controllers.Logon
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LogonController : ControllerBase
    {
        private readonly ossContext _context;

        public LogonController(ossContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<StringResult> Bejelentkezes([FromBody] LogonParameter par)
        {
            var result = new StringResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = LogonBll.Bejelentkezes(_context, par.Azonosito, par.Jelszo, 
                        par.Ip, par.WinHost, par.WinUser);

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.Message;
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
                    result.Result = LogonBll.Szerepkorok(_context, sid);

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.Message;
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
                    LogonBll.SzerepkorValasztas(_context, sid, par.ParticioKod, par.CsoportKod);

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.Message;
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
                    LogonBll.Kijelentkezes(_context, sid);

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.Message;
                }

            return result;
        }
    }
}