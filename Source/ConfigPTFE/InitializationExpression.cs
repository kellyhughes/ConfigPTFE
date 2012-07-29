using System;
using System.Collections.Generic;

namespace ConfigPTFE {
	public class InitializationExpression : IInitializationExpression {
		public InitializationExpression() {
			ConfigSectionLocations = new Dictionary<string, string>();
		}

		public string EnvironmentFileName { set; get; }
		public string KeyFilename { set; get; }

		public string EnvironmentName { set; get; }
		public string EnvironmentRole { set; get; }
		public Dictionary<string, string> ConfigSectionLocations { set; get; }

		public byte[] AesKey { get; set; }
		public byte[] AesIV { get; set; }
	}
}