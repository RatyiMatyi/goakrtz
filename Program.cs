using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        // Időpontfoglalási rendszert kezelő osztály
        class FoglalasiRendszer
        {
            // Kulcs: Év-Hónap-Nap-Óra (pl. "2026-09-26-15"), Érték: azon versenyzők azonosítói, akik abban az órában lefoglalták a pályát
            private Dictionary<string, List<string>> foglalasok = new Dictionary<string, List<string>>();

            // Megjeleníti a hónap végéig fennmaradó napok időszalagját
            public void IdoszalagMegjelenites()
            {
                DateTime ma = DateTime.Now;
                int napokAHonapban = DateTime.DaysInMonth(ma.Year, ma.Month);

                Console.WriteLine("\n=== FOGLALÁSI IDŐSZALAG (Zöld = Szabad | Piros = Megtelt) ===\n");

                // 1. Táblázat Felső Kerete
                Console.Write("┌────────────");
                for (int ora = 8; ora < 19; ora++)
                {
                    Console.Write("┬───────");
                }
                Console.WriteLine("┐");

                // 2. Fejléc Kiíratása (Idősávok)
                Console.Write("│ DÁTUM      ");
                for (int ora = 8; ora < 19; ora++)
                {
                    string idosav = $"{ora:00}-{ora + 1:00}";
                    Console.Write($"│ {idosav} ");
                }
                Console.WriteLine("│");

                // 3. Fejléc alatti elválasztó
                Console.Write("├────────────");
                for (int ora = 8; ora < 19; ora++)
                {
                    Console.Write("┼───────");
                }
                Console.WriteLine("┤");

                // 4. Adatsorok kiíratása (Napok)
                for (int nap = ma.Day; nap <= napokAHonapban; nap++)
                {
                    DateTime aktualisNap = new DateTime(ma.Year, ma.Month, nap);
                    Console.Write($"│ {aktualisNap:yyyy.MM.dd} ");

                    for (int ora = 8; ora < 19; ora++)
                    {
                        string kulcs = $"{aktualisNap:yyyy-MM-dd}-{ora}";
                        int letszam = foglalasok.ContainsKey(kulcs) ? foglalasok[kulcs].Count : 0;

                        Console.Write("│");

                        // Színkódolt cella kirajzolása
                        if (letszam > 0)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkRed;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write($" {letszam,2}/20 "); // Kiírja pl. " 3/20"
                        }
                        else
                        {
                            // ZÖLD CELLA (Szabad)
                            Console.BackgroundColor = ConsoleColor.DarkGreen;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(" SZABAD");
                        }

                        // Háttérszín alaphelyzetbe állítása a szegélyhez
                        Console.ResetColor();
                    }
                    Console.WriteLine("│");

                    // Sorok közötti vízszintes elválasztó (az utolsó sor kivételével)
                    if (nap < napokAHonapban)
                    {
                        Console.Write("├────────────");
                        for (int ora = 8; ora < 19; ora++)
                        {
                            Console.Write("┼───────");
                        }
                        Console.WriteLine("┤");
                    }
                }

                // 5. Táblázat Alsó Kerete
                Console.Write("└────────────");
                for (int ora = 8; ora < 19; ora++)
                {
                    Console.Write("┴───────");
                }
                Console.WriteLine("┘\n");
            }

            // Új foglalás beállítása vagy módosítása
            public bool FoglalassHozzaadasa(string azonosito, DateTime datum, int kezdOora, int orakSzama, bool csendes = false)
            {
                // Szabályok ellenőrzése
                if (kezdOora < 8 || kezdOora + orakSzama > 19)
                {
                    if (!csendes) Console.WriteLine("HIBA: A nyitvatartási idő 8:00 és 19:00 között van!");
                    return false;
                }

                if (orakSzama < 1 || orakSzama > 2)
                {
                    if (!csendes) Console.WriteLine("HIBA: Minimum 1, maximum 2 összefüggő órát lehet foglalni!");
                    return false;
                }

                // Létszámkorlát ellenőrzése (Max 20 fő/óra)
                for (int i = 0; i < orakSzama; i++)
                {
                    string kulcs = $"{datum:yyyy-MM-dd}-{kezdOora + i}";
                    int létszám = foglalasok.ContainsKey(kulcs) ? foglalasok[kulcs].Count : 0;
                    if (létszám >= 20)
                    {
                        if (!csendes) Console.WriteLine($"HIBA: A(z) {datum:MM.dd.} {kezdOora + i}:00 idősáv megtelt (max 20 fő)!");
                        return false;
                    }
                }

                // Korábbi foglalások törlése az adott versenyzőnél (átállítás funkció)
                TorolVersenyzoFoglalásai(azonosito);

                // Új foglalások rögzítése
                for (int i = 0; i < orakSzama; i++)
                {
                    string kulcs = $"{datum:yyyy-MM-dd}-{kezdOora + i}";
                    if (!foglalasok.ContainsKey(kulcs))
                    {
                        foglalasok[kulcs] = new List<string>();
                    }
                    foglalasok[kulcs].Add(azonosito);
                }

                if (!csendes)
                {
                    Console.WriteLine($"\nSIKER: {azonosito} foglalása rögzítve ({datum:MM.dd.} {kezdOora:00}:00 - {kezdOora + orakSzama:00}:00)!");
                }

                return true;
            }

            private void TorolVersenyzoFoglalásai(string azonosito)
            {
                foreach (var kulcs in foglalasok.Keys)
                {
                    foglalasok[kulcs].Remove(azonosito);
                }
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

            // 1. Versenyzők adatai kilistázása
            Console.WriteLine($"\n--- GENERÁLT VERSENYZŐK LISTÁJA ({versenyzok.Count} fő) ---");
            foreach (var v in versenyzok)
            {
                v.Kiir();
            }

            FoglalasiRendszer rendszer = new FoglalasiRendszer();

            // Véletlenszerűen lefoglalunk 5-15 időpontot
            int randomFoglalasokSzama = rand.Next(5, 16);
            DateTime maiNap = DateTime.Now;
            int napokAHonapban = DateTime.DaysInMonth(maiNap.Year, maiNap.Month);

            for (int i = 0; i < randomFoglalasokSzama; i++)
            {
                Versenyzo rVersenyzo = versenyzok[rand.Next(versenyzok.Count)];
                int rNap = rand.Next(maiNap.Day, napokAHonapban + 1);
                int rOra = rand.Next(8, 18);
                int rOrakSzama = rand.Next(1, 3);

                DateTime rDatum = new DateTime(maiNap.Year, maiNap.Month, rNap);
                rendszer.FoglalassHozzaadasa(rVersenyzo.azonosito, rDatum, rOra, rOrakSzama, csendes: true);
            }

            // 2. Kezdő időszalag megjelenítése (minden zöld)
            rendszer.IdoszalagMegjelenites();

            // 3. Manuális foglalás beállítása / átállítása
            bool tovabb = true;
            while (tovabb)
            {
                Console.WriteLine("\n--- MANUÁLIS IDŐPONTFOGLALÁS / ÁTÁLLÍTÁS ---");
                Versenyzo kivalasztott = VersenyzoKivalasztasa(versenyzok);

                if (kivalasztott != null)
                {
                    try
                    {
                        // 1. Lekérjük a pontos aktuális dátumot
                        DateTime ma = DateTime.Now;

                        // 2. Be kérjük a napot
                        Console.Write($"Adja meg a napot a jelenlegi hónapban ({ma.Day}-{DateTime.DaysInMonth(ma.Year, ma.Month)}): ");
                        int nap = int.Parse(Console.ReadLine());

                        Console.Write("Kezdő óra (8-18): ");
                        int kezdOora = int.Parse(Console.ReadLine());

                        Console.Write("Hány órára foglal (1 vagy 2): ");
                        int orakSzama = int.Parse(Console.ReadLine());

                        // 3. PONTOS DÁTUM LÉTREHOZÁSA (A ma.Year és ma.Month használatával!)
                        DateTime foglalasiDatum = new DateTime(ma.Year, ma.Month, nap);

                        // 4. Foglalás elvégzése
                        rendszer.FoglalassHozzaadasa(kivalasztott.azonosito, foglalasiDatum, kezdOora, orakSzama);

                        // 5. Frissített táblázat kirajzolása
                        rendszer.IdoszalagMegjelenites();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"HIBA: {ex.Message}\n");
                    }
                }

                Console.Write("Szeretne újabb foglalást/módosítást végezni? (i/n): ");
                tovabb = Console.ReadLine().ToLower() == "i";
            }

            Console.WriteLine("\nNyomj meg egy gombot a kilépéshez...");
            Console.ReadKey();
        }

        // Versenyző kiválasztására szolgáló függvény az azonosító alapján
        static Versenyzo VersenyzoKivalasztasa(List<Versenyzo> versenyzok)
        {
            Console.Write("Módosítandó versenyző azonosítója (pl. GO-KovacsDenes-19741204): ");
            string keresettAzonosito = Console.ReadLine().Trim();

            Versenyzo talalat = versenyzok.FirstOrDefault(v => v.azonosito.Equals(keresettAzonosito, StringComparison.OrdinalIgnoreCase));

            if (talalat == null)
            {
                Console.WriteLine("Nem található versenyző ezzel az azonosítóval!");
            }
            return talalat;
        }

        static List<string> BeolvasFajl(string fajlNev, string[] alapertelmezett)
        {
            if (File.Exists(fajlNev))
            {
                string teljesSzoveg = File.ReadAllText(fajlNev);
                string[] elemek = teljesSzoveg.Split(',');
                List<string> tisztitottNevek = new List<string>();

                foreach (string elem in elemek)
                {
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