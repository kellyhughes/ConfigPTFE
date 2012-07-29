using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ConfigPTFE;

namespace ConfigPTFE.Admin {
	internal class Program {
		public static void Main(string[] args) {
			if (args.Length == 0 || args[0] == "/?") {
				WriteUsageMessage();
				Console.Read();
			}
			else {
				var validParameterSetFound = false;
				for (int i = 0; i < args.Length; i++) {
					var arg = args[i];

					// Fix any commands copied out of a howto doc containing an emdash or endash.
					arg = arg.Replace('–', '-') //replace endash (alt + 0150) with a dash 
						.Replace('—', '-'); //replace emdash (alt + 0151) with a dash 

					switch (arg) {
						case "-k":
							if (i + 2 <= args.Length && !args[i + 1].StartsWith("-")) {
								var regexValidKeyFilename = new System.Text.RegularExpressions.Regex(@".*\.key");
								if (regexValidKeyFilename.IsMatch(args[i + 1])) {
									validParameterSetFound = true;
									CreateNewAesKey(args[i + 1]);
								}
								else {
									Console.WriteLine("The -k option's parameter was not properly formed [ex: common.key].");
								}
							}
							else {
								Console.WriteLine("The -k option did not have the required parameter.");
							}

							break;

						case "-pe":
							if (i + 3 <= args.Length && !args[i + 1].StartsWith("-") && !args[i + 2].StartsWith("-")) {
								validParameterSetFound = true;
								EncryptConfigSectionWithAES(args[i + 1], args[i + 2]);
							}
							else {
								Console.WriteLine("The -pe option did not have the required number of associated parameters.");
							}

							break;

						case "-pd":
							if (i + 3 <= args.Length && !args[i + 1].StartsWith("-") && !args[i + 2].StartsWith("-")) {
								validParameterSetFound = true;
								DecryptConfigSectionWithAES(args[i + 1], args[i + 2]);
							}
							else {
								Console.WriteLine("The -pd option did not have the required number of associated parameters.");
							}

							break;

						case "-dpp":
							if (i + 2 <= args.Length && !args[i + 1].StartsWith("-")) {
								validParameterSetFound = true;
								ProtectAesKeyWithDPAPI(args[i + 1]);
							}
							else {
								Console.WriteLine("The -dpp option did not have the required number of associated parameters");
							}

							break;

						case "-dpu":
							if (i + 2 <= args.Length && !args[i + 1].StartsWith("-")) {
								validParameterSetFound = true;
								UnProtectAesKeyWithDPAPI(args[i + 1]);
							}
							else {
								Console.WriteLine("The -dpu option did not have the required number of associated parameters");
							}

							break;

						case "-ce":
							validParameterSetFound = true;
							CreateSampleEnvironmentFile();
							break;

						default:
							break;
					}
				}

				if (!validParameterSetFound) {
					Console.WriteLine("Incorrect Usage.");
					WriteUsageMessage();
				}
			}
		}

		private static void WriteUsageMessage() {
			Console.Write(
				@"-- RelocatedConnectionstringSectionConfigProvider CONFIGURATION TOOL OPTIONS --

-k <path>               Create a new AES key with the provided filename
                        at the following location:
                        %ApplicationData%\ConfigPTFE\Crypto\AES\MachineKeys\
                        
                        Where is %ApplicationData% ?
                        Windows XP & Windows Server 2003: 
                            ""C:\Documents and Settings\All Users\Application Data\""
                        Windows Vista / Windows 7 / Windows Server 2008:    
                            ""C:\ProgramData\""

    Example: 
    -k ""common.key""


-pe <path> <filename>   Encrypt the relocated configuration section at 
                        the provided filepath with the specified key.
    Example: 
    -pe ""c:\sourcecontrol\configuration\connectionstrings\connectionstrings.config"" ""common.key""


-pd <path> <filename>   Decrypt the relocated configuration section at 
                        the provided filepath with the specified key.
    Example: 
    -pd ""c:\sourcecontrol\configuration\connectionstrings\connectionstrings.config"" ""common.key""

-dpp <filename>         Protect the provided AES encryption key with the DPAPI

    Example:
    -dpp ""common.key""

-dpu <filename>         UnProtect the provided AES encryption key with the DPAPI

    Example:
    -dpu ""common.key""

-ce                     Create a sample environment file at the following location:
                        %ApplicationData%\ConfigPTFE\ApplicationEnvironment\
                        
                        Where is %ApplicationData% ?
                        Windows XP & Windows Server 2003: 
                            ""C:\Documents and Settings\All Users\Application Data\""
                        Windows Vista / Windows 7 / Windows Server 2008:    
                            ""C:\ProgramData\""

USAGE NOTES:
Options will be run in the order they are provided."
				);
		}

