using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Kodgenerator
    {
        public int Id { get; set; }
        public int Particiokod { get; set; }
        public string Kodnev { get; set; }
        public int Kovetkezokod { get; set; }

        public virtual Particio ParticiokodNavigation { get; set; }
    }
}
