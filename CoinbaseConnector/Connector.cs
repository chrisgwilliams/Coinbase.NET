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
	// For full documentation on all Coinbase API calls, please visit https://coinbase.com/api/doc

	// Disclaimer: I do not work for Coinbase.com, but I will attempt to answer any  
	// questions you may have about THIS wrapper (not about Coinbase) if you post them 
	// to my GitHub repo: http://www.github.com/chrisgwilliams/coinbase.NET or message 
	// me via Twitter: @chrisgwilliams 
	
	public class Connector
    {
		private string API_KEY = APIKeys.API_KEY;
		private string API_SECRET = APIKeys.API_SECRET;

		private string URL_BASE = "https://coinbase.com/api/v1/";
		private const String GET = "GET";
		private const String POST = "POST";
		private const String PUT = "PUT";
		private const String DELETE = "DELETE";
		
		// Account Changes
		public string GetAccountChanges(int page = 1)
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
		public string GetAddressList(int page = 1, int limit = 25, String query = "")
		{
			// Page field is optional. Default is 1
			return JsonRequest(URL_BASE + "addresses?page=" + page + "&limit=" + limit + "&query=" + query, GET);
		}

		// OAuth Applications
		public string GetOAuthApplicationsList(int page = 1)
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
		public string CreatePaymentButton(String name, String price, String currency, String type = "buy_now", 
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
			sb.Append("&button[price_string]=" + price);
			// Price currency as ISO 4217 Currency Code (i.e. USD, BTC)
			sb.Append("&button[price_currency_iso]=" + currency);

			// OPTIONAL PARAMS
			// Type must be one of buy_now, donation, or subscription. Default is buy_now
			if (type != "") sb.Append("&button[type]=" + type);
			// Style must be one of buy_now_large, buy_now_small, donation_large, donation_small, 
			// subscription_large, subscription_small, custom_large, custom_small, and none. Default is buy_now_large
			if (style != "") sb.Append("&button[style]=" + style);
			// Text may be used on custom_large or custom_small styles (above.) Default is "Pay With Bitcoin."
			if (text != "") sb.Append("&button[text]=" + text);
			// Description may be used to add more infomation to transaction notes
			if (description != "") sb.Append("&button[description]=" + description);
			// Custom usually represents an Order, User or Product ID corresponding to a record in your database.
			if (custom != "") sb.Append("&button[custom]=" + custom);
			// Custom Secure should be set to TRUE to prevent the custom parameter from being viewed or modified after 
			// the button has been created. Use this if you are storing sensitive data in the custom parameter which you 
			// don’t want to be faked or leaked to the end user. Defaults to FALSE.
			if (custom_secure != false) sb.Append("&button[custom_secure]=" + custom_secure);
			// A custom callback URL specific to this button. It will receive the same information that would otherwise 
			// be sent to your Instant Payment Notification URL. If you have an Instant Payment Notification URL set on 
			// your account, this will be called instead — they will not both be called.
			if (callback_url != "") sb.Append("&button[callback_url]=" + callback_url);
			// A custom success URL specific to this button. The user will be redirected to this URL after a successful 
			// payment. It will receive the same parameters that would otherwise be sent to the default success url set
			// on the account.
			if (success_url != "") sb.Append("&button[success_url]=" + success_url);
			// A custom cancel URL specific to this button. The user will be redirected to this URL after a canceled 
			// order. It will receive the same parameters that would otherwise be sent to the default cancel url set 
			// on the account.
			if (cancel_url != "") sb.Append("&button[cancel_url]=" + cancel_url);
			// A custom info URL specific to this button. Displayed to the user after a successful purchase for sharing.
			// It will receive the same parameters that would otherwise be sent to the default info url set on the account.
			if (info_url != "") sb.Append("&button[info_url]=" + info_url);
			// Auto-redirect users to success or cancel url after payment. (Cancel url if the user pays the wrong amount.)
			// Default is TRUE
			if (auto_redirect != true) sb.Append("&button[auto_redirect]=" + auto_redirect);
			// Allow users to change the price on the generated button. Default is FALSE
			if (variable_price != false) sb.Append("&button[variable_price]=" + variable_price);
			// Show some suggested prices. Default is FALSE
			if (choose_price != false) sb.Append("&button[choose_price]=" + choose_price);
			// Collect shipping address from customer (not for use with inline iframes). Default is TRUE
			if (include_address != true) sb.Append("&button[include_address]=" + include_address);
			// Collect email address from customer (not for use with inline iframes). Default is TRUE
			if (include_email != true) sb.Append("&button[include_email]=" + include_email);
			// Suggested price 1
			if (price1 != "") sb.Append("&button[price1]=" + price1);
			// Suggested price 2
			if (price2 != "") sb.Append("&button[price2]=" + price2);
			// Suggested price 3
			if (price3 != "") sb.Append("&button[price3]=" + price3);
			// Suggested price 4
			if (price4 != "") sb.Append("&button[price4]=" + price4);
			// Suggested price 5
			if (price5 != "") sb.Append("&button[price5]=" + price5);

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
		public string PurchaseBitcoin(float qty, Boolean agree_btc_amount_varies = false, String payment_method_id = "")
		{
			// The agree_btc_amount_varies parameter is optional and indicates whether or not the buyer would still like
			// to buy if they have to wait for their money to arrive to lock in a price. Default value is FALSE
			return JsonRequest(URL_BASE + "buys?qty=" + qty + "&agree_btc_amount_varies=" + agree_btc_amount_varies 
				+ "&payment_method_id=" + payment_method_id, POST);
		}

		// Contacts
		public string GetEmailContactsList(int page = 1, int limit = 25, String query = "")
		{
			if (limit > 1000) limit = 1000;
			return JsonRequest(URL_BASE + "contacts?page=" + page + "&limit=" + limit + "&query=" + query, GET);
		}

		// Currencies
		public string GetSupportedCurrenciesList()
		{
			return JsonRequest(URL_BASE + "currencies", GET);
		}
		public string GetBTCExchangeRate()
		{
			return JsonRequest(URL_BASE + "exchange_rates", GET);
		}

		// Exports
		public string GenerateCSVReport(String type, String timeRange, String timeRangeStart="", String timeRangeEnd = "", String email = "")
		{
			// Valid values for type: transactions, orders, transfers
			// Valid values for time_range: today, yesterday, past_7, past_30, month_to_date, last_full_month, year_to_date, 
			// last_full_year, all, custom.  If custom, must supply time_range_start and time_range_end.
			// If an email address is not provided, address on account will be used.
			var sb = new StringBuilder();

			// REQUIRED PARAMS
			sb.Append("?type=" + type);
			sb.Append("&time_range=" + timeRange);

			// OPTIONAL PARAMS
			if (email != "") sb.Append("&email=" + email);

			// CONDITIONAL PARAMS
			if (type == "custom") sb.Append("&time_range_start=" + timeRangeStart + "&time_range_end" + timeRangeEnd);

			return JsonRequest(URL_BASE + "exports" + sb.ToString(), POST);
		}

		// Orders
		public string GetReceivedMerchantOrdersList(int page = 1)
		{
			// Page field is optional. Default is 1
			return JsonRequest(URL_BASE + "orders?page=" + page, GET);
		}
		// Use this endpoint to create a one-time unique order that does not use the Coinbase merchant tools.
		// Ex: Generating a bitcoin address for an order and displaying it directly in your page, to only one user.
		public string CreateNewOrder(String name, String price, String currency, String type = "buy_now",
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
			if (type != "") sb.Append("&button[type]=" + type);
			// Style must be one of buy_now_large, buy_now_small, donation_large, donation_small, 
			// subscription_large, subscription_small, custom_large, custom_small, and none. Default is buy_now_large
			if (style != "") sb.Append("&button[style]=" + style);
			// Text may be used on custom_large or custom_small styles (above.) Default is "Pay With Bitcoin."
			if (text != "") sb.Append("&button[text]=" + text);
			// Description may be used to add more infomation to transaction notes
			if (description != "") sb.Append("&button[description]=" + description);
			// Custom usually represents an Order, User or Product ID corresponding to a record in your database.
			if (custom != "") sb.Append("&button[custom]=" + custom);
			// Custom Secure should be set to TRUE to prevent the custom parameter from being viewed or modified after 
			// the button has been created. Use this if you are storing sensitive data in the custom parameter which you 
			// don’t want to be faked or leaked to the end user. Defaults to FALSE.
			if (custom_secure != false) sb.Append("&button[custom_secure]=" + custom_secure);
			// A custom callback URL specific to this button. It will receive the same information that would otherwise 
			// be sent to your Instant Payment Notification URL. If you have an Instant Payment Notification URL set on 
			// your account, this will be called instead — they will not both be called.
			if (callback_url != "") sb.Append("&button[callback_url]=" + callback_url);
			// A custom success URL specific to this button. The user will be redirected to this URL after a successful 
			// payment. It will receive the same parameters that would otherwise be sent to the default success url set
			// on the account.
			if (success_url != "") sb.Append("&button[success_url]=" + success_url);
			// A custom cancel URL specific to this button. The user will be redirected to this URL after a canceled 
			// order. It will receive the same parameters that would otherwise be sent to the default cancel url set 
			// on the account.
			if (cancel_url != "") sb.Append("&button[cancel_url]=" + cancel_url);
			// A custom info URL specific to this button. Displayed to the user after a successful purchase for sharing.
			// It will receive the same parameters that would otherwise be sent to the default info url set on the account.
			if (info_url != "") sb.Append("&button[info_url]=" + info_url);
			// Auto-redirect users to success or cancel url after payment. (Cancel url if the user pays the wrong amount.)
			// Default is TRUE
			if (auto_redirect != true) sb.Append("&button[auto_redirect]=" + auto_redirect);
			// Allow users to change the price on the generated button. Default is FALSE
			if (variable_price != false) sb.Append("&button[variable_price]=" + variable_price);
			// Show some suggested prices. Default is FALSE
			if (choose_price != false) sb.Append("&button[choose_price]=" + choose_price);
			// Collect shipping address from customer (not for use with inline iframes). Default is TRUE
			if (include_address != true) sb.Append("&button[include_address]=" + include_address);
			// Collect email address from customer (not for use with inline iframes). Default is TRUE
			if (include_email != true) sb.Append("&button[include_email]=" + include_email);
			// Suggested price 1
			if (price1 != "") sb.Append("&button[price1]=" + price1);
			// Suggested price 2
			if (price2 != "") sb.Append("&button[price2]=" + price2);
			// Suggested price 3
			if (price3 != "") sb.Append("&button[price3]=" + price3);
			// Suggested price 4
			if (price4 != "") sb.Append("&button[price4]=" + price4);
			// Suggested price 5
			if (price5 != "") sb.Append("&button[price5]=" + price5);

			// CONDITIONAL PARAMS
			// Repeat must be one of never, daily, weekly, every_two_weeks, monthly, quarterly, or yearly. 
			// Required if type = subscription. Default value is never.
			sb.Append("&button[repeat]=" + repeat);

			return JsonRequest(URL_BASE + "orders" + sb.ToString(), POST);
		}
		// ID can represent an actual Order ID or a custom merchant field.
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
			// qty is optional. Default value is 1
			// currency is optional. Default value is USD (this is the only supported value at this time.)
			return JsonRequest(URL_BASE + "prices/buy?qty=" + qty + "&currency=" + currency, GET);
		}
		public string GetTotalSellPriceForBitcoin(float qty = 1, String currency = "USD")
		{
			// qty is optional. Default value is 1.
			// currency is optional. Default value is USD (this is the only supported value at this time.)
			return JsonRequest(URL_BASE + "prices/sell?qty=" + qty + "&currency=" + currency, GET);
		}
		public string GetSpotPriceForBitcoin(String currency = "USD")
		{
			// Currency must be an ISO 4217 Currency Code. Default is USD
			return JsonRequest(URL_BASE + "prices/spot_rate?currency=" + currency, GET);
		}
		public string GetHistoricalSpotPriceForBitcoin(int page = 1)
		{
			// Page field is optional. Default is 1
			return JsonRequest(URL_BASE + "prices/historical?page=" + page, GET);
		}

		// Recurring Payments
		public string GetRecurringPaymentsList(String ID = "", int page = 1, int limit = 25)
		{
			// ID field is optional. Default is no parameter. 
			// If you specify an ID, you get an individual recurring payment, otherwise you get a list
			if (ID != "") return JsonRequest(URL_BASE + "recurring_payments/" + ID, GET);

			return JsonRequest(URL_BASE + "recurring_payments?page=" + page + "&limit=" + limit, GET);
		}

		// Sells
		public string SellBitcoin(float qty, String payment_method_id = "")
		{
			// Quantity of Bitcoin to sell is required.
			// Payment Method ID is optional. Will use default account ID. Must have verified bank account to work.
			return JsonRequest(URL_BASE + "sells?qty=" + qty + "&payment_method_id=" + payment_method_id, POST);	
		}

		// Subscribers
		public string GetSubscribersList(String ID = "", int page = 1, int limit = 25)
		{
			// ID field is optional. Default is no parameter. 
			// If you specify an ID, you get an individual customer subscription, otherwise you get a list
			if (ID != "") return JsonRequest(URL_BASE + "subscribers/" + ID, GET);

			return JsonRequest(URL_BASE + "subscribers?page=" + page + "&limit=" + limit, GET);
		}
		
		// Tokens
		public string CreateToken()
		{
			// This call creates a token redeemable for Bitcoin. Returned Bitcoin address can be used to send money 
			// to the token, and will be credited to the account of the token redeemer if money is sent.
			return JsonRequest(URL_BASE + "tokens", POST);
		}
		public string RedeemToken(String tokenID = "")
		{
			// This call claims a redeemable token for its address and bitcoin(s).
			return JsonRequest(URL_BASE + "tokens/redeem?token_id=" + tokenID, POST);
		}

		// Transactions
		public string GetTransactionsList(String ID = "", int page = 1, int limit = 25) 
		{
			// ID field is optional. Default is no parameter. 
			// If you specify an ID, you get an individual transaction, otherwise you get a list
			if (ID != "") return JsonRequest(URL_BASE + "transactions/" + ID, GET);

			return JsonRequest(URL_BASE + "transactions?page=" + page + "&limit=" + limit, GET);
		}
		public string SendMoney(String email, String amount = "", String amountString = "", String amountCurrencyISO = "", 
								String notes = "", String userFee = "", String referrerID = "", String idem = "", 
								Boolean instantBuy = false)
		{
			// This lets you send money to an email or bitcoin address. If you pass an amount param it will be 
			// interpreted as a bitcoin amount. Alternatively you can pass an amount_string and amount_currency_iso 
			// such as ‘USD’ or ‘EUR’ and the equivalent amount of bitcoin will be sent at current exchange rates.

			// The instant_buy parameter can be used to purchase the necessary funds first, then send them. This will 
			// only work if Instant Buy is enabled on your account and the amount being purchased is less than or 
			// equal to the remainder of your daily limit. If the instant buy is successful, the response will come 
			// back with an additional transfer field representing the purchase.

			// If you provide a new email address in the "to" field, this will create the user and send them a payment,
			// asking them to sign in and claim the amount (they will choose a password on this first step). If they
			// don’t claim the amount within 30 days the bitcoin will be automatically returned to you.

			var sb = new StringBuilder();

			// REQUIRED PARAMS
			// This must be an email address.
			sb.Append("?transaction[to]=" + email); 

			// CONDITIONAL PARAMS
			// If you supply values for amount, amount_string AND amount_currency_iso, then amount takes precedence.
			if (amount != "") 
			{ 
				sb.Append("&transaction[amount]=" + amount);
			} else {
				sb.Append("&transaction[amount_string]=" + amountString);
				sb.Append("&transaction[amount_currency_iso]=" + amountCurrencyISO);
			}
			
			// OPTIONAL PARAMS
			if (notes != "") sb.Append("&transaction[notes]=" + notes);
			if (userFee != "") sb.Append("&transaction[user_fee]=" + userFee);
			if (referrerID != "") sb.Append("&transaction[referrer_id]=" + referrerID);
			if (idem != "") sb.Append("&transaction[idem]=" + idem);
			if (instantBuy != false) sb.Append("&transaction[instant_buy]=" + instantBuy.ToString());

			return JsonRequest(URL_BASE + "transactions/send_money" + sb.ToString(), POST);
		}
		public string SendInvoice(String from, String amount = "", String amountString = "", String amountCurrencyISO = "",
								  String notes = "")
		{
			// This lets the user request money from a bitcoin address. If you pass an amount param it will be 
			// interpreted as a bitcoin amount. Alternatively you can pass an amount_string and amount_currency_iso 
			// such as ‘USD’ or ‘EUR’ and the equivalent amount of bitcoin will be sent at current exchange rates.
			var sb = new StringBuilder();

			// REQUIRED PARAMS
			sb.Append("?transaction[from]=" + from);
			
			// CONDITIONAL PARAMS
			// If you supply values for amount, amount_string AND amount_currency_iso, then amount takes precedence.
			if (amount != "")
			{
				sb.Append("&transaction[amount]=" + amount);
			}
			else
			{
				sb.Append("&transaction[amount_string]=" + amountString);
				sb.Append("&transaction[amount_currency_iso]=" + amountCurrencyISO);
			}

			// OPTIONAL PARAMS
			if (notes != "") sb.Append("&transaction[notes]=" + notes);

			return JsonRequest(URL_BASE + "transactions/request_money" + sb.ToString(), POST);
		}
		public string ResendInvoice(String ID)
		{
			// This lets the user resend a money request.
			return JsonRequest(URL_BASE + "transactions/" + ID + "/resend_request", PUT);
		}
		public string CancelMoneyRequest(String ID)
		{
			// This lets a user cancel a money request. Money requests can be canceled by the sender or the recipient.
			return JsonRequest(URL_BASE + "transactions/" + ID + "/cancel_request", DELETE);
		}
		public string CompleteMoneyRequest(String ID)
		{
			// This lets a user complete a money request. Money requests can only be completed by the sender (not the 
			// recipient.) The sender in this context is the user who is sending money (not sending the invoice.)
			return JsonRequest(URL_BASE + "transactions/" + ID + "/complete_request", PUT);
		}

		// Transfers
		public string GetTransfersList(int page = 1, int limit = 25)
		{
			// This returns the user's bitcoin purchases and sells. Sorted by created_at, descending.
			// page param is optional, default value is 1.
			// limit param is optional, default value is 25, max value is 1000.
			if (limit > 1000) limit = 1000;
			return JsonRequest(URL_BASE + "transfers?page=" + page + "&limit=" + limit, GET);
		}
		
		// Users
		public string CreateNewUser(String email, String password, String referrerID = "", String clientID = "")
		{
			// This method creates a user with an email and password. The receive address for the user is returned 
			// as well if you’d like to send a first payment to them. To generate additional receive addresses you 
			// will need to be authenticated as this user.

			// This method is useful if you would only like to create the user, or would like to send to their 
			// bitcoin address instead of an email address.

			var sb = new StringBuilder();

			// REQUIRED PARAMS
			sb.Append("?user[email]=" + email);
			// A strong password - at least eight digits without dictionary words.
			sb.Append("&user[password]=" + password);

			// OPTIONAL PARAMS
			if (referrerID != "") sb.Append("&user[referrer_id]=" + referrerID);
			if (clientID != "") sb.Append("&user[client_id]=" + clientID);

			return JsonRequest(URL_BASE + "users" + sb.ToString(), POST);

		}
		public string GetAccountSettings()
		{
			// Show current user with account settings.
			return JsonRequest(URL_BASE + "users", GET);
		}
		public string UpdateAccountSettings(String id, String name = "", String email = "", String pin = "", 
											String nativeCurrency = "", String timeZone = "")
		{
			// This lets you update account settings for the current user. Only these fields are updatable.
			var sb = new StringBuilder();

			sb.Append("&id=" + id);
			if (name != "") sb.Append("&user[name]=" + name);
			if (email != "") sb.Append("&user[email]=" + email);
			if (pin != "") sb.Append("&user[pin]=" + pin);
			if (nativeCurrency != "") sb.Append("&user[native_currency]=" + nativeCurrency);
			if (timeZone != "") sb.Append("&user[time_zone]=" + timeZone);

			return JsonRequest(URL_BASE + "users/" + id + sb.ToString(), PUT);
		}





		private string JsonRequest(string url, string method)
		{
			// take care of any spaces in params
			url = Uri.EscapeUriString(url);

			string returnData = String.Empty;

			var webRequest = HttpWebRequest.Create(url) as HttpWebRequest;
			if (webRequest != null)
			{
				webRequest.Accept = "*/*";
				webRequest.UserAgent = ".NET";
				webRequest.Method = method;
				webRequest.ContentType = "application/json";
				webRequest.Host = "coinbase.com";

				string nonce = Convert.ToInt64(DateTime.Now.Ticks).ToString();
				string message = nonce + url;
				string signature = HashEncode(HashHMAC(StringEncode(API_SECRET), StringEncode(message)));

				var whc = new WebHeaderCollection();
				whc.Add("ACCESS_KEY: " + API_KEY);
				whc.Add("ACCESS_SIGNATURE: " + signature);
				whc.Add("ACCESS_NONCE: " + nonce);
				webRequest.Headers = whc;

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
