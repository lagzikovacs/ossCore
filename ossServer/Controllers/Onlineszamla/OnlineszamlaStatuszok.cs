using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Onlineszamla
{
    public enum OnlineszamlaStatuszok
    {
        TokenKeres = 0,
        Feltoltes = 1,
        Feltoltve = 2,
        KeszEmail = 3,
        Kesz = 4,
        HibaEmail = 5,
        Hiba = 6
    }

    public static class OnlineszamlaStatuszNevek
    {
        public static string[] Member = {
            "Tokenkérés", "Feltöltés", "Feltöltve", "Kész, email", "Kész", "Hiba, email", "Hiba"
        };
    }
}
