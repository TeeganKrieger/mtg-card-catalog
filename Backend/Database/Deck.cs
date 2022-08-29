using MTGCC.Database;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTGCC.Database
{
    public class Deck
    {
        [Key]
        [Column(Order = 0, TypeName = "nvarchar(36)")]
        public string ID { get; set; }

        [Required]
        [Column(Order = 1)]
        public virtual AppUser Owner { get; set; }

        [Column(Order = 2, TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Required]
        [Column(Order = 3)]
        public virtual List<DeckCard> Cards { get; set; }

        public Deck()
        {
            this.ID = null;
            this.Owner = null;
            this.Name = null;
            this.Cards = new List<DeckCard>();
        }

        public Deck(AppUser owner, string name)
        {
            this.ID = Guid.NewGuid().ToString();
            this.Owner = owner;
            this.Name = name;
            this.Cards = new List<DeckCard>();
        }

    }
}
