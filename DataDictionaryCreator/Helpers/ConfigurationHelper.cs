using DataDictionaryCreator.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataDictionaryCreator.Helpers
{
    public static class ConfigurationHelper
    {
        /// <summary>
        /// Gets or sets the base path.
        /// </summary>
        /// <value>
        /// The base path.
        /// </value>
        public static string BasePath { get; set; } = Path.Combine(Environment.CurrentDirectory.Replace(@"\bin\Debug\netcoreapp3.1", ""));

        /// <summary>
        /// Gets or sets the base configuration.
        /// </summary>
        /// <value>
        /// The base configuration.
        /// </value>
        private static IConfiguration BaseConfiguration { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is set configuration.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is set configuration; otherwise, <c>false</c>.
        /// </value>
        private static bool IsSetConfiguration { get; set; }

        /// <summary>
        /// Sets the configuration.
        /// </summary>
        private static void SetConfiguration()
        {
            if (!IsSetConfiguration)
            {
                BaseConfiguration = new ConfigurationBuilder()
                               .SetBasePath(BasePath)
                               .AddJsonFile("AppSetting.json", optional: true, reloadOnChange: true)
                               .AddEnvironmentVariables()
                               .Build();
                IsSetConfiguration = true;
            }
        }

        /// <summary>
        /// Gets the application settings.
        /// </summary>
        /// <exception cref="NullReferenceException">Base Configuration should not be null.</exception>
        public static void GetAppSettings()
        {
            SetConfiguration();
            if (BaseConfiguration != null)
            {
                GlobalProperties.ConnectionString = BaseConfiguration["ConnectionString"];
                GlobalProperties.FilePath = BaseConfiguration["FilePath"];
            }
            else
            {
                throw new NullReferenceException("Base Configuration should not be null.");
            }
        }
    }
}
