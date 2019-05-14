using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Bizonylat
{
    public class Azaz
    {
        private static string Szovegge_EgeszResz_Csoport(int csoport)
        {
            // Csoport = 0...999

            string szazasSzoveggel = string.Empty;
            int szazas = csoport / 100;

            switch (szazas)
            {
                case 0:
                    break;
                case 1:
                    szazasSzoveggel = "egy";
                    break;
                case 2:
                    szazasSzoveggel = "kettő";
                    break;
                case 3:
                    szazasSzoveggel = "három";
                    break;
                case 4:
                    szazasSzoveggel = "négy";
                    break;
                case 5:
                    szazasSzoveggel = "öt";
                    break;
                case 6:
                    szazasSzoveggel = "hat";
                    break;
                case 7:
                    szazasSzoveggel = "hét";
                    break;
                case 8:
                    szazasSzoveggel = "nyolc";
                    break;
                case 9:
                    szazasSzoveggel = "kilenc";
                    break;
            }
            if (szazasSzoveggel != string.Empty)
                szazasSzoveggel += "száz";

            var tizes = (csoport - szazas * 100) / 10;
            var egyes = csoport % 10;
            var tizesSzoveggel = string.Empty;

            switch (tizes)
            {
                case 0:
                    break;
                case 1:
                    tizesSzoveggel = egyes == 0 ? "tíz" : "tizen";
                    break;
                case 2:
                    tizesSzoveggel = egyes == 0 ? "húsz" : "huszon";
                    break;
                case 3:
                    tizesSzoveggel = "harminc";
                    break;
                case 4:
                    tizesSzoveggel = "negyven";
                    break;
                case 5:
                    tizesSzoveggel = "ötven";
                    break;
                case 6:
                    tizesSzoveggel = "hatvan";
                    break;
                case 7:
                    tizesSzoveggel = "hetven";
                    break;
                case 8:
                    tizesSzoveggel = "nyolcvan";
                    break;
                case 9:
                    tizesSzoveggel = "kilencven";
                    break;
            }

            var egyesSzoveggel = string.Empty;
            if (egyes > 0)
                switch (egyes)
                {
                    case 0:
                        break;
                    case 1:
                        egyesSzoveggel = "egy";
                        break;
                    case 2:
                        egyesSzoveggel = "kettő";
                        break;
                    case 3:
                        egyesSzoveggel = "három";
                        break;
                    case 4:
                        egyesSzoveggel = "négy";
                        break;
                    case 5:
                        egyesSzoveggel = "öt";
                        break;
                    case 6:
                        egyesSzoveggel = "hat";
                        break;
                    case 7:
                        egyesSzoveggel = "hét";
                        break;
                    case 8:
                        egyesSzoveggel = "nyolc";
                        break;
                    case 9:
                        egyesSzoveggel = "kilenc";
                        break;
                }

            string result = szazasSzoveggel + tizesSzoveggel + egyesSzoveggel;
            return result;
        }

        private static string Szovegge_EgeszResz(decimal szam)
        {
            // AAABBBCCCDDD
            // AAA = milliárdos csoport
            // BBB = milliós csoport
            // CCC = ezres csoport
            // DDD = százas csoport
            string egeszResz;
            if (szam == 0)
                egeszResz = "nulla";
            else
            {
                var szazasCsoport = Szovegge_EgeszResz_Csoport(Convert.ToInt32(Math.Truncate(szam % 1000)));
                var ezresCsoport = Szovegge_EgeszResz_Csoport(Convert.ToInt32(Math.Truncate(szam / 1000) % 1000));
                if (ezresCsoport.Length > 0)
                    ezresCsoport += "ezer";
                var milliosCsoport = Szovegge_EgeszResz_Csoport(Convert.ToInt32(Math.Truncate(szam / 1000000) % 1000));
                if (milliosCsoport.Length > 0)
                    milliosCsoport += "millió";
                var milliardosCsoport = Szovegge_EgeszResz_Csoport(Convert.ToInt32(Math.Truncate(szam / 1000000000) % 1000));
                if (milliardosCsoport.Length > 0)
                    milliardosCsoport += "milliárd";

                if (szam < 2000)
                    egeszResz = ezresCsoport + szazasCsoport;
                else
                {
                    egeszResz = $"{milliardosCsoport}-{milliosCsoport}-{ezresCsoport}-{szazasCsoport}";
                    egeszResz = egeszResz.Replace("--", "-");
                    if (egeszResz.StartsWith("-"))
                        egeszResz = egeszResz.Remove(0, 1);
                    if (egeszResz.EndsWith("-"))
                        egeszResz = egeszResz.Remove(egeszResz.Length - 1, 1);
                }
            }
            return egeszResz;
        }

        private static string Szovegge_TortResz(int tort)
        {
            // 00..99
            var tized = tort / 10;
            var szazad = tort % 10;
            var szoveggel = string.Empty;
            switch (tized)
            {
                case 0:
                    break;
                case 1:
                    szoveggel = szazad == 0 ? "egy" : "tizen";
                    break;
                case 2:
                    szoveggel = szazad == 0 ? "kettő" : "huszon";
                    break;
                case 3:
                    szoveggel = szazad == 0 ? "három" : "harminc";
                    break;
                case 4:
                    szoveggel = szazad == 0 ? "négy" : "negyven";
                    break;
                case 5:
                    szoveggel = szazad == 0 ? "öt" : "ötven";
                    break;
                case 6:
                    szoveggel = szazad == 0 ? "hat" : "hatvan";
                    break;
                case 7:
                    szoveggel = szazad == 0 ? "hét" : "hetven";
                    break;
                case 8:
                    szoveggel = szazad == 0 ? "nyolc" : "nyolcvan";
                    break;
                case 9:
                    szoveggel = szazad == 0 ? "kilenc" : "kilencven";
                    break;
            }

            switch (szazad)
            {
                case 0:
                    szoveggel += " tized";
                    break;
                case 1:
                    szoveggel += "egy század";
                    break;
                case 2:
                    szoveggel += "kettő század";
                    break;
                case 3:
                    szoveggel += "három század";
                    break;
                case 4:
                    szoveggel += "négy század";
                    break;
                case 5:
                    szoveggel += "öt század";
                    break;
                case 6:
                    szoveggel += "hat század";
                    break;
                case 7:
                    szoveggel += "hét század";
                    break;
                case 8:
                    szoveggel += "nyolc század";
                    break;
                case 9:
                    szoveggel += "kilenc század";
                    break;
            }

            return szoveggel;
        }

        public static string Szovegge(decimal szam)
        {
            var negativ = szam < 0;
            szam = Math.Abs(szam);
            var egesz = Convert.ToInt32(Math.Truncate(szam));
            var tort = Convert.ToInt32((szam - egesz) * 100); // Két tizedig akarunk úgyis csak kiírni
            var egeszResz = Szovegge_EgeszResz(egesz);
            var tortResz = tort == 0 ? string.Empty : Szovegge_TortResz(tort);

            string result;
            if (egeszResz.Length > 0 && tortResz.Length > 0)
                result = $"{egeszResz} egész {tortResz}";
            else if (egeszResz.Length == 0)
                result = $"nulla egész {tortResz}";
            else
                result = egeszResz;

            if (negativ)
                result = "mínusz " + result;
            //result = char.ToUpper(result[0]) + result.Substring(1, result.Length - 1);
            return result;
        }
    }
}
