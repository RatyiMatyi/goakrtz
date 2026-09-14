using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace goakrtz
{
    internal class Program
    {
        class Gokart_palya
        {
            public string Nev { get; set; }
            public string Cim { get; set; }
            public string Telefonszam { get; set; }
            public string Weboldal { get; set; }

            public Gokart_palya(string nev, string cim, string telefonszam, string weboldal)
            {
                Nev = nev;
                Cim = cim;
                Telefonszam = telefonszam;
                Weboldal = weboldal;
            }

            public void KiirAdatok()
            {
                Console.WriteLine($"=== {Nev} ===");
                Console.WriteLine($"Cím: {Cim}");
                Console.WriteLine($"Telefonszám: {Telefonszam}");
                Console.WriteLine($"Weboldal: {Weboldal}");
                Console.WriteLine("--------------------------------------------------");
            }
        }

        class Versenyzo
        {
            public string vezetek_nev { get; set; }
            public string kereszt_nev { get; set; }
            public DateTime szul_datum { get; set; }
            public bool felnot_eves { get; set; }
            public string azonosito { get; set; }
            public string EmailCim { get; set; }

            public Versenyzo(string vNev, string kNev, DateTime szul)
            {
                vezetek_nev = vNev;
                kereszt_nev = kNev;
                szul_datum = szul;

                // 18. életév vizsgálata
                DateTime ma = DateTime.Now;
                int kor = ma.Year - szul_datum.Year;
                if (szul_datum.Date > ma.AddYears(-kor))
                {
                    kor--;
                }
                felnot_eves = kor >= 18;

                // Ékezetmentesítés az azonosítóhoz és e-mailhez
                string vMentes = EkezetMentesit(vezetek_nev);
                string kMentes = EkezetMentesit(kereszt_nev);

                // Azonosító: GO-KovacsDenes-19741204
                string datumStr = szul_datum.ToString("yyyyMMdd");
                azonosito = $"GO-{vMentes}{kMentes}-{datumStr}";

                // Email: kovacs.denes@gmail.com
                EmailCim = $"{vMentes.ToLower()}.{kMentes.ToLower()}@gmail.com";
            }

            private string EkezetMentesit(string szoveg)
            {
                string ekezetes = "áéíóöőúüűÁÉÍÓÖŐÚÜŰ";
                string mentes = "aeioouuuuAEIOOUUUU";

                StringBuilder sb = new StringBuilder();
                foreach (char c in szoveg)
                {
                    int index = ekezetes.IndexOf(c);
                    if (index != -1)
                        sb.Append(mentes[index]);
                    else
                        sb.Append(c);
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
            Console.WriteLine("==================================================");
            Console.WriteLine("BB-Gokart projekt 2026.09.07");
            Console.WriteLine("==================================================\n");

            Gokart_palya palya = new Gokart_palya(
                "Mokec racse verseny pálya",
                "Mokec racse verseny pálya, 6320 Vasút utca 4",
                "06 30 123 4567",
                "www.mokec.hu"
            );
            palya.KiirAdatok();

            // Fájlok beolvasása a speciális vesszős formátum szerint
            List<string> vezeteknevek = BeolvasFajl("vezeteknevek.txt", new string[] { "Kovács", "Nagy", "Szabó", "Tóth" });
            List<string> keresztnevek = BeolvasFajl("keresztnevek.txt", new string[] { "Dénes", "Anna", "Bence", "Katalin" });

            Random rand = new Random();
            int versenyoSzam = rand.Next(1, 151);
            List<Versenyzo> versenyzok = new List<Versenyzo>();

            for (int i = 0; i < versenyoSzam; i++)
            {
                string vNev = vezeteknevek[rand.Next(vezeteknevek.Count)];
                string kNev = keresztnevek[rand.Next(keresztnevek.Count)];

                int ev = rand.Next(1950, 2016);
                int honap = rand.Next(1, 13);
                int nap = rand.Next(1, DateTime.DaysInMonth(ev, honap) + 1);
                DateTime szuletesiIdo = new DateTime(ev, honap, nap);

                versenyzok.Add(new Versenyzo(vNev, kNev, szuletesiIdo));
            }

            Console.WriteLine($"Generált versenyzők száma: {versenyzok.Count}\n");
            foreach (var v in versenyzok)
            {
                v.Kiir();
            }

            Console.WriteLine("Nyomj meg egy gombot a kilépéshez...");
            Console.ReadKey();
        }

        // Fájlbeolvasó, ami a vesszővel és aposztróffal elválasztott neveket kezeli
        static List<string> BeolvasFajl(string fajlNev, string[] alapertelmezett)
        {
            if (File.Exists(fajlNev))
            {
                string teljesSzoveg = File.ReadAllText(fajlNev);

                // Vessző mentén feldaraboljuk a szöveget
                string[] elemek = teljesSzoveg.Split(',');
                List<string> tisztitottNevek = new List<string>();

                foreach (string elem in elemek)
                {
                    // Eltávolítjuk a szóközöket, idézőjeleket/aposztrófokat (')
                    string nev = elem.Trim().Trim('\'', '"');
                    if (!string.IsNullOrWhiteSpace(nev))
                    {
                        tisztitottNevek.Add(nev);
                    }
                }

                return tisztitottNevek.Count > 0 ? tisztitottNevek : new List<string>(alapertelmezett);
            }
            return new List<string>(alapertelmezett);
        }
    }
}