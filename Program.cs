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
            public DateTime szul_datum { get; set; }
            public bool felnot_eves { get; set; } = false;
            public string azonosito { get; set; }
            public string EmailCim { get; set; }

            public Versenyzo(string vezetek_nev, string kereszt_nev, DateTime szul_datum, bool felnot_eves, string azonosito)
            {
                this.vezetek_nev = vezetek_nev;
                this.kereszt_nev = kereszt_nev;
                this.szul_datum = szul_datum;

                DateTime ma = DateTime.Now;
                int kor = ma.Year - szul_datum.Year;
                if (szul_datum.Date > ma.AddYears(-kor))
                {
                    kor--;
                }
                felnot_eves = kor >= 18;

                string ekezetmentesVezeteknev = EkezetMentesit(vezetek_nev);
                string ekezetmentesKeresztnev = EkezetMentesit(kereszt_nev);



                // Azonosító generálása: GO-KovacsDenes-19741204
                string datumFormatum = szul_datum.ToString("yyyyMMdd");
                azonosito = $"GO-{ekezetmentesVezeteknev}{ekezetmentesKeresztnev}-{datumFormatum}";

                // Email cím generálása: kovacs.denes@gmail.com
                EmailCim = $"{ekezetmentesVezeteknev.ToLower()}.{ekezetmentesKeresztnev.ToLower()}@gmail.com";
            }
            

                private string EkezetMentesit(string szoveg)
            {
                string ekezetes = "áéíóöőúüűÁÉÍÓÖŐÚÜŰ";
                string mentes = "aeioouuuuAEIOOUUUU";

                StringBuilder sb = new StringBuilder(szoveg);
                for (int i = 0; i < ekezetes.Length; i++)
                {
                    sb.Replace(ekezetes[i], mentes[i]);
                }
                return sb.ToString();
            }
            public void Kiir()
            {
                Console.WriteLine($"Azonosító: {azonosito}");
                Console.WriteLine($"Név: {vezetek_nev} {kereszt_nev}");
                Console.WriteLine($"Születési idő: {szul_datum:yyyy.MM.dd}");
                Console.WriteLine($"Elmúlt 18 éves: {(felnot_eves ? "Igen" : "Nem")}");
                Console.WriteLine($"E-mail: {EmailCim}");
                Console.WriteLine("--------------------------------------------------");
            }
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
