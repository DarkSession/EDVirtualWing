namespace ED_Virtual_Wing.WebSockets.Messages
{
    public class WingUnsubscribedData
    {
        public Guid WingId { get; set; }
        public WingUnsubscribedData(Guid wingId)
        {
            WingId = wingId;
        }
    }
}
