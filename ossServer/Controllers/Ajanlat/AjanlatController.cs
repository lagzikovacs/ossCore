using Microsoft.AspNetCore.Mvc;
using ossServer.BaseResults;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ossServer.Controllers.Ajanlat
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AjanlatController : ControllerBase
    {
        private readonly ossContext _context;

        public AjanlatController(ossContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<AjanlatParamResult> CreateNew([FromQuery] string sid)
        {
            var result = new AjanlatParamResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await AjanlatBll.CreateNewAsync(_context, sid);

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
        public async Task<Int32Result> AjanlatKeszites([FromQuery] string sid, 
            [FromBody] AjanlatParam ap)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    // TODO: ez csak hack...
                    var fi = new List<SzMT>
                    {
                        new SzMT {Szempont = Szempont.Ervenyes, Minta = ap.Ervenyes},
                        new SzMT {Szempont = Szempont.Tajolas, Minta = ap.Tajolas},
                        new SzMT {Szempont = Szempont.Termeles, Minta = ap.Termeles},
                        new SzMT {Szempont = Szempont.Megjegyzes, Minta = ap.Megjegyzes},
                        new SzMT {Szempont = Szempont.SzuksegesAramerosseg, Minta = ap.SzuksegesAramerosseg},
                    };

                    result.Result = await AjanlatBll.AjanlatKesztitesAsync(_context, sid, 
                        ap.ProjektKod, ap.AjanlatBuf, fi);

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
        public async Task<AjanlatParamResult> AjanlatCalc([FromQuery] string sid, 
            [FromBody] AjanlatParam ap)
        {
            var result = new AjanlatParamResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = AjanlatBll.AjanlatCalc(_context, sid, ap);

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