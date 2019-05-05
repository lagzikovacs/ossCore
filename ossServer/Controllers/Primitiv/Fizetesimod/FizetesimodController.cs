using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ossServer.Models;

namespace ossServer.Controllers.Primitiv.Fizetesimod
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FizetesimodController : ControllerBase
    {
        private readonly ossContext _context;

        public FizetesimodController(ossContext context)
        {
            _context = context;
        }
    }
}