using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ossServer.Models;

namespace ossServer.Controllers.Primitiv.Helyseg
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HelysegController : ControllerBase
    {
        private readonly ossContext _context;

        public HelysegController(ossContext context)
        {
            _context = context;
        }
    }
}