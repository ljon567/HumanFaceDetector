using Newtonsoft.Json;

namespace FaceDetector.DataModels
{
    public class stats
    {
        [JsonProperty(PropertyName = "ID")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "Probability")]
        public float Probability { get; set; }
    }
}
