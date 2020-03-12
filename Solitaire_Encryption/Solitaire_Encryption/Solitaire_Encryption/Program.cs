using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solitaire_Encryption
{
    class Program
    {
        static void Main(string[] args)
        {
            
        }

        static string Encrypt(string plaintext, List<string> deck)
        {
            // PLAINTEXT MUST BE LETTERS ONLY

            plaintext = plaintext.ToUpper();
            plaintext = plaintext.Replace(" ", "");

            while ((plaintext.Length % 5) != 0)
                plaintext += "X";

            string keystream = "";
            for (int i = 0; i < plaintext.Length; i++)
                keystream += GenerateLetter(deck);

            string ciphertext = "";
            for (int i = 0; i < plaintext.Length; i++)
                ciphertext += (char)((((plaintext[i] - 64) + (keystream[i] - 65)) % 26) + 65);

            return ciphertext;
        }

        static string Decrypt(string ciphertext, List<string> deck)
        {
            // CIPHERTEXT MUST BE LETTERS ONLY

            ciphertext = ciphertext.ToUpper();
            ciphertext = ciphertext.Replace(" ", "");

            string keystream = "";
            for (int i = 0; i < ciphertext.Length; i++)
                keystream += GenerateLetter(deck);

            string plaintext = "";
            for (int i = 0; i < ciphertext.Length; i++)
            {
                int cipherAdd = 0;
                if (ciphertext[i] <= keystream[i])
                    cipherAdd = 26;
                int temp1 = (char)(ciphertext[i] - 64 + cipherAdd);
                int temp2 = (char)(keystream[i] - 64);
                plaintext += (char)((((ciphertext[i] - 64 + cipherAdd) - (keystream[i] - 64)) % 26) + 64);
            }

            return plaintext;
        }

        static List<string> CreateDeck()
        {
            List<string> deck_holder = new List<string>();
            for (int i = 1; i < 53; i++)
                deck_holder.Add(i.ToString());
            deck_holder.Add("A");
            deck_holder.Add("B");
            return deck_holder;
        }

        static string GenerateLetter(List<string> deck)
        {
            string generated_letter = "";
            do
            {
                MoveCard(deck, "A", 1);
                MoveCard(deck, "B", 2);

                TripleCut(deck);

                CountCut(deck);

                generated_letter = OutputCard(deck);
            } while (generated_letter == "");

            return generated_letter;
        }

        #region Card Moving Functions
        static void MoveCard(List<string> deck, string card_to_move, int num_of_moves)
        {
            int current_place = deck.FindIndex(c => c.Equals(card_to_move));

            int place_to_move = current_place;
            if (current_place + num_of_moves >= deck.Count)
            {
                /* The soliatire cypher says to move below top card, but seems misinformed for actual code
                if (current_place + num_of_moves == deck.Count)
                    num_of_moves += 1;
                */
                place_to_move = (current_place + num_of_moves) % deck.Count;
            }
            else
                place_to_move = current_place + num_of_moves;

            deck.Remove(card_to_move);
            deck.Insert(place_to_move, card_to_move);
        }

        static void TripleCut(List<string> deck)
        {
            int jokerA_place = deck.FindIndex(c => c.Equals("A"));
            int jokerB_place = deck.FindIndex(c => c.Equals("B"));

            int top_joker = 0;
            int bottom_joker = 0;
            if ( jokerA_place < jokerB_place)
            {
                top_joker = jokerA_place;
                bottom_joker = jokerB_place;
            }
            else
            {
                top_joker = jokerB_place;
                bottom_joker = jokerA_place;
            }

            List<string> top_cut = deck.GetRange(0, top_joker);
            List<string> bottom_cut = deck.GetRange(bottom_joker + 1, deck.Count - bottom_joker - 1);

            if (top_cut.Count != 0)
            {
                foreach (string c in top_cut)
                {
                    deck.Remove(c);
                    deck.Insert(bottom_joker, c);
                }
            }

            if (bottom_cut.Count != 0)
            {
                bottom_cut.Reverse();
                foreach (string c in bottom_cut)
                {
                    deck.Remove(c);
                    deck.Insert(0, c);
                }
            }
        }

        static void CountCut(List<string> deck)
        {
            int count_card = FindCardValue(deck[53]);

            List<string> top_cut = deck.GetRange(0, count_card);
            deck.RemoveRange(0, count_card);
            deck.InsertRange(deck.Count - 1, top_cut);
        }

        static string OutputCard(List<string> deck)
        {
            int count_card = FindCardValue(deck[0]);

            if (deck[count_card] == "A" || deck[count_card] == "B")
                return "";
            else
            {
                int num = Int32.Parse(deck[count_card]);
                if (num > 26)
                    num -= 26;
                char c = (char)(num + 64);
                return c.ToString();
            }
        }
        #endregion

        #region Multiple Use Functions
        static int FindCardValue(string s)
        {
            if (s == "A" || s == "B")
                return 53;
            else
                return Int32.Parse(s);
        }
        #endregion

        #region Key Shuffle Functions
        static List<string> PassphraseShuffle(string passphrase)
        {
            // PASSPHRASE MUST BE LETTERS ONLY

            passphrase = passphrase.ToUpper();
            passphrase = passphrase.Replace(" ", "");
            
            List<string> deck = CreateDeck();

            foreach (char c in passphrase)
            {
                List<string> top_cut = deck.GetRange(0, (int)(c - 65));
                deck.RemoveRange(0, (int)(c - 65));

                List<string> bottom_cut = deck.GetRange(1, deck.Count - 1);
                deck.RemoveRange(1, deck.Count - 1);

                deck.InsertRange(1, top_cut);
                deck.InsertRange(0, bottom_cut);
            }

            return deck;
        }

        static List<string> NumberShuffle(int num)
        {
            List<string> deck = CreateDeck();
            for (int i = 0; i <= num; i++)
                GenerateLetter(deck);
            return deck;
        }
        #endregion
    }
}