using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ED_Virtual_Wing.PlayerJournal.Events.StationServices
{
    public class ShipyardSwap : JournalEventHandler
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public Ship? ShipType { get; set; }

        public override ValueTask ProcessEntry(Commander commander, ApplicationDbContext applicationDbContext)
        {
            commander.Ship = ShipType;
            commander.ShipName = string.Empty;
            return ValueTask.CompletedTask;
        }
    }
}
