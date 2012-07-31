using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.Common;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace ConfigPTFE {
	public class AdvancedConfigurationProvider : ProtectedConfigurationProvider {
		#region Constants

		protected const string AesCryptoServiceProviderNotInitialized =
			@"AesCryptoServiceProvider has not been initialized. 

TIP: Check for the existance of a key file in the ApplicationData directory.  
Ex. Windows XP / Windows Server 2003: 
  ""C:\Documents and Settings\All Users\Application Data\ConfigPTFE\Crypto\AES\MachineKeys""
Windows Vista / Windows 7 / Windows Server 2008: 
  ""C:\ProgramData\ConfigPTFE\Crypto\AES\MachineKeys""";

		protected const string ConfigFileNotFound =
			@"Configuration section(s) not found at specified path(s).
File expected at the following location(s):
{0}
{1}";

		protected const string ConfigSectionLocationEntryMissing =
			@"Environment specified in config section file location(s) not found in list of locations specified in environment file.
{0}
{1}";

		#endregion


		#region Fields

		protected static AesCryptoServiceProvider Aes = new AesCryptoServiceProvider();

		#endregion


		#region Properties

		// Encryption specific properties

		/// <summary>
		/// The absolute path on the filesystem to the encryption key used by this library.
		/// </summary>
		public string KeyFilePath { get; private set; }

		/// <summary>
		/// The location inside the "ApplicationData" directory where this ConfigurationProvider is expecting the AES encryption/decryption key to reside.
		/// </summary>
		/// <remarks>
		/// The "ApplicationData" directory location varies by operating system.
		/// </remarks>
		/// <example>
		/// Windows XP / Windows Server 2003:                   "C:\Documents and Settings\All Users\Application Data\ConfigPTFE\Crypto\AES\MachineKeys"
		/// Windows Vista / Windows 7 / Windows Server 2008:    "C:\ProgramData\ConfigPTFE\Crypto\AES\MachineKeys"
		/// </example>
		public string KeyDirectory {
			get {
				return string.Format(
					"{0}{1}",
					Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
					string.Format("{0}ConfigPTFE{0}Crypto{0}AES{0}MachineKeys{0}", Path.DirectorySeparatorChar)
					);
			}
		}

		private bool _isAesCryptoServiceProviderInitialized = false;

		/// <summary>
		/// Indicates whether the global AesCryptoServiceProvider object has had its "Key" and "IV" properties has been set.
		/// </summary>
		/// <remarks>
		/// The "Key" and "IV" properties are set from text found in the specified ".key" file on the filesystem.
		/// The ".key" file is located in the known location specified in the "KeyFilePath" property on this object.
		/// </remarks>
		public bool IsAesCryptoServiceProviderInitialized {
			get { return _isAesCryptoServiceProviderInitialized; }
		}

		/// <summary>
		/// String used to add an additional layer of DPAPI protection for the encryption key used by this library.
		/// </summary>
		/// <remarks>
		/// If a user-supplied entropy value is provided during the creation/administration of the encryption key 
		/// (before the ProtectKeyWithDPAPI and UnProtectKeyWithDPAPI methods are called by an admin tool)
		/// it will need to be supplied by your application at runtime during initialization before the "Decrypt" method is called 
		/// (the method used at runtime by the .NET framework to retrieve config values in plaintext via this library.)
		/// </remarks>
		private string _entropy;
		protected string Entropy {
			set { _entropy = value; }
		}


		// Environment specific properties

		/// <summary>
		/// The name of the machine or machine group used by this library to find the appropriate config file.
		/// </summary>
		/// <example>
		/// PRODUCTION
		/// </example>
		/// <remarks>
		/// This value allows the ConfigPTFE library to merge the appropriate "diff" config files into the app.config/web.config.
		/// For example, given the following files:
		///   connectionstrings.config, connectionstrings.DEV.config, and connectionstrings.PRODUCTION.config
		/// If this value is set to "PRODUCTION", the configsection files "connectionstrings.config" and "connectionstrings.PRODUCTION.config" would be merged app.config or web.config at runtime.
		/// If this value is set to "DEV", the configsection files "connectionstrings.config" and "connectionstrings.DEV.config" would be merged app.config or web.config at runtime.
		/// </remarks>
		public string EnvironmentName { get; private set; }

		/// <summary>
		/// The name of the role (within a machine group) used by this library to find the appropriate config file.  This is an optional property.
		/// </summary>
		/// <example>
		/// WEBSERVER
		/// </example>
		/// /// <remarks>
		/// This value allows the ConfigPTFE library to merge the appropriate "diff" config file into the app.config/web.config.
		/// For example, given the following files:
		///   appsettings.config, appsettings.DEV.WEBSERVER.config, appsettings.DEV.WINDOWSSERVICES.config
		/// If this value is set to "WEBSERVER", the configsection files "appsettings.config" and "appsettings.DEV.WEBSERVER.config" would be merged app.config or web.config at runtime.
		/// If this value is set to "WINDOWSSERVICES", the configsection files "appsettings.config" and "appsettings.DEV.WINDOWSSERVICES.config" would be merged app.config or web.config at runtime.
		/// </remarks>
		public string EnvironmentRole { get; private set; }

		/// <summary>
		/// The filesystem path to the "environment file" used to store the environment name, environment role, and the filepaths to all configsection files 
		/// (ex. connectionstring.config, appsettings.config, etc.)
		/// </summary>
		public string EnvironmentFilePath { get; private set; }

		/// <summary>
		/// Have the values stored in the Environment file been loaded into local variables?
		/// </summary>
		public bool IsEnvironmentInitialized { get; private set; }

		/// <summary>
		/// The locations of the directories used to store configsection and diff files.
		/// </summary>
		/// <example>
		/// "CONNECTIONSTRINGS", "c:\configuration\connectionstrings"
		/// </example>
		/// <remarks>
		/// This dictionary is loaded from the environment file.  Its' values decouple the location on the filesystem of the various settings 
		/// you wish to pull back into your application at runtime.
		/// On a workstation, the "CONNECTIONSTRINGS" directory might exist at "D:\WORKSPACE\configuration\connectionstrings".
		/// On a webserver, the "CONNECTIONSTRINGS" directory might exist at "C:\configuration\connectionstrings".
		/// </remarks>
		public Dictionary<string, string> ConfigSectionLocations { get; private set; }

		/// <summary>
		/// The location inside the "ApplicationData" directory where this ConfigurationProvider is expecting the Environment File to reside.
		/// </summary>
		/// <remarks>
		/// The "ApplicationData" directory location varies by operating system.
		/// </remarks>
		/// <example>
		/// Windows XP / Windows Server 2003:                   "C:\Documents and Settings\All Users\Application Data\ConfigPTFE\Crypto\AES\MachineKeys"
		/// Windows Vista / Windows 7 / Windows Server 2008:    "C:\ProgramData\ConfigPTFE\Crypto\AES\MachineKeys"
		/// </example>
		public string EnvironmentFileDirectory {
			get {
				return string.Format(
					"{0}{1}",
					Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
					string.Format("{0}ConfigPTFE{0}ApplicationEnvironment{0}", Path.DirectorySeparatorChar)
					);
			}
		}

		#endregion


		public AdvancedConfigurationProvider() {
			ConfigSectionLocations = new Dictionary<string, string>();
			Entropy = "b14ckh4t$d3$3rv3j41Lt1m3"; // set default DPAPI entropy.
		}

		private static IInitializationExpression InitializationData { get; set; }

		/// <summary>
		/// This method may be used during the applications' initialization methods (ex. Global.asax, etc.)
		/// to provide environment and encryption values without needing to provide them via files on the filesystem
		/// (ex. "environment.config" or "common.key")
		/// </summary>
		/// <param name="action"></param>
		public static void Initialize(Action<IInitializationExpression> action) {
			lock (typeof (AdvancedConfigurationProvider)) {
				var expression = new InitializationExpression();
				action(expression);

				InitializationData = expression;
			}

			if (InitializationData.AesKey != null && InitializationData.AesIV == null)
				throw new ArgumentException("If an AES key is provided, an AES IV must also be provided.");

			if (InitializationData.AesKey == null && InitializationData.AesIV != null)
				throw new ArgumentException("If an AES IV is provided, an AES key must also be provided.");
		}


		#region ProtectedConfigurationProvider Overrides

		/// <summary>
		/// This method is called by the .NET configuration classes, once for each xml node in the app.config/web.config 
		/// for each section protected by this provider.
		/// </summary>
		/// <remarks>
		/// If this ConfigurationProvider was set to protect the "connectionStrings" and "appSettings" nodes,
		/// this method would be called once when the first call for a connectionstring happens and one more time
		/// when the first call for an appSetting happens.
		/// </remarks>
		/// <param name="name"></param>
		/// <param name="config"></param>
		public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config) {
			// setup environment awareness
			if (!IsEnvironmentInitialized) {
				if (config.AllKeys.Contains("environmentFileName")) {
					var environmentFileName = config["environmentFileName"];
					EnvironmentFilePath = BuildEnvironmentFilePath(environmentFileName);
					TryReadEnvironmentFile(EnvironmentFilePath);
				}
				else {
					SetEnvironmentFromStaticInitializationData();
				}

				if (!ConfigSectionLocations.ContainsKey("LOCAL") &&
				    AppDomain.CurrentDomain.SetupInformation.ConfigurationFile != string.Empty) {
					var configFileInfo = new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
					ConfigSectionLocations.Add("LOCAL", configFileInfo.Directory.FullName);
				}
			}


			// setup encryption awareness
			if (!IsAesCryptoServiceProviderInitialized) {
				if (config.AllKeys.Contains("keyFileName")) {
					var keyFileName = config["keyFileName"];
					KeyFilePath = BuildKeyFilePath(keyFileName);
					TryReadKey(KeyFilePath);
				}
				else {
					TrySetEncryptionFromStaticInitializationData();
				}
			}


			base.Initialize(name, config);
		}

		/// <summary>
		/// This method has not been overridden and will throw a NotImplementedException at runtime.
		/// </summary>
		/// <remarks>
		/// This method does not make sense in the context of this library.  The Microsoft "aspnet_regiis" tool usually initiates the call of this method 
		/// which would normally encrypt whatever is passed in and then pass the encrypted data back to the .NET configuration library to place that 
		/// encrypted data directly into the web.config / app.config.   Since we are utilizing the area in the web.config / app.config usually used
		/// to hold the encrypted data to instead hold the filepath to a directory(or directories), we don't want it overritten at this point. We'll provide
		/// a custom tool to replace "aspnet_regiis" instead...
		/// </remarks>
		/// <param name="node"></param>
		/// <returns></returns>
		public override XmlNode Encrypt(XmlNode node) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Resolves the correct configsection files to decrypt, merge, and pass back to the .NET runtime for merging into the applications' web.config or app.config.
		/// </summary>
		/// <remarks>
		/// This method has been overridden to accept as input a filepath/filename pattern, to decrypt and merge/transform one or more configsections (ex. connectionstrings or appsettings)
		/// and then return those configsections back to the .NET runtime for merging into the consuming applications' app.config or web.config.
		/// </remarks>
		/// <param name="node"></param>
		/// <returns></returns>
		public override XmlNode Decrypt(XmlNode node) {
			if (!IsEnvironmentInitialized)
				throw new Exception("Environment file could not be loaded.");

			// Grab the custom xmlnode(s) from the app.config / web.config
			// ex.:  <sectionInfo file="[CONNECTIONSTRINGS]\connectionstrings.[ENVIRONMENT].config"/>
			XmlNodeList sectionInfoNodes = node.SelectNodes("/EncryptedData/sectionInfo");

			if (sectionInfoNodes == null)
				throw new ApplicationException("SectionInfoNodes were not found in the application/web config file.");

			// Create an XmlNode to return the combined content of our files
			var doc = new XmlDocument();
			doc.LoadXml("<EncryptedData></EncryptedData>");

			XmlNode masterMergedXmlNode = doc.SelectSingleNode("/*");

			// Merge all the nodes from the target config files into our master XmlNode to return.
			foreach (XmlNode xn in sectionInfoNodes) {
				if (xn.Attributes["file"] != null) {
					/* Support defining appSettings in xml config files via sectionInfo nodes like the following:
					 * (this could be appSettings, connectionStrings, or any other configSection including custom sections.)
					  <appSettings configProtectionProvider="LowFrictionConfigProvider">
						<EncryptedData>
						  <sectionInfo file="[COMMONSETTINGS]\appSettings.[ENVIRONMENT].config"/>
						  <sectionInfo file="[LOCAL]\ConfigSections\AppSettings\appsettings.[ENVIRONMENT].config"/>
						</EncryptedData>
					  </appSettings>
					 */
					var sectionFilePath = xn.Attributes["file"].Value;
					var dxn = GetDecryptedXmlNode(sectionFilePath);

					// Extract the relocated xml nodes out of their wrapper.
					foreach (XmlNode child in dxn.SelectSingleNode("/EncryptedData").ChildNodes) {

						// Merge with the master XML node we will be returning
						var n = doc.ImportNode(child, false);
						masterMergedXmlNode.AppendChild(n);
					}
				}
				else if (xn.Attributes["connectionStringName"] != null && xn.Attributes["databaseTable"] != null && xn.Attributes["keyColumn"] != null && (xn.Attributes["valueColumn"] != null || xn.Attributes["xmlValueColumn"] != null)) {
					/* Support defining appSettings in database tables via sectionInfo nodes */
					var connectionStringNode = ConfigurationManager.ConnectionStrings[xn.Attributes["connectionStringName"].Value];
					if (connectionStringNode == null)
						throw new ApplicationException(string.Format("The connectionstring specified in the sectionInfo node of the application config file named '{0}' was not found.", xn.Attributes["connectionStringName"].Value));

					var connectionString = connectionStringNode.ConnectionString;
					var commandtext = string.Format("SELECT * FROM {0}", xn.Attributes["databaseTable"].Value);

					var throwLoadFailureExceptions = (
						xn.Attributes["throwLoadFailureExceptions"] != null &&
						xn.Attributes["throwLoadFailureExceptions"].Value != null &&
						xn.Attributes["throwLoadFailureExceptions"].Value.ToLower() == "true"
					);

					try {
						if (connectionStringNode.ProviderName == "System.Data.SqlClient") {
							using (var conn = new System.Data.SqlClient.SqlConnection(connectionString)) {
								using (var command = new System.Data.SqlClient.SqlCommand(commandtext, conn)) {
									if (conn.State != System.Data.ConnectionState.Open)
										conn.Open();

									var dr = command.ExecuteReader();

									GetConfigDataFromDbDataReader(doc, dr, masterMergedXmlNode, xn);

									if (conn.State != System.Data.ConnectionState.Closed)
										conn.Close();
								}
							}
						}
						else {
							using (var conn = new System.Data.OleDb.OleDbConnection(connectionString)) {
								using (var command = new System.Data.OleDb.OleDbCommand(commandtext, conn)) {
									if (conn.State != System.Data.ConnectionState.Open)
										conn.Open();

									var dr = command.ExecuteReader();

									GetConfigDataFromDbDataReader(doc, dr, masterMergedXmlNode, xn);

									if (conn.State != System.Data.ConnectionState.Closed)
										conn.Close();
								}
							}
						}
					}
					catch (Exception ex) {
						if (throwLoadFailureExceptions)
							throw new ApplicationException(
								string.Format("Exception encountered during load of appSettings from database with connectionStringName {0}.", xn.Attributes["connectionStringName"].Value),
								ex
							);
					}
				}
			}

			return masterMergedXmlNode;
		}

		private void GetConfigDataFromDbDataReader(XmlDocument doc, DbDataReader dr, XmlNode masterMergedXmlNode, XmlNode xn) {
			/*
			* (to return a xml node in the form of a key/value pair like those used in appSettings, use the "valueColumn" attribute)
			<appSettings configProtectionProvider="LowFrictionConfigProvider">
			<EncryptedData>
				<sectionInfo databaseTable="[Configuration].[ApplicationSetting]" connectionStringName="Rivendell" keyColumn="Name" valueColumn="Value" throwLoadFailureExceptions="false" />
			</EncryptedData>
			</appSettings>
			*/
			bool IsDbTableFilledWithXML = xn.Attributes["xmlValueColumn"] != null;
			var keyColumn = xn.Attributes["keyColumn"].Value;
			
			if (!IsDbTableFilledWithXML) {
				var valueColumn = xn.Attributes["valueColumn"].Value;
				while (dr.Read()) {
					var n = doc.CreateNode(XmlNodeType.Element, "add", string.Empty);
					var xaKey = doc.CreateAttribute("key");
					xaKey.Value = dr[keyColumn].ToString();

					var xaValue = doc.CreateAttribute("value");
					xaValue.Value = dr[valueColumn].ToString();

					n.Attributes.Prepend(xaValue);
					n.Attributes.Prepend(xaKey);

					masterMergedXmlNode.AppendChild(n);
				}
			}
			else {
				/*
				 * (to return a custom xml node already properly formed and stored in the database)
				  <customConfigSection configProtectionProvider="LowFrictionConfigProvider">
					<EncryptedData>
					  <sectionInfo databaseTable="[Configuration].[ApplicationSetting]" connectionStringName="Rivendell" keyColumn="Name" xmlValueColumn="Value" throwLoadFailureExceptions="false" />
					</EncryptedData>
				  </customConfigSection>
				*/
				var xmlValueColumn = xn.Attributes["xmlValueColumn"].Value; 
				while (dr.Read()) {
					var xfrag = doc.CreateDocumentFragment();

					xfrag.InnerXml = dr[xmlValueColumn].ToString();

					masterMergedXmlNode.AppendChild(xfrag);
				}
			}
		}

		#endregion


		#region Encryption Key Specific Methods

		/// <summary>
		/// Creates an AES encryption key at a known location for use by this library in protecting configuration values.
		/// </summary>
		/// <remarks>
		/// The encryption key will be created in the "Environment.SpecialFolder.CommonApplicationData" directory. 
		/// Windows XP / Windows Server 2003: 
		///   "C:\Documents and Settings\All Users\Application Data\ConfigPTFE\Crypto\AES\MachineKeys" 
		/// Windows Vista / Windows 7 / Windows Server 2008: 
		///   "C:\ProgramData\ConfigPTFE\Crypto\AES\MachineKeys"
		/// </remarks>
		/// <param name="keyFileName"></param>
		public void CreateKey(string keyFileName) {
			// use defaults of KeySize of 256.
			Aes.GenerateKey();
			Aes.GenerateIV();
			_isAesCryptoServiceProviderInitialized = true;

			if (!Directory.Exists(KeyDirectory))
				Directory.CreateDirectory(KeyDirectory);

			var keyFilePath = BuildKeyFilePath(keyFileName);

			using (var sw = new StreamWriter(keyFilePath, false)) {
				sw.WriteLine("IsProtectedByDPAPI=false;");
				sw.WriteLine(ByteToHex(Aes.Key));
				sw.WriteLine(ByteToHex(Aes.IV));
				sw.Close();
			}
		}

		/// <summary>
		/// Encrypts the AES key used by this library by using the Windows Data Protection API (DPAPI) with the "LocalMachine" DataProtectionScope.
		/// </summary>
		/// <remarks>
		/// This provides an layer of protection against an attacker gaining access to your encryption key. 
		/// Please ensure that in addition to this step, that the directory the key is stored in has an appropriate set of permissions assigned 
		/// so that access to the key file is provided to only those Windows accounts that need it.
		/// </remarks>
		/// <see cref="Entropy"/>
		/// <param name="FilePath"></param>
		public void ProtectKeyWithDPAPI(string FilePath) {
			UpdateKeyDpapiProtection(FilePath, true);
		}

		/// <summary>
		/// Decrypts the AES key used by this library by using the Windows Data Protection API (DPAPI)
		/// </summary>
		/// <param name="FilePath"></param>
		public void UnProtectKeyWithDPAPI(string FilePath) {
			UpdateKeyDpapiProtection(FilePath, false);
		}

		protected void UpdateKeyDpapiProtection(string KeyFileName, bool Protect) {
			var filePath = BuildKeyFilePath(KeyFileName);
			if (!File.Exists(filePath))
				throw new FileNotFoundException("Key file not found at specified path.");

			var dpapiStatus = string.Empty;
			var key = string.Empty;
			var iv = string.Empty;

			using (var sr = new StreamReader(filePath, false)) {
				dpapiStatus = sr.ReadLine();
				key = sr.ReadLine();
				iv = sr.ReadLine();
				sr.Close();
			}

			var isProtectedByDPAPI = IsDpapiProtectionAttributeTrue(dpapiStatus);

			if (Protect && isProtectedByDPAPI)
				throw new Exception("Key appears to already be protected by DPAPI.");

			if (!Protect && !isProtectedByDPAPI)
				throw new Exception("Key does not appear to be protected by DPAPI.");

			using (var sw = new StreamWriter(filePath, false)) {
				if (Protect) {
					sw.WriteLine("IsProtectedByDPAPI=true;");
					sw.WriteLine(EncryptStringWithDPAPI(key));
					sw.WriteLine(EncryptStringWithDPAPI(iv));
				}
				else {
					sw.WriteLine("IsProtectedByDPAPI=false;");
					sw.WriteLine(DecryptStringWithDPAPI(key));
					sw.WriteLine(DecryptStringWithDPAPI(iv));
				}
				sw.Close();
			}
		}

		protected bool TryReadKey(string FilePath) {
			if (File.Exists(FilePath)) {
				using (var sr = new StreamReader(FilePath)) {
					var firstLine = sr.ReadLine();
					var isProtectedByDPAPI = IsDpapiProtectionAttributeTrue(firstLine);

					if (isProtectedByDPAPI) {
						Aes.Key = HexToByte(DecryptStringWithDPAPI(sr.ReadLine()));
						Aes.IV = HexToByte(DecryptStringWithDPAPI(sr.ReadLine()));
					}
					else {
						Aes.Key = HexToByte(sr.ReadLine());
						Aes.IV = HexToByte(sr.ReadLine());
					}

					_isAesCryptoServiceProviderInitialized = true;
				}
			}

			return _isAesCryptoServiceProviderInitialized;
		}

		#endregion


		#region Environment Specific Methods

		/// <summary>
		/// Loads the configuration data specific to the the machine this library is running on.
		/// </summary>
		/// <remarks>
		/// This method loads data such as "environment name", "environment role", and "configSectionLocations".
		/// </remarks>
		/// <param name="filePath"></param>
		/// <returns>true or false</returns>
		protected bool TryReadEnvironmentFile(string filePath) {
			var success = false;

			if (File.Exists(filePath)) {
				var doc = XDocument.Load(filePath);
				EnvironmentName = doc.Element("configuration")
					.Element("environment").Attribute("name").Value;

				if (doc.Element("configuration").Element("environment").Attribute("role") != null)
					EnvironmentRole = doc.Element("configuration")
						.Element("environment").Attribute("role").Value;

				ConfigSectionLocations = doc.Element("configuration")
					.Element("configSectionLocations")
					.Elements("add")
					.ToDictionary(x => x.Attribute("key").Value, x => x.Attribute("value").Value);

				IsEnvironmentInitialized = true;
			}

			return success;
		}

		public string CreateSampleEnvironmentConfig() {
			if (!Directory.Exists(EnvironmentFileDirectory))
				Directory.CreateDirectory(EnvironmentFileDirectory);

			var sampleFilePath = BuildEnvironmentFilePath("sample.environment.config");

			if (File.Exists(sampleFilePath)) {
				File.Delete(sampleFilePath);
			}

			var windowsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
			var windowsDrive = windowsFolderPath.Substring(0, windowsFolderPath.IndexOf(Path.VolumeSeparatorChar));
			var xml =
@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<configuration>
    <environment name=""DEV"" role=""WEBSERVER"" />
    <configSectionLocations>
        <add key=""COMMONSETTINGS"" value=""{0}:\Configuration\CommonSettings\Samples"" />
        <add key=""CONNECTIONSTRINGS"" value=""{0}:\Configuration\Connectionstrings\Samples"" />
    </configSectionLocations>
</configuration>";

			using (var sw = new StreamWriter(sampleFilePath, false)) {
				sw.Write(string.Format(xml, windowsDrive));
				sw.Close();
			}
			return sampleFilePath;
		}

		public string CreateSampleAppSettingsConfigSectionFileSet() {
			var windowsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
			var windowsDrive = windowsFolderPath.Substring(0, windowsFolderPath.IndexOf(Path.VolumeSeparatorChar));

			var directoryPath = string.Format(@"{0}:\Configuration\CommonSettings\Samples\", windowsDrive);
			if (!Directory.Exists(directoryPath))
				Directory.CreateDirectory(directoryPath);


			// Create the base file
			var sampleFilePathBase = string.Format("{0}{1}",directoryPath, "appSettings.config");
			if (File.Exists(sampleFilePathBase)) {
				File.Delete(sampleFilePathBase);
			}

			using (var sw = new StreamWriter(sampleFilePathBase, false)) {
				sw.Write(
@"<?xml version=""1.0"" encoding=""utf-8""?>
<EncryptedData IsEncrypted=""false"">
	<add key=""BillingServiceAddress"" value=""http://localhost/DefaultValue.svc"" />

	<!-- EnrollmentServiceAddress is defaulted to a valid value, which may be overridden by values in an environment specific diff file -->
	<add key=""EnrollmentServiceAddress"" value=""http://localhost/DefaultValue.svc"" />
</EncryptedData>");

				sw.Close();
			}


			// Create the diff file for DEV
			var sampleFilePathDev = string.Format("{0}{1}", directoryPath, "appSettings.DEV.config");
			if (File.Exists(sampleFilePathDev)) {
				File.Delete(sampleFilePathDev);
			}

			using (var sw = new StreamWriter(sampleFilePathDev, false)) {
				sw.Write(
@"<?xml version=""1.0"" encoding=""utf-8""?>
<EncryptedData IsEncrypted=""false"" xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform"">
	<add key=""EnrollmentServiceAddress"" value=""http://localhost/Enrollment.svc"" xdt:Transform=""SetAttributes"" xdt:Locator=""Match(key)"" />
</EncryptedData>");

				sw.Close();
			}


			// Create the diff file for INTEGRATION
			var sampleFilePathInt = string.Format("{0}{1}", directoryPath, "appSettings.INTEGRATION.config");
			if (File.Exists(sampleFilePathInt)) {
				File.Delete(sampleFilePathInt);
			}

			using (var sw = new StreamWriter(sampleFilePathInt, false)) {
				sw.Write(
@"<?xml version=""1.0"" encoding=""utf-8""?>
<EncryptedData IsEncrypted=""false"" xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform"">
	<add key=""EnrollmentServiceAddress"" value=""http://integration/Enrollment.svc"" xdt:Transform=""SetAttributes"" xdt:Locator=""Match(key)"" />
</EncryptedData>");

				sw.Close();
			}


			// Create the diff file for REGRESSION
			var sampleFilePathReg = string.Format("{0}{1}", directoryPath, "appSettings.REGRESSION.config");
			if (File.Exists(sampleFilePathReg)) {
				File.Delete(sampleFilePathReg);
			}

			using (var sw = new StreamWriter(sampleFilePathReg, false)) {
				sw.Write(
@"<?xml version=""1.0"" encoding=""utf-8""?>
<EncryptedData IsEncrypted=""false"" xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform"">
	<add key=""EnrollmentServiceAddress"" value=""http://regression/Enrollment.svc"" xdt:Transform=""SetAttributes"" xdt:Locator=""Match(key)"" />
</EncryptedData>");

				sw.Close();
			}


			// Create the diff file for PRODUCTION
			var sampleFilePathProd = string.Format("{0}{1}", directoryPath, "appSettings.PRODUCTION.config");
			if (File.Exists(sampleFilePathProd)) {
				File.Delete(sampleFilePathProd);
			}

			using (var sw = new StreamWriter(sampleFilePathProd, false)) {
				sw.Write(
@"<?xml version=""1.0"" encoding=""utf-8""?>
<EncryptedData IsEncrypted=""false"" xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform"">
	<add key=""EnrollmentServiceAddress"" value=""http://production/Enrollment.svc"" xdt:Transform=""SetAttributes"" xdt:Locator=""Match(key)"" />
</EncryptedData>");

				sw.Close();
			}

			return directoryPath;
		}

		public string CreateSampleConnectionStringsConfigSectionFileSet() {
			var windowsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
			var windowsDrive = windowsFolderPath.Substring(0, windowsFolderPath.IndexOf(Path.VolumeSeparatorChar));

			var directoryPath = string.Format(@"{0}:\Configuration\Connectionstrings\Samples\", windowsDrive);
			if (!Directory.Exists(directoryPath))
				Directory.CreateDirectory(directoryPath);


			// Create the base file
			var sampleFilePathBase = string.Format("{0}{1}", directoryPath, "connectionstrings.config");
			if (File.Exists(sampleFilePathBase)) {
				File.Delete(sampleFilePathBase);
			}

			using (var sw = new StreamWriter(sampleFilePathBase, false)) {
				sw.Write(
@"<?xml version=""1.0"" encoding=""utf-8""?>
<EncryptedData IsEncrypted=""false"">
	<!-- ""Rivendell"" is defaulted to invalid values, so we may recognize at runtime that an appropriate environment specific diff file was not merged. -->
	<add name=""Rivendell"" connectionString=""..."" providerName=""..."" />

	<!-- This connectionstring is not defaulted to an invalid value because we are not overriding it in the diff files. -->
	<add name=""AdventureWorks2008"" connectionString=""Server=.; Database=AdventureWorks2008; Trusted_Connection=True; Connection LifeTime=300;"" providerName=""System.Data.SqlClient"" />
</EncryptedData>");

				sw.Close();
			}


			// Create the diff file for DEV
			var sampleFilePathDev = string.Format("{0}{1}",directoryPath, "connectionstrings.DEV.config");
			if (File.Exists(sampleFilePathDev)) {
				File.Delete(sampleFilePathDev);
			}

			using (var sw = new StreamWriter(sampleFilePathDev, false)) {
				sw.Write(
@"<?xml version=""1.0"" encoding=""utf-8""?>
<EncryptedData IsEncrypted=""false"" xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform"">
	<add name=""Rivendell"" connectionString=""Server=.; Database=RivendellDev; Trusted_Connection=True; Connection LifeTime=300;"" providerName=""System.Data.SqlClient"" xdt:Transform=""SetAttributes"" xdt:Locator=""Match(name)"" />
</EncryptedData>");

				sw.Close();
			}


			// Create the diff file for INTEGRATION
			var sampleFilePathInt = string.Format("{0}{1}",directoryPath, "connectionstrings.INTEGRATION.config");
			if (File.Exists(sampleFilePathInt)) {
				File.Delete(sampleFilePathInt);
			}

			using (var sw = new StreamWriter(sampleFilePathInt, false)) {
				sw.Write(
@"<?xml version=""1.0"" encoding=""utf-8""?>
<EncryptedData IsEncrypted=""false"" xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform"">
	<add name=""Rivendell"" connectionString=""Server=.; Database=RivendellIntegration; Trusted_Connection=True; Connection LifeTime=300;"" providerName=""System.Data.SqlClient"" xdt:Transform=""SetAttributes"" xdt:Locator=""Match(name)"" />
</EncryptedData>");

				sw.Close();
			}


			// Create the diff file for REGRESSION
			var sampleFilePathReg = string.Format("{0}{1}", directoryPath, "connectionstrings.REGRESSION.config");
			if (File.Exists(sampleFilePathReg)) {
				File.Delete(sampleFilePathReg);
			}

			using (var sw = new StreamWriter(sampleFilePathReg, false)) {
				sw.Write(
@"<?xml version=""1.0"" encoding=""utf-8""?>
<EncryptedData IsEncrypted=""false"" xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform"">
	<add name=""Rivendell"" connectionString=""Server=.; Database=RivendellRegression; Trusted_Connection=True; Connection LifeTime=300;"" providerName=""System.Data.SqlClient"" xdt:Transform=""SetAttributes"" xdt:Locator=""Match(name)"" />
</EncryptedData>");

				sw.Close();
			}


			// Create the diff file for PRODUCTION
			var sampleFilePathProd = string.Format("{0}{1}", directoryPath, "connectionstrings.PRODUCTION.config");
			if (File.Exists(sampleFilePathProd)) {
				File.Delete(sampleFilePathProd);
			}

			using (var sw = new StreamWriter(sampleFilePathProd, false)) {
				sw.Write(
@"<?xml version=""1.0"" encoding=""utf-8""?>
<EncryptedData IsEncrypted=""false"" xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform"">
	<add name=""Rivendell"" connectionString=""Server=.; Database=RivendellProduction; Trusted_Connection=True; Connection LifeTime=300;"" providerName=""System.Data.SqlClient"" xdt:Transform=""SetAttributes"" xdt:Locator=""Match(name)"" />
</EncryptedData>");

				sw.Close();
			}

			return directoryPath;
		}

		/// <summary>
		/// Transforms a target configsection file by applying the "Locator" and "Transform" attributes in the supplied source xml element
		/// </summary>
		/// <remarks>
		/// For example, if this method were supplied an XDocument created from the configsection file "connectionstrings.config" and 
		/// an xml element with a name attribute of "Titan", a Transform attribute of "SetAttributes", and a Locator attribute of "Match(name)"
		/// the method would search the XDocument for a match based on the "name" attribute and then set all its' attributes to match those 
		/// of the provided xml element.   The available Transform and Locator values conform to those provided by the Microsoft XML Document Transform
		/// (http://schemas.microsoft.com/XML-Document-Transform).
		/// </remarks>
		/// <param name="targetDoc"></param>
		/// <param name="sourceElement"></param>
		private void Transform(ref XDocument targetDoc, XElement sourceElement) {
			var xpath = sourceElement.GetXPath();
			var locatorAction = sourceElement.Attributes()
				.Where(x => x.Name.LocalName == "Locator")
				.Select(x => x.Value)
				.SingleOrDefault();

			var transformAction = sourceElement.Attributes()
				.Where(x => x.Name.LocalName == "Transform")
				.Select(x => x.Value)
				.SingleOrDefault();

			if (transformAction == null)
				throw new Exception(
					"If you want two config files merged, you need to state which XML nodes should be merged and how they should be merged.  Environment specific config file must have a \"Transform\" attribute on at least one XML node for a config merge operation to occur.");

			var transformActionXPath = GetXPathFromTransformAction(transformAction);
			var transformAttributeList = GetAttributeListFromTransformAction(transformAction);

			if (locatorAction != null) {
				if (locatorAction.StartsWith("Match("))
					xpath = AddMatchFilter(xpath, sourceElement, locatorAction);
				else if (locatorAction.StartsWith("Condition("))
					xpath = AddConditionFilter(xpath, locatorAction);
				else if (locatorAction.StartsWith("XPath("))
					xpath = AddXPathFilter(locatorAction);
			}

			var targetNodes = targetDoc.XPathSelectElements(xpath).ToList();
			var targetSiblings = new List<XElement>();

			var sanitizedSourceElement = new XElement(sourceElement);
			sanitizedSourceElement.Attributes()
				.Where(x => x.Name.NamespaceName == "http://schemas.microsoft.com/XML-Document-Transform")
				.Remove();

			switch (transformAction) {
				case "Replace":
					if (targetNodes.Count > 0) {
						targetNodes[0] = sourceElement;
					}
					break;

				case "Remove":
					if (targetNodes.Count > 0) {
						targetNodes[0].Remove();
					}
					break;

				case "RemoveAll":
					targetNodes[0].Parent.RemoveNodes();
					break;

				case "Insert":
					var parentxpath = xpath.Remove(xpath.LastIndexOf('/'));
					var targetParentNodes = targetDoc.XPathSelectElements(parentxpath).ToList();
					foreach (XElement parentNode in targetParentNodes) {
						parentNode.Add(sanitizedSourceElement);
					}
					break;

				case "SetAttributes":
					if (transformAttributeList.Count == 0) {
						// find the attributes with the "xdt" namespace (i.e. "Locator" and "Transform")
						var sourceAttributes = sourceElement.Attributes()
							.Where(x => x.Name.NamespaceName != "http://schemas.microsoft.com/XML-Document-Transform")
							.Select(x => x)
							.ToList();

						foreach (XElement targetNode in targetNodes) {
							foreach (var att in sourceAttributes) {
								if (att.Name.LocalName != "xdt") {
									if (targetNode.Attribute(att.Name.LocalName) == null)
										targetNode.Add(new XAttribute(att.Name.LocalName, att.Value));
									else
										targetNode.Attribute(att.Name.LocalName).Value = att.Value;
								}
							}
						}
					}
					else {
						foreach (XElement targetNode in targetNodes) {
							foreach (var name in transformAttributeList) {
								if (name != "xdt") {
									if (targetNode.Attribute(name) == null)
										targetNode.Add(new XAttribute(sourceElement.Attribute(name).Name, sourceElement.Attribute(name).Value));
									else
										targetNode.Attribute(name).Value = sourceElement.Attribute(name).Value;
								}
							}
						}
					}
					break;

				case "RemoveAttributes":
					if (transformAttributeList.Count == 0) {
						foreach (XElement node in targetNodes) {
							node.RemoveAttributes();
						}
					}
					else {
						foreach (XElement node in targetNodes) {
							foreach (var name in transformAttributeList) {
								node.Attribute(name).Remove();
							}
						}
					}
					break;

				case "InsertAfter":
					targetSiblings = targetDoc.XPathSelectElements(transformActionXPath).ToList();
					foreach (var sibling in targetSiblings) {
						sibling.AddAfterSelf(sanitizedSourceElement);
					}
					break;

				case "InsertBefore":
					targetSiblings = targetDoc.XPathSelectElements(transformActionXPath).ToList();
					foreach (var sibling in targetSiblings) {
						sibling.AddBeforeSelf(sanitizedSourceElement);
					}
					break;

				default:
					break;
			}
		}

		#endregion


		#region Configuration Section Specific Methods

		public void EncryptRelocatedConfigSectionWithAES(string FilePath) {
			if (!File.Exists(FilePath))
				throw new FileNotFoundException("Configuration section not found at specified path");

			var source = new XmlDocument();
			source.Load(FilePath);
			var cd = source.SelectSingleNode("/EncryptedData");

			var tryToEncrypt = false;

			if (cd.Attributes["IsEncrypted"] != null) {
				if (cd.Attributes["IsEncrypted"].Value.ToLower() == "false")
					tryToEncrypt = true;
				else
					throw new Exception(
						"Configuration section appears to already be encrypted. 'IsEncrypted' attribute in configuration section was not set to the expected value (false.)");
			}
			else {
				var xa = source.CreateAttribute("IsEncrypted");
				xa.Value = "false";
				cd.Attributes.Append(xa);

				tryToEncrypt = true;
			}

			if (tryToEncrypt) {
				try {
					cd.InnerText = EncryptStringWithAES(cd.InnerXml);
					cd.Attributes["IsEncrypted"].Value = "true";
					source.Save(FilePath);
				}
				catch (Exception ex) {
					throw new Exception(
						string.Format("Error encountered while attempting to encrypt the values at {0}.  {1}.", FilePath, ex.Message), ex);
				}
			}
		}

		public void DecryptRelocatedConfigSectionWithAES(string FilePath) {
			if (!File.Exists(FilePath))
				throw new FileNotFoundException("Configuration section not found at specified path");

			var source = new XmlDocument();
			source.Load(FilePath);
			var cd = source.SelectSingleNode("/EncryptedData");

			var tryToDecrypt = false;

			if (cd.Attributes["IsEncrypted"] != null) {
				if (cd.Attributes["IsEncrypted"].Value.ToLower() == "true")
					tryToDecrypt = true;
				else
					throw new Exception(
						"Configuration section appears to already be decrypted. 'IsEncrypted' attribute in configuration section was not set to the expected value (true.)");
			}
			else {
				throw new Exception("'IsEncrypted' attribute in configuration section was not found.");
			}

			if (tryToDecrypt) {
				try {
					cd.InnerXml = DecryptStringWithAES(cd.InnerText);
					cd.Attributes["IsEncrypted"].Value = "false";
					source.Save(FilePath);
				}
				catch (Exception ex) {
					throw new Exception(
						string.Format("Error encountered while attempting to encrypt the values at {0}.  {1}.", FilePath, ex.Message), ex);
				}
			}
		}

		#endregion


		#region Encryption Helper Methods

		protected string ByteToHex(byte[] byteArray) {
			return byteArray
				.Aggregate(
					string.Empty,
					(current, b) => current + b.ToString("X2")
				);
		}

		protected byte[] HexToByte(string hexString) {
			var result = new byte[hexString.Length/2];

			for (var i = 0; i < result.Length; i++) {
				result[i] = Convert.ToByte(hexString.Substring(i*2, 2), 16);
			}

			return result;
		}

		protected string EncryptStringWithAES(string plainText) {
			if (!IsAesCryptoServiceProviderInitialized)
				throw new Exception(AesCryptoServiceProviderNotInitialized);

			byte[] valBytes = Encoding.Unicode.GetBytes(plainText);

			ICryptoTransform transform = Aes.CreateEncryptor();

			var ms = new MemoryStream();
			var cs = new CryptoStream(ms, transform, CryptoStreamMode.Write);
			cs.Write(valBytes, 0, valBytes.Length);
			cs.FlushFinalBlock();
			byte[] returnBytes = ms.ToArray();
			cs.Close();

			return Convert.ToBase64String(returnBytes);
		}

		protected string DecryptStringWithAES(string encryptedText) {
			if (!IsAesCryptoServiceProviderInitialized)
				throw new Exception(AesCryptoServiceProviderNotInitialized);

			byte[] valBytes = Convert.FromBase64String(encryptedText);

			ICryptoTransform transform = Aes.CreateDecryptor();

			MemoryStream ms = new MemoryStream();
			CryptoStream cs = new CryptoStream(ms, transform, CryptoStreamMode.Write);
			cs.Write(valBytes, 0, valBytes.Length);
			cs.FlushFinalBlock();
			byte[] returnBytes = ms.ToArray();
			cs.Close();

			return Encoding.Unicode.GetString(returnBytes);
		}

		protected string EncryptStringWithDPAPI(string plainText) {
			byte[] valBytes = Convert.FromBase64String(plainText);
			byte[] entropy = Encoding.Unicode.GetBytes(_entropy);
			byte[] returnBytes = ProtectedData.Protect(valBytes, entropy, DataProtectionScope.LocalMachine);
			return Convert.ToBase64String(returnBytes);
		}

		protected string DecryptStringWithDPAPI(string encryptedText) {
			byte[] valBytes = Convert.FromBase64String(encryptedText);
			byte[] entropy = Encoding.Unicode.GetBytes(_entropy);
			byte[] returnBytes = ProtectedData.Unprotect(valBytes, entropy, DataProtectionScope.LocalMachine);
			return Convert.ToBase64String(returnBytes);
		}

		protected bool IsKeyProtectedByDPAPI(string keyFileName) {
			var isProtectedByDPAPI = false;
			var filePath = BuildKeyFilePath(keyFileName);

			if (!File.Exists(filePath))
				throw new FileNotFoundException("Key file not found at specified path.");

			using (var sr = new StreamReader(filePath)) {
				var firstLine = sr.ReadLine();
				isProtectedByDPAPI = IsDpapiProtectionAttributeTrue(firstLine);
			}
			return isProtectedByDPAPI;
		}

		protected bool IsDpapiProtectionAttributeTrue(string dpapiProtectionAttribute) {
			return (dpapiProtectionAttribute.Trim().ToLower().Replace(" ", "") == "isprotectedbydpapi=true;");
		}

		public string BuildKeyFilePath(string keyFileName) {
			var keyFilePath = string.Format("{0}{1}", KeyDirectory, keyFileName);
			return keyFilePath;
		}

		private void SetEnvironmentFromStaticInitializationData() {
			IsEnvironmentInitialized = true;
			EnvironmentName = InitializationData.EnvironmentName ?? string.Empty;
			EnvironmentRole = InitializationData.EnvironmentRole ?? string.Empty;
			ConfigSectionLocations = InitializationData.ConfigSectionLocations ?? new Dictionary<string, string>();
		}

		private void TrySetEncryptionFromStaticInitializationData() {
			if (InitializationData == null) return;
			if (InitializationData.AesKey == null || InitializationData.AesIV == null) return;

			Aes.Key = InitializationData.AesKey;
			Aes.IV = InitializationData.AesIV;
			_isAesCryptoServiceProviderInitialized = true;
		}

		#endregion


		#region Environment Helper Methods

		public string BuildEnvironmentFilePath(string environmentFileName) {
			var environmentFilePath = string.Format("{0}{1}", EnvironmentFileDirectory, environmentFileName);
			return environmentFilePath;
		}

		protected XmlNode GetDecryptedXmlNode(string sectionFilePath) {
			// Insert the environment name into the filepaths of the relocated config files retrieved above.
			var commonSectionFilePath = sectionFilePath
				.ToLower()
				.Replace(".[environment]", string.Empty)
				.Replace(".[role]", string.Empty);

			var environmentSpecificSectionFilePath = sectionFilePath
				.ToLower()
				.Replace("[environment]", EnvironmentName)
				.Replace(".[role]", string.Empty);

			var roleSpecificSectionFilePath = string.Empty;
			if (sectionFilePath.ToLower().Contains("[role]")) {
				roleSpecificSectionFilePath = sectionFilePath
					.ToLower()
					.Replace("[environment]", EnvironmentName)
					.Replace("[role]", EnvironmentRole);
			}


			// If the environment file has locations specified for configsections, try to insert the one specified in the sectionInfo filepath
			foreach (KeyValuePair<string, string> item in ConfigSectionLocations) {
				var patternToReplace = string.Format("[{0}]", item.Key.ToLower());
				commonSectionFilePath = commonSectionFilePath.ToLower().Replace(patternToReplace, item.Value);
				environmentSpecificSectionFilePath = environmentSpecificSectionFilePath.ToLower().Replace(patternToReplace,
				                                                                                          item.Value);

				if (roleSpecificSectionFilePath != string.Empty)
					roleSpecificSectionFilePath = roleSpecificSectionFilePath.ToLower().Replace(patternToReplace, item.Value);
			}

			// Make sure we haven't missed any things to replace in the sectionInfo filepaths
			if (commonSectionFilePath.Contains("[") || environmentSpecificSectionFilePath.Contains("["))
				throw new ArgumentException(string.Format(ConfigSectionLocationEntryMissing, commonSectionFilePath,
				                                          environmentSpecificSectionFilePath));

			// Verify the files exist.
			if (!File.Exists(commonSectionFilePath))
				throw new FileNotFoundException(string.Format(ConfigFileNotFound, sectionFilePath, commonSectionFilePath));

			var environmentSpecificFileExists = File.Exists(environmentSpecificSectionFilePath);

			var roleSpecificFileExists = false;
			if (roleSpecificSectionFilePath != string.Empty)
				roleSpecificFileExists = File.Exists(roleSpecificSectionFilePath);

			// Grab the template (ex. connectionstrings.config or appsettings.config)
			var commonSection = XDocument.Load(commonSectionFilePath);
			var innerCommon = commonSection.XPathSelectElement("/EncryptedData");

			// Grab the environment specific file (ex. connectionstrings.DEV05.config or appsettings.DEV05.config)
			XDocument environmentSpecificSection = new XDocument();
			XElement innerEnvironment = new XElement("tempElementToSatisfyCompiler_thisElementWillBeOverwritten");
			if (environmentSpecificFileExists) {
				environmentSpecificSection = XDocument.Load(environmentSpecificSectionFilePath);
				innerEnvironment = environmentSpecificSection.XPathSelectElement("/EncryptedData");
			}

			// Grab the role specific file (ex. appsettings.DEV05.WEBSERVER.config or appsettings.DEV05.SERVICES.config or appsettings.DEV05.REPORTINGSERVER.config)
			XDocument roleSpecificSection = new XDocument();
			XElement innerRole = new XElement("tempElementToSatisfyCompiler_thisElementWillBeOverwritten");
			if (roleSpecificFileExists) {
				roleSpecificSection = XDocument.Load(roleSpecificSectionFilePath);
				innerRole = roleSpecificSection.XPathSelectElement("/EncryptedData");
			}


			// DeCrypt common file...
			XDocument unencryptedCommon = null;
			if (innerCommon.Attribute("IsEncrypted") != null && innerCommon.Attribute("IsEncrypted").Value.ToLower() == "true")
				unencryptedCommon =
					XDocument.Parse(string.Format(@"<EncryptedData>{0}</EncryptedData>", DecryptStringWithAES(innerCommon.Value)));
			else
				unencryptedCommon = XDocument.Parse(string.Format(@"<EncryptedData>{0}</EncryptedData>", innerCommon.InnerXml()));


			// Merge the common and env. specific files
			if (environmentSpecificFileExists) {
				// DeCrypt environment specific file...
				XDocument unencryptedEnvSpecific = null;
				var parentNodeTemplate =
					"<EncryptedData xmlns:xdt=\"http://schemas.microsoft.com/XML-Document-Transform\">{0}</EncryptedData>";

				if (innerEnvironment.Attribute("IsEncrypted") != null &&
				    innerEnvironment.Attribute("IsEncrypted").Value.ToLower() == "true")
					unencryptedEnvSpecific =
						XDocument.Parse(string.Format(parentNodeTemplate, DecryptStringWithAES(innerEnvironment.Value)));
				else
					unencryptedEnvSpecific = environmentSpecificSection;
						// XDocument.Parse(string.Format(parentNodeTemplate, innerEnvironment.InnerXml()));

				// Get the elements in the environment specific file which have attributes marking them as elements which need to be combined with the master config file.
				var elementsToMerge = unencryptedEnvSpecific.Root.Descendants()
					.Where(x => x.Attributes().Any(y => y.Name.LocalName == "Transform"))
					.Select(x => x)
					.ToArray();
				// Merge each element into the common file.
				foreach (var e in elementsToMerge) {
					Transform(ref unencryptedCommon, e);
				}
			}

			// Merge the common and role specific files
			if (roleSpecificFileExists) {
				// DeCrypt role specific file...
				XDocument unencryptedRoleSpecific = null;
				var parentNodeTemplate =
					"<EncryptedData xmlns:xdt=\"http://schemas.microsoft.com/XML-Document-Transform\">{0}</EncryptedData>";

				if (innerRole.Attribute("IsEncrypted") != null && innerRole.Attribute("IsEncrypted").Value.ToLower() == "true")
					unencryptedRoleSpecific = XDocument.Parse(string.Format(parentNodeTemplate, DecryptStringWithAES(innerRole.Value)));
				else
					unencryptedRoleSpecific = roleSpecificSection;
						// XDocument.Parse(string.Format(parentNodeTemplate, innerRole.InnerXml()));

				// Get the elements in the environment specific file which have attributes marking them as elements which need to be combined with the master config file.
				var elementsToMerge = unencryptedRoleSpecific.Root.Descendants()
					.Where(x => x.Attributes().Any(y => y.Name.LocalName == "Transform"))
					.Select(x => x)
					.ToArray();
				// Merge each element into the common file.
				foreach (var e in elementsToMerge) {
					Transform(ref unencryptedCommon, e);
				}
			}


			return unencryptedCommon.Root.GetXmlNode();
		}

		/// <summary>
		/// Supplements the provided xpath with additional "match" assertions stated in the provided locatorAction.
		/// </summary>
		/// <param name="xpath"></param>
		/// <param name="sourceElement"></param>
		/// <param name="locatorAction"></param>
		/// <returns></returns>
		private string AddMatchFilter(string xpath, XElement sourceElement, string locatorAction) {
			var closeParensLocation = locatorAction.Length - 1;
			var la = locatorAction.Remove(closeParensLocation).Replace("Match(", string.Empty);
			var attributenames = la.Split(',');
			var count = attributenames.Count();

			xpath += "[";
			if (count == 1) {
				var an = attributenames[0];
				xpath += string.Format("@{0}='{1}'", an, sourceElement.Attribute(an).Value);
			}
			else {
				var i = count;
				foreach (var an in attributenames) {
					xpath += string.Format("@{0}='{1}'{2}", an, sourceElement.Attribute(an).Value, --i > 0 ? " and " : string.Empty);
				}
			}
			xpath += "]";

			return xpath;
		}

		/// <summary>
		/// Supplements the provided xpath with additional "filter" assertions stated in the provided locatorAction.
		/// </summary>
		/// <param name="xpath"></param>
		/// <param name="locatorAction"></param>
		/// <returns></returns>
		private string AddConditionFilter(string xpath, string locatorAction) {
			var closeParensLocation = locatorAction.Length - 1;
			var condition = locatorAction.Remove(closeParensLocation).Replace("Condition(", string.Empty);
			return string.Format("{0}[{1}]", xpath, condition);
		}

		/// <summary>
		/// Provides an xpath value from the supplied xpath value nested in the locatorAction.
		/// </summary>
		/// <param name="locatorAction"></param>
		/// <returns></returns>
		private string AddXPathFilter(string locatorAction) {
			var closeParensLocation = locatorAction.Length - 1;
			var condition = locatorAction.Remove(closeParensLocation).Replace("XPath(", string.Empty);
			return condition;
		}

		private List<string> GetAttributeListFromTransformAction(string transformAction) {
			var attributes = new List<string>();

			if (transformAction.Contains("SetAttributes(") || transformAction.Contains("RemoveAttributes(")) {
				var closeParensLocation = transformAction.Length - 1;
				var arr = transformAction.Remove(closeParensLocation)
					.Replace("SetAttributes(", string.Empty)
					.Replace("RemoveAttributes(", string.Empty);
				attributes = arr.Split(',').ToList();
			}

			return attributes;
		}

		private string GetXPathFromTransformAction(string transformAction) {
			string xpath = null;

			if (transformAction.Contains("InsertAfter") || transformAction.Contains("InsertBefore")) {
				var closeParensLocation = transformAction.Length - 1;
				xpath = transformAction.Remove(closeParensLocation)
					.Replace("InsertAfter(", string.Empty)
					.Replace("InsertBefore(", string.Empty);
			}

			return xpath;
		}

		#endregion
	}
}