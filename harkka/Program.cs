using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace harkka
{
    class Program
    {
        static void Main(string[] args)
        {
            //vertailujen lukumäärät tallennetaan listaan tyylillä:
            //  "haunNimi" : lkm
            List<KeyValuePair<string, int>> hakujenVertailujenLkm = new List<KeyValuePair<string, int>>();
            //luodaan random luokan instanssi jotta voidaan luoda random numeroita
            Random random = new Random();

            //tehdään testitauluja, haetaan niistä arvottua numeroa ja tallennetaan vertailujen lkm listaan
            for (int x = 0; x<1000; x++)
            {
                //luodaan taulu kokonaislukuja
                int[] luvut = LuoTauluKokonaislukuja(random, 100000, 2);
                //arvotaan haettava luku
                int haettava = random.Next(luvut[0], luvut[luvut.Length - 1]);
                //suoritetaan haut
                int[] puolitusHaku = Puolitushaku(luvut, haettava, 0, luvut.Length);
                int perakkaisHaku = Perakkaishaku(luvut, haettava);
                //lisätään vertailujen lkm listaan
                hakujenVertailujenLkm.Add(new KeyValuePair<string, int>("puolitushaku", puolitusHaku[1]));
                hakujenVertailujenLkm.Add(new KeyValuePair<string, int>("peräkkäishaku", perakkaisHaku));
            }

            Console.WriteLine("Puolitushaun vertailujen keskiarvo: "+getAverage("puolitushaku", hakujenVertailujenLkm));
            Console.WriteLine("Peräkkäishaun vertailujen keskiarvo: " + getAverage("peräkkäishaku", hakujenVertailujenLkm));

        }

        //palauttaa int[]{ etsityn numeron indeksi, vertailujen lkm }
        public static int[] Puolitushaku(int[] taulu, int haettava, int vasen, int oikea)
        {
            int vertailut = 0;
            while (vasen <= oikea)
            {
                vertailut++;
                int keski = (vasen + oikea) / 2;
                if (taulu[keski] == haettava)
                {
                    //vertailut++;
                    return new int[]{ keski, vertailut };
                }
                if(haettava < taulu[keski])
                {
                    //vertailut++;
                    oikea = keski - 1;
                }
                else
                {
                    //vertailut++;
                    vasen = keski + 1;
                }
            }
            return new int[] { -1, vertailut };
        }

        //palauttaa int[]{ etsityn numeron indeksi, vertailujen lkm }
        public static int Perakkaishaku(int[] taulu, int haettava)
        {
            for(int x = 0; x<taulu.Length; x++)
            {
                if (taulu[x] == haettava)
                {
                    return x;
                }
            }
            return taulu.Length-1;
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
        public static double getAverage(string key, List<KeyValuePair<string,int>> lista)
        {
            List<int> nums = new List<int>();
            foreach (KeyValuePair<string, int> record in lista)
            {
                if (record.Key == key)
                {
                    nums.Add(record.Value);
                }
            }
            return nums.Average();
        }
    }
}
