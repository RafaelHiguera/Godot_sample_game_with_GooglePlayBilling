using Godot;
using System;

// Todo : use the save data to remember that the background color was bought
public class BuyBackground : TextureButton
{
	public static Color red = new Color("#a02929");
	public static Color green = new Color("#2b8341");
	
	public void _on_BuyRed_pressed()
	{
		bool result = CoinsManager.buyBackgroundWithCoins(red);
		if(result){
			Node button = GetParent().GetNode<TextureButton>("BuyRed");
			button.QueueFree();
			GetParent().GetNode<RichTextLabel>("CoinsCounter").SetText(CoinsManager.numberOfCoins.ToString());
		}
	}
}
