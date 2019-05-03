using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Csoport
{
    public class CsoportDto
    {
        public int Csoportkod { get; set; }
        public int Particiokod { get; set; }
        public string Particiomegnevezes { get; set; }
        public string Csoport1 { get; set; }

        public System.DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public System.DateTime Modositva { get; set; }
        public string Modositotta { get; set; }
    }
}
