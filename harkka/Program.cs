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
        static void Main(string[] args)
        {
            //vertailujen lukumäärät tallennetaan listaan tyylillä:
            //  "haunNimi" : vertailujenLkm : aika
            List<Tuple<string, int, double>> hakujenTulokset = new List<Tuple<string, int, double>>();
            //luodaan random luokan instanssi jotta voidaan luoda random numeroita
            Random random = new Random();
            Stopwatch sw = new Stopwatch();

            //tehdään testitauluja, haetaan niistä arvottua numeroa ja tallennetaan vertailujen lkm listaan
            for (int x = 0; x<1000; x++)
            {
                //luodaan taulu kokonaislukuja
                int[] luvut = LuoTauluKokonaislukuja(random, 100000, 2);
                //arvotaan haettava luku
                int haettava = random.Next(luvut[0], luvut[luvut.Length - 1]);

                //suoritetaan puolitushaku
                double[] puolitusHaku = Puolitushaku(luvut, haettava, 0, luvut.Length, sw);
                //lisätään tulokset listaan
                hakujenTulokset.Add(new Tuple<string, int, double>("puolitushaku", Convert.ToInt32(puolitusHaku[1]), puolitusHaku[2]));
                //nollataan kello
                sw.Reset();

                //suoritetaan peräkkäishaku
                double[] perakkaisHaku = Perakkaishaku(luvut, haettava, sw);
                //lisätään tulokset listaan
                hakujenTulokset.Add(new Tuple<string, int, double>("peräkkäishaku", Convert.ToInt32(perakkaisHaku[0]), perakkaisHaku[1]));
                //nollataan kello
                sw.Reset();

            }

            double[] puolitusHaunKeskiarvot = getAverages("puolitushaku", hakujenTulokset);
            double[] peräkkäisHaunKeskiarvot = getAverages("peräkkäishaku", hakujenTulokset);


            Console.WriteLine(String.Format("Stopwatchin frekvenssi eli kuinka monta tickiä / sekuntti: {0}.", Stopwatch.Frequency));
            Console.WriteLine(String.Format("Vertailuja puolitushaussa keskimäärin: {0}.\nAikaa kulunut puoltushakuun keskimäärin: {1} ticks",puolitusHaunKeskiarvot[0], puolitusHaunKeskiarvot[1]));
            Console.WriteLine(String.Format("Vertailuja peräkkäishaussa keskimäärin: {0}.\nAikaa kulunut peräkkäishakuun keskimäärin: {1} ticks", peräkkäisHaunKeskiarvot[0], peräkkäisHaunKeskiarvot[1]));
        }

        //palauttaa double[]{ etsityn numeron indeksi, vertailujen lkm, hakuun kuluneet millisekunnit}
        public static double[] Puolitushaku(int[] taulu, int haettava, int vasen, int oikea, Stopwatch sw)
        {
            int vertailut = 0;
            sw.Start();
            while (vasen <= oikea)
            {
                vertailut++;
                int keski = (vasen + oikea) / 2;
                if (taulu[keski] == haettava)
                {
                    sw.Stop();
                    return new double[]{ keski, vertailut, Convert.ToDouble(sw.ElapsedTicks) };
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
            return new double[] { -1, vertailut, Convert.ToDouble(sw.ElapsedTicks) };
        }

        //palauttaa double[]{ etsityn numeron indeksi, vertailujen lkm, hakuun kuluneet millisekunnit }
        public static double[] Perakkaishaku(int[] taulu, int haettava, Stopwatch sw)
        {
            sw.Start();
            for(int x = 0; x<taulu.Length; x++)
            {
                if (taulu[x] == haettava)
                {
                    sw.Stop();
                    return new double[] { x, Convert.ToDouble(sw.ElapsedTicks) };
                }
            }
            sw.Stop();
            return new double[] { taulu.Length - 1, Convert.ToDouble(sw.ElapsedTicks) };
        }


        //luo taulun
        //ensimmöinen luku on random luku väliltä 0-annettu mutator
        //seuraava luku on 
        public static int[] LuoTauluKokonaislukuja(Random random, int taulunKoko, int mutator)
        {
            int[] taulu = new int[taulunKoko];
            for (int x = 0; x < taulu.Length; x++)
            {
                //lisätään ensimmäinen luku
                if (x == 0)
                {
                    taulu[x] = random.Next(mutator);
                }
                if (x > 0)
                {
                    //edellinen luku + 1-20
                    taulu[x] = taulu[x - 1] + random.Next(mutator);
                }
            }
            return taulu;
        }

        //palauttaa keskiarvon
        public static double[] getAverages(string key, List<Tuple<string, int, double>> lista)
        {
            List<int> nums = new List<int>();
            List<double> ajat = new List<double>();

            foreach (Tuple<string, int, double> record in lista)
            {
                if (record.Item1 == key)
                {
                    //lisätään vertailujen lkm
                    nums.Add(record.Item2);
                    //lisätään aika
                    ajat.Add(record.Item3);
                }
            }

            return new double[] { nums.Average(), ajat.Average() };
        }
    }
}
