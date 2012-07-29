using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ConfigPTFE.SystemTrayApp {
	public partial class DialogGetNewEnvironmentConfigFileName : Form {
		private void DialogGetNewEnvironmentConfigFileName_Load(object sender, EventArgs e) {
			tbNewEnvConfigFilename.Focus();
		}

		public DialogGetNewEnvironmentConfigFileName() {
			InitializeComponent();
		}

		private void tbNewEnvConfigFilename_Validating(object sender, CancelEventArgs e) {
			TextBox tb = sender as TextBox;

			if (string.IsNullOrWhiteSpace(tb.Text))
				errorProvider1.SetError(tb, "Required");
			else if (!IsFileNameAvailable(tb.Text.Trim()))
				errorProvider1.SetError(tb, "A file with that name exists.");
			else
				errorProvider1.SetError(tb, string.Empty);

			e.Cancel = errorProvider1.GetError(tb).Length > 0;
		}

		private void tbNewEnvConfigFilename_Validated(object sender, EventArgs e) {
			TextBox tb = sender as TextBox;
			errorProvider1.SetError(tb, string.Empty);
		}

		private bool IsFileNameAvailable(string environmentConfigFileName) {
			var filePath =
				Path.Combine(
					Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
					"ConfigPTFE", "ApplicationEnvironment", string.Format("{0}.environment.config", environmentConfigFileName)
					);

			return !File.Exists(filePath);
		}
	}
}