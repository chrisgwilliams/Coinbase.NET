using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Xml;
using System.Configuration;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;

namespace CoinbaseConnector
{
    public class Connector
    {
		private string API_KEY = APIKeys.API_KEY;
		private string API_SECRET = APIKeys.API_SECRET;

		private string URL_BASE = "https://coinbase.com/api/v1/";
		private const string GET = "GET";
		private const string POST = "POST";
		
		// Account Changes
		public string GetAccountChanges(String page = "1")
		{
			// Page field is optional. Default is 1
			return JsonRequest(URL_BASE + "account_changes?page=" + page, GET);
		}

		// Account
		public string GetAccountBalance()
		{
			return JsonRequest(URL_BASE + "account/balance", GET);
		}
		public string GetCurrentReceiveAddress()
		{
			return JsonRequest(URL_BASE + "account/receive_address", GET);
		}
		public string GenerateReceiveAddress()
		{
			return JsonRequest(URL_BASE + "account/generate_receive_address", POST);
		}
		public string GenerateReceiveAddress(String callbackURL, String label)
		{
			return JsonRequest(URL_BASE + "account/generate_receive_address?address[callback_url]=" + callbackURL+"&address[label]=" + label, POST); 
		}

		// Addresses
		public string GetAddressList(String page = "1", String limit = "25", String query = "")
		{
			// Page field is optional. Default is 1
			return JsonRequest(URL_BASE + "addresses?page=" + page + "&limit=" + limit + "&query=" + query, GET);
		}

		// OAuth Applications
		public string GetOAuthApplicationsList(String page = "1")
		{
			return JsonRequest(URL_BASE + "oauth/applications?page=" + page, GET);
		}
		public string GetOauthApplicationByID(String ID)
		{
			return JsonRequest(URL_BASE + "oauth/applications/" + ID, GET);
		}
		public string CreateOAuthApplication(String name, String redirectURI)
		{
			return JsonRequest(URL_BASE + "oauth/applications?application[name]=" + name + "&application[redirect_uri]=" + redirectURI, POST);
		}

		// Authorization
		public string GetApplicationAccountAccessInfo()
		{
			return JsonRequest(URL_BASE + "authorization", GET);
		}

