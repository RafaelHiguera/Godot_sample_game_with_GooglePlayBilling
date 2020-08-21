using Godot;
using System;

public class Coins : Node
{
	private int numberOfCoins = 0;

	public void UpdateCoins(){
		numberOfCoins++;
		GD.Print(GetNode<RichTextLabel>("PointsCounter"));
	}

}
