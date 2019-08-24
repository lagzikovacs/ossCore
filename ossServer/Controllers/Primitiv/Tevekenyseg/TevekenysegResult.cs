using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.Primitiv.Tevekenyseg
{
    public class TevekenysegResult : EmptyResult
    {
        public List<TevekenysegDto> Result { get; set; }
    }
}
