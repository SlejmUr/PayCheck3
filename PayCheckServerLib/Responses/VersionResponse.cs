using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Jsons;
using System.Text.RegularExpressions;

namespace PayCheckServerLib.Responses {
	public class VersionResponse {
		static bool GenericVersionResponse(HttpRequest request, PC3Server.PC3Session session) {
			ResponseCreator response = new ResponseCreator();

			var regex = new Regex(@"\/(.*)\/version");
			Debugger.PrintDebug(request.Url);
			MatchCollection matches = regex.Matches(request.Url);
			var endpoint = "";
			foreach (Match match in matches) {
				foreach (Group group in match.Groups) {
					if(!group.Value.Contains("/")) {
						endpoint = group.Value;
					}
				}
			}

			// data taken from https://nebula.starbreeze.com/agreement/version and https://nebula.starbreeze.com/platform/version
			VersionJson version = new() {
				Name = endpoint == "agreement" ? "justice-legal-service" : String.Format("justice-{0}-service", endpoint),
				Realm = "prod",
				Version = "1.28.0",
				GitTag = "1.28.0",
				GitHash = "b55c3fcbc9",
				GitBranchName = "release-candidate",
				BuildDate = "2023-03-13T01:07:41+00:00",
				BuildID = "1.28.0",
				BuildBy = "Gradle 6.9.1",
				BuildOS = "Linux amd64 5.13.0-1021-aws",
				BuildJDK = "1.8.0_232 (Eclipse OpenJ9 openj9-0.17.0)",
				VersionRolesSeeding = "0.0.3"
			};

			response.SetBody(JsonConvert.SerializeObject(version));
			session.SendResponse(response.GetResponse());
			return true;
		}
		[HTTP("GET", "/iam/version")]
		public static bool IamVersion(HttpRequest request, PC3Server.PC3Session session) { return GenericVersionResponse(request, session); }
		[HTTP("GET", "/agreement/version")]
		public static bool AgreementVersion(HttpRequest request, PC3Server.PC3Session session) { return GenericVersionResponse(request, session); }
		[HTTP("GET", "/basic/version")]
		public static bool BasicVersion(HttpRequest request, PC3Server.PC3Session session) { return GenericVersionResponse(request, session); }
		[HTTP("GET", "/platform/version")]
		public static bool PlatformVersion(HttpRequest request, PC3Server.PC3Session session) { return GenericVersionResponse(request, session); }
		[HTTP("GET", "/social/version")]
		public static bool SocialVersion(HttpRequest request, PC3Server.PC3Session session) { return GenericVersionResponse(request, session); }
		[HTTP("GET", "/leaderboard/version")]
		public static bool LeaderboardVersion(HttpRequest request, PC3Server.PC3Session session) { return GenericVersionResponse(request, session); }
		[HTTP("GET", "/achievement/version")]
		public static bool AchievementVersion(HttpRequest request, PC3Server.PC3Session session) { return GenericVersionResponse(request, session); }
		[HTTP("GET", "/cloudsave/version")]
		public static bool CloudSaveVersion(HttpRequest request, PC3Server.PC3Session session) { return GenericVersionResponse(request, session); }
		[HTTP("GET", "/ugc/version")]
		public static bool UGCVersion(HttpRequest request, PC3Server.PC3Session session) { return GenericVersionResponse(request, session); }
		[HTTP("GET", "/lobby/version")]
		public static bool LobbyVersion(HttpRequest request, PC3Server.PC3Session session) { return GenericVersionResponse(request, session); }
		[HTTP("GET", "/group/version")]
		public static bool GroupVersion(HttpRequest request, PC3Server.PC3Session session) { return GenericVersionResponse(request, session); }
		[HTTP("GET", "/qosm/version")]
		public static bool QOSMVersion(HttpRequest request, PC3Server.PC3Session session) { return GenericVersionResponse(request, session); }
		[HTTP("GET", "/dsmcontroller/version")]
		public static bool DSMControllerVersion(HttpRequest request, PC3Server.PC3Session session) { return GenericVersionResponse(request, session); }
		[HTTP("GET", "/game-telemetry/version")]
		public static bool GameTelemetryVerion(HttpRequest request, PC3Server.PC3Session session) { return GenericVersionResponse(request, session); }
	}
}
