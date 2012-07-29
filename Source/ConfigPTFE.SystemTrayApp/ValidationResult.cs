using System.Collections.Generic;

namespace ConfigPTFE.SystemTrayApp {
	public class ValidationResult {
		public ValidationResult() {
			ErrorMessages = new List<string>();
		}

		public bool IsValid {
			get { return ErrorMessages.Count == 0; }
		}

		public IList<string> ErrorMessages { get; set; }
	}
}