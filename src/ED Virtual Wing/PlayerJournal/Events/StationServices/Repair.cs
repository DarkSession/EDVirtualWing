using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;

namespace ED_Virtual_Wing.PlayerJournal.Events.StationServices
{
    public class Repair : JournalEventHandler
    {
        public string? Item { get; set; }
        public List<string>? Items { get; set; }
        public override ValueTask ProcessEntry(Commander commander, ApplicationDbContext applicationDbContext)
        {
            if (Item == "Hull" || (Items?.Contains("Hull") ?? false))
            {
                commander.ShipHullHealth = 1m;
            }
            return ValueTask.CompletedTask;
        }
    }
}
