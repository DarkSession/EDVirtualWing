using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;

namespace ED_Virtual_Wing.PlayerJournal.Events.StationServices
{
    public class RepairAll : JournalEventHandler
    {
        public override ValueTask ProcessEntry(Commander commander, ApplicationDbContext applicationDbContext)
        {
            commander.ShipHullHealth = 1m;
            return ValueTask.CompletedTask;
        }
    }
}
