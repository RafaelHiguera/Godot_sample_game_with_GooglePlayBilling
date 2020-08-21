using Godot;
using System;

public class Store : Node2D
{
	private bool isPlayBillingInitialize = false;
	
	public override void _Ready()
	{
		if(!isPlayBillingInitialize){
			new PurchaseManager();
			isPlayBillingInitialize = true;
		}
		GetNode<RichTextLabel>("CoinsCounter").SetText(CoinsManager.numberOfCoins.ToString());
	}
	
	public void _on_BackButton_pressed()
	{
		GetTree().ChangeScene("res://pong.tscn");
	}
}



