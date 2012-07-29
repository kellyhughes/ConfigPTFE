namespace ConfigPTFE.SystemTrayApp {
	public interface ILocalApplicationDataRepository {
		void Save(LocalApplicationData settings);
		LocalApplicationData Get();
		void CreateNewSettingsFile();
	}
}