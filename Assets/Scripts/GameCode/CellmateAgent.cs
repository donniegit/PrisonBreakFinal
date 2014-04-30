using UnityEngine;
using System.Collections;
using System;

/*
 * Represents Guard Agent. Guards chase Player Agent.
 */
public class CellmateAgent : GameAgent {

	public CellmateAgent() {
		CellmateExploredMap = new int[GameConstants.MapWidthPixels, GameConstants.MapHeightPixels];
	}

	public void updateLocation() {
		//Get the next direction. 
		int nextDirection = GameEngine.learner.UpdateLearner ();
		this.MovingDirection = nextDirection;

		/*
		 * CHANGE THE LOCATION BASED ON THE NEW DIRECTION.
		 * SEONG TO DO : If you decide to just use the direction, please contact me. I need to change the below code
		 * such that the next direction wont screw up the update of the explored map. 
		 */
		if (nextDirection == GameConstants.Left) {
			this.LocationY--;
			GameDebugger.PrintMessage("Turning Left.");
		} else if (nextDirection == GameConstants.Right) {
			this.LocationY++;
			GameDebugger.PrintMessage("Turning Right.");
		} else if (nextDirection == GameConstants.Up) {
			this.LocationX--;
			GameDebugger.PrintMessage("Turning Up.");
		} else {
			this.LocationX++;
			GameDebugger.PrintMessage("Turning Down.");
		}
		/*
		//Check to see if we have entered the next cell. If so, update the explored map. 
		if (--distanceFromNewCell == 0) {
			distanceFromNewCell=GameConstants.TurningFloorCellHeightPixels;
			int centerX = this.LocationX;
			int centerY = this.LocationY;
			//Find the center based on the direction that we are turning in. 
			if (nextDirection == GameConstants.Left) {
				centerY -= GameConstants.TurningFloorCellHeightPixels / 2;
			} else if (nextDirection == GameConstants.Right) {
				centerY += GameConstants.TurningFloorCellHeightPixels / 2;
			} else if (nextDirection == GameConstants.Up) {
				centerX -= GameConstants.TurningFloorCellWidthPixels / 2;
			} else if (nextDirection == GameConstants.Down) {
				centerX += GameConstants.TurningFloorCellWidthPixels / 2;
			}
			//Now, update the entire cell in the explored array to one's.
			for (int i=(centerY - GameConstants.TurningFloorCellWidthPixels); i < centerY + GameConstants.TurningFloorCellWidthPixels; i++) {
				for (int j=(centerX - GameConstants.TurningFloorCellHeightPixels); j < centerX + GameConstants.TurningFloorCellHeightPixels; j++) {
					CellmateExploredMap[i,j] = 1;
				}
			}
		} */
	}

	/*
	 * Check to see if the given location is the center of a cell. 
	 */
	public bool isCenter(int locationX, int locationY) {
		bool result = false;
		int cellCenter = GameConstants.TurningFloorCellHeightPixels / 2 + 1;
		int modX = (locationX - cellCenter) % GameConstants.TurningFloorCellWidthPixels;
		int modY = (locationY - cellCenter) % GameConstants.TurningFloorCellHeightPixels;
		if (modX == 0 && modY == 0) {
			result = true;
		}
		return result;
	}

	/*Indicates whether the player has been respawned*/
	public bool Respawned { get; set; }

	/* Ones represent floor cells Agent has walked on */
	public int[,] CellmateExploredMap { get; set; }

	/*To keep track of the new cell's distance from the */
	public int distanceFromNewCell;
}

