using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayCheckServerLib.Jsons
{
	public class Profile
	{
		public partial class ProfileResp
		{
			[JsonProperty("userId")]
			public string UserId { get; set; }
			[JsonProperty("avatarLargeUrl")]
			public string AvatarLargeUrl { get; set; }
			[JsonProperty("avatarSmallUrl")]
			public string AvatarSmallUrl { get; set; }
			[JsonProperty("avatarUrl")]
			public string AvatarUrl { get; set; }

			[JsonProperty("customAttributes")]
			public object CustomAttributes { get; set; }
			[JsonProperty("namespace")]
			public string Namespace { get; set; }
			[JsonProperty("publicId")]
			public string PublicId { get; set; }
		}
	}
}
