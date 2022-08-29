using MTGCC.Json;

namespace MTGCC.Database
{
    public class MTGCardImages
    {
        public string Png { get; private set; }

        public MTGCardImages(JsonCardImages jsonCardImages)
        {
            if (jsonCardImages == null)
                return;

            this.Png = jsonCardImages.png;
        }
    }
}
