using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ossServer.Models;
using ossServer.Utils;

namespace ossServer.Controllers.Menu
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly ossContext _context;

        public MenuController(ossContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<AngularMenuResult> Add([FromQuery] string sid)
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