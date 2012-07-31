using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ConfigPTFE.SampleMvcApp {
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication {
		public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
			filters.Add(new HandleErrorAttribute());
		}

		public static void RegisterRoutes(RouteCollection routes) {
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new {controller = "Home", action = "Index", id = UrlParameter.Optional} // Parameter defaults
				);
		}

		protected void Application_Start() {
			AreaRegistration.RegisterAllAreas();

			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);

			BootstrapConfigPTFE();
		}

		/// <summary>
		/// Example usage of programatically setting the ConfigPTFE environment and encryption values
		/// </summary>
		/// <remarks>
		/// The contents of this method would be called in the ApplicationStart method of the Global.asax in a web application.
		/// Alternatively, the same configuration may be provided via config files on the filesystem (i.e. the environment.config and AES key files)
		/// </remarks>
		/// <param name="environmentnameChoice"></param>
		private static void BootstrapConfigPTFE() {
			ConfigPTFE.AdvancedConfigurationProvider.Initialize(
				x => {
					x.EnvironmentName = "DEV";
					x.EnvironmentRole = string.Empty;
					x.ConfigSectionLocations.Add("COMMONSETTINGS", @"C:\Configuration\CommonSettings\Samples");
					x.ConfigSectionLocations.Add("CONNECTIONSTRINGS", @"C:\Configuration\Connectionstrings\Samples");
					x.AesKey = GetAesKey();
					x.AesIV = GetAesIV();
				}
				);
		}

		#region Helpers

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
			var result = new byte[hexString.Length / 2];

			for (var i = 0; i < result.Length; i++) {
				result[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
			}

			return result;
		}

		#endregion
	}
}