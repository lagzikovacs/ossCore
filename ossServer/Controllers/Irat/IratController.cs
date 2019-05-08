using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ossServer.Controllers.Irat
{
    [Route("api/[controller]")]
    [ApiController]
    public class IratController : ControllerBase
    {
        [HttpPost]
        [Route("api/" + Name + "/" + nameof(Add))]
        public async Task<Int32Result> Add([FromUri] string sid, [FromBody] IratDto dto)
        {
            var result = new Int32Result();
            var task = new Task<Int32Result>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));
                  if (dto == null)
                      throw new ArgumentNullException(nameof(dto));

                  result.Result = new IratBll(sid).Add(dto);
              })
            );
            task.Start();
            return await task;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/" + Name + "/" + nameof(CreateNew))]
        public async Task<IratResult> CreateNew([FromUri] string sid)
        {
            var result = new IratResult();
            var task = new Task<IratResult>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));

                  result.Result = new List<IratDto> { new IratBll(sid).CreateNew() };
              })
            );
            task.Start();
            return await task;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/" + Name + "/" + nameof(Delete))]
        public async Task<EmptyResult> Delete([FromUri] string sid, [FromBody] IratDto dto)
        {
            var result = new EmptyResult();
            var task = new Task<EmptyResult>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));
                  if (dto == null)
                      throw new ArgumentNullException(nameof(dto));

                  new IratBll(sid).Delete(dto);
              })
            );
            task.Start();
            return await task;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/" + Name + "/" + nameof(Get))]
        public async Task<IratResult> Get([FromUri] string sid, [FromBody] int key)
        {
            var result = new IratResult();
            var task = new Task<IratResult>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));

                  result.Result = new List<IratDto> { new IratBll(sid).Get(key) };
              })
            );
            task.Start();
            return await task;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/" + Name + "/" + nameof(Update))]
        public async Task<Int32Result> Update([FromUri] string sid, [FromBody] IratDto dto)
        {
            var result = new Int32Result();
            var task = new Task<Int32Result>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));
                  if (dto == null)
                      throw new ArgumentNullException(nameof(dto));

                  result.Result = new IratBll(sid).Update(dto);
              })
            );
            task.Start();
            return await task;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/" + Name + "/" + nameof(Select))]
        public async Task<IratResult> Select([FromUri] string sid, [FromBody] IratParam par)
        {
            var result = new IratResult();
            var task = new Task<IratResult>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));
                  if (par == null)
                      throw new ArgumentNullException(nameof(par));

                  result.Result = new IratBll(sid).Select(par.RekordTol, par.LapMeret, par.Fi, out var osszesRekord);
                  result.OsszesRekord = osszesRekord;
              })
            );
            task.Start();
            return await task;
        }
    }
}