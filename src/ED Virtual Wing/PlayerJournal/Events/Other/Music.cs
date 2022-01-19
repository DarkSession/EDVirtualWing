using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;

namespace ED_Virtual_Wing.PlayerJournal.Events.Other
{
    class Music : JournalEventHandler
    {
        public string MusicTrack { get; set; } = string.Empty;

        public override ValueTask ProcessEntry(Commander commander, ApplicationDbContext applicationDbContext)
        {
            if (!string.IsNullOrEmpty(MusicTrack) && MusicTrack.StartsWith("Combat_"))
            {
                commander.ExtraFlags |= GameExtraFlags.InCombat;
            }
            else
            {
                commander.ExtraFlags ^= GameExtraFlags.InCombat;
            }
            return ValueTask.CompletedTask;
        }
    }
}
