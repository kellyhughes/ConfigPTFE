using System.Collections.Generic;

namespace ConfigPTFE {
	public interface IInitializationExpression {
		string EnvironmentFileName { set; get; }
		string KeyFilename { get; set; }
		string EnvironmentName { set; get; }
		string EnvironmentRole { set; get; }
		Dictionary<string, string> ConfigSectionLocations { set; get; }
		byte[] AesKey { get; set; }
		byte[] AesIV { get; set; }
	}
}