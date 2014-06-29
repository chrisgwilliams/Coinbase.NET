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
			var merchantordersResult = JsonConvert.DeserializeObject<MerchantOrders_Result>(cbc.GetReceivedMerchantOrdersList());
			foreach (var order in merchantordersResult.orders)
			{
				Console.WriteLine("Order ID: " + order.order.id);
				Console.WriteLine("Status: " + order.order.status);
			}
			Console.WriteLine("Total: " + merchantordersResult.total_count);
			Console.WriteLine("");

			Console.WriteLine("Create New Order With Default Values: ");
			var createorderResult = JsonConvert.DeserializeObject<CreateOrder_Result>(cbc.CreateNewOrder("Sample Item", "0.005", "BTC"));
			var ID = createorderResult.order.id;
			Console.WriteLine("Order ID: " + ID);
			Console.WriteLine("Receive Address: " + createorderResult.order.receive_address);
			Console.WriteLine("Success: " + createorderResult.success);
			Console.WriteLine("");
			
			Console.WriteLine("Get Merchant Order By ID: " + ID);
			var response = cbc.GetMerchantOrderByID(ID);
			var merchantorderResult = JsonConvert.DeserializeObject<MerchantOrder_Result>(response);
			// Coinbase returns an error message if the order is null. The format is completely different than the standard response.
			if (merchantorderResult.order != null)
			{
				Console.WriteLine("ID: " + merchantorderResult.order.id);
				Console.WriteLine("Status: " + merchantorderResult.order.status);
				Console.WriteLine("Receive Address: " + merchantorderResult.order.receive_address);
			}
			else
			{
				var error = JsonConvert.DeserializeObject<Error>(response);
				Console.WriteLine("Success: " + error.success);
				Console.WriteLine("Error: " + error.error);
			}
			Console.WriteLine("");

			// Currently, this method WILL FAIL if the calling user has no payment methods defined in their account. A
			// bug has been submitted to Coinbase engineers to return json instead of throwing an error. Until then, be
			// sure to use a TRY/Catch clause to prevent your app from blowing up.
			Console.WriteLine("Get Associated Payment Methods: ");
			var paymentmethodsResult = JsonConvert.DeserializeObject<PaymentMethods_Result>(cbc.GetAssociatedPaymentMethods());
			foreach (var method in paymentmethodsResult.payment_methods)
			{
				Console.WriteLine("ID: " + method.payment_method.id);
				Console.WriteLine("Name: " + method.payment_method.name);
				Console.WriteLine("Buy? " + method.payment_method.can_buy);
				Console.WriteLine("Sell? " + method.payment_method.can_sell);
			}
			Console.WriteLine("Default Buy: " + paymentmethodsResult.default_buy);
			Console.WriteLine("Default Sell: " + paymentmethodsResult.default_sell);
			Console.WriteLine("");

			Console.WriteLine("Get Total Buy Price For BitCoin: ");
			var buypriceResult = JsonConvert.DeserializeObject<BuyPrice_Result>(cbc.GetTotalBuyPriceForBitcoin());
			Console.WriteLine("Subtotal: " + buypriceResult.subtotal.amount + " " + buypriceResult.subtotal.currency);
			Console.WriteLine("Coinbase Fee: " + buypriceResult.fees[0].coinbase.amount + " " + buypriceResult.fees[0].coinbase.currency);
			Console.WriteLine("Bank Fee: " + buypriceResult.fees[1].bank.amount + " " + buypriceResult.fees[1].bank.currency);
			Console.WriteLine("Total: " + buypriceResult.total.amount + " " + buypriceResult.total.currency);
			Console.WriteLine("");

			Console.WriteLine("Get Total Sell Price For BitCoin: ");
			var sellpriceResult = JsonConvert.DeserializeObject<SellPrice_Result>(cbc.GetTotalSellPriceForBitcoin());
			Console.WriteLine("Subtotal: " + sellpriceResult.subtotal.amount + " " + sellpriceResult.subtotal.currency);
			Console.WriteLine("Coinbase Fee: " + sellpriceResult.fees[0].coinbase.amount + " " + sellpriceResult.fees[0].coinbase.currency);
			Console.WriteLine("Bank Fee: " + sellpriceResult.fees[1].bank.amount + " " + sellpriceResult.fees[1].bank.currency);
			Console.WriteLine("Total: " + sellpriceResult.total.amount + " " + sellpriceResult.total.currency);
			Console.WriteLine("");

			Console.WriteLine("Get Spot Price For BitCoin: ");
			var spotpriceResult = JsonConvert.DeserializeObject<SpotPrice_Result>(cbc.GetSpotPriceForBitcoin());
			Console.WriteLine("Spot Price: " + spotpriceResult.amount + " " + spotpriceResult.currency);
			Console.WriteLine("");

			// This method returns a CSV feed and does not deserialize.
			Console.WriteLine("Get Historical Spot Price For BitCoin: ");
			Console.WriteLine(cbc.GetHistoricalSpotPriceForBitcoin());
			Console.WriteLine("");

			Console.WriteLine("Get Recurring Payments List: ");
			var recurringPaymentsResult = JsonConvert.DeserializeObject<RecurringPayments_Result>(cbc.GetRecurringPaymentsList());
			foreach (RecurringPaymentAsCustomer recurringPayment in recurringPaymentsResult.recurring_payments)
			{
				Console.WriteLine("Get Recurring Payment By ID: " + recurringPayment.id);
				Console.WriteLine(cbc.GetRecurringPaymentsList(recurringPayment.id));
			}
			Console.WriteLine("");

			Console.WriteLine("Get List Of CSV Reports: ");
			var reportsListResult = JsonConvert.DeserializeObject<Reports_Result>(cbc.GetReportsList());
			foreach (NestedReport report in reportsListResult.reports)
			{
				Console.WriteLine("Report ID: " + report.report.id);
				Console.WriteLine("Sent To: " + report.report.email);
				Console.WriteLine("Report Type: " + report.report.type);
			}
			Console.WriteLine("");

			Console.WriteLine("Generate CSV Report: ");
			var reportResult = JsonConvert.DeserializeObject<GenerateReport_Result>(cbc.GenerateCSVReport("test@test.com", "transactions"));
			var reportID = reportResult.report.id;
			Console.WriteLine("ID: " + reportID);
			Console.WriteLine("Sent To: " + reportResult.report.email);
			Console.WriteLine("Report Type: " + reportResult.report.type);
			Console.WriteLine("");

			Console.WriteLine("Get CSV Report by ID: " + reportID);
			//var reportdetailsResult = JsonConvert.DeserializeObject<ReportDetails_Result>(cbc.GetReportByID(reportID));
			//Console.WriteLine("Report ID: " + reportdetailsResult.report.id);
			//Console.WriteLine("Sent To: " + reportdetailsResult.report.email);
			//Console.WriteLine("Report Type: " + reportdetailsResult.report.type);
			Console.WriteLine("");

			Console.WriteLine("Sell BitCoin: ");
			var sellbitcoinResult = JsonConvert.DeserializeObject<SellBitcoin_Result>(cbc.SellBitcoin(0));
			Console.WriteLine("Success: " + sellbitcoinResult.success);
			Console.WriteLine("Transfer Type: " + sellbitcoinResult.transfer.type);
			Console.WriteLine("Transfer Code: " + sellbitcoinResult.transfer.code);
			Console.WriteLine("Total Amount: " + sellbitcoinResult.transfer.total.cents + " " + sellbitcoinResult.transfer.total.currency_iso);
			Console.WriteLine("");

			Console.WriteLine("Get Subscribers List: ");
			var subscriberlistResult = JsonConvert.DeserializeObject<SubscribersList_Result>(cbc.GetSubscribersList());
			String subscriberID = "";
			foreach (RecurringPaymentAsMerchant recurringPayment in subscriberlistResult.recurring_payments)
			{
				subscriberID = recurringPayment.id;
				Console.WriteLine("ID: " + subscriberID);
				Console.WriteLine("Name: " + recurringPayment.button.name);
				Console.WriteLine("Desc: " + recurringPayment.button.description);
				Console.WriteLine("");
			}

			Console.WriteLine("Get Subscribers By ID: " + subscriberID);
			var subscriber = JsonConvert.DeserializeObject<Subscriber_Result>(cbc.GetSubscribersList(subscriberID));
			if (subscriber.recurring_payment != null)
			{
				Console.WriteLine("ID: " + subscriber.recurring_payment.id);
				Console.WriteLine("Name: " + subscriber.recurring_payment.button.name);
				Console.WriteLine("Desc: " + subscriber.recurring_payment.button.description);
				Console.WriteLine("");
			}
			Console.WriteLine("");

			Console.WriteLine("Create Token: ");
			var createtokenResult = JsonConvert.DeserializeObject<CreateToken_Result>(cbc.CreateToken());
			var tokenID = createtokenResult.token.token_id;
			Console.WriteLine("Token ID: " + tokenID);
			Console.WriteLine("Success: " + createtokenResult.success);
			Console.WriteLine("");

			Console.WriteLine("Redeem Token: ");
			var redeemtokenResult = JsonConvert.DeserializeObject<RedeemToken_Result>(cbc.RedeemToken(tokenID));
			Console.WriteLine("Success: " + redeemtokenResult.success);
			Console.WriteLine("");

			Console.WriteLine("Get Transactions List: ");
		    var gettransactionsResult = JsonConvert.DeserializeObject<GetTransactionList_Result>(cbc.GetTransactionsList());
            Console.WriteLine("Name: " + gettransactionsResult.current_user.name);
            Console.WriteLine("ID: " + gettransactionsResult.current_user.id);
            Console.WriteLine("Balance: " + gettransactionsResult.balance.amount + ' ' + gettransactionsResult.balance.currency);
			string transactionID = "";
			foreach (NestedTransaction transaction in gettransactionsResult.transactions)
			{
				transactionID = transaction.transaction.id;
				Console.WriteLine("--Transaction ID: " + transactionID);
                Console.WriteLine("--Created: " + transaction.transaction.created_at);
                Console.WriteLine("--Sender: " + transaction.transaction.sender.name);
                Console.WriteLine("--Recipient: " + transaction.transaction.recipient.name);
				Console.WriteLine("");
		    }
			Console.WriteLine("");

			Console.WriteLine("Show Transaction Details: ");
			var transactiondetailsResult = JsonConvert.DeserializeObject<TransactionDetails_Result>(cbc.GetTransactionsList(transactionID));
			Console.WriteLine("Recipient: " + transactiondetailsResult.transaction.recipient.name);
			Console.WriteLine("Sender: " + transactiondetailsResult.transaction.sender.name);
			Console.WriteLine("ID: " + transactiondetailsResult.transaction.id);
			Console.WriteLine("");

			Console.WriteLine("Send Money: ");
			var sendmoneyResult = JsonConvert.DeserializeObject<SendMoney_Result>(cbc.SendMoney("test@test.com", "0"));
			Console.WriteLine("Success: " + sendmoneyResult.success);
			Console.WriteLine("Sender: " + sendmoneyResult.transaction.sender.name);
			Console.WriteLine("Recipient: " + sendmoneyResult.transaction.recipient.name);
			Console.WriteLine("");

			Console.WriteLine("Send Invoice: ");
			var createinvoiceResult = JsonConvert.DeserializeObject<CreateInvoice_Result>(cbc.SendInvoice("test@test.com","0.001"));
			Console.WriteLine("Success: " + createinvoiceResult.success);
			Console.WriteLine("Sender: " + createinvoiceResult.transaction.sender);
			Console.WriteLine("Recipient: " + createinvoiceResult.transaction.recipient);
			var InvoiceID = createinvoiceResult.transaction.id;
			Console.WriteLine("");

			Console.WriteLine("Resend Invoice: " + InvoiceID);
			var resendResult = JsonConvert.DeserializeObject<ResendInvoice_Result>(cbc.ResendInvoice(InvoiceID));
			Console.WriteLine("Success: " + resendResult.success);
			Console.WriteLine("");

			Console.WriteLine("Cancel Money Request: " + InvoiceID);
			var cancelrequestResult = JsonConvert.DeserializeObject<CancelRequest_Result>(cbc.CancelMoneyRequest(InvoiceID));
			Console.WriteLine("Success: " + cancelrequestResult.success);
			Console.WriteLine("");

			Console.WriteLine("Complete Money Request: ");
			var completerequestResult = JsonConvert.DeserializeObject<CompleteRequest_Result>(cbc.CompleteMoneyRequest(InvoiceID));
			Console.WriteLine("Success: " + completerequestResult.success);
			Console.WriteLine("");

			Console.WriteLine("Get Transfers List: ");
			var transfersResult = JsonConvert.DeserializeObject<Transfers_Result>(cbc.GetTransfersList());
			foreach (var transfers in transfersResult.transfers)
			{
				Console.WriteLine("Transaction ID: " + transfers.transfer.transaction_id);
				Console.WriteLine("Type: " + transfers.transfer.type);
				Console.WriteLine("Created At: " + transfers.transfer.created_at);
			}
			Console.WriteLine("");

			Console.WriteLine("Create New User: ");
			var createuserResult = JsonConvert.DeserializeObject<CreateUser_Result>(cbc.CreateNewUser("newuseremail@email.com", "badpassword"));
			Console.WriteLine("Success: " + createuserResult.success);
			Console.WriteLine("Name: " + createuserResult.user.name);
			Console.WriteLine("");

			Console.WriteLine("Get Account Settings: ");
			var accountsettingsResult = JsonConvert.DeserializeObject<AccountSettings_Result>(cbc.GetAccountSettings());
			foreach (User user in accountsettingsResult.users)
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
