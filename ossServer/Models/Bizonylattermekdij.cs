using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Bizonylattermekdij
    {
        public int Bizonylattermekdijkod { get; set; }
        public int Particiokod { get; set; }
        public int Bizonylatkod { get; set; }
        public decimal Ossztomegkg { get; set; }
        public int Termekdijkod { get; set; }
        public string Termekdijkt { get; set; }
        public string Termekdijmegnevezes { get; set; }
        public decimal Termekdijegysegar { get; set; }
        public decimal Termekdij { get; set; }

        public virtual Bizonylat BizonylatkodNavigation { get; set; }
        public virtual Particio ParticiokodNavigation { get; set; }
        public virtual Termekdij TermekdijkodNavigation { get; set; }
    }
}
