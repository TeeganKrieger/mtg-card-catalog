using MTGCC.Json;

namespace MTGCC.Database
{
    public class MTGCard
    {
        public string ID { get; set; }
        public string OracleID { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string TypeLine { get; set; }
        public double ConvertedManaCost { get; set; }
        public Symbol[] ColorIdentity { get; set; }
        public Symbol[] Colors { get; set; }

        public Symbol[] ManaCost { get; set; }

        public string Power { get; set; }
        public string Toughness { get; set; }

        public MTGCardFace[] Faces { get; set; }

        public MTGCardImages Images { get; set; }

        public MTGCard(JsonCard jsonCard)
        {
            this.ID = jsonCard.id;
            this.OracleID = jsonCard.oracle_id;

            this.Name = jsonCard.name;
            this.Description = jsonCard.oracle_text;
            this.TypeLine = jsonCard.type_line;
            this.ConvertedManaCost = jsonCard.cmc;

            this.ColorIdentity = jsonCard.color_identity.Select(x => new Symbol($"{{{x}}}")).ToArray();
            this.Colors = jsonCard.colors?.Select(x => new Symbol($"{{{x}}}")).ToArray();

            this.ManaCost = jsonCard.mana_cost?.Split("}", StringSplitOptions.RemoveEmptyEntries).Select(x => new Symbol($"{x}}}")).ToArray();

            this.Power = jsonCard.power;
            this.Toughness = jsonCard.toughness;

            if (jsonCard.card_faces != null)
                this.Faces = jsonCard.card_faces.Select(x => new MTGCardFace(x)).ToArray();

            this.Images = new MTGCardImages(jsonCard.image_uris);
        }

        public string[] GetSearchTerms()
        {
            HashSet<string> uniqueTerms = new HashSet<string>();

            foreach (Symbol s in this.ColorIdentity)
                uniqueTerms.Add(s.ToString());

            if (this.Faces != null)
            {
                if (this.Colors != null)
                    foreach (Symbol s in this.Colors)
                        uniqueTerms.Add(s.ToString());

                foreach (MTGCardFace face in this.Faces)
                    face.GetSearchTerms(uniqueTerms);
            }
            else
            {
                foreach (Symbol s in this.Colors)
                    uniqueTerms.Add(s.ToString());

                foreach (Symbol s in this.ManaCost)
                    uniqueTerms.Add(s.ToString());

                string[] nameSplit = this.Name.Split(' ');

                foreach (string s in nameSplit)
                    uniqueTerms.Add(s);
            }

            return uniqueTerms.ToArray();
        }

        public override bool Equals(object? obj)
        {
            return obj != null && obj is MTGCard card && card.ID == this.ID;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}
