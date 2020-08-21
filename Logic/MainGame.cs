using Godot;
using System;

public class MainGame : Node2D
{
	public static Color backgroundColor = BuyBackground.green;
	private static bool gameStartUp = false;
	
	public override void _Ready()
	{
		GetNode<ColorRect>("Background").SetFrameColor(backgroundColor);
		GetNode<RichTextLabel>("CoinsCounter").SetText(CoinsManager.numberOfCoins.ToString());
	}
	
	public void _on_StoreButton_pressed()
	{
		GetTree().ChangeScene("res://Store.tscn");
	}
}

