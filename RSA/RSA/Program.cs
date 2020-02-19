using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSA
{
    class Program
    {
        static int[] primes = { 15013, 15017, 15031, 15053, 15061, 15073,
  15077, 15083, 15091, 15101, 15107, 15121, 15131, 15137, 15139, 15149,
  15161, 15173, 15187, 15193, 15199, 15217, 15227, 15233, 15241, 15259,
  15263, 15269, 15271, 15277, 15287, 15289, 15299, 15307, 15313, 15319,
  15329, 15331, 15349, 15359, 15361, 15373, 15377, 15383, 15391, 15401,
  15413, 15427, 15439, 15443, 15451, 15461, 15467, 15473, 15493, 15497,
  15511, 15527, 15541, 15551, 15559, 15569, 15581, 15583, 15601, 15607,
  15619, 15629, 15641, 15643, 15647, 15649, 15661, 15667, 15671, 15679,
  15683, 15727, 15731, 15733, 15737, 15739, 15749, 15761, 15767, 15773,
  15787, 15791, 15797, 15803, 15809, 15817, 15823, 15859, 15877, 15881,
  15887, 15889, 15901, 15907, 15913, 15919, 15923, 15937, 15959, 15971,
  15973, 15991, 16001, 16007, 16033, 16057, 16061, 16063, 16067, 16069,
  16073, 16087, 16091, 16097, 16103, 16111, 16127, 16139, 16141, 16183,
  16187, 16189, 16193, 16217, 16223, 16229, 16231, 16249, 16253, 16267,
  16273, 16301, 16319, 16333, 16339, 16349, 16361, 16363, 16369, 16381,
  16411, 16417, 16421, 16427, 16433, 16447, 16451, 16453, 16477, 16481,
  16487, 16493, 16519, 16529, 16547, 16553, 16561, 16567, 16573, 16603,
  16607, 16619, 16631, 16633, 16649, 16651, 16657, 16661, 16673, 16691,
  16693, 16699, 16703, 16729, 16741, 16747, 16759, 16763, 16787, 16811,
  16823, 16829, 16831, 16843, 16871, 16879, 16883, 16889, 16901, 16903,
  16921, 16927, 16931, 16937, 16943, 16963, 16979, 16981, 16987, 16993,
  17011, 17021, 17027, 17029, 17033, 17041, 17047, 17053, 17077, 17093,
  17099, 17107, 17117, 17123, 17137, 17159, 17167, 17183, 17189, 17191,
  17203, 17207, 17209, 17231, 17239, 17257, 17291, 17293, 17299, 17317,
  17321, 17327, 17333, 17341, 17351, 17359, 17377, 17383, 17387, 17389,
  17393, 17401, 17417, 17419, 17431, 17443, 17449, 17467, 17471, 17477,
  17483, 17489, 17491, 17497, 17509};
        static void Main(string[] args)
        {
            StartRSA();
        }

        static void StartRSA()
        {
            do
            {
                int[] publicKeys = GenerateKeys();
            } while (Repeat());
        }
        
        static bool Repeat()
        {
            Console.Write("Would you like to generate another set of keys?(y/n): ");
            if (Console.ReadLine().Contains('y'))
                return true;
            else
                return false;
        }

        static int[] GenerateKeys()
        {
            Random rand = new Random();
            int p = primes[rand.Next(0, primes.Length)];
            int q = 0;
            do
            {
                q = primes[rand.Next(0, primes.Length)];
            } while (p == q);

            int N = p * q;
            int Euler = (p - 1) * (q - 1);

            int e = 0;
            do
            {
                e = rand.Next(10001, Euler);
            } while (gcd(e, Euler) != 1);

            int d = gcdInverse(e, Euler);

            DisplayAllKeys(p, q, N, Euler, e, d);

            return new int[] { e, N };
        }

        static void DisplayAllKeys(int p, int q, int N, int Euler, int e, int d)
        {
            Console.WriteLine("\np = " + p +
                "\nq = " + q +
                "\nN = " + N +
                "\nφ(N) = " + Euler +
                "\ne = " + e +
                "\nd = " + d + "\n");
        }

        #region GCD Functions
        static int gcd(int num1, int num2)
        {
            while (num1 != 0 && num2 != 0)
            {
                if (num1 > num2)
                    num1 %= num2;
                else
                    num2 %= num1;
            }
            return num1 == 0 ? num2 : num1;
        }

        static int gcdInverse(int first, int modded)
        {
            int x = 0;
            int y = 0;
            int g = gcdExtended(first, modded, ref x, ref y);
            if (g != 1)
                return -1;
            else
                return (((x % modded) + modded) % modded);
        }

        static int gcdExtended(int num1, int num2, ref int x, ref int y)
        {
            if (num1 == 0)
            {
                x = 0;
                y = 1;
                return num2;
            }

            int x1 = 0;
            int y1 = 0;
            int gcd = gcdExtended(num2 % num1, num1, ref x1, ref y1);

            x = y1 - (num2 / num1) * x1;
            y = x1;

            return gcd;
        }
        #endregion
    }
}
