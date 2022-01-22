using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using Microsoft.EntityFrameworkCore;

namespace ED_Virtual_Wing.PlayerJournal.Events.Travel
{
    class SupercruiseExit : JournalEventHandler
    {
        public long SystemAddress { get; set; }
        public string Body { get; set; } = string.Empty;
        public int BodyID { get; set; }
        public string? BodyType { get; set; }
        public override async ValueTask ProcessEntry(Commander commander, ApplicationDbContext applicationDbContext)
        {
            commander.GameActivity = GameActivity.None;
            if (commander.Location != null)
            {
                if (BodyType == "Station" && !string.IsNullOrEmpty(Body))
                {
                    Station? station = await applicationDbContext.Stations.FirstOrDefaultAsync(s => s.StarSystem == commander.Location.StarSystem && s.Name == Body);
                    if (station != null)
                    {
                        commander.Location.SetLocationStation(station);
                    }
                    else
                    {
                        commander.Location.SetLocationName(Body);
                    }
                }
                else if (commander.Target?.StarSystem?.SystemAddress == SystemAddress &&
                    (commander.Target?.Body?.BodyId ?? commander.Target?.FallbackBodyId ?? 0) == BodyID &&
                    !string.IsNullOrEmpty(commander.Target?.Name))
                {
                    commander.Location.Name = commander.Target.Name;
                }
            }
        }
    }
}
