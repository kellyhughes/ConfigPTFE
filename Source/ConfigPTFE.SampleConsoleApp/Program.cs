using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ConfigPTFE.SampleConsoleApp {
	internal class Program {
		private static void Main(string[] args) {
			Console.WriteLine("This example relies on initializing the AdvancedConfigurationProvider in code instead of with an environment.config file.");
			Console.WriteLine("The app.config settings have the only additional info we need to get our data from appSettings.config, connectionStrings.config et. al.");

			SetupValidEnvironmentChoices();
			var environmentnameChoice = GetEnvironmentName();

			BootstrapConfigPTFE(environmentnameChoice);

			Console.WriteLine(
				"\r\n\r\nMerging files: 'appSettings.{0}.config' -> 'appSettings.config' -> App.config\\appSettings",
				environmentnameChoice);
			Console.WriteLine("Results:");
			Console.WriteLine("-----------");
			foreach (var key in ConfigurationManager.AppSettings.AllKeys) {
				Console.WriteLine("{0}{1}", (key + ":").PadRight(30), ConfigurationManager.AppSettings[key]);
			}

			Console.WriteLine(
				"\r\n\r\nMerging files 'connectionStrings.{0}.config' -> 'connectionStrings.config'  -> App.config\\connectionStrings",
				environmentnameChoice);
			Console.WriteLine("Results:");
			Console.WriteLine("-----------");
			foreach (ConnectionStringSettings connection in ConfigurationManager.ConnectionStrings) {
				if (connection.Name != "LocalSqlServer" && connection.Name != "LocalMySqlServer")
					Console.WriteLine("{0}{1}", (connection.Name + ":").PadRight(30), connection.ConnectionString);
			}

			Console.WriteLine("\r\n\r\n Press any key to continue...");

			Console.Read();
		}

		/// <summary>
		/// Example usage of programatically setting the ConfigPTFE environment and encryption values
		/// </summary>
		/// <remarks>
		/// The contents of this method would be called in the ApplicationStart method of the Global.asax in a web application.
		/// Alternatively, the same configuration may be provided via config files on the filesystem (i.e. the environment.config and AES key files)
		/// </remarks>
		/// <param name="environmentnameChoice"></param>
		private static void BootstrapConfigPTFE(string environmentnameChoice) {
			var environmentName = (environmentnameChoice == "none" ? string.Empty : environmentnameChoice);

			ConfigPTFE.AdvancedConfigurationProvider.Initialize(
				x => {
					x.EnvironmentName = environmentName;
					x.EnvironmentRole = string.Empty;
					x.ConfigSectionLocations.Add("COMMONSETTINGS", @"C:\Configuration\CommonSettings\Samples");
					x.ConfigSectionLocations.Add("CONNECTIONSTRINGS", @"C:\Configuration\Connectionstrings\Samples");
					x.AesKey = GetAesKey();
					x.AesIV = GetAesIV();
				}
				);
		}


		#region Helpers

		private static Dictionary<char, string> ValidEnvironmentChoices { get; set; }

		private static void SetupValidEnvironmentChoices() {
			ValidEnvironmentChoices = new Dictionary<char, string>();
			ValidEnvironmentChoices.Add('0', "none");
			ValidEnvironmentChoices.Add('1', "DEV");
			ValidEnvironmentChoices.Add('2', "INTEGRATION");
			ValidEnvironmentChoices.Add('3', "REGRESSION");
			ValidEnvironmentChoices.Add('4', "PRODUCTION");
		}

		private static string GetEnvironmentName() {
			var environmentName = string.Empty;

			Console.WriteLine("Which environment name should we use?");
			foreach (var choice in ValidEnvironmentChoices) {
				Console.WriteLine("{0}.) {1}", choice.Key, choice.Value);
			}

			var input = Console.ReadKey(true);

			if (ValidEnvironmentChoices.ContainsKey(input.KeyChar)) {
				environmentName = ValidEnvironmentChoices[input.KeyChar];
			}
			else {
				Console.WriteLine("invalid choice\r\n\r\n");
				environmentName = GetEnvironmentName();
			}

			return environmentName;
		}

		/// <summary>
		/// Stub for returning the AES encryption key needed for decrypting encrypted config diff files
		/// </summary>
		/// <remarks>
		/// The AES encryption key returned by this method would normally be stored in some safe location and transmitted to the applicaiton via a secure channel.
		/// In other words, please don't store your encryption data like this embedded in your assemblies.
		/// </remarks>
		/// <returns></returns>
		private static byte[] GetAesKey() {
			return HexToByte("3B9D50552449BBDD1A2F8484BB89412DDEC700506C1386D64F67AF0626D50ED8");
		}

		/// <summary>
		/// Stub for returning the AES encryption IV needed for decrypting encrypted config diff files
		/// </summary>
		/// <remarks>
		/// The AES encryption IV returned by this method would normally be stored in some safe location and transmitted to the applicaiton via a secure channel
		/// /// In other words, please don't store your encryption data like this embedded in your assemblies.
		/// </remarks>
		/// <returns></returns>
		private static byte[] GetAesIV() {
			return HexToByte("842F00A89D44B81DFF4D09E524ACB3B0");
		}

		private static byte[] HexToByte(string hexString) {
			var result = new byte[hexString.Length/2];

			for (var i = 0; i < result.Length; i++) {
				result[i] = Convert.ToByte(hexString.Substring(i*2, 2), 16);
			}

			return result;
		}

		#endregion
	}
}