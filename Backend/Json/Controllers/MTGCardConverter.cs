using MTGCC.Database;
using Newtonsoft.Json;

namespace Backend.Json.Controllers
{
    public class MTGCardConverter : JsonConverter<MTGCard>
    {
        public override MTGCard? ReadJson(JsonReader reader, Type objectType, MTGCard? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, MTGCard? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }    

            writer.WriteStartObject();

            writer.WritePropertyName("id");
            writer.WriteValue(value.ID);

            writer.WritePropertyName("oracleId");
            writer.WriteValue(value.OracleID);

            writer.WritePropertyName("name");
            writer.WriteValue(value.Name);

            writer.WritePropertyName("description");
            writer.WriteValue(value.Description);

            writer.WritePropertyName("convertedManaCost");
            writer.WriteValue(value.ConvertedManaCost);

            writer.WritePropertyName("power");
            writer.WriteValue(value.Power);

            writer.WritePropertyName("toughness");
            writer.WriteValue(value.Toughness);

            WriteSymbolArray(writer, "colorIdentity", value.ColorIdentity);
            WriteSymbolArray(writer, "colors", value.Colors);
            WriteSymbolArray(writer, "manaCost", value.ManaCost);

            WriteImages(writer, "images", value.Images);

            WriteFaces(writer, "faces", value.Faces);

            writer.WriteEndObject(); 
        }

        private void WriteSymbolArray(JsonWriter writer, string name, Symbol[] array)
        {
            writer.WritePropertyName(name);

            if (array == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteStartArray();

            for (int i = 0; i < array.Length; i++)
            {
                writer.WriteValue(array[i].ToString());
            }

            writer.WriteEndArray();
        }

        private void WriteImages(JsonWriter writer, string name, MTGCardImages images)
        {
            writer.WritePropertyName(name);

            if (images == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteStartObject();

            writer.WritePropertyName("png");
            writer.WriteValue(images.Png);

            writer.WriteEndObject();
        }

        private void WriteFaces(JsonWriter writer, string name, MTGCardFace[] faces)
        {
            writer.WritePropertyName(name);

            if (faces == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteStartArray();

            for (int i = 0; i < faces.Length; i++)
            {
                MTGCardFace face = faces[i];

                writer.WriteStartObject();

                writer.WritePropertyName("name");
                writer.WriteValue(face.Name);

                writer.WritePropertyName("description");
                writer.WriteValue(face.Description);

                writer.WritePropertyName("convertedManaCost");
                writer.WriteValue(face.ConvertedManaCost);

                writer.WritePropertyName("power");
                writer.WriteValue(face.Power);

                writer.WritePropertyName("toughness");
                writer.WriteValue(face.Toughness);

                WriteSymbolArray(writer, "colors", face.Colors);
                WriteSymbolArray(writer, "manaCost", face.ManaCost);

                WriteImages(writer, "images", face.Images);

                writer.WriteEndObject();
            }

            writer.WriteEndArray();
        }
    }
}
