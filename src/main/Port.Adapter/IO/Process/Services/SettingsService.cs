﻿using System;
using ei8.Data.Tag.Application;
using ei8.Data.Tag.Port.Adapter.Common;

namespace ei8.Data.Tag.Port.Adapter.IO.Process.Services
{
    public class SettingsService : ISettingsService
    {
        public string EventSourcingInBaseUrl => Environment.GetEnvironmentVariable(EnvironmentVariableKeys.EventSourcingInBaseUrl);

        public string EventSourcingOutBaseUrl => Environment.GetEnvironmentVariable(EnvironmentVariableKeys.EventSourcingOutBaseUrl);
    }
}
