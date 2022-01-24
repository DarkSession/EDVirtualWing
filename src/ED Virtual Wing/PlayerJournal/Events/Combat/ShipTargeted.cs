using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Text.RegularExpressions;

namespace ED_Virtual_Wing.PlayerJournal.Events.Combat
{
    public class ShipTargeted : JournalEventHandler
    {
        private static Regex NpcShipName { get; } = new(@"^\$npc_name_decorate:#name=(.*?);$");
        private static Dictionary<string, string> NpcShipNameLocalisation { get; } = new()
        {
            { "$ShipName_Military_Independent;", "System Defence Force" },
            { "$ShipName_Police_Independent;", "System Authority Vessel" },
            { "$ShipName_PassengerLiner_Cruise;", "Cruise Ship" },
        };
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
                    string shipTargetName = PilotName_Localised;
                    if (!string.IsNullOrEmpty(PilotName))
                    {
                        Match npcShipMatch = NpcShipName.Match(PilotName);
                        if (npcShipMatch.Success)
                        {
                            shipTargetName = npcShipMatch.Groups[1].Value;
                        }
                        else if (NpcShipNameLocalisation.TryGetValue(PilotName, out string? localisedName))
                        {
                            shipTargetName = localisedName;
                        }
                        else if (!await applicationDbContext.TranslationsPendings.AnyAsync(t => t.NonLocalized == PilotName))
                        {
                            applicationDbContext.TranslationsPendings.Add(new TranslationsPending()
                            {
                                NonLocalized = PilotName,
                                LocalizedExample = PilotName_Localised,
                            });
                            await applicationDbContext.SaveChangesAsync();
                        }
                    }
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
