using ossServer.Utils;
using System;
using System.Collections.Generic;

namespace ossServer.Controllers.Ajanlat
{
    public class AjanlatParam
    {
        public int ProjektKod { get; set; }
        public DateTime Ervenyes { get; set; }
        public string Tajolas { get; set; }
        public int Termeles { get; set; }
        public string Megjegyzes { get; set; }
        public string SzuksegesAramerosseg { get; set; }
        public List<AjanlatBuf> AjanlatBuf { get; set; }
        public decimal Netto { get; set; }
        public decimal Afa { get; set; }
        public decimal Brutto { get; set; }

        //TODO: csak webnél nem kell
        public List<SzMT> Fi { get; set; }
    }
}
