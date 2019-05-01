using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Bizonylatafa
    {
        public int Bizonylatafakod { get; set; }
        public int Particiokod { get; set; }
        public int Bizonylatkod { get; set; }
        public int Afakulcskod { get; set; }
        public string Afakulcs { get; set; }
        public decimal Afamerteke { get; set; }
        public decimal Netto { get; set; }
        public decimal Afa { get; set; }
        public decimal Brutto { get; set; }

        public virtual Afakulcs AfakulcskodNavigation { get; set; }
        public virtual Bizonylat BizonylatkodNavigation { get; set; }
        public virtual Particio ParticiokodNavigation { get; set; }
    }
}
