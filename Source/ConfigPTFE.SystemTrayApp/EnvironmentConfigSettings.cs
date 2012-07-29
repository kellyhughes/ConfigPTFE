using System.ComponentModel;

namespace ConfigPTFE.SystemTrayApp {
	public class EnvironmentConfigSettings {
		public EnvironmentConfigSettings() {
			ConfigSectionLocations = new BindingList<ConfigSectionLocation>();
		}

		public string EnvironmentName { get; set; }
		public string EnvironmentRole { get; set; }
		public BindingList<ConfigSectionLocation> ConfigSectionLocations { get; set; }
	}
}