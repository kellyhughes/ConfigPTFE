using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using StructureMap;

namespace ConfigPTFE.SystemTrayApp {
	public partial class OptionsForm : Form {
		private ILocalApplicationDataRepository _localApplicationDataRepository;
		private IEnvironmentConfigRepository _environmentConfigRepository;

		private BindingList<StringWrapper> _environmentNamesToDisplayInTrayMenu;
		private BindingList<StringWrapper> _environmentRolesToDisplayInTrayMenu;

		public OptionsForm(ILocalApplicationDataRepository localApplicationDataRepository,
		                   IEnvironmentConfigRepository environmentConfigRepository) {
			_localApplicationDataRepository = localApplicationDataRepository;
			_environmentConfigRepository = environmentConfigRepository;

			InitializeComponent();
		}

		private void OptionsForm_Load(object sender, EventArgs e) {
			PopulateDdlWithAvailableEnvironmentConfigFiles();
			PopulateDataGridViews();
		}

		private void btnOK_Click(object sender, EventArgs e) {
			SaveSettings();
			this.Close();
		}

		private void btnCancel_Click(object sender, EventArgs e) {
			this.Close();
		}

		private void SaveSettings() {
			var settings = _localApplicationDataRepository.Get();

			settings.EnvironmentConfigFileName = ddlEnvironmentConfigFileNames.Text;
			settings.EnvironmentNames =
				new BindingList<StringWrapper>(
					_environmentNamesToDisplayInTrayMenu
						.Where(x => x.Text != string.Empty)
						.Select(x => new StringWrapper() {Text = x.Text})
						.ToList()
					);

			settings.EnvironmentRoles =
				new BindingList<StringWrapper>(
					_environmentRolesToDisplayInTrayMenu
						.Where(x => x.Text != string.Empty)
						.Select(x => new StringWrapper() {Text = x.Text})
						.ToList()
					);

			_localApplicationDataRepository.Save(settings);
		}

		private void btnEditEnvironmentConfigFile_Click(object sender, EventArgs e) {
			if (ddlEnvironmentConfigFileNames.SelectedItem != null) {
				var dlg = ObjectFactory.GetInstance<DialogEditEnvironmentConfig>();

				// tell the form which environment config file to edit
				dlg.TargetEnvironmentConfigFileName = ddlEnvironmentConfigFileNames.Text;

				// setup the appropriate Env. Name & Role suggestions from the list on this form.
				dlg.EnvironmentNames =
					new BindingList<string>(
						_environmentNamesToDisplayInTrayMenu.Select(x => x.Text).ToList()
						);

				dlg.EnvironmentRoles =
					new BindingList<string>(
						_environmentRolesToDisplayInTrayMenu.Select(x => x.Text).ToList()
						);

				dlg.ShowDialog(this);
			}
			else {
				MessageBox.Show("Please select a environment config file to edit.");
			}
		}

		private void linkCreateNewEnvironmentConfigFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			var dialog = new DialogGetNewEnvironmentConfigFileName();
			var result = dialog.ShowDialog();
			var newApplicationEnvironmentConfigFileName = string.Empty;

			if (result == DialogResult.OK) {
				newApplicationEnvironmentConfigFileName =
					string.Format("{0}.environment.config", dialog.tbNewEnvConfigFilename.Text);

				try {
					// remember the currently selected ddl item
					string previouslySelectedItem = string.Empty;
					if (ddlEnvironmentConfigFileNames.SelectedItem != null)
						previouslySelectedItem = (string) ddlEnvironmentConfigFileNames.SelectedItem;

					CreateSampleEnvironmentConfigFile(newApplicationEnvironmentConfigFileName);
					PopulateDdlWithAvailableEnvironmentConfigFiles();

					// select the previously selected item, or the only one in the list if applicable
					ddlEnvironmentConfigFileNames.SelectedItem =
						ddlEnvironmentConfigFileNames.Items.Count == 1
							? ddlEnvironmentConfigFileNames.Items[0]
							: ddlEnvironmentConfigFileNames.Items[ddlEnvironmentConfigFileNames.Items.IndexOf(previouslySelectedItem)];

					MessageBox.Show(
						"The file was successfully created.\r\n\r\nNOTE: sample config section locations were added to this file. You may need to adjust these values for your environment.",
						"Success!",
						MessageBoxButtons.OK,
						MessageBoxIcon.Information
						);
				}
				catch (Exception ex) {
					MessageBox.Show(
						string.Format("An error has occured while attempting to create the config file: \r\n", ex.Message),
						"Error",
						MessageBoxButtons.OK,
						MessageBoxIcon.Error
						);
				}
			}
		}

