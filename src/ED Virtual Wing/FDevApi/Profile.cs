using Newtonsoft.Json;

namespace ED_Virtual_Wing.FDevApi
{
    public class Profile
    {
        [JsonProperty("commander")]
        public ProfileCommander? Commander { get; set; }
    }

    public class ProfileCommander
    {
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
    }
}
