using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class UIGameMap {

	/* Contains 2D floor cells with type values (0, 2, 3, ????) */
	public int[,] UIGameMapArray;
		
	/*
	 * Creates map info
	 */
	public int[,] CreateMap() {
		UIGameMapArray = new int[GameConstants.MapWidthPixels, GameConstants.MapHeightPixels];
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
					UIGameMapArray[index,j] = (int)char.GetNumericValue(source[j]);
				}
				index++;
				source = sr.ReadLine();
			}
		}catch(System.Exception ee){
			Debug.Log("Exception");
			return UIGameMapArray;
		}
		return UIGameMapArray;
	}
}
