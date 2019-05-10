using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Projekt
{
    public class ProjektDto
    {
        public int Projektkod { get; set; }
        public int Statusz { get; set; }
        public string Muszakiallapot { get; set; }
        public int Ugyfelkod { get; set; }
        public string Ugyfelnev { get; set; }
        public string Ugyfelcim { get; set; }
        public string Ugyfeltelefonszam { get; set; }
        public string Ugyfelemail { get; set; }
        public string Telepitesicim { get; set; }
        public string Projektjellege { get; set; }
        public string Inverter { get; set; }
        public string Inverterallapot { get; set; }
        public string Napelem { get; set; }
        public string Napelemallapot { get; set; }
        public decimal Dckw { get; set; }
        public decimal Ackva { get; set; }
        public decimal Vallalasiarnetto { get; set; }
        public int Penznemkod { get; set; }
        public string Penznem { get; set; }
        public string Munkalapszam { get; set; }
        public DateTime Keletkezett { get; set; }
        public DateTime? Megrendelve { get; set; }
        public DateTime? Kivitelezesihatarido { get; set; }
        public string Megjegyzes { get; set; }


        public System.DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public System.DateTime Modositva { get; set; }
        public string Modositotta { get; set; }
    }
}
