﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ossServer.Models;

namespace ossServer.Controllers.Primitiv.Me
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MennyisegiegysegController : ControllerBase
    {
        private readonly ossContext _context;

        public MennyisegiegysegController(ossContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("api/" + Name + "/" + nameof(Add))]
        public async Task<Int32Result> Add([FromUri] string sid, [FromBody] MennyisegiEgysegDto dto)
        {
            var result = new Int32Result();
            var task = new Task<Int32Result>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));
                  if (dto == null)
                      throw new ArgumentNullException(nameof(dto));

                  result.Result = new MennyisegiEgysegBll(sid).Add(dto);
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
        public async Task<MennyisegiEgysegResult> CreateNew([FromUri] string sid)
        {
            var result = new MennyisegiEgysegResult();
            var task = new Task<MennyisegiEgysegResult>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));

                  result.Result = new List<MennyisegiEgysegDto> { new MennyisegiEgysegBll(sid).CreateNew() };
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
        public async Task<EmptyResult> Delete([FromUri] string sid, [FromBody] MennyisegiEgysegDto dto)
        {
            var result = new EmptyResult();
            var task = new Task<EmptyResult>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));
                  if (dto == null)
                      throw new ArgumentNullException(nameof(dto));

                  new MennyisegiEgysegBll(sid).Delete(dto);
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
        public async Task<MennyisegiEgysegResult> Get([FromUri] string sid, [FromBody] int key)
        {
            var result = new MennyisegiEgysegResult();
            var task = new Task<MennyisegiEgysegResult>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));

                  result.Result = new List<MennyisegiEgysegDto> { new MennyisegiEgysegBll(sid).Get(key) };
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
        public async Task<MennyisegiEgysegResult> Read([FromUri] string sid, [FromBody] string maszk)
        {
            var result = new MennyisegiEgysegResult();
            var task = new Task<MennyisegiEgysegResult>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));
                  if (maszk == null)
                      throw new ArgumentNullException(nameof(maszk));

                  result.Result = new MennyisegiEgysegBll(sid).Read(maszk);
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
        public async Task<Int32Result> Update([FromUri] string sid, [FromBody] MennyisegiEgysegDto dto)
        {
            var result = new Int32Result();
            var task = new Task<Int32Result>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));
                  if (dto == null)
                      throw new ArgumentNullException(nameof(dto));

                  result.Result = new MennyisegiEgysegBll(sid).Update(dto);
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
        public async Task<EmptyResult> ZoomCheck([FromUri] string sid, [FromBody] MeZoomParameter par)
        {
            var result = new EmptyResult();
            var task = new Task<EmptyResult>(() =>
              CEUtils.CatchException(result, () =>
              {
                  if (sid == null)
                      throw new ArgumentNullException(nameof(sid));
                  if (par == null)
                      throw new ArgumentNullException(nameof(par));

                  new MennyisegiEgysegBll(sid).ZoomCheck(par.Mekod, par.Me);
              })
            );
            task.Start();
            return await task;
        }
    }
}