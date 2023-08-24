using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons {
	public class VersionJson {
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("realm")]
		public string Realm { get; set; }

		[JsonProperty("version")]
		public string Version { get; set; }

		[JsonProperty("gitTag")]
		public string GitTag { get; set; }

		[JsonProperty("gitHash")]
		public string GitHash { get; set; }

		[JsonProperty("gitBranchName")]
		public string GitBranchName { get; set; }

		[JsonProperty("buildDate")]
		public string BuildDate { get; set; }

		[JsonProperty("buildID")]
		public string BuildID { get; set; }

		[JsonProperty("buildBy")]
		public string BuildBy { get; set; }

		[JsonProperty("buildOS")]
		public string BuildOS { get; set; }

		[JsonProperty("buildJDK")]
		public string BuildJDK { get; set; }

		[JsonProperty("version-roles-seeding")]
		public string VersionRolesSeeding { get; set; }
	}
}
