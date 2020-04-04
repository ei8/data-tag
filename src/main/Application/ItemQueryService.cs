﻿using org.neurul.Common.Domain.Model;
using org.neurul.Common.Http;
using System;
using System.Threading;
using System.Threading.Tasks;
using works.ei8.EventSourcing.Client;
using works.ei8.Data.Tag.Common;
using works.ei8.Data.Tag.Domain.Model;

namespace works.ei8.Data.Tag.Application
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

        public async Task<ItemData> GetItemById(string avatarId, Guid id, CancellationToken token = default)
        {
            AssertionConcern.AssertArgumentNotEmpty(avatarId, "Specified parameter cannot be null or empty.", nameof(avatarId));
            AssertionConcern.AssertArgumentValid(
                g => g != Guid.Empty,
                id,
                Messages.Exception.InvalidId,
                nameof(id)
                );

            // Using a random Guid for Author as we won't be saving anyway
            var eventSource = this.eventSourceFactory.Create(
                Helper.UrlCombine(this.settingsService.EventSourcingInBaseUrl, avatarId) + "/",
                Helper.UrlCombine(this.settingsService.EventSourcingOutBaseUrl, avatarId) + "/",
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
