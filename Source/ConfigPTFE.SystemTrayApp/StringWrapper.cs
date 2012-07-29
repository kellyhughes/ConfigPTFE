using System;

namespace ConfigPTFE.SystemTrayApp {
	/// <summary>
	/// Wrapping the string value allows for easier databinding to datagridviews.
	/// </summary>
	[Serializable]
	public class StringWrapper {
		public string Text { get; set; }
	}
}