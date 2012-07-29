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