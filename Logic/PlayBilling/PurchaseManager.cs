using GodotGooglePlayBilling;
using Godot;
using System.Linq;
using System;

public class PurchaseManager : Node
{
	private GooglePlayBilling _payment;

	private string _testItemPurchaseToken;
	
	public PurchaseManager(){
		_payment = new GooglePlayBilling();
		_payment.StartConnection();
	}
	
	private void _on_Buy20Coins_pressed()
	{
		var response = _payment.Purchase("20_coins");
		CoinsManager.numberOfCoins = 100;		
	}

	private void OnConnected()
	{
		GD.Print("PurchaseManager connected");

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
		await ToSignal(GetTree().CreateTimer(10), "timeout");
		_payment.StartConnection();
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

	

	private void OnSkuDetailsQueryCompleted(Godot.Collections.Array arrSkuDetails)
	{


		var skuDetails = GooglePlayBillingUtils.ConvertSkuDetailsDictionaryArray(arrSkuDetails);
		foreach (var skuDetail in skuDetails)
		{
			GD.Print($"Sku {skuDetail.Sku}");
		}
	}

	private void OnPurchaseButton_pressed()
	{
		var response = _payment.Purchase("TestItemSku");
	}

	private void OnConsumeButton_pressed()
	{

		_payment.ConsumePurchase(_testItemPurchaseToken);
	}
}



