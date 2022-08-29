using MTGCC.Database;
using MTGCC.Json;

namespace MTGCC.Translators
{
    public class SearchResponse
    {
        public JsonCard[] data { get; set; }
        public int total_cards { get; set; }
        public int page { get; set; }

        public int total_pages => (total_cards / 176) + 1;

        public MTGCard[] AsMTGCards()
        {
            if (data == null)
                return null;

            MTGCard[] mtgCards = new MTGCard[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                mtgCards[i] = new MTGCard(data[i]);
            }

            return mtgCards;
        }
    }
}
