using System;
using System.Collections.Generic;

namespace DeckOfCardsManager.Models
{
    public class Game
    {
        public String name { get; set; }
        public Guid id { get; set; }
        public Deck drawPile { get; set; }
        public Deck discardPile { get; set; }
        public List<Player> players { get; set; }
        public int currentPlayerNumber { get; set; }
        public Boolean reversed { get; set; }
        public Boolean aceHigh { get; set; }

        public Game(String name, int numberOfPlayers, Boolean aceHigh)
        {
            this.name = name;
            id = Guid.NewGuid();
            drawPile = new Deck(aceHigh);
            discardPile = new Deck();
            players = new List<Player>(numberOfPlayers);
            currentPlayerNumber = 0;
            reversed = false;
            this.aceHigh = aceHigh;
        }

        public Guid addPlayer(String name)
        {
            players.Add(new Player(name));

            return players[players.Count - 1].id;
        }

        public Player currentPlayer
        {
            get
            {
                if (players.Count > 0 && currentPlayerNumber < players.Count)
                {
                    return players[currentPlayerNumber];
                }
                else
                {
                    return null;
                }
            }
        }

        public Card topDiscard()
        {
            if (discardPile.Count > 0)
            {
                return discardPile[0];
            }
            else
            {
                return null;
            }
        }

        public void deal(int initialCards)
        {
            for (int i = 0; i < initialCards; i++)
            {
                foreach (Player thisplayer in players)
                {
                    if (drawPile.Count == 0)
                    {
                        break;
                    }
                    else
                    {
                        thisplayer.hand.Add(drawPile.take(1)[0]);
                    }
                }
                if (drawPile.Count == 0)
                {
                    break;
                }
            }
        }

        public void turn(int takeCount, List<Card> put = null, Boolean reverse = false)
        {
            List<Card> taken = new List<Card>(takeCount);

            currentPlayer.hand.InsertRange(0, drawPile.take(Math.Min(takeCount, drawPile.Count)));

            if (put != null)
            {
                foreach (Card discarded in put)
                {
                    currentPlayer.hand.Remove(discarded);
                }
                discardPile.InsertRange(0, put);
            }

            if (reversed)
            {
                currentPlayerNumber--;
                if (currentPlayerNumber < 0)
                {
                    currentPlayerNumber = players.Count - 1;
                }
            }
            else
            {
                currentPlayerNumber++;
                if (currentPlayerNumber >= players.Count)
                {
                    currentPlayerNumber = 0;
                }
            }

            if(reverse)
            {
                reversed = !reversed;
            }
        }
    }
}
