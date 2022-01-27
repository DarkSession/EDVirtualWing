namespace ED_Virtual_Wing.WebSockets.Messages
{
    class JournalStreamingChangedMessage
    {
        public bool Status { get; set; }
        public JournalStreamingChangedMessage(bool status)
        {
            Status = status;
        }
    }
}
