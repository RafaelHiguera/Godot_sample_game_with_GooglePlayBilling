using GodotGooglePlayBilling;
using Godot;
using System.Linq;
using System;

// Todo : Review Play Billing logic
public class PurchaseManager : Node
{
	private GooglePlayBilling _payment;
	private string _testItemPurchaseToken;
	const string COINS_20_ID = "20_coins";
	
	public PurchaseManager(){
		_payment = new GooglePlayBilling();
		if (_payment.IsAvailable) {
			// No params.
			_payment.Connect(nameof(GooglePlayBilling.Connected), this, nameof(OnConnected));
			// No params.
			_payment.Connect(nameof(GooglePlayBilling.Disconnected), this, nameof(OnDisconnected));
			// Response ID (int), Debug message (string).
			_payment.Connect(nameof(GooglePlayBilling.ConnectError), this, nameof(OnConnectError));
			// Purchases (Dictionary[]).
			_payment.Connect(nameof(GooglePlayBilling.PurchasesUpdated), this, nameof(OnPurchasesUpdated));
			// Response ID (int), Debug message (string).
			_payment.Connect(nameof(GooglePlayBilling.PurchaseError), this, nameof(OnPurchaseError));
			// SKUs (Dictionary[]).
			_payment.Connect(nameof(GooglePlayBilling.SkuDetailsQueryCompleted), this, nameof(OnSkuDetailsQueryCompleted));
			// Response ID (int), Debug message (string), Queried SKUs (string[]).
			_payment.Connect(nameof(GooglePlayBilling.SkuDetailsQueryError), this, nameof(OnSkuDetailsQueryError));
			// Purchase token (string).
			_payment.Connect(nameof(GooglePlayBilling.PurchaseAcknowledged), this, nameof(OnPurchaseAcknowledged));
			// Response ID (int), Debug message (string), Purchase token (string).
			_payment.Connect(nameof(GooglePlayBilling.PurchaseAcknowledgementError), this, nameof(OnPurchaseAcknowledgementError));
			// Purchase token (string).
			_payment.Connect(nameof(GooglePlayBilling.PurchaseConsumed), this, nameof(OnPurchaseConsumed));           

			_payment.StartConnection();
		} 
		else
		{
			ShowAlert("Android IAP support is not enabled. Make sure you have enabled 'Custom Build' and installed and enabled the GodotGooglePlayBilling plugin in your Android export settings! This application will not work.");
		}
	}
	
	private void _on_Buy20Coins_pressed()
	{
		var response = _payment.Purchase(COINS_20_ID);
		if (response != null && response.Status != (int)Error.Ok)
		{
			ShowAlert($"Purchase error {response.ResponseCode} {response.DebugMessage}");
		} 
		else {
			CoinsManager.numberOfCoins = 20;	
		}	
	}

	private void OnConnected()
	{
		GD.Print("PurchaseManager connected");
	
		_payment.QuerySkuDetails(new string[] { COINS_20_ID }, PurchaseType.InApp); // Use "subs" for subscriptions.

		// We must acknowledge all puchases.
		// See https://developer.android.com/google/play/billing/integrate#process for more information
		var purchasesResult = _payment.QueryPurchases(PurchaseType.InApp);
		if (purchasesResult.Status == (int)Error.Ok)
		{
			foreach (var purchase in purchasesResult.Purchases)
			{
				if (!purchase.IsAcknowledged)
				{
					GD.Print($"Purchase {purchase.Sku} has not been acknowledged. Acknowledging...");
					_payment.AcknowledgePurchase(purchase.PurchaseToken);
				}
			}
		}
		else
		{
			GD.Print($"Purchase query failed: {purchasesResult.ResponseCode} - {purchasesResult.DebugMessage}");
		}
	}

	private async void OnDisconnected()
	{
		ShowAlert("GodotGooglePlayBilling disconnected. Will try to reconnect in 10s...");
		await ToSignal(GetTree().CreateTimer(10), "timeout");
		_payment.StartConnection();
	}

	private void OnConnectError()
	{
		ShowAlert("PurchaseManager connect error");
	}

	private void OnPurchasesUpdated(Godot.Collections.Array arrPurchases)
		{
		GD.Print($"Purchases updated: {JSON.Print(arrPurchases)}");

		// See OnConnected
		var purchases = GooglePlayBillingUtils.ConvertPurchaseDictionaryArray(arrPurchases);

		foreach (var purchase in purchases)
		{
			if (!purchase.IsAcknowledged)
			{
				GD.Print($"Purchase {purchase.Sku} has not been acknowledged. Acknowledging...");
				_payment.AcknowledgePurchase(purchase.PurchaseToken);
			}
		}

		if (purchases.Length > 0)
		{
			_testItemPurchaseToken = purchases.Last().PurchaseToken;
		}
	}

	private void OnPurchaseError(int code, string message)
	{
		ShowAlert($"Purchase error {code}: {message}");
	}

	private void OnSkuDetailsQueryCompleted(Godot.Collections.Array arrSkuDetails)
	{
		ShowAlert(JSON.Print(arrSkuDetails));

		var skuDetails = GooglePlayBillingUtils.ConvertSkuDetailsDictionaryArray(arrSkuDetails);
		foreach (var skuDetail in skuDetails)
		{
			GD.Print($"Sku {skuDetail.Sku}");
		}
	}
	private void OnSkuDetailsQueryError(int code, string message)
	{
		ShowAlert($"SKU details query error {code}: {message}");
	}

	private void OnPurchaseAcknowledged(string purchaseToken)
	{
		ShowAlert($"Purchase acknowledged: {purchaseToken}");
	}

	private void OnPurchaseAcknowledgementError(int code, string message)
	{
		ShowAlert($"Purchase acknowledgement error {code}: {message}");
	}

	private void OnPurchaseConsumed(string purchaseToken)
	{
		ShowAlert($"Purchase consumed successfully: {purchaseToken}");
	}
	
	private void ShowAlert(string text)
	{
	  GD.Print(text);
	}
}



