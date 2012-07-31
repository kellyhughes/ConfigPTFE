using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ConfigPTFE.SampleConsoleAppSharedConfigs {
	internal class Program {
		private static void Main(string[] args) {

			// create sample config files
			Console.WriteLine("Creating sample config files...");
			var provider = new ConfigPTFE.AdvancedConfigurationProvider();
			
			Console.WriteLine("\r\nCreating environment.config file.");
			var sampleEnvironmentConfigFileName = provider.CreateSampleEnvironmentConfig();	// this holds the "environment name" and the directory paths to our config data
			Console.WriteLine("  done creating file: {0}", sampleEnvironmentConfigFileName);

			Console.WriteLine("\r\nCreating multiple appSetting config files.");
			var sampleAppSettingsConfigSectionFileSetDir = provider.CreateSampleAppSettingsConfigSectionFileSet();	// these hold the application data we are sharing between applications.
			Console.WriteLine("  done creating multiple files in this directory: {0}", sampleAppSettingsConfigSectionFileSetDir);

			Console.WriteLine("\r\nCreating multiple connectionString config files.");
			var sampleConnectionStringsConfigSectionFileSetDir = provider.CreateSampleConnectionStringsConfigSectionFileSet();	// these hold the connectionstrings we are sharing between applications.
			Console.WriteLine("  done creating multiple files in this directory: {0}", sampleConnectionStringsConfigSectionFileSetDir);


			// show how we're able to get the merged appSetting data from the config files located in an arbitrary location
			Console.WriteLine("\r\n\r\nRetrieve application settings from config files shared between multiple applications");
			Console.WriteLine("Merging files: 'appSettings.DEV.config' -> 'appSettings.config' -> App.config\\appSettings");
			Console.WriteLine("Results:");
			Console.WriteLine("-----------");
			foreach (var key in ConfigurationManager.AppSettings.AllKeys) {
				Console.WriteLine("{0}{1}", (key + ":").PadRight(30), ConfigurationManager.AppSettings[key]);
			}

			// show how we're able to get the merged connectionString data from the config files located in an arbitrary location
			Console.WriteLine("\r\n\r\nRetrieve connectionstrings from config files shared between multiple applications");
			Console.WriteLine("Merging files 'connectionStrings.DEV.config' -> 'connectionStrings.config'  -> App.config\\connectionStrings");
			Console.WriteLine("Results:");
			Console.WriteLine("-----------");
			foreach (ConnectionStringSettings connection in ConfigurationManager.ConnectionStrings) {
				if (connection.Name != "LocalSqlServer" && connection.Name != "LocalMySqlServer")
					Console.WriteLine("{0}{1}", (connection.Name + ":").PadRight(30), connection.ConnectionString);
			}

			Console.WriteLine("\r\n\r\n Press any key to continue...");

			Console.Read();

		}
	}
}
