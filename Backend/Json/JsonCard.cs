using Newtonsoft.Json;

namespace MTGCC.Json
{
    public class JsonCard
    {
        public string id { get; set; }
        public string oracle_id { get; set; }


        public string name { get; set; }
        public string oracle_text { get; set; }
        public string type_line { get; set; }
        public double cmc { get; set; }
        public string[] color_identity { get; set; }
        public string[] colors { get; set; }

        public string mana_cost { get; set; }

        public string power { get; set; }
        public string toughness { get; set; }

        public JsonCard[] card_faces { get; set; }
        public JsonCardImages image_uris { get; set; }

        public JsonCard()
        {

        }
    }
}
