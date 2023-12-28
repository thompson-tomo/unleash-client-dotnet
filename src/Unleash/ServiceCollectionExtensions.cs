#if !NET45 && !NET451 && !NET46
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Unleash.Extensions
{

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUnleash(this IServiceCollection services, UnleashActions actions = null)
        {
            return services.AddUnleash("Unleash", actions);
        }

        public static IServiceCollection AddUnleash(this IServiceCollection services, string configSectionPath, UnleashActions actions = null)
        {
            services.AddOptions<UnleashOptions>()
                .BindConfiguration(configSectionPath);

            if(actions != null)
            {
                services.AddOptions<UnleashActions>()
                    .Configure(options =>
                    {
                        options.FileSystem = actions.FileSystem;
                        options.HttpClientFactory = actions.HttpClientFactory;
                        options.JsonSerializer = actions.JsonSerializer;
                        options.ToggleBootstrapProvider = actions.ToggleBootstrapProvider;
                        options.ScheduledTaskManager = actions.ScheduledTaskManager;
                        options.UnleashApiClient = actions.UnleashApiClient;
                        options.UnleashContextProvider = actions.UnleashContextProvider;
                        options.UnleashCustomHttpHeaderProvider = actions.UnleashCustomHttpHeaderProvider;
                    });
                //services.Configure(actions);
            }

            services.AddSingleton<IUnleash, DefaultUnleash>();
            return services;
        }
    }
}
#endif
