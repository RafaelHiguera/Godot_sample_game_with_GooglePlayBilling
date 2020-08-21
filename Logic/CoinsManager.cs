using Godot;
using System;

static class CoinsManager
{
	public static int numberOfCoins = 0;
	private static int BACKGROUND_PRICE_IN_COINS = 20;
	
	public static bool buyBackgroundWithCoins(Color color){
		if(numberOfCoins < BACKGROUND_PRICE_IN_COINS)
			return false;
		
		numberOfCoins -= BACKGROUND_PRICE_IN_COINS;
		MainGame.backgroundColor = color;
		
		return true;
	}
}
