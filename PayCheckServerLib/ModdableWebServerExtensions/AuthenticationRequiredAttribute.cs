using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayCheckServerLib.ModdableWebServerExtensions
{
	[AttributeUsage(AttributeTargets.Method)]
	public class AuthenticationRequiredAttribute : Attribute
	{
		[Flags]
		public enum Access : int
		{
			None = 0,

			CREATE = 1 << 0,
			READ = 1 << 1,
			UPDATE = 1 << 2,
			DELETE = 1 << 3,

			ALL = CREATE | READ | UPDATE | DELETE
		}
		// https://docs.accelbyte.io/gaming-services/services/access/authorization/master-permissions/
		public string? PermissionRequired { get; private set; }
		public Access AccessRequired {  get; private set; }
		public bool NeedsToken {  get; private set; }
		/// <summary>
		/// Attribute to protect methods from being executed without the required permissions.
		/// 
		/// Passing an empty string into permission required, or Access.None in access required will revert to default behaviour and not secure the method at all!
		/// </summary>
		/// <param name="permissionRequired"></param>
		/// <param name="accessRequired"></param>
		/// <param name="needsToken">
		/// If permissionRequired and accessRequired are empty or None respectively, setting needsToken to true will make the request require a valid authentication token.
		/// 
		/// This parameter is only really required for IAM endpoints, as logging in as a user does not need a token, but getting information about the user of a token does need a token.
		/// </param>
		public AuthenticationRequiredAttribute(string? permissionRequired = "", Access accessRequired = Access.None, bool needsToken = false)
		{
			this.PermissionRequired = permissionRequired;
			this.AccessRequired = accessRequired;
			this.NeedsToken = needsToken;
		}
	}
}
