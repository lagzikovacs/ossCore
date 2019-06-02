using Microsoft.AspNetCore.Mvc;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Threading.Tasks;

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

        [HttpPost]
        public async Task<VolumeResult> Read([FromQuery] string sid)
        {
            var result = new VolumeResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = VolumeBll.Read(_context, sid);

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
        public async Task<DokumentumkodByVolumeResult> DokumentumkodByVolume([FromQuery] string sid, 
            [FromBody] int volumeKod)
        {
            var result = new DokumentumkodByVolumeResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = VolumeBll.DokumentumkodByVolume(_context, sid, volumeKod);

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
        public async Task<ColumnSettingsResult> GetGridSettings([FromQuery] string sid)
        {
            var result = new ColumnSettingsResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = VolumeBll.GridSettings(_context, sid);

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
        public async Task<ColumnSettingsResult> GetReszletekSettings([FromQuery] string sid)
        {
            var result = new ColumnSettingsResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = VolumeBll.ReszletekSettings(_context, sid);

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