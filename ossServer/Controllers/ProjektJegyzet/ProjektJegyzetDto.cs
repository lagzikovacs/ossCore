﻿using System;

namespace ossServer.Controllers.ProjektJegyzet
{
    public class ProjektJegyzetDto
    {
        public int Projektjegyzetkod { get; set; }
        public int Projektkod { get; set; }
        public string Leiras { get; set; }
        public System.DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public System.DateTime Modositva { get; set; }
        public string Modositotta { get; set; }
    }
}
