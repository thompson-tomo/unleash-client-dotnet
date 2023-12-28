using System;
using System.Collections.Generic;
using System.Text;
using Unleash.Communication;
using Unleash.Internal;
using Unleash.Scheduling;
using Unleash.Serialization;
using Unleash.Strategies;
using Unleash.Utilities;

namespace Unleash
{
    public class UnleashActions
    {

        /// <summary>
        /// Gets or sets a provider that returns a dictionary of custom http headers
        /// which will be included when communicating with the backend server.
        /// This provider will be called before each outgoing request to the unleash server.
        /// </summary>
        public IUnleashCustomHttpHeaderProvider UnleashCustomHttpHeaderProvider { get; set; } = new DefaultCustomHttpHeaderProvider();

        /// <summary>
        /// Gets or sets the unleash context provider. This is needed when using any of the activation strategies
        /// that needs application specific context like userid etc.
        ///
        /// Default: A provider with no context.
        /// </summary>
        public IUnleashContextProvider UnleashContextProvider { get; set; } = new DefaultUnleashContextProvider();

        /// <summary>
        /// Gets or sets a json serializer.
        ///
        /// Default: A serializer based on Newtonsoft will be used, given that these assemblies are loaded into the appdomain already.
        /// </summary>
        public IJsonSerializer JsonSerializer { get; set; } = new DynamicNewtonsoftJsonSerializer();

        /// <summary>
        /// Get or sets a factory class for creating the HttpClient instance used for communicating with the backend.
        /// </summary>
        public IHttpClientFactory HttpClientFactory { get; set; } = new DefaultHttpClientFactory();

        /// <summary>
        /// Gets or sets the scheduled task manager used for syncing feature toggles and metrics with the backend in the background.
        /// Default: An implementation based on System.Threading.Timers
        /// </summary>
        public IUnleashScheduledTaskManager ScheduledTaskManager { get; set; } = new SystemTimerScheduledTaskManager();


        /// <summary>
        /// INTERNAL: Gets or sets an api client instance. Can be used for testing/mocking etc.
        /// </summary>
        internal IUnleashApiClient UnleashApiClient { get; set; }

        /// <summary>
        /// INTERNAL: Gets or sets the file system abstraction. Can be used for testing/mocking etc.
        /// </summary>
        internal IFileSystem FileSystem { get; set; }

        /// <summary>
        /// Gets or sets the toggle bootstrap provider (file, url, etc). Can be used for testing/mocking etc.
        /// </summary>
        public IToggleBootstrapProvider ToggleBootstrapProvider { get; set; }

        /// <summary>
        /// Gets or sets the the strategies to be used for determining feature flag state
        /// </summary>
        public IStrategy[] Strategies { get; set; } = new IStrategy[0];

        /// <summary>
        /// Returns info about the unleash setup.
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder("## Unleash settings ##");

            sb.AppendLine($"HttpClient Factory: {HttpClientFactory.GetType().Name}");
            sb.AppendLine($"Json serializer: {JsonSerializer.GetType().Name}");
            sb.AppendLine($"Context provider: {UnleashContextProvider.GetType().Name}");

            sb.AppendLine($"Bootstrap provider: {ToggleBootstrapProvider?.GetType().Name ?? "null"}");

            return sb.ToString();
        }
    }
}
