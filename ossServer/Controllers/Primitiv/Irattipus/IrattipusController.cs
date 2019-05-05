﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ossServer.Models;

namespace ossServer.Controllers.Primitiv.Irattipus
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IrattipusController : ControllerBase
    {
        private readonly ossContext _context;

        public IrattipusController(ossContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("api/" + Name + "/" + nameof(Add))]
        public async Task<Int32Result> Add([FromUri] string sid, [FromBody] IratTipusDto dto)
        {
            var result = new Int32Result();
            var task = new Task<Int32Result>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));
                  if (dto == null)
                      throw new ArgumentNullException(nameof(dto));

                  result.Result = new IratTipusBll(sid).Add(dto);
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
        public async Task<IratTipusResult> CreateNew([FromUri] string sid)
        {
            var result = new IratTipusResult();
            var task = new Task<IratTipusResult>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));

                  result.Result = new List<IratTipusDto> { new IratTipusBll(sid).CreateNew() };
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
        public async Task<EmptyResult> Delete([FromUri] string sid, [FromBody] IratTipusDto dto)
        {
            var result = new EmptyResult();
            var task = new Task<EmptyResult>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));
                  if (dto == null)
                      throw new ArgumentNullException(nameof(dto));

                  new IratTipusBll(sid).Delete(dto);
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
        public async Task<IratTipusResult> Get([FromUri] string sid, [FromBody] int key)
        {
            var result = new IratTipusResult();
            var task = new Task<IratTipusResult>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));

                  result.Result = new List<IratTipusDto> { new IratTipusBll(sid).Get(key) };
              })
            );
            task.Start();
            return await task;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="maszk"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/" + Name + "/" + nameof(Read))]
        public async Task<IratTipusResult> Read([FromUri] string sid, [FromBody] string maszk)
        {
            var result = new IratTipusResult();
            var task = new Task<IratTipusResult>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));
                  if (maszk == null)
                      throw new ArgumentNullException(nameof(maszk));

                  result.Result = new IratTipusBll(sid).Read(maszk);
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
        public async Task<Int32Result> Update([FromUri] string sid, [FromBody] IratTipusDto dto)
        {
            var result = new Int32Result();
            var task = new Task<Int32Result>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));
                  if (dto == null)
                      throw new ArgumentNullException(nameof(dto));

                  result.Result = new IratTipusBll(sid).Update(dto);
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
        [Route("api/" + Name + "/" + nameof(ZoomCheck))]
        public async Task<EmptyResult> ZoomCheck([FromUri] string sid, [FromBody] IrattipusZoomParameter par)
        {
            var result = new EmptyResult();
            var task = new Task<EmptyResult>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));
                  if (par == null)
                      throw new ArgumentNullException(nameof(par));

                  new IratTipusBll(sid).ZoomCheck(par.Irattipuskod, par.Irattipus);
              })
            );
            task.Start();
            return await task;
        }
    }
}