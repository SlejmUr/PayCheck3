using Newtonsoft.Json;

namespace PayCheckServerLib.Jsons 
{
	public class WalletInfo 
	{
		[JsonProperty("balance")]
		public int Balance { get; set; }

		[JsonProperty("balanceOrigin")]
		public string BalanceOrigin { get; set; }

		[JsonProperty("createdAt")]
		public string CreatedAt { get; set; }

		[JsonProperty("currencyCode")]
		public string CurrencyCode { get; set; }

		[JsonProperty("currencySymbol")]
		public string CurrencySymbol { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("namespace")]
		public string Namespace { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("timeLimitedBalances")]
		public List<object> TimeLimitedBalances { get; set; }

		[JsonProperty("totalPermanentBalance")]
		public int TotalPermanentBalance { get; set; }

		[JsonProperty("totalTimeLimitedBalance")]
		public int TotalTimeLimitedBalance { get; set; }

		[JsonProperty("updatedAt")]
		public string UpdatedAt { get; set; }

		[JsonProperty("userId")]
		public string UserId { get; set; }
	}
	public class CurrencyJson 
	{
		[JsonProperty("balance")]
		public int Balance { get; set; }

		[JsonProperty("currencyCode")]
		public string CurrencyCode { get; set; }

		[JsonProperty("currencySymbol")]
		public string CurrencySymbol { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("namespace")]
		public string Namespace { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("userId")]
		public string UserId { get; set; }

		[JsonProperty("walletInfos")]
		public List<WalletInfo> WalletInfos { get; set; }

		[JsonProperty("walletStatus")]
		public string WalletStatus { get; set; }
	}
}
