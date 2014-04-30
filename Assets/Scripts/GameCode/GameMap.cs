using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/*
 * Represents 2D game map
 */
//[System.Serializable]
public static class GameMap {
	/* Contains 2D floor cells with type values (0, 2, 3, ????) */
	public static int[,] GameMapArray;

	/* Contains distance each floor cell is to the exit (key: hash value of floor cell, value: distance to exit) */
	public static Dictionary<int, int> FloorCellToExitDistanceMap;

	/*
	 * Creates map info
	 */
	public static void CreateMap() {
		GameMapArray = new int[GameConstants.MapWidthPixels, GameConstants.MapHeightPixels];
		FloorCellToExitDistanceMap = new Dictionary<int, int>();
		// Map size (dimensions) - GameConstants class
		// Floor cell size (dimensions) - GameConstants class
		// Generate unity 3D map (coins, prison cells, floor, walls, start, and exit)
		// Initialize 2D map array (indexed position is a floor cell)
		// Initialize reinforcement learning turning floor cell positions (TurningFloorCell class)
		// Initialize each cell distance from exit
		// Initialize floor sensor locations (which floor cells will have sensors)
		int index = 0;
		try{
			TextAsset textFile = Resources.Load("text/result5") as TextAsset;
			StringReader sr = new StringReader(textFile.text);
			string source = sr.ReadLine();
			Debug.Log (source.Length);
			while(source != null){
				for(int j = 0; j < source.Length; j++){
					GameMapArray[index,j] = (int)char.GetNumericValue(source[j]);
				}
				index++;
				source = sr.ReadLine();
			}
		}catch(System.Exception ee){
			Debug.Log("Exception");
			return;
		}
	}

	/*
	 * Creates unique value for floor cell
	 * 
	 * TODO: does this need to return a long?
	 */
	public static ulong HashFloorCell(int locationX, int locationY) {
		//Getting the hash value for the given location. 
		return (ulong)(((ulong)locationX)*((ulong)27191) + ((ulong)locationY));
	}

}
