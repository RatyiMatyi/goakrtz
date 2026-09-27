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

                        // Ha megvan a min. 8 fő (vagy megtelt), PIROS lesz
                        if (letszam >= 8)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkRed;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write($" {letszam,2}/20 "); // Piros háttér (elindul a menet / tele van)
                        }
                        // Ha van foglalás, de még 8 fő alatt van (pl. 1-7 fő)
                        else if (letszam > 0)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkYellow;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write($" {letszam,2}/20 "); // Sárga háttér (még csatlakozhatnak)
                        }
                        // Ha teljesen üres
                        else
                        {
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

            // Adott napra vonatkozó foglalások és versenyzők lekérdezése
            public void Napilekerdezes(DateTime datum, List<Versenyzo> versenyzok)
            {
                Console.WriteLine($"\n==================================================");
                Console.WriteLine($" FOGLALÁSOK RÉSZLETEZÉSE: {datum:yyyy.MM.dd.}");
                Console.WriteLine($"==================================================");

                bool voltFoglalas = false;

                for (int ora = 8; ora < 19; ora++)
                {
                    string kulcs = $"{datum:yyyy-MM-dd}-{ora}";

                    if (foglalasok.ContainsKey(kulcs) && foglalasok[kulcs].Count > 0)
                    {
                        voltFoglalas = true;
                        List<string> azonosítók = foglalasok[kulcs];

                        Console.WriteLine($"\n 🕒 Idősáv: {ora:00}:00 - {ora + 1:00}:00 (Összesen: {azonosítók.Count}/20 fő)");
                        Console.WriteLine(" --------------------------------------------------");

                        foreach (string azonosito in azonosítók)
                        {
                            // Megkeressük a versenyző objektumát a teljes listából a név megjelenítéséhez
                            Versenyzo v = versenyzok.FirstOrDefault(x => x.azonosito == azonosito);
                            if (v != null)
                            {
                                Console.WriteLine($"   • {v.vezetek_nev} {v.kereszt_nev} ({v.azonosito})");
                            }
                            else
                            {
                                Console.WriteLine($"   • {azonosito}");
                            }
                        }
                    }
                }

                if (!voltFoglalas)
                {
                    Console.WriteLine("\n Ezen a napon még egyetlen idősávra sincs foglalás.");
                }

                Console.WriteLine("==================================================\n");
            }

            // Adott versenyző-azonosító alapján a foglalások lekérdezése
            public void VersenyzoLekerdezes(string azonosito, List<Versenyzo> versenyzok)
            {
                // Megkeressük a versenyzőt a név kiírásához
                Versenyzo v = versenyzok.FirstOrDefault(x => x.azonosito.Equals(azonosito, StringComparison.OrdinalIgnoreCase));

                Console.WriteLine($"\n==================================================");
                if (v != null)
                {
                    Console.WriteLine($" VERSENYZŐ FOGLALÁSAI: {v.vezetek_nev} {v.kereszt_nev}");
                }
                Console.WriteLine($" Azonosító: {azonosito}");
                Console.WriteLine($"==================================================");

                bool voltFoglalas = false;

                // Végigmennyünk az összes elmentett foglalási kulcson
                foreach (var elem in foglalasok)
                {
                    if (elem.Value.Contains(azonosito, StringComparer.OrdinalIgnoreCase))
                    {
                        voltFoglalas = true;

                        // Kulcs formátuma: YYYY-MM-DD-HH (pl. 2026-09-27-15)
                        string[] reszek = elem.Key.Split('-');
                        string datumStr = $"{reszek[0]}.{reszek[1]}.{reszek[2]}.";
                        int ora = int.Parse(reszek[3]);

                        Console.WriteLine($" 🗓️ Dátum: {datumStr} | 🕒 Idősáv: {ora:00}:00 - {ora + 1:00}:00");
                    }
                }

                if (!voltFoglalas)
                {
                    Console.WriteLine(" Ennek a versenyzőnek jelenleg NINCS aktív foglalása.");
                }

                Console.WriteLine("==================================================\n");
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

            // KISORSOLUNK PÁR IDŐPONTOT, AHOVA DIVERZ CSOPORTOKAT BEOSZTUNK
            DateTime maiNap = DateTime.Now;
            int napokAHonapban = DateTime.DaysInMonth(maiNap.Year, maiNap.Month);

            // Generálunk pl. 4-8 olyan idősávot, ahol elindul a menet (min. 8 fővel)
            int foglaltIdosavokSzama = rand.Next(4, 9);

            for (int i = 0; i < foglaltIdosavokSzama; i++)
            {
                int rNap = rand.Next(maiNap.Day, napokAHonapban + 1);
                int rOra = rand.Next(8, 19); // 8-18 óra között
                DateTime rDatum = new DateTime(maiNap.Year, maiNap.Month, rNap);

                // Kisorsolunk egy csoportlétszámot 8 és 15 között
                int csoportszam = rand.Next(8, 16);

                // Egy ideiglenes listával biztosítjuk, hogy egy idősávba ne kerüljön be kétszer ugyanaz az ember
                List<int> marKivalasztottIndexek = new List<int>();

                for (int j = 0; j < csoportszam; j++)
                {
                    int rIndex;
                    // Addig sorsolunk új indexet, amíg olyan versenyzőt nem találunk, aki még nincs benne ebben a csoportban
                    do
                    {
                        rIndex = rand.Next(0, versenyzok.Count);
                    }
                    while (marKivalasztottIndexek.Contains(rIndex) && marKivalasztottIndexek.Count < versenyzok.Count);

                    marKivalasztottIndexek.Add(rIndex);
                    Versenyzo kisorsoltVersenyzo = versenyzok[rIndex];

                    // Rögzítjük a foglalást
                    rendszer.FoglalassHozzaadasa(kisorsoltVersenyzo.azonosito, rDatum, rOra, 1, csendes: true);
                }
            }

            // 2. Kezdő időszalag megjelenítése (minden zöld)
            rendszer.IdoszalagMegjelenites();

            // 3. Manuális foglalás beállítása / átállítása
            bool tovabb = true;
            while (tovabb)
            {
                Console.WriteLine("\n--- VÁLASSZ AZ ALÁBBI LEHETŐSÉGEK KÖZÜL ---");
                Console.WriteLine("1 - Adott nap foglalásainak lekérdezése (kik foglaltak)");
                Console.WriteLine("2 - Manuális időpontfoglalás / átállítás");
                Console.WriteLine("3 - Időszalag újragenerálása/megjelenítése");
                Console.WriteLine("4 - Versenyző foglalásainak lekérdezése azonosító alapján"); 
                Console.WriteLine("0 - Kilépés");
                Console.Write("Választás (0-4): ");

                string valasz = Console.ReadLine().Trim();
                DateTime ma = DateTime.Now;

                switch (valasz)
                {
                    case "1":
                        try
                        {
                            Console.Write($"\nAdja meg a lekérdezni kívánt napot a jelenlegi hónapban ({ma.Day}-{DateTime.DaysInMonth(ma.Year, ma.Month)}): ");
                            int nap = int.Parse(Console.ReadLine());
                            DateTime lekerdezettDatum = new DateTime(ma.Year, ma.Month, nap);

                            rendszer.Napilekerdezes(lekerdezettDatum, versenyzok);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"HIBA: {ex.Message}\n");
                        }
                        break;

                    case "2":
                        Console.WriteLine("\n--- MANUÁLIS IDŐPONTFOGLALÁS / ÁTÁLLÍTÁS ---");
                        Versenyzo kivalasztott = VersenyzoKivalasztasa(versenyzok);

                        if (kivalasztott != null)
                        {
                            try
                            {
                                Console.Write($"Adja meg a napot a jelenlegi hónapban ({ma.Day}-{DateTime.DaysInMonth(ma.Year, ma.Month)}): ");
                                int nap = int.Parse(Console.ReadLine());

                                Console.Write("Kezdő óra (8-18): ");
                                int kezdOora = int.Parse(Console.ReadLine());

                                Console.Write("Hány órára foglal (1 vagy 2): ");
                                int orakSzama = int.Parse(Console.ReadLine());

                                DateTime foglalasiDatum = new DateTime(ma.Year, ma.Month, nap);
                                rendszer.FoglalassHozzaadasa(kivalasztott.azonosito, foglalasiDatum, kezdOora, orakSzama);

                                rendszer.IdoszalagMegjelenites();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"HIBA: {ex.Message}\n");
                            }
                        }
                        break;

                    case "3":
                        rendszer.IdoszalagMegjelenites();
                        break;

                    case "4": 
                        Console.Write("\nAdja meg a keresett versenyző azonosítóját (pl. GO-KovacsDenes-19741204): ");
                        string kerAzonosito = Console.ReadLine().Trim();

                        rendszer.VersenyzoLekerdezes(kerAzonosito, versenyzok);
                        break;

                    case "0":
                        tovabb = false;
                        break;

                    default:
                        Console.WriteLine("Erre az opcióra nincs menüpont!");
                        break;
                }
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

                // Elválasztás új sornál, vesszőnél és pontosvesszőnél is!
                string[] elemek = teljesSzoveg.Split(new char[] { ',', '\n', '\r', ';' }, StringSplitOptions.RemoveEmptyEntries);
                List<string> tisztitottNevek = new List<string>();

                foreach (string elem in elemek)
                {
                    string nev = elem.Trim().Trim('\'', '"');
                    if (!string.IsNullOrWhiteSpace(nev))
                    {
                        tisztitottNevek.Add(nev);
                    }
                }

                if (tisztitottNevek.Count > 0)
                {
                    return tisztitottNevek;
                }
            }

            // Ha nem találja a fájlt, egy nagyobb alapértelmezett listát adunk vissza
            return new List<string>(alapertelmezett);
        }
    }
}