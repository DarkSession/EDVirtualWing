using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;

namespace ED_Virtual_Wing.PlayerJournal.Events.Odyssey
{
    public class Embark : JournalEventHandler
    {
        public bool? SRV { get; set; }
        public bool? Multicrew { get; set; }
        public bool? Taxi { get; set; }
        public bool? OnStation { get; set; }
        public bool? OnPlanet { get; set; }
        public override ValueTask ProcessEntry(Commander commander, ApplicationDbContext applicationDbContext)
        {
            if (SRV == true)
            {
                commander.GameActivity = GameActivity.InSrv;
            }
            else if (commander.GameActivity == GameActivity.OnFoot)
            {
                if (OnStation == true)
                {
                    commander.GameActivity = GameActivity.Docked;
                }
                else if (OnPlanet == true)
                {
                    commander.GameActivity = GameActivity.Landed;
                }
                else
                {
                    commander.GameActivity = GameActivity.None;
                }
            }
            if (Taxi == true || Multicrew == true)
            {
                commander.Ship = null;
            }
            return ValueTask.CompletedTask;
        }
    }
}