		// Buttons
		public string CreatePaymentButton(String name, Decimal price, String currency, String type = "buy_now", 
										  String repeat = "never", String style = "buy_now_large", String text = "Pay With Bitcoin",
										  String description = "", String custom = "", Boolean custom_secure = false,
										  String callback_url = "", String success_url = "", String cancel_url = "", 
										  String info_url = "", Boolean auto_redirect = true, Boolean variable_price = false,
										  Boolean choose_price = false, Boolean include_address = true, Boolean include_email = true,
										  String price1 = "", String price2 = "", String price3 = "", String price4 = "", 
										  String price5 = "")
		{
			var sb = new StringBuilder();

			// REQUIRED PARAMS
			sb.Append("?button[name]=" + name);
			// Can be more then two significant digits if price_currency_iso equals BTC
			if (currency != "BTC") string.Format("{0:0.00}", price);
			sb.Append("&button[price_string]=" + price.ToString());
			// Price currency as ISO 4217 Currency Code (i.e. USD, BTC)
			sb.Append("&button[price_currency_iso]=" + currency);

			// OPTIONAL PARAMS
			// Type must be one of buy_now, donation, or subscription. Default is buy_now
			sb.Append("&button[type]=" + type);
			// Style must be one of buy_now_large, buy_now_small, donation_large, donation_small, 
			// subscription_large, subscription_small, custom_large, custom_small, and none. Default is buy_now_large
			sb.Append("&button[style]=" + style);
			// Text may be used on custom_large or custom_small styles (above.) Default is "Pay With Bitcoin."
			sb.Append("&button[text]=" + text);
			// Description may be used to add more infomation to transaction notes
			sb.Append("&button[description]=" + description);
			// Custom usually represents an Order, User or Product ID corresponding to a record in your database.
			sb.Append("&button[custom]=" + custom);
			// Custom Secure should be set to TRUE to prevent the custom parameter from being viewed or modified after 
			// the button has been created. Use this if you are storing sensitive data in the custom parameter which you 
			// don’t want to be faked or leaked to the end user. Defaults to FALSE.
			sb.Append("&button[custom_secure]=" + custom_secure);
			// A custom callback URL specific to this button. It will receive the same information that would otherwise 
			// be sent to your Instant Payment Notification URL. If you have an Instant Payment Notification URL set on 
			// your account, this will be called instead — they will not both be called.
			sb.Append("&button[callback_url]=" + callback_url);
			// A custom success URL specific to this button. The user will be redirected to this URL after a successful 
			// payment. It will receive the same parameters that would otherwise be sent to the default success url set
			// on the account.
			sb.Append("&button[success_url]=" + success_url);
			// A custom cancel URL specific to this button. The user will be redirected to this URL after a canceled 
			// order. It will receive the same parameters that would otherwise be sent to the default cancel url set 
			// on the account.
			sb.Append("&button[cancel_url]=" + cancel_url);
			// A custom info URL specific to this button. Displayed to the user after a successful purchase for sharing.
			// It will receive the same parameters that would otherwise be sent to the default info url set on the account.
			sb.Append("&button[info_url]=" + info_url);
			// Auto-redirect users to success or cancel url after payment. (Cancel url if the user pays the wrong amount.)
			// Default is TRUE
			sb.Append("&button[auto_redirect]=" + auto_redirect);
			// Allow users to change the price on the generated button. Default is FALSE
			sb.Append("&button[variable_price]=" + variable_price);
			// Show some suggested prices. Default is FALSE
			sb.Append("&button[choose_price]=" + choose_price);
			// Collect shipping address from customer (not for use with inline iframes). Default is TRUE
			sb.Append("&button[include_address]=" + include_address);
			// Collect email address from customer (not for use with inline iframes). Default is TRUE
			sb.Append("&button[include_email]=" + include_email);
			// Suggested price 1
			sb.Append("&button[price1]=" + price1);
			// Suggested price 2
			sb.Append("&button[price2]=" + price2);
			// Suggested price 3
			sb.Append("&button[price3]=" + price3);
			// Suggested price 4
			sb.Append("&button[price4]=" + price4);
			// Suggested price 5
			sb.Append("&button[price5]=" + price5);

			// CONDITIONAL PARAMS
			// Repeat must be one of never, daily, weekly, every_two_weeks, monthly, quarterly, or yearly. 
			// Required if type = subscription. Default value is never.
			sb.Append("&button[repeat]=" + repeat);

			return JsonRequest(URL_BASE + "buttons" + sb.ToString(), POST);
		}
		public string CreateOrderForButton(String code)
		{
			return JsonRequest(URL_BASE + "buttons/" + code + "/create_order", POST);
		}

		// Buys
		// The agree_btc_amount_varies parameter is optional and indicates whether or not the buyer would still like
		// to buy if they have to wait for their money to arrive to lock in a price. Default value is FALSE
		public string PurchaseBitcoin(float qty, Boolean agree_btc_amount_varies = false, String payment_method_id = "")
		{
			return JsonRequest(URL_BASE + "buys?qty=" + qty + "&agree_btc_amount_varies=" + agree_btc_amount_varies 
				+ "&payment_method_id=" + payment_method_id, POST);
		}

		// Contacts
		public string GetEmailContactsList(int page = 1, int limit = 25, String query = "")
		{
			if (limit > 1000) limit = 1000;
			return JsonRequest(URL_BASE + "contacts", GET);
		}

		// Currencies
		public string GetSupportedCurrencies()
		{
			return JsonRequest(URL_BASE + "currencies", GET);
		}
		public string GetBTCExchangeRate()
		{
			return JsonRequest(URL_BASE + "exchange_rates", GET);
		}

		// Orders
		public string GetReceivedMerchantOrders(string page = "1")
		{
			// Page field is optional. Default is 1
			return JsonRequest(URL_BASE + "orders?page=" + page, GET);
		}
		public string CreateNewOrder()
		{
			string jsonParams = ""; // TODO
			return JsonRequest(URL_BASE + "orders", POST, jsonParams);
		}
		public string GetMerchantOrderByID(string ID = "")
		{
			return JsonRequest(URL_BASE + "orders/" + ID, GET);
		}

		// Payment Methods
		public string GetAssociatedPaymentMethods()
		{
			return JsonRequest(URL_BASE + "payment_methods", GET);
		}

