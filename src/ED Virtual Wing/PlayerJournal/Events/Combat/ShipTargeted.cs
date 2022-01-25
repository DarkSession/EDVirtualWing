using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using Newtonsoft.Json;

namespace ED_Virtual_Wing.PlayerJournal.Events.Combat
{
    public class ShipTargeted : JournalEventHandler
    {
        public bool TargetLocked { get; set; }
        public short ScanStage { get; set; }
        public string PilotName { get; set; } = string.Empty;
        public string PilotName_Localised { get; set; } = string.Empty;
        public string Ship { get; set; } = string.Empty;

        public override async ValueTask ProcessEntry(Commander commander, ApplicationDbContext applicationDbContext)
        {
            if (commander.Target != null)
            {
                if (TargetLocked)
                {
                    string shipTargetName = (await EDTranslatedString.Translate(PilotName, PilotName_Localised, applicationDbContext)) ?? string.Empty;
                    try
                    {
                        commander.Target.ShipTarget = ToEnum<Ship>(Ship);
                    }
                    catch
                    {
                        Console.WriteLine($"Unknown ship: {Ship}");
                    }
                    commander.Target.ShipTargetName = shipTargetName;
                }
                else
                {
                    commander.Target.ResetShipTarget();
                }
            }
        }

        public static T? ToEnum<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>($"\"{value}\"");
        }
    }
}
