using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.Csoport
{
    public class LehetsegesJogResult : EmptyResult
    {
        public List<LehetsegesJogDto> Result { get; set; }
    }
}
