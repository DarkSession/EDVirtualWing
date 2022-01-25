using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using Newtonsoft.Json;

namespace ED_Virtual_Wing.PlayerJournal.Events.Startup
{
    public class LoadGame : JournalEventHandler
    {
        [JsonProperty(Required = Required.Default)]
        public bool? Odyssey { get; set; }
        [JsonProperty(Required = Required.Default)]
        public bool? Horizons { get; set; }
        public string? Commander { get; set; }
        public string? GameMode { get; set; }
        public string? Group { get; set; }
        public string Ship { get; set; } = string.Empty;
        public string? ShipName { get; set; }

        public override ValueTask ProcessEntry(Commander commander, ApplicationDbContext applicationDbContext)
        {
            if (Odyssey == true)
            {
                commander.GameVersion = GameVersion.Odyssey;
            }
            else if (Horizons == true)
            {
                commander.GameVersion = GameVersion.Horizons;
            }
            else
            {
                commander.GameVersion = GameVersion.Base;
            }
            if (!string.IsNullOrEmpty(Commander))
            {
                commander.Name = Commander;
            }
            commander.GameMode = GameMode switch
            {
                "Open" => Models.GameMode.Open,
                "Group" => Models.GameMode.Group,
                "Solo" => Models.GameMode.Solo,
                _ => Models.GameMode.Unknown,
            };
            commander.GameModeGroupName = (commander.GameMode == Models.GameMode.Group) ? Group : null;
            if (Ship.ToLower().StartsWith("tacticalsuit"))
            {
                commander.Suit = Suit.Dominator;
            }
            else if (Ship.ToLower().StartsWith("utilitysuit"))
            {
                commander.Suit = Suit.Maverick;
            }
            else if (Ship.ToLower().StartsWith("explorationsuit"))
            {
                commander.Suit = Suit.Artemis;
            }
            else if (Ship.ToLower().StartsWith("flightsuit"))
            {
                commander.Suit = Suit.Flight;
            }
            else
            {
                commander.Ship = ToEnum<Ship>(Ship);
            }
            commander.ShipName = ShipName;
            commander.Target?.ResetShipTarget();
            return ValueTask.CompletedTask;
        }

        public static T? ToEnum<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>($"\"{value}\"");
        }
    }
}
