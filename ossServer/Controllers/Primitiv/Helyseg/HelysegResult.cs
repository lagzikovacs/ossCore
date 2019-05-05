using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.Primitiv.Helyseg
{
    public class HelysegResult : EmptyResult
    {
        public List<HelysegDto> Result { get; set; }
    }
}
