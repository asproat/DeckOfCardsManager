using System;
using System.Collections.Generic;

namespace DeckOfCardsManager.Models
{
    public class Deck : List<Card>
    {
        private String ranks = "A23456789TJQK";
        private String suits = "CDHS";
       
        public Deck(Boolean aceHigh, Boolean doShuffle = true)
        {
            Capacity = 52;

            foreach(Char suit in suits.ToCharArray())
            {
                foreach(Char rank in ranks.ToCharArray())
                {
                    this.Add(new Card(suit.ToString(), rank.ToString()));
                }
            }

            if(doShuffle)
            {
                shuffle();
            }

        }

        public Deck()
        {
            Capacity = 52;
        }

        public void shuffle()
        {
            Random rng = new Random();

            int n = this.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card value = this[k];
                this[k] = this[n];
                this[n] = value;
            }
        }

        public List<Card> take(int count) 
        {
            List<Card> taken = new List<Card>(count);

            taken = this.GetRange(0, count);
            this.RemoveRange(0, count);

            return taken;
        }

        public void put(List<Card> cards)
        {
            this.InsertRange(0, cards);
        }
    }
}
