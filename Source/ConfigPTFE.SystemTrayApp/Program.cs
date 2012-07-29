using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Windows.Forms;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using ConfigPTFE.SystemTrayApp;
using StructureMap;

namespace ConfigPTFE.SystemTrayApp {
	internal class Program : Form {
		#region Fields

		private NotifyIcon trayIcon;
		private ContextMenu trayMenu;
		private static LocalApplicationData _localApplicationData;

		private IEnvironmentConfigRepository _environmentConfigRepository;
		private ILocalApplicationDataRepository _localApplicationDataRepository;

		#endregion


		public Program(IEnvironmentConfigRepository environmentConfigRepository,
		               ILocalApplicationDataRepository localApplicationDataRepository) {
			// Remember our dependencies
			_environmentConfigRepository = environmentConfigRepository;
			_localApplicationDataRepository = localApplicationDataRepository;

			// Create a simple tray menu.
			trayMenu = new ContextMenu();

			// create new tray settings file for first run
			var testSettings = _localApplicationDataRepository.Get();
			if (testSettings == null)
				_localApplicationDataRepository.CreateNewSettingsFile();

			// Get our previously defined settings: environment.config location, list of environment and role names, etc.
			_localApplicationData = _localApplicationDataRepository.Get();

			BuildContextMenu();

			// Create a tray icon. 
			trayIcon = new NotifyIcon();
			trayIcon.Text = "Application Environment File Configurator";
				// Runtime Exception is thrown when this value is > 63 characters long.
			trayIcon.Icon = new Icon(GetType(), "World.ico");

			// Add menu to tray icon and show it.
			trayIcon.ContextMenu = trayMenu;
			trayIcon.Visible = true;
		}

		[STAThread]
		public static void Main() {
			ContainerBootstrapper.BootstrapStructureMap();
			Application.Run(
				ObjectFactory.GetInstance<Program>()
				);
		}

		protected override void OnLoad(EventArgs e) {
			Visible = false; // Hide form window.
			ShowInTaskbar = false; // Remove from taskbar.

			base.OnLoad(e);
		}

		private void OnExit(object sender, EventArgs e) {
			_localApplicationDataRepository.Save(_localApplicationData);
			Application.Exit();
		}

		protected override void Dispose(bool isDisposing) {
			if (isDisposing) {
				// Release the icon resource.
				trayIcon.Dispose();
			}

			base.Dispose(isDisposing);
		}

		private void MenuItem_Click(Object sender, System.EventArgs e) {
			if (!_environmentConfigRepository.Exists(_localApplicationData.EnvironmentConfigFileName)) {
				MessageBox.Show("Please select an application environment config file to adjust.");

				var options = ObjectFactory.GetInstance<OptionsForm>();
				options.Show(this);
			}
			else {
				var s = (MenuItem) sender;

				// set root level menuitems text value to display currently selected choice.
				var selectedText = s.Text.Replace("[", string.Empty).Replace("]", string.Empty); // remove brackets from "[none]"
				var baseName = ((MenuItemMetaData) s.Parent.Tag).BaseName;
				((MenuItem) s.Parent).Text = string.Format("{0} [{1}]", baseName, selectedText);

				// deselect any previously selected menuitems
				foreach (MenuItem mi in s.Parent.MenuItems) {
					if (mi.Checked)
						mi.Checked = false;
				}

				s.Checked = true;


				// find the selected menuitems
				var envnames =
					trayMenu.MenuItems.Cast<MenuItem>().Where(x => x.Text.Contains("Environment Name")).Single().MenuItems;
				var envroles =
					trayMenu.MenuItems.Cast<MenuItem>().Where(x => x.Text.Contains("Environment Role")).Single().MenuItems;
				var name = ((MenuItemMetaData) envnames.Cast<MenuItem>().Where(x => x.Checked).Single().Tag).BaseName;
				var role = ((MenuItemMetaData) envroles.Cast<MenuItem>().Where(x => x.Checked).Single().Tag).BaseName;

				// persist changes to environment.config file
				var envConfig = _environmentConfigRepository.Get(_localApplicationData.EnvironmentConfigFileName);
				envConfig.EnvironmentName = name;
				envConfig.EnvironmentRole = role;
				_environmentConfigRepository.Save(_localApplicationData.EnvironmentConfigFileName, envConfig);
			}
		}

		private void OnOptionsSelection(object sender, EventArgs e) {
			var options = ObjectFactory.GetInstance<OptionsForm>();
			options.ShowDialog(this);

			// Get our previously defined settings: environment.config location, list of environment and role names, etc.
			_localApplicationData = _localApplicationDataRepository.Get();

			BuildContextMenu();
		}

