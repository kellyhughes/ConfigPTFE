using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ConfigPTFE.SystemTrayApp {
	public class LocalApplicationDataRepository : ILocalApplicationDataRepository {
		public void Save(LocalApplicationData settings) {
			var localApplicationDataFolderPath =
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ConfigPTFE",
				             "ApplicationEnvironmentFileConfigurator");

			if (!Directory.Exists(localApplicationDataFolderPath))
				Directory.CreateDirectory(localApplicationDataFolderPath);

			var pathToSettingsConfig = Path.Combine(localApplicationDataFolderPath, "settings.config");

			var xmlSerializer = new XmlSerializer(settings.GetType());
			using (var fileStream = File.Open(pathToSettingsConfig, FileMode.Create)) {
				xmlSerializer.Serialize(fileStream, settings);
			}
		}

		public LocalApplicationData Get() {
			var settings = new LocalApplicationData();
			var localApplicationDataFolderPath =
				Path.Combine(
					Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
					"ConfigPTFE", "ApplicationEnvironmentFileConfigurator"
					);

			var pathToSettingsConfig = Path.Combine(localApplicationDataFolderPath, "settings.config");

			var xmlSerializer = new XmlSerializer(settings.GetType());
			if (File.Exists(pathToSettingsConfig)) {
				using (var fileStream = File.OpenRead(pathToSettingsConfig)) {
					settings = (LocalApplicationData) xmlSerializer.Deserialize(fileStream);
				}
			}
			else {
				settings = null;
			}

			return settings;
		}

		public void CreateNewSettingsFile() {
			var settings = 
				new LocalApplicationData() {
					EnvironmentConfigFileName = "common.environment.config",
					EnvironmentNames = 
						new BindingList<StringWrapper>() {
							new StringWrapper() {Text = "DEV"},
							new StringWrapper() {Text = "INTEGRATION"},
							new StringWrapper() {Text = "TEST"},
							new StringWrapper() {Text = "REGRESSION"},
							new StringWrapper() {Text = "PRODUCTION"}
			            },
					EnvironmentRoles = 
						new BindingList<StringWrapper>() {
			                new StringWrapper() {Text = "[none]"},
			                new StringWrapper() {Text = "REPORTS"},
			                new StringWrapper() {Text = "SERVICES"},
			                new StringWrapper() {Text = "WEBSERVER"}
			            },
			    };
			Save(settings);
		}
	}
}