using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ossServer.Models;

namespace ossServer.Controllers.Dokumentum
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DokumentumController : ControllerBase
    {
        private readonly ossContext _context;

        public DokumentumController(ossContext context)
        {
            _context = context;
        }
    }
}