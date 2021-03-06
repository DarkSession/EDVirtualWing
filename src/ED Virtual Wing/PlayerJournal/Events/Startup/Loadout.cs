using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;

namespace ED_Virtual_Wing.PlayerJournal.Events.Startup
{
    public class Loadout : JournalEventHandler
    {
        public decimal HullHealth { get; set; }
        public string? ShipName { get; set; }
        public string? Ship { get; set; }
        public override ValueTask ProcessEntry(Commander commander, ApplicationDbContext applicationDbContext)
        {
            commander.ShipHullHealth = HullHealth;
            commander.ShipName = ShipName;
            if (!string.IsNullOrEmpty(Ship))
            {
                try
                {
                    Ship ship = Functions.ToEnum<Ship>(Ship);
                    if (ship < Models.Ship.NotShipsAbove)
                    {
                        commander.Ship = ship;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            commander.Target?.ResetShipTarget();
            return ValueTask.CompletedTask;
        }
    }
}
