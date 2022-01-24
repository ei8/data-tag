using CQRSlite.Commands;
using neurUL.Common.Domain.Model;
using neurUL.Common.Http;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ei8.EventSourcing.Client;
using ei8.EventSourcing.Client.In;
using ei8.Data.Tag.Domain.Model;
using CQRSlite.Domain;
using CQRSlite.Events;

namespace ei8.Data.Tag.Application
{
    public class ItemCommandHandlers : 
        ICancellableCommandHandler<ChangeTag>        
    {
        private readonly IAuthoredEventStore eventStore;
        private readonly ISession session;

        public ItemCommandHandlers(IEventStore eventStore, ISession session)
        {
            AssertionConcern.AssertArgumentNotNull(eventStore, nameof(eventStore));
            AssertionConcern.AssertArgumentValid(
                es => es is IAuthoredEventStore, 
                eventStore, 
                "Specified 'eventStore' must be an IAuthoredEventStore implementation.", 
                nameof(eventStore)
                );
            AssertionConcern.AssertArgumentNotNull(session, nameof(session));

            this.eventStore = (IAuthoredEventStore) eventStore;
            this.session = session;
        }

        public async Task Handle(ChangeTag message, CancellationToken token = default(CancellationToken))
        {
            AssertionConcern.AssertArgumentNotNull(message, nameof(message));

            this.eventStore.SetAuthor(message.AuthorId);

            if ((await this.eventStore.Get(message.Id, 0)).Count() == 0)
            {
                var item = new Item(message.Id, message.NewTag);
                await this.session.Add(item, token);
            }
            else
            {
                Item item = await this.session.Get<Item>(message.Id, nameof(item), message.ExpectedVersion, token);
                item.ChangeTag(message.NewTag);
            }
            
            await this.session.Commit(token);
        }
    }
}