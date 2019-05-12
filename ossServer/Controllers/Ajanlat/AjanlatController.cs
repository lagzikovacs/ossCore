using Microsoft.AspNetCore.Mvc;
using ossServer.BaseResults;
using ossServer.Models;
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
            var task = new Task<AjanlatParamResult>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));

                  result.Result = new AjanlatBll(sid).CreateNew();
              })
            );
            task.Start();
            return await task;
        }

        [HttpPost]
        public async Task<Int32Result> AjanlatKeszites([FromQuery] string sid, [FromBody] AjanlatParam ap)
        {
            var result = new Int32Result();
            var task = new Task<Int32Result>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));
                  if (ap == null)
                      throw new ArgumentNullException(nameof(ap));

            // TODO: ez csak hack...
            var fi = new List<SzMT> {
            new SzMT {Szempont = Szempont.Ervenyes, Minta = ap.Ervenyes},
            new SzMT {Szempont = Szempont.Tajolas, Minta = ap.Tajolas},
            new SzMT {Szempont = Szempont.Termeles, Minta = ap.Termeles},
            new SzMT {Szempont = Szempont.Megjegyzes, Minta = ap.Megjegyzes},
            new SzMT {Szempont = Szempont.SzuksegesAramerosseg, Minta = ap.SzuksegesAramerosseg},
                };

                  result.Result = new AjanlatBll(sid).AjanlatKesztites(ap.ProjektKod, ap.AjanlatBuf, fi);
              })
            );
            task.Start();
            return await task;
        }

        [HttpPost]
        public async Task<AjanlatParamResult> AjanlatCalc([FromQuery] string sid, 
            [FromBody] AjanlatParam ap)
        {
            var result = new AjanlatParamResult();
            var task = new Task<AjanlatParamResult>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));
                  if (ap == null)
                      throw new ArgumentNullException(nameof(ap));

                  result.Result = new AjanlatBll(sid).AjanlatCalc(ap);
              })
            );
            task.Start();
            return await task;
        }
    }
}