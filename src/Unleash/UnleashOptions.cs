using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
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
    public class UnleashOptions
    {
        internal readonly Encoding Encoding = Encoding.UTF8;

        internal readonly string FeatureToggleFilename = "unleash.toggles.json";
        internal readonly string EtagFilename = "unleash.etag.txt";

        /// <summary>
        /// Gets the version of unleash client running.
        /// </summary>
        public string SdkVersion { get; } = GetSdkVersion();

        /// <summary>
        /// Gets or set the uri for the backend unleash server.
        ///
        /// Default: http://unleash.herokuapp.com/api/
        /// </summary>
        public Uri UnleashApi { get; set; } = new Uri("http://unleash.herokuapp.com/api/");

        /// <summary>
        /// Gets or sets an application name. Used for communication with backend api.
        /// </summary>
        public string AppName { get; set; } = "my-awesome-app";

        /// <summary>
        /// Gets or sets an environment. Used for communication with backend api.
        /// </summary>
        public string Environment { get; set; } = "default";

        /// <summary>
        /// Gets or sets an instance tag. Used for communication with backend api.
        /// </summary>
        public string InstanceTag { get; set; } = GetDefaultInstanceTag();

        /// <summary>
        /// Sets the project to fetch feature toggles for.
        /// </summary>
        public string ProjectId { get; set; } = null;
        /// <summary>
        /// Should the default strategies replaced with the provided strategies or the strategies added to the defaults
        /// </summary>
        public bool OverrideDefaultStrategies { get; set; } = false;

        /// <summary>
        /// Gets or sets the interval in which feature toggle changes are re-fetched.
        ///
        /// Default: 30 seconds
        /// </summary>
        public TimeSpan FetchTogglesInterval { get; set; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Gets or sets the interval in which metrics are sent to the server. When null, no metrics are sent.
        ///
        /// Default: 60s
        /// </summary>
        public TimeSpan? SendMetricsInterval { get; set; } = TimeSpan.FromSeconds(60);

        /// <summary>
        /// Gets or set a directory for storing temporary files (toggles and current etag values).
        ///
        /// Default: Path.GetTempPath()
        /// </summary>
        public Func<string> LocalStorageFolder { get; set; } = Path.GetTempPath;

        /// <summary>
        /// Gets or sets a collection of custom http headers which will be included when communicating with the backend server.
        /// </summary>
        public Dictionary<string, string> CustomHttpHeaders { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets the override behaviour of the Bootstrap Toggles feature
        /// </summary>
        public bool BootstrapOverride { get; set; } = true;

        /// <summary>
        /// INTERNAL: Gets or sets if the feature toggle fetch should be immeditely scheduled. Used by the client factory to prevent redundant initial fetches.
        /// </summary>
        internal bool ScheduleFeatureToggleFetchImmediatly { get; set; } = true;

        private static string GetSdkVersion()
        {
            var assemblyName = Assembly.GetExecutingAssembly().GetName();
            var version = assemblyName.Version.ToString(3);

            return $"unleash-client-dotnet:v{version}";
        }

        private static string GetDefaultInstanceTag()
        {
            var hostName = Dns.GetHostName();

            return $"{hostName}-generated-{Guid.NewGuid()}";
        }

        /// <summary>
        /// Returns info about the unleash setup.
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder("## Unleash settings ##");

            sb.AppendLine($"Application name: {AppName}");
            sb.AppendLine($"Environment: {Environment}");
            sb.AppendLine($"Instance tag: {InstanceTag}");
            sb.AppendLine($"Project Id: {ProjectId}");
            sb.AppendLine($"Server Uri: {UnleashApi}");
            sb.AppendLine($"Sdk version: {SdkVersion}");

            sb.AppendLine($"Fetch toggles interval: {FetchTogglesInterval.TotalSeconds} seconds");
            var metricsInterval = SendMetricsInterval.HasValue
                ? $"{SendMetricsInterval.Value.TotalSeconds} seconds"
                : "never";
            sb.AppendLine($"Send metrics interval: {metricsInterval}");

            sb.AppendLine($"Local storage folder: {LocalStorageFolder()}");
            sb.AppendLine($"Backup file: {FeatureToggleFilename}");
            sb.AppendLine($"Etag file: {EtagFilename}");

            return sb.ToString();
        }
    }
}
