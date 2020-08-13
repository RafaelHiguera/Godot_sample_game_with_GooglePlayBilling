using Godot;
using System;



public class BuyBackground : TextureButton
{
	public void _on_BuyRed_pressed()
	{
		CoinsManager.buyBackgroundWithCoins(new Color("#a02929"));
		Node button = GetParent().GetNode<TextureButton>("BuyRed");
		button.QueueFree();
		GetParent().GetNode<RichTextLabel>("CoinsCounter").SetText(CoinsManager.numberOfCoins.ToString());
			
	}
}
