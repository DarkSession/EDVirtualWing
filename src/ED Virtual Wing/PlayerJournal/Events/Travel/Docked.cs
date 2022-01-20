using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ED_Virtual_Wing.PlayerJournal.Events.Travel
{
    public class Docked : JournalEventHandler
    {
        public long MarketID { get; set; }
        public long SystemAddress { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public StationType StationType { get; set; }
        public string StationName { get; set; } = string.Empty;
        public decimal DistFromStarLS { get; set; }

        public override async ValueTask ProcessEntry(Commander commander, ApplicationDbContext applicationDbContext)
        {
            if (commander.Location == null)
            {
                return;
            }
            StarSystem? starSystem = await applicationDbContext.StarSystems.FirstOrDefaultAsync(s => s.SystemAddress == SystemAddress);
            if (starSystem != null)
            {
                Station? station = await applicationDbContext.Stations.FirstOrDefaultAsync(s => s.MarketId == MarketID);
                if (station == null)
                {
                    station = new()
                    {
                        MarketId = MarketID,
                        Name = StationName,
                        StarSystem = starSystem,
                        DistanceFromStarLS = DistFromStarLS,
                        StationType = StationType,
                    };
                    applicationDbContext.Stations.Add(station);
                    await applicationDbContext.SaveChangesAsync();
                }
                commander.Location.SetLocationStation(starSystem, station);
                commander.GameActivity = GameActivity.Docked;
                return;
            }
            commander.Location.SetLocationStation(null);
        }
    }
}
