using System.Text.Json.Serialization;
namespace Project_Lykos.Word_Checker;

public struct JsonDictionary
{
    [JsonPropertyName("title")]
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

public struct Datum
{
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }

    [JsonPropertyName("arpabet")]
    public string Arpabet { get; set; }
}