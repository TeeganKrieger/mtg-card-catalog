using MTGCC.Json;

namespace MTGCC.Database
{
    public class MTGCardFace
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TypeLine { get; set; }
        public double ConvertedManaCost { get; set; }
        public Symbol[] Colors { get; set; }

        public Symbol[] ManaCost { get; set; }

        public string Power { get; set; }
        public string Toughness { get; set; }

        public MTGCardImages Images { get; set; }

        public MTGCardFace(JsonCard jsonCard)
        {
            this.Name = jsonCard.name;
            this.Description = jsonCard.oracle_text;
            this.TypeLine = jsonCard.type_line;

            this.ConvertedManaCost = jsonCard.cmc;

            this.Colors = jsonCard.colors?.Select(x => new Symbol($"{{{x}}}")).ToArray();

            this.ManaCost = jsonCard.mana_cost.Split("}", StringSplitOptions.RemoveEmptyEntries).Select(x => new Symbol($"{x}}}")).ToArray();

            this.Power = jsonCard.power;
            this.Toughness = jsonCard.toughness;

            this.Images = new MTGCardImages(jsonCard.image_uris);
        }

        public void GetSearchTerms(HashSet<string> uniqueTerms)
        {
            if (this.Colors != null)
                foreach (Symbol s in this.Colors)
                    uniqueTerms.Add(s.ToString());

            foreach (Symbol s in this.ManaCost)
                uniqueTerms.Add(s.ToString());

            string[] nameSplit = this.Name.Split(' ');

            foreach (string s in nameSplit)
                uniqueTerms.Add(s);
        }

    }
}
