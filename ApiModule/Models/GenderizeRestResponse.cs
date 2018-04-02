namespace ApiModule.Models
{
    using Newtonsoft.Json;

    public class GenderizeRestResponse
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("probability")]
        public string Probability { get; set; }

        [JsonProperty("count")]
        public string Count { get; set; }
        public bool IsFemale { get { return this.Gender == "female"; } }
    }
}
