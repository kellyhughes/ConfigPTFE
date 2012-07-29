using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;

namespace ConfigPTFE.SystemTrayApp {
	public static class ContainerBootstrapper {
		public static void BootstrapStructureMap() {
			// Initialize the static ObjectFactory container
			ObjectFactory.Initialize(x => {
			                         	x.For<IEnvironmentConfigRepository>().Use<EnvironmentConfigRepository>();
			                         	x.For<ILocalApplicationDataRepository>().Use<LocalApplicationDataRepository>();
			                         });
		}
	}
}