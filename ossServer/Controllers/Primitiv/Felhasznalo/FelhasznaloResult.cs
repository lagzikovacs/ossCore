using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.Primitiv.Felhasznalo
{
    public class FelhasznaloResult : EmptyResult
    {
        public List<FelhasznaloDto> Result { get; set; }
    }
}
