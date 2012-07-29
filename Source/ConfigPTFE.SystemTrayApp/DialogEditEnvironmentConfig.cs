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
	public partial class DialogEditEnvironmentConfig : Form {
		private ILocalApplicationDataRepository _localApplicationDataRepository;
		private IEnvironmentConfigRepository _environmentConfigRepository;

		public string TargetEnvironmentConfigFileName { get; set; }
		public BindingList<string> EnvironmentNames { get; set; }
		public BindingList<string> EnvironmentRoles { get; set; }
		public BindingList<ConfigSectionLocation> ConfigSectionLocations { get; set; }

		private string SelectedEnvironmentName = string.Empty;
		private string SelectedEnvironmentRole = string.Empty;

		public DialogEditEnvironmentConfig(ILocalApplicationDataRepository localApplicationDataRepository,
		                                   IEnvironmentConfigRepository environmentConfigRepository) {
			_localApplicationDataRepository = localApplicationDataRepository;
			_environmentConfigRepository = environmentConfigRepository;

			InitializeComponent();
		}

		private void dialogEditEnvironmentConfig_Load(object sender, EventArgs e) {
			LoadConfigSectionLocations();

			ddlEnvironmentName.DataSource = EnvironmentNames;
			ddlEnvironmentRole.DataSource = EnvironmentRoles;
			dgvConfigSectionLocations.DataSource = ConfigSectionLocations;

			InitializeSelections();
		}

		private void InitializeSelections() {
			var enIndex = ddlEnvironmentName.Items.IndexOf(SelectedEnvironmentName);
			if (enIndex != -1) {
				ddlEnvironmentName.SelectedIndex = enIndex;
			}

			var erIndex = ddlEnvironmentRole.Items.IndexOf(SelectedEnvironmentRole);
			if (erIndex != -1) {
				ddlEnvironmentRole.SelectedIndex = erIndex;
			}
		}

/*
        private void dgvConfigSectionLocations_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            var cell = dgvConfigSectionLocations[e.ColumnIndex, e.RowIndex];

            // validate key column
            if (e.ColumnIndex == 0 && cell.IsInEditMode) {
                for (int i = 0; i < ((BindingList<ConfigSectionLocation>)dgvConfigSectionLocations.DataSource).Count-1; i++) {
                    if ( i != e.RowIndex && 
                        ((string)dgvConfigSectionLocations[0, i].FormattedValue) == (string)e.FormattedValue ) 
                    {
                        e.Cancel = true;
                        //cell.ErrorText = "Duplicate key value.";
                        cell.OwningRow.ErrorText = "Duplicate key value.";
                        break;
                    }
                }
            }

            if (e.ColumnIndex == 1 && cell.IsInEditMode) {
                if (!Directory.Exists((string)e.FormattedValue)) {
                    cell.OwningRow.ErrorText = "Invalid Directory path.";
                    e.Cancel = true;
                }
            }
        }

        private void dgvConfigSectionLocations_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dgvConfigSectionLocations.Rows[e.RowIndex].ErrorText = string.Empty;
        }
*/

		private void btnOK_Click(object sender, EventArgs e) {
			var formValidationResult = ValidateForm();

			if (formValidationResult.IsValid) {
				SaveEnvironmentConfig();
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
			else {
				var errorMessage =
					formValidationResult.ErrorMessages
						.Aggregate((current, next) => current.ToString() + "\r\n" + next.ToString());

				MessageBox.Show(string.Format("Please correct these errors before continuing:\r\n{0}", errorMessage),
				                "Validation Errors",
				                MessageBoxButtons.OK,
				                MessageBoxIcon.Stop);
			}
		}

		private void btnCancel_Click(object sender, EventArgs e) {
			this.DialogResult = DialogResult.Cancel;
		}

		private void LoadConfigSectionLocations() {
			var environmentConfigSettings = _environmentConfigRepository.Get(TargetEnvironmentConfigFileName);
			ConfigSectionLocations = environmentConfigSettings.ConfigSectionLocations;
			SelectedEnvironmentName = environmentConfigSettings.EnvironmentName;

			var role = environmentConfigSettings.EnvironmentRole.Trim();
			SelectedEnvironmentRole =
				role == string.Empty
					? "[none]"
					: environmentConfigSettings.EnvironmentRole;
		}

		private void SaveEnvironmentConfig() {
			var environmentConfigSettings = _environmentConfigRepository.Get(TargetEnvironmentConfigFileName);
			environmentConfigSettings.EnvironmentName = ddlEnvironmentName.Text;
			environmentConfigSettings.EnvironmentRole = ddlEnvironmentRole.Text;
			environmentConfigSettings.ConfigSectionLocations =
				new BindingList<ConfigSectionLocation>(
					dgvConfigSectionLocations.Rows.Cast<DataGridViewRow>()
						.Where(x =>
						       ((string) x.Cells[0].FormattedValue) != string.Empty &&
						       ((string) x.Cells[1].FormattedValue) != string.Empty
						)
						.Select(x =>
						        new ConfigSectionLocation(
						        	(string) x.Cells[0].FormattedValue,
						        	(string) x.Cells[1].FormattedValue
						        	)
						)
						.ToList()
					);
			_environmentConfigRepository.Save(TargetEnvironmentConfigFileName, environmentConfigSettings);
		}

		private void ddlEnvironmentName_Validating(object sender, CancelEventArgs e) {
			if (string.IsNullOrWhiteSpace(((ComboBox) sender).Text)) {
				errorProvider1.SetError(ddlEnvironmentName, "Required. Empty string or whitespace disallowed.");
				e.Cancel = true;
			}
		}

		private void ddlEnvironmentName_TextChanged(object sender, EventArgs e) {
			errorProvider1.SetError(ddlEnvironmentName, string.Empty);
		}

/*
        private void dgvConfigSectionLocations_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            var grid = (DataGridView) sender;
            if (grid.IsCurrentRowDirty) {
                var row = dgvConfigSectionLocations.Rows[e.RowIndex];
                var keyCell = row.Cells[0];
                var valueCell = row.Cells[1];
                //e.Cancel = !(IsKeyValid(keyCell) && IsValueValid(valueCell));
                IsKeyValid(keyCell);
                IsValueValid(valueCell);
            }
        }

        private void dgvConfigSectionLocations_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            errorProvider1.SetError(ddlEnvironmentName, string.Empty);
            foreach (DataGridViewCell cell in dgvConfigSectionLocations.Rows[e.RowIndex].Cells) {
                cell.ErrorText = string.Empty;
            }
        }


        private bool IsKeyValid(DataGridViewCell cell)
        {
            if(string.IsNullOrWhiteSpace(cell.FormattedValue.ToString())) {
                cell.ErrorText = "Required";
            }
            else
            {
                var count =
                    dgvConfigSectionLocations.Rows
                        .Cast<DataGridViewRow>()
                        .Where(x => x.Cells[0].FormattedValue.ToString() == cell.FormattedValue.ToString())
                        .Count();

                if (count > 1)
                    cell.ErrorText = "Duplicate key.";
            }

            return (cell.ErrorText.Length == 0);
        }

        private bool IsValueValid(DataGridViewCell cell)
        {
            var result = true;

            if (!Directory.Exists((string)cell.FormattedValue)) {
                cell.ErrorText = "Invalid Directory path.";
                result = false;
            }

            return result;
        }
*/

		private ValidationResult ValidateForm() {
			var environmentNameValidation = ValidateEnvironmentName();
			var environmentRoleValidation = ValidateEnvironmentRole();
			var configSectionLocationsGridValidation = ValidateConfigSectionLocationsGrid();

			var result = new ValidationResult();

			result.ErrorMessages =
				environmentNameValidation.ErrorMessages
					.Concat(environmentRoleValidation.ErrorMessages)
					.Concat(configSectionLocationsGridValidation.ErrorMessages)
					.ToList();

			return result;
		}

		private ValidationResult ValidateEnvironmentName() {
			var result = new ValidationResult();

			if (ddlEnvironmentName.Text.Trim().Length == 0) {
				result.ErrorMessages.Add("Environment Name is a required field.");
				errorProvider1.SetError(ddlEnvironmentName, "Required");
			}
			else {
				errorProvider1.SetError(ddlEnvironmentName, string.Empty);
			}

			return result;
		}

		private ValidationResult ValidateEnvironmentRole() {
			var result = new ValidationResult();
			return result;
		}

		private ValidationResult ValidateConfigSectionLocationsGrid() {
			var result = new ValidationResult();

			// Check for duplicate key values
			ValidateForUniqueKeyValues(ref dgvConfigSectionLocations, ref result);

			// Check for empty keys
			ValidateForEmptyKeys(ref dgvConfigSectionLocations, ref result);

			// Check for invalid directory paths in value column
			ValidateForInvalidDirectories(ref dgvConfigSectionLocations, ref result);

			errorProvider1.SetError(dgvConfigSectionLocations,
			                        result.IsValid
			                        	? string.Empty
			                        	: result.ErrorMessages.Aggregate((current, next) => current + "\r\n" + next));

			return result;
		}

		protected internal void ValidateForInvalidDirectories(ref DataGridView dgv, ref ValidationResult result) {
			//  grab all the directory paths
			var badDirectoryPaths =
				dgv.Rows.Cast<DataGridViewRow>()
					.Where(
						r => ((string) r.Cells[0].FormattedValue) != string.Empty && ((string) r.Cells[1].FormattedValue) != string.Empty)
					// do not capture the "add new" row
					.Where(r => !Directory.Exists((string) r.Cells[1].FormattedValue))
					.Select(x => ((string) x.Cells[1].FormattedValue))
					.ToList();

			//  append error message for those paths which are invalid
			foreach (var badDirectoryPath in badDirectoryPaths) {
				result.ErrorMessages.Add(string.Format("Invalid path: \"{0}\"", badDirectoryPath));
			}
		}

		protected internal void ValidateForEmptyKeys(ref DataGridView dgv, ref ValidationResult result) {
			var numberOfEmptyKeys =
				dgv.Rows.Cast<DataGridViewRow>()
					.Count(x => string.IsNullOrWhiteSpace((string) x.Cells[0].FormattedValue));

			if (numberOfEmptyKeys > 1) {
				result.ErrorMessages.Add(string.Format("{0} empty key(s) found.", numberOfEmptyKeys - 1));
			}
		}

		protected internal void ValidateForUniqueKeyValues(ref DataGridView dgv, ref ValidationResult result) {
			//  do a "groupby" to count the number of times each unique string shows up in our key column
			var groupByResult =
				dgv.Rows.Cast<DataGridViewRow>()
					.Where(r => ((string) r.Cells[0].FormattedValue) != string.Empty)
					.GroupBy(x => ((string) x.Cells[0].FormattedValue))
					.Select(x => new {key = x.Key, number = x.Count()});

			//  ensure each string shows up in key list only once.
			if (groupByResult.Count(x => x.number > 1) > 0)
				result.ErrorMessages.Add("Duplicate keys found.");
		}
	}
}