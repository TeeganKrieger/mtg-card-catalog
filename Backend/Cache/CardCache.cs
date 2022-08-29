using MTGCC.Database;
using MTGCC.Json;
using System.Text.Json;

namespace MTGCC
{
    public static class CardCache
    {
        private const string EXCP_FILE_NOT_FOUND = "Failed to load dataset at location \"{0}\". The file could not be found";

        private static Dictionary<string, MTGCard> CardsByID;

        public static void Init(string datasetLocation)
        {
            if (datasetLocation == null)
                throw new ArgumentNullException(nameof(datasetLocation));
            if (!File.Exists(datasetLocation))
                throw new ArgumentException(string.Format(EXCP_FILE_NOT_FOUND, datasetLocation), nameof(datasetLocation));

            string datasetJson = File.ReadAllText(datasetLocation);

            JsonCard[] jsonCards = JsonSerializer.Deserialize<JsonCard[]>(datasetJson);

            IEnumerable<MTGCard> allCards = jsonCards.Select(x => new MTGCard(x));

            CardsByID = new Dictionary<string, MTGCard>();

            int progress = 0;

            foreach (MTGCard card in allCards)
            {
                if (progress % 100 == 0)
                    Console.WriteLine($"Loading Progress: {progress / (double)jsonCards.Length}");
                progress++;

                CardsByID.Add(card.ID, card);
            }
        }

        public static MTGCard SearchByID(string id)
        {
            if (CardsByID.TryGetValue(id, out MTGCard card))
                return card;
            else
                return null;
        }
    }
}
