using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Fajlrendszer
    {
        public int Fajlrendszerkod { get; set; }
        public int Particiokod { get; set; }

        public virtual Particio ParticiokodNavigation { get; set; }
    }
}
