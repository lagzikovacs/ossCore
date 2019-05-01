using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Volume
    {
        public Volume()
        {
            Dokumentum = new HashSet<Dokumentum>();
        }

        public int Volumekod { get; set; }
        public int Particiokod { get; set; }
        public int Volumeno { get; set; }
        public string Eleresiut { get; set; }
        public int Maxmeret { get; set; }
        public int Jelenlegimeret { get; set; }
        public int Utolsokonyvtar { get; set; }
        public int Fajlokszamautolsokonyvtarban { get; set; }
        public string Allapot { get; set; }
        public DateTime Allapotkelte { get; set; }

        public virtual Particio ParticiokodNavigation { get; set; }
        public virtual ICollection<Dokumentum> Dokumentum { get; set; }
    }
}
