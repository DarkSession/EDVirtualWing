using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;

namespace ED_Virtual_Wing.PlayerJournal.Events.Other
{
    class Music : JournalEventHandler
    {
        public string? MusicTrack { get; set; }

        public override ValueTask ProcessEntry(Commander commander, ApplicationDbContext applicationDbContext)
        {
            if (!string.IsNullOrEmpty(MusicTrack) && MusicTrack.StartsWith("Combat_"))
            {
                commander.GameActivity = GameActivity.Combat;
            }
            else if (commander.GameActivity == GameActivity.Combat)
            {
                commander.GameActivity = GameActivity.None;
            }
            return ValueTask.CompletedTask;
        }
    }
}
