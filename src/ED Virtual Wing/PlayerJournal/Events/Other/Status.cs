using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;

namespace ED_Virtual_Wing.PlayerJournal.Events.Other
{
    public class Status : JournalEventHandler
    {
        public VehicleStatusFlags Flags { get; set; }

        public override ValueTask ProcessEntry(Commander commander, ApplicationDbContext applicationDbContext)
        {
            commander.VehicleStatusFlags = Flags;
            if ((Flags & VehicleStatusFlags.Supercruise) == VehicleStatusFlags.Supercruise && (Flags & VehicleStatusFlags.FsdJump) != VehicleStatusFlags.FsdJump)
            {
               commander.GameActivity = GameActivity.Supercruise;
            }
            else if ((Flags & VehicleStatusFlags.Landed) == VehicleStatusFlags.Landed)
            {
                commander.GameActivity = GameActivity.Landed;
            }
            else if ((Flags & VehicleStatusFlags.Docked) == VehicleStatusFlags.Docked)
            {
                commander.GameActivity = GameActivity.Docked;
            }
            else if ((Flags & VehicleStatusFlags.FsdCharging) != VehicleStatusFlags.FsdCharging)
            {
                switch (commander.GameActivity)
                {
                    case GameActivity.Supercruise:
                    case GameActivity.Landed:
                    case GameActivity.Docked:
                        {
                            commander.GameActivity = GameActivity.None;
                            break;
                        }
                }
            }
            return ValueTask.CompletedTask;
        }
    }
}
