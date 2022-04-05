using Core.Domain.Models;
using Core.Messages.Commands;
using Core.Messages.Events;
using Core.Persistence;
using Light.GuardClauses;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Handlers.Commands
{
    public class OrderCommandHandler : IRequestHandler<CreateOrderCommand>
    {
        private readonly UserZoomContext UserZoomContext;
        private readonly IBus BusClient;
        private readonly ILogger<OrderCommandHandler> Logger;

        public OrderCommandHandler(
            UserZoomContext context,
            IBus bus,
            ILogger<OrderCommandHandler> logger)
        {
            this.UserZoomContext = context;
            this.BusClient = bus.MustNotBeDefault(nameof(bus));
            this.Logger = logger;
        }

        public async Task<Unit> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            // TODO: atomically instantiate a new OrderAggregateRoot, persist it and send OrderCreatedEvent

            //He intentado usar Marten para insertar el documento:
            // Abrir una conexión con el  openLightsession
            // Insertar el documento con el método store
            // Lo que no he podido hacer es incluirlo dentro de la misma transaccion que el INSERT en la tabla Orders
            // Lo otro que se me había ocurrido era usar TransactionScope pero no lo he probado

            //Agregar automapper con profiles

            var orderAggregateRoot = new OrderAggregateRoot(request);

            using var transaction = UserZoomContext.Database.BeginTransaction();
            try
            {
                var eventId = Guid.NewGuid();
                var order = new Orders() { EventId = eventId, OrderId = orderAggregateRoot.Id, Payload = orderAggregateRoot.Events };
                UserZoomContext.Orders.Add(order);
                await UserZoomContext.SaveChangesAsync();

                var orderCreatedEvent = new OrderCreatedEvent() { Id = eventId, Correlation = order.OrderId };

                await BusClient.Publish(orderCreatedEvent);

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                await transaction.RollbackAsync();
            }

            return await Unit.Task;
        }
    }
}