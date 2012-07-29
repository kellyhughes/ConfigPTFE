namespace ConfigPTFE.SystemTrayApp {
	public interface IEnvironmentConfigRepository {
		bool Exists(string fileName);
		EnvironmentConfigSettings Get(string fileName);
		void Save(string fileName, EnvironmentConfigSettings settings);
	}
}