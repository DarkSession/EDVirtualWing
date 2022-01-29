using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using Microsoft.EntityFrameworkCore;

namespace ED_Virtual_Wing.PlayerJournal.Events.Other
{
    public class Status : JournalEventHandler
    {
        public VehicleStatusFlags Flags { get; set; }
        public OnFootStatusFlags Flags2 { get; set; }
        public decimal Latitude { get; set; }
        public decimal Altitude { get; set; }
        public decimal Longitude { get; set; }
        public StatusDestination? Destination { get; set; }

        public class StatusDestination
        {
            public long System { get; set; }
            public int Body { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Name_Localised { get; set; } = string.Empty;
        }

        public override async ValueTask ProcessEntry(Commander commander, ApplicationDbContext applicationDbContext)
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
            else if ((Flags & VehicleStatusFlags.InSRV) == VehicleStatusFlags.InSRV)
            {
                commander.GameActivity = GameActivity.InSrv;
            }
            else if ((Flags2 & OnFootStatusFlags.OnFoot) == OnFootStatusFlags.OnFoot)
            {
                commander.GameActivity = GameActivity.OnFoot;
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
            if (commander.Location != null)
            {
                commander.Location.Latitude = Latitude;
                commander.Location.Altitude = Altitude;
                commander.Location.Longitude = Longitude;
            }
            StarSystem? starSystem = null;
            StarSystemBody? starSystemBody = null;
            if (Destination != null)
            {
                if (Destination.System != 0)
                {
                    starSystem = await applicationDbContext.StarSystems.FirstOrDefaultAsync(s => s.SystemAddress == Destination.System);
                }
                if (Destination.Body != 0 && starSystem != null)
                {
                    starSystemBody = await applicationDbContext.StarSystemBodies.FirstOrDefaultAsync(s => s.StarSystem == starSystem && s.BodyId == Destination.Body);
                }
            }
            if (commander.Target != null)
            {
                string destinationName = string.Empty;
                if (Destination != null)
                {
                    string defaultIfEmpty = string.IsNullOrEmpty(Destination.Name_Localised) ? Destination.Name : Destination.Name_Localised;
                    destinationName = (await EDTranslatedString.Translate(Destination.Name, defaultIfEmpty, applicationDbContext)) ?? string.Empty;
                }
                commander.Target.StarSystem = starSystem;
                commander.Target.Body = starSystemBody;
                commander.Target.FallbackBodyId = Destination?.Body ?? 0;
                commander.Target.Name = destinationName;
            }
        }
    }
}
