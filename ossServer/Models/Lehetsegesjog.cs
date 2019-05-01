using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Lehetsegesjog
    {
        public Lehetsegesjog()
        {
            Csoportjog = new HashSet<Csoportjog>();
        }

        public int Lehetsegesjogkod { get; set; }
        public string Jogkod { get; set; }
        public string Jog { get; set; }

        public virtual ICollection<Csoportjog> Csoportjog { get; set; }
    }
}
