You can read a more extensive overview here:
http://kellyhughes.github.com/ConfigPTFE/

This library helps software development teams accomplish two goals:
1.) share common configuration data between multiple applications.
2.) switch configuration data used by applications via a global "environment name".

Some Benefits:
--------
1.) Uses technology you already know. Access settings with System.ConfigurationManager.
2.) Config sections may be easily encrypted with AES encryption.
3.) Settings may be stored in xml config files or in database tables
4.) Less config values duplication.  Environment specific values are merged into a base config file at runtime, so values shared accross envionments don't have to be duplicated accross multiple files.
5.) Easy application deployments. Environment specific config values are removed from the web.config/app.config and the environment appropriate values are merged into the web.config/app.config at runtime.
6.) Very customizable. Since the environment name value is not held in source control, a developer can easily create custom connectionstring or applicationsetting config files or databases in thier development or test environment to feed a system under test any values they need.


Several samples of ConfigPTFE usage are included in the solution. 

Some examples may require you to setup databases via the provided sql scripts.   

Some examples may require you to install configuration files at a common location on your filesystem via the Administration utility provided as a project in this solution.



Setting up your environment & project:
-------------------------------------
For a simple setup where your main goal is to leverage the "environment aware" configs, you'll need to do this:
1.)	Use the command line Admin tool (one of the projects in the solution) to create a sample "environment config" file at the expected location on your development machine.
2.)	Amend this sample environment config with the appropriate "environment name" (ex. "DEV" for your local workstation)
3.)	Place a "configProtectedData" section to your application's web.config or app.config which references this filename in the "environmentFileName" attribute of the "add" node.
4.)	Place an "appSettings" and/or "connectionStrings" node in your web/app.config in this form:
<appSettings configProtectionProvider="LowFrictionConfigProvider">
		<EncryptedData>
			<sectionInfo file="[LOCAL]\ConfigSections\AppSettings\appSettings.[ENVIRONMENT].config" />
		</EncryptedData>
	</appSettings>

	<connectionStrings configProtectionProvider="LowFrictionConfigProvider">
		<EncryptedData>
			<sectionInfo file="[LOCAL]\ConfigSections\Connectionstrings\connectionStrings.[ENVIRONMENT].config" />
		</EncryptedData>
	</connectionStrings>
5.)	Create the folder(s) "ConfigSections\Connectionstrings" &/or "ConfigSections\AppSettings"in your application
6.)	Create a base config file in each of those folders to hold all your settings which will be used in all environments (DEV, TEST, INTEGRATION, REGRESSION, PRODUCTION, etc.)
7.)	Create several environment specific configs side by side with that base config, each filled with the specific values you intend to override in the base config for that environment (don't forget that you'll need to use the appropriate Config Transform commands on each node.)
8.)	Run your application and see the appropriate values get pulled.

If you want to also share config values across multiple applications via a single file (ex. Connectionstrings):
1.)	Add/Amend the appropriate "configSectionLocations" entries in the environment.config file (ex. Set the correct file location(s) for the "COMMONSETTINGS" or "CONNECTIONSTRINGS" tokens.
2.)	Add the appropriate "sectionInfo" nodes in your applications' web.config/app.config appsettings or connectionstrings nodes (cf. step 4 above.)
