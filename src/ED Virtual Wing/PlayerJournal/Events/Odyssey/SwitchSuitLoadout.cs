﻿using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;

namespace ED_Virtual_Wing.PlayerJournal.Events.Odyssey
{
    public class SwitchSuitLoadout : JournalEventHandler
    {
        public string SuitName { get; set; } = string.Empty;
        public override ValueTask ProcessEntry(Commander commander, ApplicationDbContext applicationDbContext)
        {
            if (SuitName.StartsWith("tacticalsuit"))
            {
                commander.Suit = Suit.Dominator;
            }
            else if (SuitName.StartsWith("utilitysuit"))
            {
                commander.Suit = Suit.Maverick;
            }
            else if (SuitName.StartsWith("explorationsuit"))
            {
                commander.Suit = Suit.Artemis;
            }
            else
            {
                commander.Suit = Suit.Flight;
            }
            return ValueTask.CompletedTask;
        }
    }
}
