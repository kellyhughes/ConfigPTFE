using System;
using System.ComponentModel;

namespace ConfigPTFE.SystemTrayApp {
	[Serializable]
	public class LocalApplicationData {
		public LocalApplicationData() {
			EnvironmentNames = new BindingList<StringWrapper>();
			EnvironmentRoles = new BindingList<StringWrapper>();
		}

		public string EnvironmentConfigFileName { get; set; }
		public BindingList<StringWrapper> EnvironmentNames { get; set; }
		public BindingList<StringWrapper> EnvironmentRoles { get; set; }
	}
}