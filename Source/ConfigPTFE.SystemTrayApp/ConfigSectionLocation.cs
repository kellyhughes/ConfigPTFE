namespace ConfigPTFE.SystemTrayApp {
	public class ConfigSectionLocation {
		public ConfigSectionLocation() {
		}

		public ConfigSectionLocation(string key, string value) {
			Key = key;
			Value = value;
		}

		public string Key { get; set; }
		public string Value { get; set; }
	}
}