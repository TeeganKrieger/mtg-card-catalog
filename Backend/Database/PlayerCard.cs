using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTGCC.Database
{
    public class PlayerCard
    {
        [Key]
        [Column(Order = 0, TypeName = "nvarchar(36)")]
        public string ID { get; set; }

        [Column(Order = 1, TypeName = "nvarchar(36)")]
        public string CardID { get; set; }

        [Required]
        [Column(Order = 3)]
        public virtual AppUser Owner { get; set; }

        [Column(Order = 2, TypeName = "date")]
        public DateTime AddedDate { get; set; }

        public PlayerCard()
        {
            this.ID = null;
            this.CardID = null;
            this.Owner = null;
            this.AddedDate = default;
        }

        public PlayerCard(string cardID, AppUser owner)
        {
            this.ID = Guid.NewGuid().ToString();
            this.Owner = owner;
            this.CardID = cardID;
            this.AddedDate = DateTime.Now;
        }

        public MTGCard GetMTGCard()
        {
            return CardCache.SearchByID(this.CardID);
        }

    }
}
