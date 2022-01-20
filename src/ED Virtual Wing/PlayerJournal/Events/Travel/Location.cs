using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ED_Virtual_Wing.PlayerJournal.Events.Travel
{
    public class Location : JournalEventHandler
    {
        public string StarSystem { get; set; } = string.Empty;
        public long SystemAddress { get; set; }
        public List<decimal>? StarPos { get; set; }
        public bool? Docked { get; set; }
        public long MarketID { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public StationType StationType { get; set; }
        public string StationName { get; set; } = string.Empty;
        public decimal DistFromStarLS { get; set; }

        public override async ValueTask ProcessEntry(Commander commander, ApplicationDbContext applicationDbContext)
        {
            commander.GameActivity = GameActivity.None;
            StarSystem? starSystem = await applicationDbContext.StarSystems.FirstOrDefaultAsync(s => s.SystemAddress == SystemAddress);
            if (starSystem == null && StarPos != null && StarPos.Count == 3)
            {
                starSystem = new()
                {
                    SystemAddress = SystemAddress,
                    Name = StarSystem,
                    LocationX = StarPos[0],
                    LocationY = StarPos[1],
                    LocationZ = StarPos[2],
                };
                applicationDbContext.StarSystems.Add(starSystem);
                await applicationDbContext.SaveChangesAsync();
            }
            if (commander.Location != null && starSystem != null)
            {
                commander.Location.SetLocationSystem(starSystem);
            }
            if (Docked == true)
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
                if (commander.Location != null)
                {
                    commander.Location.SetLocationStation(station);
                }
                commander.GameActivity = GameActivity.Docked;
            }
        }
    }
}
