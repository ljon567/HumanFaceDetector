using Newtonsoft.Json;

namespace FaceDetector.DataModels
{
    public class stats
    {
        //Holds probability of humanity and unique ID number for each entry 
        [JsonProperty(PropertyName = "ID")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "createdAt")]
        public string createdAt { get; set; }

        [JsonProperty(PropertyName = "Probability")]
        public float Probability { get; set; }
    }
}
