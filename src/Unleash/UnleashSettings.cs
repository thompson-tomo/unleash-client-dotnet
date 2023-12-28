#if !NET45 && !NET451 && !NET46
using Microsoft.Extensions.Options;
#endif
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Unleash.Communication;
using Unleash.Internal;
using Unleash.Scheduling;
using Unleash.Serialization;
using Unleash.Utilities;

namespace Unleash
{
    /// <summary>
    /// Unleash settings
    /// </summary>
    public class UnleashSettings
    {
#if !NET45 && !NET451 && !NET46

        public UnleashSettings(IOptions<UnleashOptions> options)
        {
            UnleashOptions = options.Value;
            UnleashActions = new UnleashActions();
        }

        public UnleashSettings(IOptions<UnleashOptions> options, IOptions<UnleashActions> actions)
        {
            UnleashOptions = options.Value;
            UnleashActions = actions.Value;
        }
#endif

        public UnleashSettings()
        {

        }

        internal readonly Encoding Encoding = Encoding.UTF8;

        internal readonly string FeatureToggleFilename = "unleash.toggles.json";
        internal readonly string EtagFilename = "unleash.etag.txt";
        internal readonly UnleashOptions UnleashOptions = new UnleashOptions();
        internal readonly UnleashActions UnleashActions = new UnleashActions();

        /// <summary>
        /// Gets the version of unleash client running.
        /// </summary>
        public string SdkVersion => UnleashOptions.SdkVersion;

        /// <summary>
        /// Gets or set the uri for the backend unleash server.
        ///
        /// Default: http://unleash.herokuapp.com/api/
        /// </summary>
        public Uri UnleashApi { get => UnleashOptions.UnleashApi; set => UnleashOptions.UnleashApi = value; }

        /// <summary>
        /// Gets or sets an application name. Used for communication with backend api.
        /// </summary>
        public string AppName { get => UnleashOptions.AppName; set => UnleashOptions.AppName = value; }

        /// <summary>
        /// Gets or sets an environment. Used for communication with backend api.
        /// </summary>
        public string Environment { get => UnleashOptions.Environment; set => UnleashOptions.Environment = value; }

        /// <summary>
        /// Gets or sets an instance tag. Used for communication with backend api.
        /// </summary>
        public string InstanceTag { get => UnleashOptions.InstanceTag; set => UnleashOptions.InstanceTag = value; }

        /// <summary>
        /// Sets the project to fetch feature toggles for.
        /// </summary>
        public string ProjectId { get => UnleashOptions.ProjectId; set => UnleashOptions.ProjectId = value; }

        /// <summary>
        /// Should the default strategies replaced with the provided strategies or the strategies added to the defaults
        /// </summary>
        public bool OverrideDefaultStrategies { get => UnleashOptions.OverrideDefaultStrategies; set => UnleashOptions.OverrideDefaultStrategies = value; }

        /// <summary>
        /// Gets or sets the interval in which feature toggle changes are re-fetched.
        ///
        /// Default: 30 seconds
        /// </summary>
        public TimeSpan FetchTogglesInterval { get => UnleashOptions.FetchTogglesInterval; set => UnleashOptions.FetchTogglesInterval = value; }

        /// <summary>
        /// Gets or sets the interval in which metrics are sent to the server. When null, no metrics are sent.
        ///
        /// Default: 60s
        /// </summary>
        public TimeSpan? SendMetricsInterval { get => UnleashOptions.SendMetricsInterval; set => UnleashOptions.SendMetricsInterval = value; }

        /// <summary>
        /// Gets or set a directory for storing temporary files (toggles and current etag values).
        ///
        /// Default: Path.GetTempPath()
        /// </summary>
        public Func<string> LocalStorageFolder { get => UnleashOptions.LocalStorageFolder; set => UnleashOptions.LocalStorageFolder = value; }

        /// <summary>
        /// Gets or sets a collection of custom http headers which will be included when communicating with the backend server.
        /// </summary>
        public Dictionary<string, string> CustomHttpHeaders { get => UnleashOptions.CustomHttpHeaders; set => UnleashOptions.CustomHttpHeaders = value; }

        /// <summary>
        /// Gets or sets a provider that returns a dictionary of custom http headers
        /// which will be included when communicating with the backend server.
        /// This provider will be called before each outgoing request to the unleash server.
        /// </summary>
        public IUnleashCustomHttpHeaderProvider UnleashCustomHttpHeaderProvider { get => UnleashActions.UnleashCustomHttpHeaderProvider; set => UnleashActions.UnleashCustomHttpHeaderProvider = value; }

