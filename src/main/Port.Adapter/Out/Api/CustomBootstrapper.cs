using Nancy;
using Nancy.TinyIoc;
using ei8.EventSourcing.Client;
using ei8.Data.Tag.Application;
using ei8.Data.Tag.Port.Adapter.IO.Process.Services;
using CQRSlite.Domain;
using CQRSlite.Events;

namespace ei8.Data.Tag.Port.Adapter.Out.Api
{
    public class CustomBootstrapper : DefaultNancyBootstrapper
    {
        public CustomBootstrapper()
        {
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);

            container.Register<IEventSerializer, EventSerializer>();
            container.Register<ISettingsService, SettingsService>();
            container.Register<IEventStoreUrlService>(
                (tic, npo) => {
                    var ss = container.Resolve<ISettingsService>();
                    return new EventStoreUrlService(
                        ss.EventSourcingInBaseUrl + "/",
                        ss.EventSourcingOutBaseUrl + "/"
                        );
                });
            container.Register<IEventStore, HttpEventStoreClient>();
            container.Register<IRepository, Repository>();
            container.Register<ISession, Session>();
            container.Register<IItemQueryService, ItemQueryService>();
        }
    }
}
