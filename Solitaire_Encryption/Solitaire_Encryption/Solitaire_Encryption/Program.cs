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
            Solitaire();
        }

        static void Solitaire()
        {
            List<string> deck = CreateDeck();
            KeyDeck(deck);

            GenerateLetter(deck);
        }

        static string GenerateLetter(List<string> deck)
        {
            MoveCard(deck, "A", 45);
            MoveCard(deck, "B", 6);

            TripleCut(deck);

            return "temp";
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

        static void KeyDeck(List<string> deck)
        {
            // SHUFFLE FOR KEY
        }

        static void MoveCard(List<string> deck, string card_to_move, int num_of_moves)
        {
            int current_place = deck.FindIndex(c => c.Equals(card_to_move));

            int place_to_move = current_place;
            if (current_place + num_of_moves >= deck.Count)
            {
                if (current_place + num_of_moves == deck.Count)
                    num_of_moves += 1;

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

            bottom_cut.Reverse();
            foreach (string c in top_cut)
            {
                deck.Remove(c);
                deck.Insert(bottom_joker + 1, c);
            }

            foreach (string c in bottom_cut)
            {
                deck.Remove(c);
                deck.Insert(0, c);
            }
        }
    }
}