        /// <summary>
        /// Gets or sets the unleash context provider. This is needed when using any of the activation strategies
        /// that needs application specific context like userid etc.
        ///
        /// Default: A provider with no context.
        /// </summary>
        public IUnleashContextProvider UnleashContextProvider { get => UnleashActions.UnleashContextProvider; set => UnleashActions.UnleashContextProvider = value; }

        /// <summary>
        /// Gets or sets a json serializer.
        ///
        /// Default: A serializer based on Newtonsoft will be used, given that these assemblies are loaded into the appdomain already.
        /// </summary>
        public IJsonSerializer JsonSerializer { get => UnleashActions.JsonSerializer; set => UnleashActions.JsonSerializer = value; }

        /// <summary>
        /// Get or sets a factory class for creating the HttpClient instance used for communicating with the backend.
        /// </summary>
        public IHttpClientFactory HttpClientFactory { get => UnleashActions.HttpClientFactory; set => UnleashActions.HttpClientFactory = value; }

        /// <summary>
        /// Gets or sets the scheduled task manager used for syncing feature toggles and metrics with the backend in the background.
        /// Default: An implementation based on System.Threading.Timers
        /// </summary>
        public IUnleashScheduledTaskManager ScheduledTaskManager { get => UnleashActions.ScheduledTaskManager; set => UnleashActions.ScheduledTaskManager = value; }


        /// <summary>
        /// INTERNAL: Gets or sets an api client instance. Can be used for testing/mocking etc.
        /// </summary>
        internal IUnleashApiClient UnleashApiClient { get => UnleashActions.UnleashApiClient; set => UnleashActions.UnleashApiClient = value; }

        /// <summary>
        /// INTERNAL: Gets or sets the file system abstraction. Can be used for testing/mocking etc.
        /// </summary>
        internal IFileSystem FileSystem { get => UnleashActions.FileSystem; set => UnleashActions.FileSystem = value; }


        /// <summary>
        /// Gets or sets the toggle bootstrap provider (file, url, etc). Can be used for testing/mocking etc.
        /// </summary>
        public IToggleBootstrapProvider ToggleBootstrapProvider { get => UnleashActions.ToggleBootstrapProvider; set => UnleashActions.ToggleBootstrapProvider = value; }

        /// <summary>
        /// Gets or sets the override behaviour of the Bootstrap Toggles feature
        /// </summary>
        public bool BootstrapOverride { get => UnleashOptions.BootstrapOverride; set => UnleashOptions.BootstrapOverride = value; }

        /// <summary>
        /// INTERNAL: Gets or sets if the feature toggle fetch should be immeditely scheduled. Used by the client factory to prevent redundant initial fetches.
        /// </summary>
        internal bool ScheduleFeatureToggleFetchImmediatly { get => UnleashOptions.ScheduleFeatureToggleFetchImmediatly; set => UnleashOptions.ScheduleFeatureToggleFetchImmediatly = value; }

        /// <summary>
        /// Returns info about the unleash setup.
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder("## Unleash settings ##");

            sb.AppendLine(UnleashOptions.ToString());
            sb.AppendLine(UnleashActions.ToString());

            return sb.ToString();
        }

        public string GetFeatureToggleFilePath()
        {
            var tempFolder = LocalStorageFolder();
            return Path.Combine(tempFolder, PrependFileName(FeatureToggleFilename));
        }

        public string GetFeatureToggleETagFilePath()
        {
            var tempFolder = LocalStorageFolder();
            return Path.Combine(tempFolder, PrependFileName(EtagFilename));
        }

        private string PrependFileName(string filename)
        {
            var invalidFileNameChars = Path.GetInvalidFileNameChars();

            var extension = Path.GetExtension(filename);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filename);

            return new string($"{fileNameWithoutExtension}-{AppName}-{InstanceTag}-{SdkVersion}{extension}"
                .Where(c => !invalidFileNameChars.Contains(c))
                .ToArray());
        }

        public void UseBootstrapUrlProvider(string path, bool shouldThrowOnError, Dictionary<string, string> customHeaders = null)
        {
            ToggleBootstrapProvider = new ToggleBootstrapUrlProvider(path, HttpClientFactory.Create(new Uri(path)), this, shouldThrowOnError, customHeaders);
        }

        public void UseBootstrapFileProvider(string path)
        {
            ToggleBootstrapProvider = new ToggleBootstrapFileProvider(path, this);
        }
    }
}