		private static void CreateNewAesKey(string KeyFileName) {
			var createkey = true;
			var provider = new AdvancedConfigurationProvider();
			var filePath = provider.BuildKeyFilePath(KeyFileName);

			if (!Directory.Exists(provider.KeyDirectory)) {
				Directory.CreateDirectory(provider.KeyDirectory);
			}

			if (File.Exists(filePath)) {
				Console.WriteLine("A file already exists at: ", filePath);
				Console.WriteLine("Are you sure you want to continue?  Y/N");

				var k = Console.ReadKey(true);
				createkey = (k.Key != ConsoleKey.N);
			}

			if (createkey) {
				try {
					provider.CreateKey(KeyFileName);
					Console.WriteLine("\r\n♥♥♥♥♥♥♥♥♥♥♥");
					Console.WriteLine("♥ Success ♥");
					Console.WriteLine("♥♥♥♥♥♥♥♥♥♥♥\r\n");
					Console.WriteLine("New AES Key written to '{0}'\r\n", filePath);
				}
				catch (Exception ex) {
					Console.WriteLine("\r\n\r\n/////////////////////////////////");
					Console.WriteLine("//      WARNING: FAILURE!      //");
					Console.WriteLine("/////////////////////////////////\r\n");
					Console.WriteLine("Error encountered while attempting to create a new AES key.\r\n");
					Console.WriteLine(ex.Message);
				}
			}
		}

		private static void ProtectAesKeyWithDPAPI(string KeyFileName) {
			var provider = new AdvancedConfigurationProvider();
			var filePath = provider.BuildKeyFilePath(KeyFileName);

			try {
				provider.ProtectKeyWithDPAPI(KeyFileName);
				Console.WriteLine("\r\n♥♥♥♥♥♥♥♥♥♥♥");
				Console.WriteLine("♥ Success ♥");
				Console.WriteLine("♥♥♥♥♥♥♥♥♥♥♥\r\n");
				Console.WriteLine("AES Key protected by DPAPI at '{0}'\r\n", filePath);
			}
			catch (Exception ex) {
				Console.WriteLine("\r\n/////////////////////////////////");
				Console.WriteLine("//      WARNING: FAILURE!      //");
				Console.WriteLine("/////////////////////////////////\r\n");
				Console.WriteLine("Error encountered while attempting to protect AES key with DPAPI.\r\n");
				Console.WriteLine(ex.Message);
			}
		}

		private static void UnProtectAesKeyWithDPAPI(string KeyFileName) {
			var provider = new AdvancedConfigurationProvider();
			var filePath = provider.BuildKeyFilePath(KeyFileName);

			try {
				provider.UnProtectKeyWithDPAPI(KeyFileName);
				Console.WriteLine("\r\n♥♥♥♥♥♥♥♥♥♥♥");
				Console.WriteLine("♥ Success ♥");
				Console.WriteLine("♥♥♥♥♥♥♥♥♥♥♥\r\n");
				Console.WriteLine("AES Key unprotected by DPAPI at '{0}'\r\n", filePath);
			}
			catch (Exception ex) {
				Console.WriteLine("\r\n/////////////////////////////////");
				Console.WriteLine("//      WARNING: FAILURE!      //");
				Console.WriteLine("/////////////////////////////////\r\n");
				Console.WriteLine("Error encountered while attempting to unprotect AES key with DPAPI.\r\n");
				Console.WriteLine(ex.Message);
			}
		}

