using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Navfeltoltes
    {
        public int Navfeltolteskod { get; set; }
        public DateTime Idopont { get; set; }
        public int Particiokod { get; set; }
        public int Bizonylatkod { get; set; }
        public int Statusz { get; set; }
        public string Token { get; set; }
        public string Tranzakcioazonosito { get; set; }
        public string Hiba { get; set; }
        public DateTime Kovetkezoteendoidopont { get; set; }
        public int Tokenkeresszamlalo { get; set; }
        public int Feltoltesszamlalo { get; set; }
        public int Feltoltesellenorzesszamlalo { get; set; }
        public int Emailszamlalo { get; set; }
        public DateTime? Elintezve { get; set; }
        public string Elintezte { get; set; }
        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public DateTime Modositva { get; set; }
        public string Modositotta { get; set; }

        public virtual Bizonylat BizonylatkodNavigation { get; set; }
    }
}
