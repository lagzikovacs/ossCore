using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Irat
    {
        public Irat()
        {
            Bizonylatkapcsolat = new HashSet<Bizonylatkapcsolat>();
            Dokumentum = new HashSet<Dokumentum>();
            Projektkapcsolat = new HashSet<Projektkapcsolat>();
        }

        public int Iratkod { get; set; }
        public int Particiokod { get; set; }
        public string Irany { get; set; }
        public DateTime Keletkezett { get; set; }
        public int Irattipuskod { get; set; }
        public int? Ugyfelkod { get; set; }
        public string Kuldo { get; set; }
        public string Targy { get; set; }
        public string Kikuldesikod { get; set; }
        public DateTime? Kikuldesikodidopontja { get; set; }
        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public DateTime Modositva { get; set; }
        public string Modositotta { get; set; }

        public virtual Irattipus IrattipuskodNavigation { get; set; }
        public virtual Particio ParticiokodNavigation { get; set; }
        public virtual Ugyfel UgyfelkodNavigation { get; set; }
        public virtual ICollection<Bizonylatkapcsolat> Bizonylatkapcsolat { get; set; }
        public virtual ICollection<Dokumentum> Dokumentum { get; set; }
        public virtual ICollection<Projektkapcsolat> Projektkapcsolat { get; set; }
    }
}