		private static void EncryptConfigSectionWithAES(string FilePath, string KeyFilename) {
			try {
				SetConfigSection(FilePath, KeyFilename, true);
				Console.WriteLine("\r\n♥♥♥♥♥♥♥♥♥♥♥");
				Console.WriteLine("♥ Success ♥");
				Console.WriteLine("♥♥♥♥♥♥♥♥♥♥♥\r\n");
				Console.WriteLine("Config file located at '{0}' was encrypted with the AES key '{1}'\r\n", FilePath, KeyFilename);
			}
			catch (Exception ex) {
				Console.WriteLine("\r\n/////////////////////////////////");
				Console.WriteLine("//      WARNING: FAILURE!      //");
				Console.WriteLine("/////////////////////////////////\r\n");
				Console.WriteLine("Error encountered while attempting to encrypt the file '{0}' with the AES key '{1}'.\r\n",
				                  FilePath, KeyFilename);
				Console.WriteLine(ex.Message);
			}
		}

		private static void DecryptConfigSectionWithAES(string FilePath, string KeyFilename) {
			try {
				SetConfigSection(FilePath, KeyFilename, false);
				Console.WriteLine("\r\n♥♥♥♥♥♥♥♥♥♥♥");
				Console.WriteLine("♥ Success ♥");
				Console.WriteLine("♥♥♥♥♥♥♥♥♥♥♥\r\n");
				Console.WriteLine("Config file located at '{0}' was decrypted with the AES key '{1}'\r\n", FilePath, KeyFilename);
			}
			catch (Exception ex) {
				Console.WriteLine("\r\n/////////////////////////////////");
				Console.WriteLine("//      WARNING: FAILURE!      //");
				Console.WriteLine("/////////////////////////////////\r\n");
				Console.WriteLine("Error encountered while attempting to decrypt the file '{0}' with the AES key '{1}'.\r\n",
				                  FilePath, KeyFilename);
				Console.WriteLine(ex.Message);
			}
		}

		private static void SetConfigSection(string ConfigFilePath, string KeyFilename, bool EncryptSection) {
			var config = new System.Collections.Specialized.NameValueCollection();
			config.Add("keyFileName", KeyFilename);

			var provider = new AdvancedConfigurationProvider();
			provider.Initialize("ProviderForConfigTool", config);

			if (EncryptSection)
				provider.EncryptRelocatedConfigSectionWithAES(ConfigFilePath);
			else
				provider.DecryptRelocatedConfigSectionWithAES(ConfigFilePath);
		}

		private static void CreateSampleEnvironmentFile() {
			var provider = new AdvancedConfigurationProvider();
			var filePath = provider.BuildEnvironmentFilePath("sample.environment.config");
			var createFile = true;

			if (!Directory.Exists(provider.EnvironmentFileDirectory)) {
				Directory.CreateDirectory(provider.EnvironmentFileDirectory);
			}

			if (File.Exists(filePath)) {
				Console.WriteLine("A file already exists at: ", filePath);
				Console.WriteLine("Are you sure you want to continue?  Y/N");

				var k = Console.ReadKey(true);
				createFile = (k.Key != ConsoleKey.N);
			}

			if (createFile) {
				try {
					provider.CreateSampleEnvironmentConfig();
					Console.WriteLine("\r\n♥♥♥♥♥♥♥♥♥♥♥");
					Console.WriteLine("♥ Success ♥");
					Console.WriteLine("♥♥♥♥♥♥♥♥♥♥♥\r\n");
					Console.WriteLine("New sample environment file written to '{0}'\r\n", filePath);
				}
				catch (Exception ex) {
					Console.WriteLine("\r\n\r\n/////////////////////////////////");
					Console.WriteLine("//      WARNING: FAILURE!      //");
					Console.WriteLine("/////////////////////////////////\r\n");
					Console.WriteLine("Error encountered while attempting to create a new sample environment file.\r\n");
					Console.WriteLine(ex.Message);
				}
			}
		}
	}
}