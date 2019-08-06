using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using ossServer.BaseResults;
using ossServer.Controllers.Irat;
using ossServer.Hubs;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Threading.Tasks;

namespace ossServer.Controllers.Fotozas
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FotozasController : ControllerBase
    {
        private readonly ossContext _context;
        private readonly IHubContext<OssHub> _hubcontext;
        private readonly IConfiguration _config;

        public FotozasController(ossContext context, IHubContext<OssHub> hubcontext, IConfiguration config)
        {
            _context = context;
            _hubcontext = hubcontext;
            _config = config;
        }

        [HttpPost]
        public async Task<StringResult> CreateNewLink([FromQuery] string sid, [FromBody] IratDto dto)
        {
            var result = new StringResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await FotozasBll.CreateNewLinkAsync(_context, sid, dto);

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
        public async Task<StringResult> GetLink([FromQuery] string sid, [FromBody] IratDto dto)
        {
            var result = new StringResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await FotozasBll.GetLinkAsync(_context, sid, dto);

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
        public async Task<FotozasResult> Check([FromBody] string linkparam)
        {
            var result = new FotozasResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await FotozasBll.CheckAsync(_context, _hubcontext, _config, linkparam);

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