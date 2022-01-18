using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace ED_Virtual_Wing.PlayerJournal.Events.Exploration
{
    public class FSSSignalDiscovered : JournalEventHandler
    {
        private static Regex FleetCarrierIdRegex { get; } = new(@"^(.*?) ([A-Z0-9]{3}\-[A-Z0-9]{3})$", RegexOptions.IgnoreCase);

        public long SystemAddress { get; set; }
        public string SignalName { get; set; } = string.Empty;
        [JsonProperty(Required = Required.Default)]
        public bool? IsStation { get; set; }
        public override async ValueTask ProcessEntry(Commander commander, ApplicationDbContext applicationDbContext)
        {
            if (IsStation == true)
            {
                Match match = FleetCarrierIdRegex.Match(SignalName);
                if (match.Success)
                {
                    StarSystem? starSystem = await applicationDbContext.StarSystems.FindAsync(SystemAddress);
                    if (starSystem != null)
                    {
                        string carrierName = match.Groups[1].Value;
                        string carrierId = match.Groups[2].Value;
                        Station? station = await applicationDbContext.Stations.FirstOrDefaultAsync(s => s.Name == carrierId);
                        if (station != null)
                        {
                            station.NameAddon = carrierName;
                            station.StarSystem = starSystem;
                        }
                    }
                }
            }
        }
    }
}