		private void CreateSampleEnvironmentConfigFile(string environmentConfigFileName) {
			var settings =
				new EnvironmentConfigSettings() {
				                                	EnvironmentName = "DEV",
				                                	EnvironmentRole = string.Empty,
				                                	ConfigSectionLocations =
				                                		new BindingList<ConfigSectionLocation>() {
				                                		                                         	new ConfigSectionLocation(
				                                		                                         		"COMMONSETTINGS",
				                                		                                         		@"C:\SourceControl\Configuration\CommonSettings"),
				                                		                                         	new ConfigSectionLocation(
				                                		                                         		"CONNECTIONSTRINGS",
				                                		                                         		@"C:\SourceControl\Configuration\Connectionstrings")
				                                		                                         }
				                                };

			_environmentConfigRepository.Save(
				environmentConfigFileName,
				settings
				);
		}

/*
        private void OverwriteEnvironmentValuesWithLocalCache()
        {
            var environmentConfigSettings = _environmentConfigRepository.Get();

            // do not allow deletion of the "[none]" option)
            if (_environmentRoles.Count(x => x.Text == "[none]") == 0)
                _environmentRoles.Add(new StringWrapper(){Text = "[none]"});

            _.EnvironmentConfigFileName = ddlEnvironmentConfigFileNames.Text.Trim();

            SysTrayApp._localApplicationData.EnvironmentNames =
                new BindingList<StringWrapper>(
                    _environmentNames.ToList()
                    );

            SysTrayApp._localApplicationData.EnvironmentRoles =
                new BindingList<StringWrapper>(
                    _environmentRoles.OrderBy(x => x.Text).ToList()
                    );
        }

*/

		private void PopulateDataGridViews() {
			// Get a local copy of environment names list for binding against datagridview
			_environmentNamesToDisplayInTrayMenu =
				new BindingList<StringWrapper>(
					_localApplicationDataRepository.Get().EnvironmentNames.ToList()
					);

			// Get a local copy of environment roles list for binding against datagridview
			_environmentRolesToDisplayInTrayMenu =
				new BindingList<StringWrapper>(
					_localApplicationDataRepository.Get().EnvironmentRoles.ToList()
					);

			// Fill the datagridviews
			dgvEnvironmentNames.DataSource = _environmentNamesToDisplayInTrayMenu;
			dgvEnvironmentRoles.DataSource = _environmentRolesToDisplayInTrayMenu;
		}

		private void PopulateDdlWithAvailableEnvironmentConfigFiles() {
			// Fill ddl with filenames of config files on disk.
			var appEnvDirectory =
				Path.Combine(
					Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
					"ConfigPTFE", "ApplicationEnvironment"
					);

			var di = new DirectoryInfo(appEnvDirectory);
			var fis = di.GetFiles("*.environment.config");

			ddlEnvironmentConfigFileNames.Items.Clear();

			foreach (var fi in fis) {
				ddlEnvironmentConfigFileNames.Items.Add(fi.Name);
			}


			// Select the currently targeted environment config file name in our dropdownlist (if possible)
			var currentEnvironmentConfigFileName = _localApplicationDataRepository.Get().EnvironmentConfigFileName;

			var item = ddlEnvironmentConfigFileNames.Items.OfType<string>()
				.SingleOrDefault(x => x == currentEnvironmentConfigFileName);

			if (item != null) {
				ddlEnvironmentConfigFileNames.SelectedItem =
					ddlEnvironmentConfigFileNames.Items[ddlEnvironmentConfigFileNames.Items.IndexOf(item)];
			}
		}

		private void linkBrowseToConfigDirectory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			using (System.Diagnostics.Process process = new System.Diagnostics.Process()) {
				process.EnableRaisingEvents = false;
				process.StartInfo.FileName = "explorer";
				process.StartInfo.Arguments =
					Path.Combine(
						Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
						"ConfigPTFE", "ApplicationEnvironment"
						);
				process.Start();
			}
		}
	}
}