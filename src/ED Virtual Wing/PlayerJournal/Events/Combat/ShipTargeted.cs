using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ED_Virtual_Wing.PlayerJournal.Events.Combat
{
    public class ShipTargeted : JournalEventHandler
    {
        public bool TargetLocked { get; set; }
        public short ScanStage { get; set; }
        public string PilotName { get; set; } = string.Empty;
        public string PilotName_Localised { get; set; } = string.Empty;
        [JsonConverter(typeof(StringEnumConverter))]
        public Ship Ship { get; set; }

        public override ValueTask ProcessEntry(Commander commander, ApplicationDbContext applicationDbContext)
        {
            if (TargetLocked)
            {
                commander.Target.ShipTarget = Ship;
                commander.Target.ShipTargetName = PilotName_Localised;
            }
            else
            {
                commander.Target.ResetShipTarget();
            }
            return ValueTask.CompletedTask;
        }
    }
}
