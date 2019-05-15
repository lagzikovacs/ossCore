﻿using System;

namespace ossServer.Controllers.Riport
{
    public class KeszletDto
    {
        public int Cikkkod { get; set; }
        public string Cikk { get; set; }
        public string Me { get; set; }
        public decimal Keszlet { get; set; }
        public DateTime? Utolsobevet { get; set; }
        public decimal Utolsoar { get; set; }
        public string Utolsoarpenzneme { get; set; }
        public decimal Utolsoarforintban { get; set; }
        public int Beszerzesekszama { get; set; }
        public decimal Keszletertek { get; internal set; }
    }
}