		protected internal void BuildContextMenu() {
			var envConfigSettings = _environmentConfigRepository.Get(_localApplicationData.EnvironmentConfigFileName);

			trayMenu.MenuItems.Clear();

			// Add our currently set EnvironmentName to our list if we don't have it yet.
			if (!string.IsNullOrWhiteSpace(envConfigSettings.EnvironmentName) &&
			    _localApplicationData.EnvironmentNames.Count(x => x.Text == envConfigSettings.EnvironmentName) == 0) {
				_localApplicationData.EnvironmentNames.Add(new StringWrapper() {Text = envConfigSettings.EnvironmentName});
				_localApplicationDataRepository.Save(_localApplicationData);
			}

			// Add our known environment names to the context menu.
			var envNameMenuItems =
				_localApplicationData.EnvironmentNames
					.Select(x =>
					        new MenuItem(x.Text, new EventHandler(MenuItem_Click)) {
					                                                               	Tag =
					                                                               		new MenuItemMetaData() {
					                                                               		                       	BaseName = x.Text,
					                                                               		                       	Type = MenuItemType.EnvironmentName
					                                                               		                       }
					                                                               }
					).ToArray();

			// set "checked" on current config value for env. name
			if (!string.IsNullOrWhiteSpace(envConfigSettings.EnvironmentName)) {
				foreach (var menuItem in envNameMenuItems) {
					if (menuItem.Text == envConfigSettings.EnvironmentName) {
						menuItem.Checked = true;
						break;
					}
				}
			}

			// Adjust root menu to reflect currently selected items.
			var emvironmentNameMenuItemText =
				string.Format("Environment Name [{0}]",
				              string.IsNullOrWhiteSpace(envConfigSettings.EnvironmentName)
				              	? "none"
				              	: envConfigSettings.EnvironmentName
					);

			var envNameMenuItem = new MenuItem(emvironmentNameMenuItemText, envNameMenuItems);
			envNameMenuItem.Name = envNameMenuItem.Text;
			envNameMenuItem.Tag = new MenuItemMetaData() {BaseName = "Environment Name", Type = MenuItemType.EnvironmentNameRoot};
			trayMenu.MenuItems.Add(envNameMenuItem);


			// Add our currently set EnvironmentRole to our list if we don't have it yet.
			if (!string.IsNullOrWhiteSpace(envConfigSettings.EnvironmentRole) &&
			    _localApplicationData.EnvironmentRoles.Count(x => x.Text == envConfigSettings.EnvironmentRole) == 0) {
				_localApplicationData.EnvironmentRoles.Add(new StringWrapper() {Text = envConfigSettings.EnvironmentRole});
				_localApplicationDataRepository.Save(_localApplicationData);
			}


			// Add our known role names to the context menu
			var roleNameMenuItems =
				_localApplicationData.EnvironmentRoles
					.Select(x =>
					        new MenuItem(x.Text, new EventHandler(MenuItem_Click)) {
					                                                               	Tag =
					                                                               		new MenuItemMetaData() {
					                                                               		                       	BaseName = x.Text,
					                                                               		                       	Type = MenuItemType.EnvironmentName
					                                                               		                       }
					                                                               }
					).ToArray();

			// set "checked" on current config value for env. role
			foreach (var menuItem in roleNameMenuItems) {
				if (menuItem.Text == envConfigSettings.EnvironmentRole ||
				    (string.IsNullOrWhiteSpace(envConfigSettings.EnvironmentRole) && menuItem.Text == "[none]")) {
					menuItem.Checked = true;
					break;
				}
			}

			var roleNameMenuItemText =
				string.Format("Environment Role [{0}]",
				              (string.IsNullOrWhiteSpace(envConfigSettings.EnvironmentRole) ||
				               envConfigSettings.EnvironmentRole == "[none]")
				              	? "none"
				              	: envConfigSettings.EnvironmentRole
					);

			var roleNameMenuItem = new MenuItem(roleNameMenuItemText, roleNameMenuItems);
			roleNameMenuItem.Name = roleNameMenuItem.Text;
			roleNameMenuItem.Tag = new MenuItemMetaData()
			                       {BaseName = "Environment Role", Type = MenuItemType.EnvironmentRoleRoot};
			trayMenu.MenuItems.Add(roleNameMenuItem);

			// Add a separator
			trayMenu.MenuItems.Add("-");

			// Add an options selection
			trayMenu.MenuItems.Add("Options", OnOptionsSelection);

			// Add a separator
			trayMenu.MenuItems.Add("-");

			// Add a shut down selection
			trayMenu.MenuItems.Add("Exit", OnExit);
		}
	}
}