﻿using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SearchApi.Core.Adapters.Configuration;
using SearchApi.Core.Adapters.Contracts;
using SearchApi.Core.Adapters.Models;
using SearchApi.Core.Contracts;

namespace SearchApi.Core.Adapters.Middleware
{
    public class PersonSearchObserver : IConsumeMessageObserver<ExecuteSearch>
    {

        private readonly ProviderProfile _providerProfile;
        private readonly ILogger<PersonSearchObserver> _logger;

        public PersonSearchObserver(IOptions<ProviderProfileOptions> providerProfile, ILogger<PersonSearchObserver> logger)
        {
            _logger = logger;
            _providerProfile = providerProfile.Value;
        }

        public async Task PreConsume(ConsumeContext<ExecuteSearch> context)
        {
            _logger.LogInformation($"Adapter {_providerProfile.Name} provider received new person search request.");
            await Task.FromResult(0);
            return;
        }

        public async Task PostConsume(ConsumeContext<ExecuteSearch> context)
        {
            _logger.LogInformation($"Adapter {_providerProfile.Name} provider successfully processed new person search request.");
            await Task.FromResult(0);
            return;
        }

        public async Task ConsumeFault(ConsumeContext<ExecuteSearch> context, Exception exception)
        {
            _logger.LogError(exception, "Adapter Failed to execute person search.");
            await context.Publish<PersonSearchFailed>(new PersonSearchFailedEvent()
            {
                SearchRequestId = context.Message.Id,
                ProviderProfile = _providerProfile,
                Cause = exception
            });
        }
    }
}