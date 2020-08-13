using Godot;
using System;

public class MainGame : Node2D
{
	
	public static Color backgroundColor = new Color("#2b8341");
	
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

