using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using Microsoft.EntityFrameworkCore;

namespace ED_Virtual_Wing.PlayerJournal.Events.Travel
{
    public class DockingRequested : JournalEventHandler
    {
        public long MarketID { get; set; }

        public override async ValueTask ProcessEntry(Commander commander, ApplicationDbContext applicationDbContext)
        {
            Station? station = await applicationDbContext.Stations.FirstOrDefaultAsync(s => s.MarketId == MarketID);
            if (station != null && commander.Location != null)
            {
                commander.Location.SetLocationStation(station);
            }
        }
    }
}
