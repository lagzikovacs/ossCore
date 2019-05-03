using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Logon
{
    public class LogonParameter
    {
        public string Azonosito { get; set; }
        public string Jelszo { get; set; }
        public string Ip { get; set; }
        public string WinHost { get; set; }
        public string WinUser { get; set; }
    }
}
