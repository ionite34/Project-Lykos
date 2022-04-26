// To parse this JSON data:
//
//    using ARPAbetDictType;
//
//    var jsonDictionary = JsonDictionary.FromJson(jsonString);

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Project_Lykos.Word_Checker
{
    public partial class JsonDictionary
    {
        [JsonPropertyName("title"), ]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("data")]
        public Dictionary<string, Datum> Data { get; set; }
    }

    public partial class Datum
    {
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }

        [JsonPropertyName("arpabet")]
        public string? Arpabet { get; set; }
    }

    public partial class JsonDictionary
    {
        public static JsonDictionary FromJson(string json) => JsonSerializer.Deserialize<JsonDictionary>(json);
    }

    public static class Serialize
    {
        public static string ToJson(this JsonDictionary self) => JsonSerializer.Serialize(self);
    }
}