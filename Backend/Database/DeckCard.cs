using MTGCC.Database;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTGCC.Database
{
    public enum DeckCardMode
    {
        Owned,
        NotOwned
    }

    public class DeckCard
    {
        [Column(Order = 0, TypeName = "nvarchar(36)")]
        public string DeckID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1)]
        public int ID { get; set; }

        [Column(Order = 2)]
        public DeckCardMode Mode { get; set; }

        [Column(Order = 3)]
        public string CardID { get; set; }

        [Column(Order = 4)]
        [ForeignKey("PC")]
        public virtual PlayerCard PlayerCard { get; set; }

        
        public DeckCard()
        {
            this.ID = 0;
            this.Mode = DeckCardMode.NotOwned;
            this.DeckID = null;
            this.CardID = null;
            this.PlayerCard = null;
        }

        public DeckCard(Deck deck, PlayerCard playerCard)
        {
            this.Mode = DeckCardMode.Owned;
            this.DeckID = deck.ID;
            this.CardID = "";
            this.PlayerCard = playerCard;
        }

        public DeckCard(Deck deck, string cardID)
        {
            this.Mode = DeckCardMode.NotOwned;
            this.DeckID = deck.ID;
            this.CardID = cardID;
            this.PlayerCard = null;
        }

    }
}
