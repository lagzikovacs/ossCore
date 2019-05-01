using Microsoft.AspNetCore.Mvc;
using ossServer.Models;
using System.Threading.Tasks;

namespace ossServer.Controllers.Primitiv.Afakulcs
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AfakulcsController : ControllerBase
    {
        private readonly ossContext _context;

        public AfakulcsController(ossContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<AfaKulcsResult> Read([FromQuery] string sid, [FromBody] string maszk)
        {
            return await AfakulcsBll.Read(_context, sid, maszk);
        }
    }
}