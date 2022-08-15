using MassTransit;

namespace infrastructure.Persistence
{
    public class OrderStateInstance : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public Guid OrderNo { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
