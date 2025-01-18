using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayCheckServerLib.Helpers
{
	public class ErrorHelper
	{
		public enum Errors : int
		{
			/// <summary>
			/// Unauthorized access, user does not have the required permissions.
			/// </summary>
			UnauthorizedAccess = 20001,
			/// <summary>
			/// Vaguely named by AccelByte, but PayCheck3 will utilise it in the case that endpoints are given invalid data in the request.
			/// </summary>
			ValidationError = 20002,
			/// <summary>
			/// Used in the platform service whenever an item does not exist, usually if somebody attempts to purchase an invalid id.
			/// </summary>
			ItemDoesNotExistInNamespace = 30341
		}

		public class ErrorEntity
		{
			[JsonProperty("errorCode")]
			public int ErrorCode { get; set; }
			[JsonProperty("errorMessage")]
			public string ErrorMessage { get; set; }
		}

		public static string GetResponseBodyForErrorCode(int errorCode)
		{
			return GetResponseBodyForErrorCode((Errors)errorCode);
		}
		public static string GetResponseBodyForErrorCode(Errors error)
		{
			ErrorEntity errorEntity = new ErrorEntity();
			errorEntity.ErrorMessage = $"PayCheck3 has no error message for error code: {error}";
			errorEntity.ErrorCode = (int)error;
			switch (error)
			{
				case Errors.UnauthorizedAccess:
					errorEntity.ErrorMessage = "unauthorized";
					break;
				case Errors.ValidationError:
					errorEntity.ErrorMessage = "validation error";
					break;
				case Errors.ItemDoesNotExistInNamespace:
					errorEntity.ErrorMessage = "Item [{itemId}] does not exist in namespace [{namespace}]";
					break;
			}
			return JsonConvert.SerializeObject(errorEntity);
		}
	}
}
