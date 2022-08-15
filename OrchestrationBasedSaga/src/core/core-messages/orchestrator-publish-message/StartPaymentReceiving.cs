namespace core_messages.orchestrator_publish_message
{
    public class StartPaymentReceiving
    {
        public Guid OrderNo { get; set; }

        public DateTime PaymentDate { get; set; }
    }
}
