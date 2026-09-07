using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace goakrtz
{
    internal class Program
    {
        class Gokart_palya
        {
            public string palya_nev = "Mokec racse verseny pálya";
            public string palya_cime = "Mokec racse verseny pálya, 6320 Vasút utca 4";
            public string telefonszam = "06 30 123 4567";
            public string weblap = "www.mokec.hu";

            public void Palya_adatok()
            {
                Console.WriteLine("Pálya neve: " + palya_nev);
                Console.WriteLine("Pálya címe: " + palya_cime);
                Console.WriteLine("Telefonszám: " + telefonszam);
                Console.WriteLine("Weblap: " + weblap);
            }
        }
        class Versenyzo
        {
            public string vezetek_nev { get; set; }
            public string kereszt_nev { get; set; }
            public string szul_datum { get; set; }
            public bool felnot_eves { get; set; } = false;
            public string azonosito { get; set; }

        }
        static void Main(string[] args)
        {
            /* Barta Bulcsu 
             * Gokart projekt 2026.09.07*/
            Console.WriteLine("BB-Gokart projekt 2026.09.07");
            string palya_nev = "Mokec racse verseny pálya";
            string palya_cime = "Mokec racse verseny pálya, 6320 Vasút utca 4";
            string telefonszam = "06 30 123 4567";
            string weblap = "www.mokec.hu";


        }
    }
}