		// Prices
		public string GetTotalBuyPriceForBitcoin(float qty = 1, String currency = "USD")
		{
			return JsonRequest(URL_BASE + "prices/buy?qty=" + qty + "&currency=" + currency, GET);
		}
		public string GetTotalSellPriceForBitcoin(float qty = 1, String currency = "USD")
		{
			return JsonRequest(URL_BASE + "prices/sell?qty=" + qty + "&currency=" + currency, GET);
		}
		public string GetSpotPriceForBitcoin(String currency = "USD")
		{
			// Currency must be an ISO 4217 Currency Code. Default is USD
			return JsonRequest(URL_BASE + "prices/spot_rate?currency=" + currency, GET);
		}
		public string GetHistoricalSpotPriceForBitcoin(String page = "1")
		{
			// Page field is optional. Default is 1
			return JsonRequest(URL_BASE + "prices/historical?page=" + page, GET);
		}

		// Recurring Payments
		public string GetRecurringPayments(String ID = "")
		{
			// ID field is optional. Default is no parameter. 
			return JsonRequest(URL_BASE + "recurring_payments/" + ID, GET);
		}

		// Sells
		public string SellBitcoin(float qty, String payment_method_id = "")
		{
			// Quantity of Bitcoin to sell is required.
			// Payment Method ID is optional. Will use default account ID. Must have verified bank account to work.
			return JsonRequest(URL_BASE + "sells?qty=" + qty + "&payment_method_id=" + payment_method_id, POST);	
		}



#region Supporting Classes for API Parameters

		public class Order
		{
			public Button button {get; set;}
		}

		public class Button
		{
			public string name { get; set; }
			public string price_string { get; set; }
			public string price_currency_iso { get; set; }
			public string type { get; set; }
			public string repeat { get; set; }
			public string style { get; set; }
			public string text { get; set; }
			public string description { get; set; }
			public string custom { get; set; }
			public bool custom_secure { get; set; }
			public string callback_url { get; set; }
			public string success_url { get; set; }
			public string cancel_url { get; set; }
			public string info_url { get; set; }
			public bool auto_redirect { get; set; }
			public bool variable_price { get; set; }
			public bool choose_price { get; set; }
			public bool include_address { get; set; }
			public bool include_email { get; set; }
			public string price1 { get; set; }
			public string price2 { get; set; }
			public string price3 { get; set; }
			public string price4 { get; set; }
			public string price5 { get; set; }
		}

#endregion


		private string JsonRequest(string url, string method, String postdata = "")
		{
			string returnData = String.Empty;

			// strip out any escape characters in the optional parameters
			postdata = postdata.Replace(@"\", "");

			var webRequest = HttpWebRequest.Create(url) as HttpWebRequest;
			if (webRequest != null)
			{
				webRequest.Accept = "*/*";
				webRequest.UserAgent = ".NET";
				webRequest.Method = method;
				webRequest.ContentType = "application/json";
				webRequest.Host = "coinbase.com";

				string nonce = Convert.ToInt64(DateTime.Now.Ticks).ToString();
				string message = nonce + url + postdata;
				string signature = HashEncode(HashHMAC(StringEncode(API_SECRET), StringEncode(message)));

				var whc = new WebHeaderCollection();
				whc.Add("ACCESS_KEY: " + API_KEY);
				whc.Add("ACCESS_SIGNATURE: " + signature);
				whc.Add("ACCESS_NONCE: " + nonce);
				webRequest.Headers = whc;

				if (postdata != "")
				{
					ASCIIEncoding encoding = new ASCIIEncoding();
					byte[] byte1 = encoding.GetBytes(postdata);

					// Set the content length of the string being posted.
					webRequest.ContentLength = byte1.Length;
					Stream newStream = webRequest.GetRequestStream();

					newStream.Write(byte1, 0, byte1.Length);
				}

				using (WebResponse response = webRequest.GetResponse())
				{
					using (Stream stream = response.GetResponseStream())
					{
						StreamReader reader = new StreamReader(stream);
						returnData = reader.ReadToEnd();
					}
				}
			}

			return returnData;
		}

		private static byte[] StringEncode(string text)
		{
			var encoding = new ASCIIEncoding();
			return encoding.GetBytes(text);
		}

		private static string HashEncode(byte[] hash)
		{
			return BitConverter.ToString(hash).Replace("-", "").ToLower();
		}

		private static byte[] HashHMAC(byte[] key, byte[] message)
		{
			var hash = new HMACSHA256(key);
			return hash.ComputeHash(message);
		}
    }
}
