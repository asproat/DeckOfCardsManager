using System;
using System.Collections.Generic;

namespace DeckOfCardsManager.Models
{
    public class Player
    {
        public String name { get; set; }
        public Guid id { get; set; }
        public List<Card> hand { get; set; }

        public Player(String name)
        {
            this.name = name;
            id = Guid.NewGuid();
            hand = new List<Card>();
        }
    }
}
