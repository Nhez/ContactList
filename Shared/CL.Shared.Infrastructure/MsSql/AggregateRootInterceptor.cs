using CL.Shared.Abstractions.Domain;
using CL.Shared.Abstractions.Messages.Brokers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CL.Shared.Infrastructure.MsSql;

internal sealed class AggregateRootInterceptor : SaveChangesInterceptor
{
    private readonly IMessageBroker _client;

    public AggregateRootInterceptor(IMessageBroker client)
    {
        _client = client;
    }

    private async Task SendDomainEvents(DbContext eventDataContext, CancellationToken cancellationToken = default)
    {
        var entities = eventDataContext.ChangeTracker.Entries<IAggregateRoot>().ToList();

        foreach (var entity in entities.Select(e => e.Entity))
        {
            foreach (var domainEvent in entity.DomainEvents)
            {
                await _client.PublishAsync(domainEvent, cancellationToken);
            }

            eventDataContext.Entry(entity).Entity.ClearDomainEvents();
        }
    }
}