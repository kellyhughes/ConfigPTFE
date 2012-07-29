using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ConfigPTFE.SystemTrayApp {
	public class EnvironmentConfigRepository : IEnvironmentConfigRepository {
		public bool Exists(string fileName) {
			var localApplicationEnvironmentConfigPath =
				Path.Combine(
					Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
					"ConfigPTFE", "ApplicationEnvironment", fileName
					);

			return File.Exists(localApplicationEnvironmentConfigPath);
		}

		public EnvironmentConfigSettings Get(string fileName) {
			var result = new EnvironmentConfigSettings();

			var environmentConfigPath =
				Path.Combine(
					Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
					"ConfigPTFE", "ApplicationEnvironment", fileName
					);

			if (File.Exists(environmentConfigPath)) {
				using (var fileStream = File.OpenRead(environmentConfigPath)) {
					var xd = XDocument.Load(fileStream);
					var configuration = xd.Element("configuration");
					if (configuration != null) {
						var environment = configuration.Element("environment");
						if (environment != null) {
							result.EnvironmentName = environment.Attribute("name").Value;

							var roleAttribute = environment.Attribute("role");
							result.EnvironmentRole = roleAttribute == null ? string.Empty : roleAttribute.Value;
						}

						var csls = configuration.Element("configSectionLocations");
						if (csls != null) {
							foreach (var x in csls.Elements("add")) {
								result.ConfigSectionLocations.Add(new ConfigSectionLocation(x.Attribute("key").Value, x.Attribute("value").Value));
							}
						}
					}
				}
			}

			return result;
		}

		public void Save(string fileName, EnvironmentConfigSettings settings) {
			var environmentConfigPath =
				Path.Combine(
					Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
					"ConfigPTFE", "ApplicationEnvironment", fileName
					);

			XDocument xd;

			if (File.Exists(environmentConfigPath)) {
				xd = XDocument.Load(environmentConfigPath);
			}
			else {
				xd = new XDocument(
					new XDeclaration("1.0", "utf-8", ""),
					new XElement("configuration",
					             new XElement("environment",
					                          new XAttribute("name", "DEV"),
					                          new XAttribute("role", "")
					             	),
					             new XElement("configSectionLocations")
						)
					);
			}

			var configuration = xd.Element("configuration");
			if (configuration != null) {
				configuration.Element("environment").SetAttributeValue("name", settings.EnvironmentName);
				configuration.Element("environment").SetAttributeValue("role",
				                                                       settings.EnvironmentRole == "[none]"
				                                                       	? string.Empty
				                                                       	: settings.EnvironmentRole);
				configuration.Element("configSectionLocations").Elements().Remove();
				foreach (var csl in settings.ConfigSectionLocations) {
					configuration.Element("configSectionLocations")
						.Add(
							new XElement("add",
							             new XAttribute("key", csl.Key),
							             new XAttribute("value", csl.Value)
								)
						);
				}
			}

			xd.Save(environmentConfigPath);
		}
	}
}