using System;
using System.Collections.Generic;
using CoinbaseConnector;
using Newtonsoft.Json;

namespace TestApp
{
	class Program
	{
		static void Main(string[] args)
		{
			var cbc = new Connector();
			var Response = "";

			Console.WriteLine("Account Changes: ");
			var changes = JsonConvert.DeserializeObject<AccountChanges_Result>(cbc.GetAccountChanges());
			Console.WriteLine("Current User: ");
			Console.WriteLine("id: " + changes.current_user.id);
			Console.WriteLine("email: " + changes.current_user.email);
			Console.WriteLine("name:  " + changes.current_user.name);
			Console.WriteLine("");

			Console.WriteLine("Account Balance: ");
			var amount = JsonConvert.DeserializeObject<Amount>(cbc.GetAccountBalance());
			Console.WriteLine(amount.amount + " " + amount.currency);
			Console.WriteLine("");
			
			Console.WriteLine("Generate Receive Address: ");
			var genReceiveAddress = JsonConvert.DeserializeObject<GenerateReceiveAddress_Result>(cbc.GenerateReceiveAddress());
			Console.WriteLine("success: " + genReceiveAddress.success);
			Console.WriteLine("address: " + genReceiveAddress.address);
			Console.WriteLine("callback_url: " + genReceiveAddress.callback_url);
			Console.WriteLine("label: " + genReceiveAddress.label);
			Console.WriteLine("");

			Console.WriteLine("Current Receive Address: ");
			var receiveAddress = JsonConvert.DeserializeObject<ReceiveAddress_Result>(cbc.GetCurrentReceiveAddress());
			Console.WriteLine("success: " + receiveAddress.success);
			Console.WriteLine("address: " + receiveAddress.address);
			Console.WriteLine("callback_url: " + receiveAddress.callback_url);
			Console.WriteLine("");
			
			Console.WriteLine("Generate Receive Address with Callback and Label: ");
			var genReceiveAddress2 = JsonConvert.DeserializeObject<GenerateReceiveAddress_Result>(cbc.GenerateReceiveAddress("http://www.example.com", "Sample Label"));
			Console.WriteLine("success: " + genReceiveAddress2.success);
			Console.WriteLine("address: " + genReceiveAddress2.address);
			Console.WriteLine("callback_url: " + genReceiveAddress2.callback_url);
			Console.WriteLine("label: " + genReceiveAddress2.label);
			Console.WriteLine("");

			Console.WriteLine("Current (New) Receive Address: ");
			var receiveAddress2 = JsonConvert.DeserializeObject<ReceiveAddress_Result>(cbc.GetCurrentReceiveAddress());
			Console.WriteLine("success: " + receiveAddress2.success);
			Console.WriteLine("address: " + receiveAddress2.address);
			Console.WriteLine("callback_url: " + receiveAddress2.callback_url);
			Console.WriteLine("");
			

			Console.WriteLine("Address List: ");
			var addresses = JsonConvert.DeserializeObject<Addresses_Result>(cbc.GetAddressList());
			foreach (var nestedAddress in addresses.addresses)
			{
				Console.WriteLine("address: " + nestedAddress.address.address);
				Console.WriteLine("callback_url: " + nestedAddress.address.callback_url);
				Console.WriteLine("label: " + nestedAddress.address.label);
				Console.WriteLine("created_at: " + nestedAddress.address.created_at);
				Console.WriteLine("");
			}
			Console.WriteLine("Total Count: " + addresses.total_count);
			Console.WriteLine("");

			Console.WriteLine("Create OAuth Application: ");
			var OAuthApp = JsonConvert.DeserializeObject<CreateOAuthApp_Result>(cbc.CreateOAuthApplication("Sample App444", "http://www.example.com"));
			Console.WriteLine("success: " + OAuthApp.success);
			if (Convert.ToBoolean(OAuthApp.success))
			{
				Console.WriteLine("id: " + OAuthApp.application.id);
				Console.WriteLine("created_at: " + OAuthApp.application.created_at);
				Console.WriteLine("name: " + OAuthApp.application.name);
				Console.WriteLine("redirect_uri: " + OAuthApp.application.redirect_uri);
				Console.WriteLine("client_id: " + OAuthApp.application.client_id);
				Console.WriteLine("client_secret: " + OAuthApp.application.client_secret);
				Console.WriteLine("num_users: " + OAuthApp.application.num_users);
			}
			else
			{
				foreach (var error in OAuthApp.errors)
				{
					Console.WriteLine("Error: " + error);
				}
			}
			Console.WriteLine("");
			
			Console.WriteLine("OAuth Applications List: ");
			var OAuthAppsList = JsonConvert.DeserializeObject<OAuthApps_Result>(cbc.GetOAuthApplicationsList());
			foreach (var application in OAuthAppsList.applications)
			{
				Console.WriteLine("ID: " + application.id);
				Console.WriteLine("Created At: " + application.created_at);
				Console.WriteLine("Name: " + application.name);
				Console.WriteLine("Redirect_uri: " + application.redirect_uri);
				Console.WriteLine("num_users: " + application.num_users);
				Console.WriteLine("");
			}
			Console.WriteLine("Total Count: " + OAuthAppsList.total_count);
			Console.WriteLine("Num Pages: " + OAuthAppsList.num_pages);
			Console.WriteLine("Current Page: " + OAuthAppsList.current_page);
			Console.WriteLine("");
			
			Console.WriteLine("Application Account Access Info: ");
			Console.WriteLine(cbc.GetApplicationAccountAccessInfo());
			var accountAccessInfo = JsonConvert.DeserializeObject<AccountAccess_Result>(cbc.GetApplicationAccountAccessInfo());
			Console.WriteLine("Auth Type: " + accountAccessInfo.auth_type);
			Console.WriteLine("Meta: " + accountAccessInfo.meta);
			Console.WriteLine("");

			Console.WriteLine("Create Payment Button With Default Values: ");
			Response = cbc.CreatePaymentButton("Sample Item", "0.02", "USD");
			Console.WriteLine(Response);
			Console.WriteLine("");

			var buttonResult = JsonConvert.DeserializeObject<CreateButton_Result>(Response);
			var code = buttonResult.button.code;

			Console.WriteLine("Create Order For Button: " + code);
			var buttonOrderResult = JsonConvert.DeserializeObject<CreateButtonOrder_Result>(cbc.CreateOrderForButton(code));
			Console.WriteLine("Order ID: " + buttonOrderResult.order.id);
			Console.WriteLine("Order Status: " + buttonOrderResult.order.status);
			Console.WriteLine("");

			Console.WriteLine("Purchase 0 BitCoin: ");
			Console.WriteLine(cbc.PurchaseBitcoin(0));
			Console.WriteLine("");

			Console.WriteLine("Get Email Contacts List: ");
			var contactsResult = JsonConvert.DeserializeObject<Contacts_Result>(cbc.GetEmailContactsList());
			foreach (var contact in contactsResult.contacts)
			{
				Console.WriteLine("Email: " + contact.email);
			}
			Console.WriteLine("Total Count: " + contactsResult.total_count);
			Console.WriteLine("Num Pages: " + contactsResult.num_pages);
			Console.WriteLine("Current Page: " + contactsResult.current_page);
			Console.WriteLine("");

			Console.WriteLine("Get Supported Currencies List: ");
			var currenciesResult = JsonConvert.DeserializeObject<string[][]>(cbc.GetSupportedCurrenciesList());
			foreach (var currency in currenciesResult)
			{
				Console.Write("Name: " + currency[0]);
				Console.WriteLine(" Code: " + currency[1]);
			}
			Console.WriteLine("");

			Console.WriteLine("Get BitCoin Exchange Rate: ");
			var exchangeratesResult = JsonConvert.DeserializeObject<Dictionary<String, String>>(cbc.GetBTCExchangeRate());
			foreach (var rate in exchangeratesResult)
			{
				Console.Write("Conversion: " + rate.Key);
				Console.WriteLine(" Rate: " + rate.Value);
			}
			Console.WriteLine("");

			Console.WriteLine("Get Received Merchant Orders List: ");
			Console.WriteLine(cbc.GetReceivedMerchantOrdersList());
			var merchantordersResult = JsonConvert.DeserializeObject<MerchantOrders_Result>(cbc.GetReceivedMerchantOrdersList());
			foreach (var order in merchantordersResult.orders)
			{
				Console.WriteLine("Order ID: " + order.id);
				Console.WriteLine("Status: " + order.status);
			}
			Console.WriteLine("Total: " + merchantordersResult.total_count);
			Console.WriteLine("");

			Console.WriteLine("Create New Order With Default Values: ");
			Response = cbc.CreateNewOrder("Sample Item", "0.005", "BTC");
			Console.WriteLine(Response);
			Console.WriteLine("");

			var orderResult = JsonConvert.DeserializeObject<CreateOrder_Result>(Response);
			var ID = orderResult.order.id;

			Console.WriteLine("Get Merchant Order By ID: " + ID);
			Console.WriteLine(cbc.GetMerchantOrderByID(ID));
			Console.WriteLine("");

			// Currently, this method WILL FAIL if the calling user has no payment methods defined in their account.
			// A bug has been submitted to Coinbase engineers to return json instead of throwing an error.
			Console.WriteLine("Get Associated Payment Methods: ");
			Console.WriteLine(cbc.GetAssociatedPaymentMethods());
			Console.WriteLine("");

			Console.WriteLine("Get Total Buy Price For BitCoin: ");
			Console.WriteLine(cbc.GetTotalBuyPriceForBitcoin());
			Console.WriteLine("");

			Console.WriteLine("Get Total Sell Price For BitCoin: ");
			Console.WriteLine(cbc.GetTotalSellPriceForBitcoin());
			Console.WriteLine("");

			Console.WriteLine("Get Spot Price For BitCoin: ");
			Console.WriteLine(cbc.GetSpotPriceForBitcoin());
			Console.WriteLine("");

			Console.WriteLine("Get Historical Spot Price For BitCoin: ");
			Console.WriteLine(cbc.GetHistoricalSpotPriceForBitcoin());
			Console.WriteLine("");

			Console.WriteLine("Get Recurring Payments List: ");
			Response = cbc.GetRecurringPaymentsList();
			Console.WriteLine(Response);
			Console.WriteLine("");

			var recurringPaymentsResult = JsonConvert.DeserializeObject<RecurringPayments_Result>(Response);
			foreach (RecurringPayment recurringPayment in recurringPaymentsResult.recurring_payments)
			{
				Console.WriteLine("Get Recurring Payment By ID: " + recurringPayment.id);
				Console.WriteLine(cbc.GetRecurringPaymentsList(recurringPayment.id));
			}
			Console.WriteLine("");

			Console.WriteLine("Generate CSV Report: ");
			Console.WriteLine(cbc.GenerateCSVReport("transactions", "all"));
			Console.WriteLine("");

			Console.WriteLine("Sell BitCoin: ");
			Console.WriteLine(cbc.SellBitcoin(0));
			Console.WriteLine("");

			Console.WriteLine("Get Subscribers List: ");
			Console.WriteLine(cbc.GetSubscribersList());
			Console.WriteLine("");

			Console.WriteLine("Create Token: ");
			Console.WriteLine(cbc.CreateToken());
			Console.WriteLine("");

			Console.WriteLine("Redeem Token: ");
			Console.WriteLine(cbc.RedeemToken());
			Console.WriteLine("");

			Console.WriteLine("Get Transactions List: ");
			Console.WriteLine(cbc.GetTransactionsList());
			Console.WriteLine("");

			Console.WriteLine("Send Money: ");
			Console.WriteLine(cbc.SendMoney("test@MyStatisticallyImprobableEmailAddress.com", "0"));
			Console.WriteLine("");

			Console.WriteLine("Send Invoice: ");
			Response = cbc.SendInvoice("test@MyStatisticallyImprobableEmailAddress.com");
			Console.WriteLine(Response);
			Console.WriteLine("");

			var createInvoiceResult = JsonConvert.DeserializeObject<CreateInvoice_Result>(Response);
			var InvoiceID = createInvoiceResult.transaction.id;

			Console.WriteLine("Resend Invoice: " + InvoiceID);
			Console.WriteLine(cbc.ResendInvoice(InvoiceID));
			Console.WriteLine("");

			Console.WriteLine("Cancel Money Request: ");
			Console.WriteLine(cbc.CancelMoneyRequest(InvoiceID));
			Console.WriteLine("");

			Console.WriteLine("Complete Money Request: ");
			Console.WriteLine(cbc.CompleteMoneyRequest(InvoiceID));
			Console.WriteLine("");

			Console.WriteLine("Get Transfers List: ");
			Console.WriteLine(cbc.GetTransfersList());
			Console.WriteLine("");

			Console.WriteLine("Create New User: ");
			Console.WriteLine(cbc.CreateNewUser("newuseremail@email.com", "badpassword"));
			Console.WriteLine("");

			Console.WriteLine("Get Account Settings: ");
			Response = cbc.GetAccountSettings();
			Console.WriteLine(Response);
			Console.WriteLine("");

			var accountSettingsResult = JsonConvert.DeserializeObject<AccountSettings_Result>(Response);
			foreach (User user in accountSettingsResult.users)
			{
				Console.WriteLine("Update Account Settings for: " + user.id);
				Console.WriteLine(cbc.UpdateAccountSettings(user.id, "My New Name"));
			}
			Console.WriteLine("");

			Console.WriteLine("Tests Complete.");
			Console.WriteLine("");

			Console.ReadLine();
		}
	}
}
