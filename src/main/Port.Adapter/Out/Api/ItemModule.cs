﻿using Nancy;
using Nancy.Responses;
using Newtonsoft.Json;
using ei8.Data.Tag.Application;

namespace ei8.Data.Tag.Port.Adapter.Out.Api
{
    public class ItemModule : NancyModule
    {
        public ItemModule(IItemQueryService itemQueryService) : base("/data/tags")
        {
            this.Get("/{itemId}", async (parameters) => new TextResponse(JsonConvert.SerializeObject(
                await itemQueryService.GetItemById(parameters.itemId))
                )
                );
        }
    }
}