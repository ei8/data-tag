﻿using neurUL.Common.Domain.Model;
using neurUL.Common.Http;
using System;
using System.Threading;
using System.Threading.Tasks;
using ei8.EventSourcing.Client;
using ei8.Data.Tag.Common;
using ei8.Data.Tag.Domain.Model;

namespace ei8.Data.Tag.Application
{
    public class ItemQueryService : IItemQueryService
    {
        private readonly IEventSourceFactory eventSourceFactory;
        private readonly ISettingsService settingsService;

        public ItemQueryService(IEventSourceFactory eventSourceFactory, ISettingsService settingsService)
        {
            AssertionConcern.AssertArgumentNotNull(eventSourceFactory, nameof(eventSourceFactory));
            AssertionConcern.AssertArgumentNotNull(settingsService, nameof(settingsService));

            this.eventSourceFactory = eventSourceFactory;
            this.settingsService = settingsService;
        }

        public async Task<ItemData> GetItemById(Guid id, CancellationToken token = default)
        {
            AssertionConcern.AssertArgumentValid(
                g => g != Guid.Empty,
                id,
                Messages.Exception.InvalidId,
                nameof(id)
                );

            // Using a random Guid for Author as we won't be saving anyway
            var eventSource = this.eventSourceFactory.Create(
                this.settingsService.EventSourcingInBaseUrl + "/",
                this.settingsService.EventSourcingOutBaseUrl + "/",
                Guid.NewGuid()
                );

            var item = await eventSource.Session.Get<Item>(id, cancellationToken: token);

            return new ItemData()
            {
                Id = item.Id.ToString(),
                Tag = item.Tag,
                Version = item.Version
            };
        }
    }
}
