using Microsoft.AspNetCore.Mvc;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Threading.Tasks;

namespace ossServer.Controllers.Menu
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly ossContext _context;

        public MenuController(ossContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<AngularMenuResult> AngularMenu([FromQuery] string sid)
        {
            var result = new AngularMenuResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = MenuBll.AngularMenu(_context, sid);

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