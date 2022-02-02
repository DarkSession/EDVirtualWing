using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ED_Virtual_Wing.PlayerJournal.Events.Travel
{
    public class DockingRequested : JournalEventHandler
    {
        public long MarketID { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public StationType StationType { get; set; }
        public string StationName { get; set; } = string.Empty;

        public override async ValueTask ProcessEntry(Commander commander, ApplicationDbContext applicationDbContext)
        {
            Station? station = await applicationDbContext.Stations.FirstOrDefaultAsync(s => s.MarketId == MarketID);
            if (station == null && commander.Location?.StarSystem != null)
            {
                station = new()
                {
                    MarketId = MarketID,
                    Name = StationName,
                    StarSystem = commander.Location?.StarSystem,
                    StationType = StationType,
                };
                applicationDbContext.Stations.Add(station);
                await applicationDbContext.SaveChangesAsync();
            }
            if (station != null)
            {
                commander.Location?.SetLocationStation(station);
            }
        }
    }
}
