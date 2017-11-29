using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace harkka
{
    class Program
    {
        //vertailujen lukumäärät tallennetaan listaan tyylillä:
        //  "haunNimi" : aika (ticks)
        public static List<KeyValuePair<string, double>> hakujenTulokset = new List<KeyValuePair<string, double>>();
        //luodaan random luokan instanssi jotta voidaan luoda random numeroita
        public static Random random = new Random();
        public static Stopwatch sw = new Stopwatch();
        //julkinen muuttuja
        public static string valittuAlgoritmi;

        static void Main(string[] args)
        {
            /* Ohjelma pyytää aluksi suoritettavien toimintojen lkm (kuinka monta kertaa loopataan)
             * Tämä tehdään jotta saadaan tarkennettua tulosten keskiarvoja
             * Seuraavana ohjelma pyytää luotavan taulun alkioiden lukumäärän
             * Viimeisenä ohjelma pyytää "mutatorin", toiminto näkyy funktion LuoTauluKokonaislukuja() sisällä
             *  -mutator=1 luo taulun jossa seuraava luku on 1 isompi kuin edellinen
             *  -mutator=2 luo taulun jossa seuraava luku on 1-2 isompi kuin edellinen
             *  .   .   .   .
             *  -mutator=10 luo taulun jossa seuraava luku on 1-10 isompi kuin edellinen
             * Kasvattamalla mutator >100 voidaan varmistaa että mahdollisuus etsittävän luvun löytymiseen on ~0%
             * Näin voidaan testata myös algoritmien Worst-case performancea
             *      
             *  
             * Joka kierroksella:
             *      -Luodaan järjestetty lista (Lisää tietoja funktion LuoTauluKokonaislukuja() yläpuolella)
             *      -Arvotaan haettava arvo. Haettava arvo on satunnaisesti valittu arvo taulun pienimmän ja isoimman luvun väliltä
             *      -Syötetään funktiolle Puolitushaku() aiemmin luomamme taulu ja haettava numero
             *      -Puolitushaku funktio palauttaa kuluneen ajan tickeinä
             *      -Lisätään hakujenTulokset listaan uusi KeyValuePair("puolitushaku", kulunut aika (ticks));
             *      -Nollataan kellon aika
             *      -Syötetään funktiolle Perakkaishaku() aiemmin luomamme taulu ja haettava numero
             *      -Perakkaishaku funktio palauttaa tapahtuneiden vertailujen määrän ja kuluneen ajan tickeinä
             *      -Lisätään hakujenTulokset listaan uusi KeyValuePair<string, double>("peräkkäishaku", kulunut aika (ticks));
             *      -Nollataan kellon aika
             * Kun ylläoleva rumba on suoritettu käyttäjän valitseman ajan, kutsutaan getAverages() funktiota
             * getAverages laskee keskiarvot hakujenTulokset-listan elementeistä ja palauttaa ne
             * Lopuksi tulostetaan keskiarvot käyttäjälle
             * 
             */
             //määritetään ajetaanko koko ohjelma uudestaan
            bool loopataan = true;
            while (loopataan)
            {
                //pyydetään input
                int[] userInput = new int[] { -1, -1, -1 };
                while (userInput[0] == -1 || userInput[1] == -1 || userInput[2] == -1)
                {
                    Console.Clear();
                    Console.WriteLine("Syötä kunnon luvut!");
                    userInput = getUserInput();
                }
                int looppienLkm = userInput[0];
                int taulunAlkiodenLkm = userInput[1];
                int mutator = userInput[2];


                //tehdään testitauluja, haetaan niistä arvottua numeroa ja tallennetaan vertailujen lkm listaan
                for (int x = 0; x < looppienLkm; x++)
                {
                    //luodaan taulu kokonaislukuja
                    int[] luvut = LuoTauluKokonaislukuja(random, taulunAlkiodenLkm, mutator);
                    //arvotaan haettava luku
                    int haettava = random.Next(luvut[0], luvut[luvut.Length - 1]);
                    double puolitusHaku;
                    double perakkaisHaku;
                    //ajetaan valittu algoritmi
                    switch (valittuAlgoritmi)
                    {
                        case "puolitushaku":
                            //suoritetaan puolitushaku
                            puolitusHaku = Puolitushaku(luvut, haettava, 0, luvut.Length);
                            //lisätään tulokset listaan
                            hakujenTulokset.Add(new KeyValuePair<string, double>("puolitushaku", puolitusHaku));
                            //nollataan kello
                            sw.Reset();
                            break;
                        case "peräkkäishaku":
                            //suoritetaan peräkkäishaku
                            perakkaisHaku = Perakkaishaku(luvut, haettava);
                            //lisätään tulokset listaan
                            hakujenTulokset.Add(new KeyValuePair<string, double>("peräkkäishaku", perakkaisHaku));
                            //nollataan kello
                            sw.Reset();
                            break;
                        case "molemmat algoritmit":
                            //suoritetaan puolitushaku
                            puolitusHaku = Puolitushaku(luvut, haettava, 0, luvut.Length);
                            //lisätään tulokset listaan
                            hakujenTulokset.Add(new KeyValuePair<string, double>("puolitushaku", puolitusHaku));
                            //nollataan kello
                            sw.Reset();
                            //suoritetaan peräkkäishaku
                            perakkaisHaku = Perakkaishaku(luvut, haettava);
                            //lisätään tulokset listaan
                            hakujenTulokset.Add(new KeyValuePair<string, double>("peräkkäishaku", perakkaisHaku));
                            //nollataan kello
                            sw.Reset();
                            break;
                    }
                }
                //haetaan ja tulostetaan keskiarvot valittuun algoritmiin
                double puolitusHaunKeskiarvot;
                double peräkkäisHaunKeskiarvot;
                switch (valittuAlgoritmi)
                {

                    case "puolitushaku":
                        puolitusHaunKeskiarvot = getAverages("puolitushaku", hakujenTulokset);
                        Console.WriteLine(String.Format("Aikaa kulunut puolitushakuun keskimäärin: {0} ns", (puolitusHaunKeskiarvot / Stopwatch.Frequency) * 1000000000));
                        break;
                    case "peräkkäishaku":
                        peräkkäisHaunKeskiarvot = getAverages("peräkkäishaku", hakujenTulokset);
                        Console.WriteLine(String.Format("Aikaa kulunut peräkkäishakuun keskimäärin: {0} ns",(peräkkäisHaunKeskiarvot / Stopwatch.Frequency) * 1000000000));
                        break;
                    case "molemmat algoritmit":
                        puolitusHaunKeskiarvot = getAverages("puolitushaku", hakujenTulokset);
                        peräkkäisHaunKeskiarvot = getAverages("peräkkäishaku", hakujenTulokset);
                        Console.WriteLine(String.Format("Aikaa kulunut puolitushakuun keskimäärin: {0} ns", (puolitusHaunKeskiarvot / Stopwatch.Frequency) * 1000000000));
                        Console.WriteLine();
                        Console.WriteLine(String.Format("Aikaa kulunut peräkkäishakuun keskimäärin: {0} ns", (peräkkäisHaunKeskiarvot / Stopwatch.Frequency) * 1000000000));
                        break;
                }
                //kysytään ajetaanko uudestaan
                Console.WriteLine("\nSyötä y aloittaaksesi alusta");
                switch (Console.ReadLine())
                {
                    case "y":
                        Console.WriteLine("Ajetaan uudestaan!!");
                        break;
                    default:
                        loopataan = false;
                        break;
                }
            }
        }

        //palauttaa double hakuun kuluneet tickit
        public static double Puolitushaku(int[] taulu, int haettava, int vasen, int oikea)
        {
            sw.Start();
            while (vasen <= oikea)
            {
                int keski = (vasen + oikea) / 2;
                if (taulu[keski] == haettava)
                {
                    sw.Stop();
                    return Convert.ToDouble(sw.ElapsedTicks);
                }
                if(haettava < taulu[keski])
                {
                    oikea = keski - 1;
                }
                else
                {
                    vasen = keski + 1;
                }
            }
            sw.Stop();
            return Convert.ToDouble(sw.ElapsedTicks);
        }

        //palauttaa double kulunut aika tickeinä
        public static double Perakkaishaku(int[] taulu, int haettava)
        {
            sw.Start();
            for(int x = 0; x<taulu.Length; x++)
            {
                if (taulu[x] == haettava)
                {
                    sw.Stop();
                    return Convert.ToDouble(sw.ElapsedTicks);
                }
            }
            sw.Stop();
            return Convert.ToDouble(sw.ElapsedTicks);
        }


        //luo järjestetyntaulun kokonaislukuja
        public static int[] LuoTauluKokonaislukuja(Random random, int taulunKoko, int mutator)
        {
            int[] taulu = new int[taulunKoko];
            for (int x = 0; x < taulunKoko; x++)
            {
                //ensimmäinen luku arvotaan väliltä 0-mutator
                if (x == 0)
                {
                    taulu[x] = random.Next(1,mutator);
                }
                if (x > 0)
                {
                    //loput luvut luodaan lisäämällä edelliseen lukuun luku väliltä 0-mutator
                    taulu[x] = taulu[x - 1] + random.Next(1,mutator);
                }
            }
            return taulu;
        }

        //palauttaa aikojen keskiarvot
        public static double getAverages(string key, List<KeyValuePair<string, double>> lista)
        {
            List<double> ajat = new List<double>();

            foreach (KeyValuePair<string, double> kvp in lista)
            {
                if (kvp.Key == key)
                {
                    ajat.Add(kvp.Value);
                }
            }
            return ajat.Average();
        }

        //pyydetään input
        public static int[] getUserInput()
        {
            Console.WriteLine("Stopwatch.Frequency = "+Stopwatch.Frequency.ToString()+" ticks/sekuntti");
            //pyydetään mitä algoritmia ajetaan
            bool valittu = false;
            while (!valittu)
            {
                Console.WriteLine("Mitä algoritmia testataan?\n" +
                    "1: Puolitushaku\n" +
                    "2: Perättäishaku\n" +
                    "3: Molemmat\n");
                int valinta = 0;
                bool oikeaInput = int.TryParse(Console.ReadLine(), out valinta);
                if(oikeaInput && (valinta == 1 || valinta == 2 || valinta == 3))
                {
                    switch (valinta)
                    {
                        case 1:
                            Console.WriteLine("Puolitushaku valittu");
                            valittuAlgoritmi = "puolitushaku";
                            valittu = true;
                            break;
                        case 2:
                            Console.WriteLine("Perättäishaku valittu");
                            valittuAlgoritmi = "perättäishaku";
                            valittu = true;
                            break;
                        case 3:
                            Console.WriteLine("Molemmat valittu");
                            valittuAlgoritmi = "molemmat algoritmit";
                            valittu = true;
                            break;
                    }
                }
            }
            //pyydetään kuinka monta kertaa algoritmit ajetaan
            Console.WriteLine("Kuinka monta kertaa {0} ajetaan? (Isompi luku tarkentaa keskiarvojen tarkkuutta)", valittuAlgoritmi);
            bool x = int.TryParse(Console.ReadLine(), out int resultA);

            //kuinka isoja järjesteltyjä listoja luodaan?
            Console.WriteLine("Kuinka iso taulu luodaan? (Alkioiden lkm)");
            bool y = int.TryParse(Console.ReadLine(), out int resultB);

            //hajonta listan lukujen välillä (muuttuja mutator LuoTauluKokonaisLukuja funktiossa)
            Console.Write("Listassa seuraava luku on aina edellistä 1-x isompi kuin edellinen. Syötä x: ");
            bool z = int.TryParse(Console.ReadLine(), out int resultC);

            //katsotaan tuliko kunnon input
            if (x && y && z)
            {
                return new int[] { resultA, resultB, resultC };
            }
            else
            {
                return new int[] { -1, -1, -1};
            }
        }
    }
}
