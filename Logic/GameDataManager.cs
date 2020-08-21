using Godot;
using System;

// Todo : Implement logic for loading information to the game, like setting the backgrounds
class GameDataManager 
{
	private const int STARTING_NUMBER_OF_COINS = 20;
	private const string STARTING_BACKGROUND = "green";
	private static Godot.Collections.Dictionary<string, object> values = new Godot.Collections.Dictionary<string, object>();
	
	public static void SaveGameData(){
		SaveNumberOfCoins();
		var saveGame = new File();
		saveGame.Open("user://game.dat", File.ModeFlags.Write);
		saveGame.StoreLine(JSON.Print(values));
		saveGame.Close();
	}
	
	public static void LoadGameData(){
		var saveGame = new File();
		if (!saveGame.FileExists("user://game.dat")){
			LoadStandardGameData();
			return;
		}
		saveGame.Open("user://game.dat", File.ModeFlags.Read);
		values = new Godot.Collections.Dictionary<string, object>((Godot.Collections.Dictionary)JSON.Parse(saveGame.GetLine()).Result);
		CoinsManager.numberOfCoins = int.Parse(values["numberOfCoins"].ToString());
		saveGame.Close();
	}
	
	public static void AddBackground(string backgroundId){
		values.Add(backgroundId, true);
	}
	
	private static void LoadStandardGameData(){
		CoinsManager.numberOfCoins = STARTING_NUMBER_OF_COINS;
		AddBackground(STARTING_BACKGROUND);
	}
	
	private static void SaveNumberOfCoins(){
		values.Add("numberOfCoins", CoinsManager.numberOfCoins);
	}
}
