﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ossServer.Models;

namespace ossServer.Controllers.Volume
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VolumeController : ControllerBase
    {
        private readonly ossContext _context;

        public VolumeController(ossContext context)
        {
            _context = context;
        }
    }
}