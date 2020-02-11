using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDES
{
    class Program
    {
        static void Main(string[] args)
        {
            StartSDES();
        }

        static void StartSDES()
        {
            EncryptOrDecrypt();
        }

        static void EncryptOrDecrypt()
        {
            string ans = Prompt("Would you like to Encrypt or Decrypt?: ");
            if (ans.ToLower().Contains("en"))
                Encrypt(Prompt("Enter 8 bit plaintext: "), GenerateKeys(Prompt("Enter 10 bit key: ")));
            else
                Decrypt(Prompt("Enter 8 bit ciphertext: "), GenerateKeys(Prompt("Enter 10 bit key: ")));
        }

        static string Prompt(string message)
        {
            Console.Write(message);
            return Console.ReadLine();
        }

        static BitArray[] GenerateKeys(string str_bits)
        {
            BitArray bits = new BitArray(str_bits.Select(c => c == '1').ToArray());
            bits = P10(bits);
            LeftShift(bits, 1, 0, 4);
            LeftShift(bits, 1, 5, 9);
            BitArray bitKey1 = new BitArray(P8(bits));
            LeftShift(bits, 2, 0, 4);
            LeftShift(bits, 2, 5, 9);
            BitArray bitKey2 = new BitArray(P8(bits));
            BitArray[] bitKeys = new BitArray[2] { bitKey1, bitKey2 };
            return bitKeys;
        }

        static void Encrypt(string str_bits, BitArray[] keys)
        {
            BitArray bits = new BitArray(str_bits.Select(c => c == '1').ToArray());
            bits = IP(bits);
            BitArray[] bitHolder = SplitBits(bits);
            bits = EP(bits);
            bits.Xor(keys[0]);
            bitHolder[0].Xor(P4(S0S1(bits)));
            // Second Cycle
            bits = EP(CombineBits(new BitArray[] { bitHolder[0], bitHolder[0] }));
            bits.Xor(keys[1]);
            bitHolder[1].Xor(P4(S0S1(bits)));
            bits = CombineBits(new BitArray[] { bitHolder[1], bitHolder[0] });
            bits = IP_1(bits);
            Console.WriteLine("Ciphertext: " + DisplayBits(bits) + "\nKey 1: " + DisplayBits(keys[0]) + "\nKey 2: " + DisplayBits(keys[1]));
        }

        static void Decrypt(string str_bits, BitArray[] keys)
        {
            BitArray bits = new BitArray(str_bits.Select(c => c == '1').ToArray());
            bits = IP(bits);
            BitArray[] bitHolder = SplitBits(bits);
            bits = EP(bits);
            bits.Xor(keys[1]);
            bitHolder[0].Xor(P4(S0S1(bits)));
            // Second Cycle
            bits = EP(CombineBits(new BitArray[] { bitHolder[0], bitHolder[0] }));
            bits.Xor(keys[0]);
            bitHolder[1].Xor(P4(S0S1(bits)));
            bits = CombineBits(new BitArray[] { bitHolder[1], bitHolder[0] });
            bits = IP_1(bits);
            Console.WriteLine("Plaintext: " + DisplayBits(bits) + "\nKey 1: " + DisplayBits(keys[0]) + "\nKey 2: " + DisplayBits(keys[1]));
        }

        #region BitFunctions
        static string DisplayBits(BitArray bits)
        {
            string binary = "";
            foreach (bool bit in bits)
            {
                binary += Convert.ToInt32(bit);
            }
            return binary;
        }

        static BitArray CombineBits(BitArray[] bits)
        {
            BitArray combined = new BitArray(new bool[] { bits[0][0], bits[0][1], bits[0][2], bits[0][3], bits[1][0], bits[1][1], bits[1][2], bits[1][3] });
            return combined;
        }

        static BitArray[] SplitBits(BitArray bits)
        {
            BitArray[] split = new BitArray[2]
            {
                new BitArray(new bool[] { bits[0], bits[1], bits[2], bits[3] }),
                new BitArray (new bool[] { bits[4], bits[5], bits[6], bits[7] })
            };
            return split;
        }

        static BitArray P4(BitArray bits)
        {
            BitArray shiftedBits = new BitArray(4);
            shiftedBits[0] = bits[1];
            shiftedBits[1] = bits[3];
            shiftedBits[2] = bits[2];
            shiftedBits[3] = bits[0];
            return shiftedBits;
        }

        static BitArray S0S1(BitArray bits)
        {
            int[,] binary = new int[,] { { 0, 1 }, { 2, 3 } };
            string[,] S0 = new string[,] { { "01", "00", "11", "10" }, { "11", "10", "01", "00" }, { "00", "10", "01", "11" }, { "11", "01", "11", "10" } };
            string[,] S1 = new string[,] { { "00", "01", "10", "11" }, { "10", "00", "01", "11" }, { "11", "00", "01", "00" }, { "10", "01", "00", "11" } };

            string bitHolder = "";
            bitHolder += S0[binary[Convert.ToInt32(bits[0]), Convert.ToInt32(bits[3])], binary[Convert.ToInt32(bits[1]), Convert.ToInt32(bits[2])]];
            bitHolder += S1[binary[Convert.ToInt32(bits[4]), Convert.ToInt32(bits[7])], binary[Convert.ToInt32(bits[5]), Convert.ToInt32(bits[6])]];

            BitArray shiftedBits = new BitArray(bitHolder.Select(c => c == '1').ToArray());
            return shiftedBits;
        }

        static BitArray EP(BitArray bits)
        {
            BitArray shiftedBits = new BitArray(8);
            shiftedBits[0] = bits[7];
            shiftedBits[1] = bits[4];
            shiftedBits[2] = bits[5];
            shiftedBits[3] = bits[6];
            shiftedBits[4] = bits[5];
            shiftedBits[5] = bits[6];
            shiftedBits[6] = bits[7];
            shiftedBits[7] = bits[4];
            return shiftedBits;
        }

        static BitArray IP(BitArray bits)
        {
            BitArray shiftedBits = new BitArray(8);
            shiftedBits[0] = bits[1];
            shiftedBits[1] = bits[5];
            shiftedBits[2] = bits[2];
            shiftedBits[3] = bits[0];
            shiftedBits[4] = bits[3];
            shiftedBits[5] = bits[7];
            shiftedBits[6] = bits[4];
            shiftedBits[7] = bits[6];
            return shiftedBits;
        }

        static BitArray IP_1(BitArray bits)
        {
            BitArray shiftedBits = new BitArray(8);
            shiftedBits[0] = bits[3];
            shiftedBits[1] = bits[0];
            shiftedBits[2] = bits[2];
            shiftedBits[3] = bits[4];
            shiftedBits[4] = bits[6];
            shiftedBits[5] = bits[1];
            shiftedBits[6] = bits[7];
            shiftedBits[7] = bits[5];
            return shiftedBits;
        }

        static BitArray P10(BitArray bits)
        {
            BitArray shiftedBits = new BitArray(10);
            shiftedBits[0] = bits[2];
            shiftedBits[1] = bits[4];
            shiftedBits[2] = bits[1];
            shiftedBits[3] = bits[6];
            shiftedBits[4] = bits[3];
            shiftedBits[5] = bits[9];
            shiftedBits[6] = bits[0];
            shiftedBits[7] = bits[8];
            shiftedBits[8] = bits[7];
            shiftedBits[9] = bits[5];
            return shiftedBits;
        }

        static BitArray P8(BitArray bits)
        {
            BitArray shiftedBits = new BitArray(8);
            shiftedBits[0] = bits[5];
            shiftedBits[1] = bits[2];
            shiftedBits[2] = bits[6];
            shiftedBits[3] = bits[3];
            shiftedBits[4] = bits[7];
            shiftedBits[5] = bits[4];
            shiftedBits[6] = bits[9];
            shiftedBits[7] = bits[8];
            return shiftedBits;
        }

        static void LeftShift(BitArray bits, int num, int start, int end)
        {
            BitArray shiftedBits = new BitArray(bits);
            int count = 0;
            int overCount = 0;
            for (int i = start; i < end+1; i++)
            {
                if (i + num > end)
                    shiftedBits[count++] = bits[start + overCount++];
                else
                    shiftedBits[count++] = bits[i + num];
            }
            count = 0;
            for (int i = start; i < end + 1; i++)
                bits[i] = shiftedBits[count++];
        }
        #endregion
    }
}
