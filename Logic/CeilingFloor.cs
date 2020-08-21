using Godot;
using System;

public class CeilingFloor : Area2D
{
	[Export]
	private int _bounceDirection = 1;

	public void OnAreaEntered(Area2D area)
	{
		if (area is Ball ball)
		{
			ball.direction = (ball.direction + new Vector2(0, _bounceDirection)).Normalized();
		}
	}
	
	public void OnAreaEnteredLeftWall(Area2D area)
	{
		if (area is Ball ball)
		{
			ball.direction = new Vector2(1, ((float)new Random().NextDouble()) * 2 - 1).Normalized();
		}
	}
}




