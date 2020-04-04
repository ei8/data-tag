﻿using CQRSlite.Commands;
using CQRSlite.Routing;
using Nancy;
using Nancy.TinyIoc;
using org.neurul.Common.Http;
using System;
using works.ei8.EventSourcing.Client;
using works.ei8.Data.Tag.Application;
using works.ei8.Data.Tag.Port.Adapter.IO.Process.Services;

namespace works.ei8.Data.Tag.Port.Adapter.In.Api
{
    public class CustomBootstrapper : DefaultNancyBootstrapper
    {
        public CustomBootstrapper()
        {
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);
            // create a singleton instance which will be reused for all calls in current request
            var ipb = new Router();
            container.Register<ICommandSender, Router>(ipb);
            container.Register<IHandlerRegistrar, Router>(ipb);
            container.Register<IEventSerializer, EventSerializer>(new EventSerializer());
            container.Register<IEventSourceFactory, EventSourceFactory>();
            container.Register<ISettingsService, SettingsService>();
            container.Register<ItemCommandHandlers>();

            var ticl = new TinyIoCServiceLocator(container);
            container.Register<IServiceProvider, TinyIoCServiceLocator>(ticl);
            var registrar = new RouteRegistrar(ticl);
            registrar.Register(typeof(ItemCommandHandlers));

            // Here we register our user mapper as a per-request singleton.
            // As this is now per-request we could inject a request scoped
            // database "context" or other request scoped services.
            ((TinyIoCServiceLocator)container.Resolve<IServiceProvider>()).SetRequestContainer(container);
        }
    }
}
