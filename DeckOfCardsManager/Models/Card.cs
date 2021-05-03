using System;
using System.Runtime.Serialization;

namespace DeckOfCardsManager.Models
{
    [Serializable]
    public class Card : ISerializable
    {
        private String suit { get; set; }
        private String rank { get; set; }
        public String face { get { return String.Format("{0}{1}", rank, suit); } }

        public Card(String suit, String rank)
        {
            this.suit = suit;
            this.rank = rank;
        }

        public Card(SerializationInfo info, StreamingContext context)
        {
            rank = info.GetString("face").Substring(0, 1);
            suit = info.GetString("face").Substring(1, 1);
        }

        public override string ToString()
        {
            return String.Format("{0}{1}", rank, suit);
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("face", String.Format("{0}{1}", rank, suit));
        }

        public override bool Equals(object obj)
        {
            Card one = this as Card;
            Card two = obj as Card;
            return this.face.Equals(two.face);
        }
    }
}
