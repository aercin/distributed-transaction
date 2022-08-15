using core_domain.Entitites;
using core_domain.Interfaces;
using core_messages;
using core_messages.orchestrator_publish_message;
using infrastructure.Persistence;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace infrastructure.Services
{
    public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
    {
        //Saga State Machine consume eventleri;
        public Event<OrderPlaced> OrderPlacedEvent { get; set; }
        public Event<StockDecreased> StockDecreasedEvent { get; set; }
        public Event<PaymentSuccessed> PaymentSuccessedEvent { get; set; }
        public Event<PaymentFailed> PaymentFailedEvent { get; set; }
        public Event<StockDecreaseFailed> StockDecreaseFailedEvent { get; set; }

        //Saga State Machine Instance'ında tutulacak olası state bilgileri
        public State OrderCreated { get; set; }
        public State StockDecreased { get; set; }
        public State PaymentSuccessed { get; set; }
        public State OrderFailed { get; set; }

        public OrderStateMachine(IServiceProvider serviceProvider)
        {
            //State Instance classında yer alan CurrentState propertysi güncel state bilgisi tutacağını belirtiyoruz.
            InstanceState(instance => instance.CurrentState);

            //Order-Stok-Payment servislerinden gelen eventler ile ilgili state instance'ı hangi correlation bilgisine bağlı bulacağını belirtiyoruz.

            Event(() => OrderPlacedEvent, orderStateInstance => orderStateInstance.CorrelateBy<Guid>(database => database.OrderNo, @event => @event.Message.OrderNo)
                        .SelectId(e => e.Message.OrderNo));

             
            Event(() => StockDecreasedEvent, orderStateInstance => orderStateInstance.CorrelateById(@event => @event.Message.OrderNo));

            Event(() => PaymentSuccessedEvent, orderStateInstance => orderStateInstance.CorrelateById(@event => @event.Message.OrderNo));

            Event(() => PaymentFailedEvent, orderStateInstance => orderStateInstance.CorrelateById(@event => @event.Message.OrderNo));

            Event(() => StockDecreaseFailedEvent, orderStateInstance => orderStateInstance.CorrelateById(@event => @event.Message.OrderNo));

            //State Machine Work Flowu tanımlıyoruz..
            Initially(When(OrderPlacedEvent)
                     .Then(context =>
                     {
                         context.Instance.OrderNo = context.Data.OrderNo;
                         context.Instance.TotalPrice = context.Data.Items.Sum(x => x.UnitPrice * x.Quantity);
                     })
                     .TransitionTo(OrderCreated)
                     .Then(context =>
                     {
                         var outboxMessageRepo = serviceProvider.GetService<IOutboxMessageRepository>();

                         var message = JsonSerializer.Serialize(new StartStockDecreasing
                         {
                             OrderNo = context.Data.OrderNo,
                             Items = context.Data.Items
                         });

                         outboxMessageRepo.Add(OutboxMessage.CreateOutboxMessage(typeof(StartStockDecreasing).AssemblyQualifiedName, message, DateTime.Now));
                     }));

            During(OrderCreated,
                 When(StockDecreasedEvent)
                  .TransitionTo(StockDecreased)
                  .Then(context =>
                  {
                      var outboxMessageRepo = serviceProvider.GetService<IOutboxMessageRepository>();

                      var message = JsonSerializer.Serialize(new StartPaymentReceiving
                      {
                          OrderNo = context.Data.OrderNo,
                          PaymentDate = DateTime.Now
                      });

                      outboxMessageRepo.Add(OutboxMessage.CreateOutboxMessage(typeof(StartPaymentReceiving).AssemblyQualifiedName, message, DateTime.Now));
                  }),
                When(StockDecreaseFailedEvent)
                 .TransitionTo(OrderFailed)
                  .Then(context =>
                  {
                      var outboxMessageRepo = serviceProvider.GetService<IOutboxMessageRepository>();

                      var message = JsonSerializer.Serialize(new ChangeOrderToFailedState
                      {
                          OrderNo = context.Data.OrderNo
                      });

                      outboxMessageRepo.Add(OutboxMessage.CreateOutboxMessage(typeof(ChangeOrderToFailedState).AssemblyQualifiedName, message, DateTime.Now));
                  }));

            During(StockDecreased,
                When(PaymentSuccessedEvent)
                .TransitionTo(PaymentSuccessed)
                .Then(context =>
               {
                   var outboxMessageRepo = serviceProvider.GetService<IOutboxMessageRepository>();

                   var message = JsonSerializer.Serialize(new ChangeOrderToSuccessedState
                   {
                       OrderNo = context.Data.OrderNo
                   });

                   outboxMessageRepo.Add(OutboxMessage.CreateOutboxMessage(typeof(ChangeOrderToSuccessedState).AssemblyQualifiedName, message, DateTime.Now));
               }),
                When(PaymentFailedEvent)
                .TransitionTo(OrderFailed)
                .Then(context =>
               {
                   var outboxMessageRepo = serviceProvider.GetService<IOutboxMessageRepository>();

                   var message = JsonSerializer.Serialize(new ChangeOrderToFailedState
                   {
                       OrderNo = context.Data.OrderNo
                   });

                   outboxMessageRepo.Add(OutboxMessage.CreateOutboxMessage(typeof(ChangeOrderToFailedState).AssemblyQualifiedName, message, DateTime.Now));
               }));
        }
    }
}
