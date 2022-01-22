using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;

namespace ED_Virtual_Wing.PlayerJournal.Events.Combat
{
    public class HullDamage : JournalEventHandler
    {
        public decimal Health { get; set; }
        public bool PlayerPilot { get; set; }
        public bool Fighter { get; set; }
        public override ValueTask ProcessEntry(Commander commander, ApplicationDbContext applicationDbContext)
        {
            if (!Fighter && PlayerPilot)
            {
                commander.ShipHullHealth = Health;
            }
            return ValueTask.CompletedTask;
        }
    }
}
