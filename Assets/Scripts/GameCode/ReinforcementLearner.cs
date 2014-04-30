using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/*
 * Represents the knowledge of the Cellmate Agent. 
 */
public class ReinforcementLearner {
	/* Contains all floor cells that Agent can turn on. map key: unique hash value */
	Dictionary<ulong, TurningFloorCell> TurningFloorCellMap { get; set; }

	/* Previous turning cell that Agent has left */
	TurningFloorCell PrevTurningFloorCell { get; set; }

	/*Index of the previous turn cell.*/
	ulong PrevTurnFloorCellIndex{ get; set; }

	/* Previous turn direction that Agent has taken */
	public int PrevDirectionTaken { get; set; }

	/*A map sized turningfloor cell probabilities. */
	//public TurningFloorCell[,] CellMap { get; set; }

	/*variable to keep the value for the epsilon.*/
	public double epsilon;

	/*int value to set the learning rate.*/
	public double learningRate;
	/*
	 * ReinforcementLearner constructor 
	 */
	public ReinforcementLearner () {
		TurningFloorCellMap = new Dictionary<ulong, TurningFloorCell>();
		//CellMap = new TurningFloorCell[GameConstants.MapWidthPixels, GameConstants.MapHeightPixels];
		epsilon = 0.9;
		this.PrevDirectionTaken = GameConstants.Down;
		this.PrevTurnFloorCellIndex = GameMap.HashFloorCell(GameEngine.cellmate.LocationX, GameEngine.cellmate.LocationY);
		this.TurningFloorCellMap.Add(this.PrevTurnFloorCellIndex, new TurningFloorCell(GameEngine.cellmate.LocationX, GameEngine.cellmate.LocationY));
		this.PrevTurnFloorCellIndex = GameMap.HashFloorCell(GameEngine.cellmate.LocationX, GameEngine.cellmate.LocationY);
	}

	/*
	 * Updates Agents knowledge of the environment
	 * TODO: Change the code such that it takes into account the propoer initialization of the cellmate. 
	 */
	public int UpdateLearner() {
		/*
		 * Keeping the next direction to be previous. 
		 * In case, we dont reach the middle, we just have to keep stepping like before
		 */
		int nextDirection = this.PrevDirectionTaken;
		int currentX = GameEngine.cellmate.LocationX;
		int currentY = GameEngine.cellmate.LocationY;
		System.Random random = new System.Random();
		double shouldExplore = random.NextDouble ();
		//GameDebugger.PrintMessage ("Current location : " + currentX + " " + currentY);
		//Check to see if we should change direction. 
		if (this.shouldChangeDirection(GameEngine.cellmate.LocationX, GameEngine.cellmate.LocationY)) {
			/*Check to see if this is a turn cell. 
			 * If it is, find the best direction, and update the previous direction. 
			 * Also, create a new turn cell if this one didnt exist. 
			 */
			//GameDebugger.PrintMessage("Decided to change direction.");
			if (this.isTurnCell(currentX, currentY)) {
				//GameDebugger.PrintMessage("Its a turn cell.");
				//Index for the current location. 
				ulong index = GameMap.HashFloorCell(currentX, currentY);

				//Add a turning cell if it doesnt exist. 
				if(!this.TurningFloorCellMap.ContainsKey(index)) {
					this.TurningFloorCellMap.Add(index, new TurningFloorCell(currentX, currentY));
				}
				//get the next direction that should be chosen.
				//Try and explore with a certain probability, else find a probability that is random so that we can explore. 
				if (shouldExplore < GameConstants.explorationRate) {
					//try and exploit so that you can finish the whole of the map. 
					nextDirection = this.getBestDirectionTurnCell(TurningFloorCellMap[index]);
				} else {
					//Try and explore so that you can cover the whole of the map. 
					int invalidDirection = 0;
					//Find the best direction that is not the opposite of the previousdirection.
					if (this.PrevDirectionTaken == 0 || this.PrevDirectionTaken == 1) {invalidDirection = (int)(this.PrevDirectionTaken + 2);}
					if (this.PrevDirectionTaken == 2 || this.PrevDirectionTaken == 3) {invalidDirection = (int)(this.PrevDirectionTaken - 2);}
					for (int i=0; i<4; i++) {
						if (i!=invalidDirection && isDirectionValid(i, currentX, currentY)) {nextDirection = i;break;}
					}
				}
				//GameDebugger.PrintMessage("Direction chosen :" + nextDirection);
				//Also, update the previous floor cell with the probabilities based on the current cell.
				this.updatePreviousTurnCell(index, false);

				//Current values become the previous values. 
				this.PrevTurnFloorCellIndex = index;
				this.PrevDirectionTaken = nextDirection;
			} else {
				//GameDebugger.PrintMessage("Its a freaking normal cell.");
				//If its not a turning cell, we still might need to change the direction if its a dead end, or if a turn comes in the path. 
				if (!isDirectionValid(this.PrevDirectionTaken, currentX, currentY)) {
					nextDirection = this.getBestDirectionNormalCell(this.PrevDirectionTaken, currentX, currentY);
					this.PrevDirectionTaken = nextDirection;
				}
				//GameDebugger.PrintMessage("Direction chosen :" + nextDirection);
			}
			//Changed the direction. Move on. 
		} 
		//GameDebugger.PrintMessage("Updated the location and the direction.");
		return nextDirection;
	}

	/*
	 * Check if the previous direction works in the context of the current cell.
	 * Only call this function from the center of the bigger abstract entity that is 'CELL'.
	 */
	public bool isDirectionValid(int direction, int currentX, int currentY) {
		bool result = true;
		int nextCell = GameConstants.TurningFloorCellHeightPixels/2 + 1;
		int tempX = currentX;
		int tempY = currentY;
		//Update the cell location to that of the new cells starting. 
		if (direction == GameConstants.Left) {
			tempY -= nextCell;
		} else if (direction == GameConstants.Right) {
			tempY += nextCell;
		} else if (direction == GameConstants.Up) {
			tempX -= nextCell;
		} else if (direction == GameConstants.Down) {
			tempX += nextCell;
		}
		//GameDebugger.PrintMessage ("this location to be checked for a valid cell :" + tempX + " " + tempY);
		try {
		//Check if this is indeed part of the hallway. 
			if (GameMap.GameMapArray[tempX, tempY] != GameConstants.MapHallwayFloorcell  && GameMap.GameMapArray[tempX, tempY] != GameConstants.MapTurningFloorcell) {
			result = false;
			//GameDebugger.PrintMessage("The previous direction is invalid.");
		}
		} catch (IndexOutOfRangeException ee) {
			//GameDebugger.PrintMessage("Error caused due to the location : " + tempX + " " + tempY);
		}
		return result;
	}

	/*
	 * Code to update the probabilities of the previous cell. 
	 * TODO : change the code once you initialize the currentlocation properly. 
	 */
	public void updatePreviousTurnCell(ulong index, Boolean cameFromDeadEnd) {
		int currentX = TurningFloorCellMap[index].LocationX;
		int currentY = TurningFloorCellMap[index].LocationX;

		if (cameFromDeadEnd) {
			double temp  = TurningFloorCellMap[this.PrevTurnFloorCellIndex].cornerProbabilities[this.PrevDirectionTaken] = 0.0;
			//Reduce the prev probability to zero. 
			TurningFloorCellMap[this.PrevTurnFloorCellIndex].cornerProbabilities[this.PrevDirectionTaken] = 0.0;
			//Distribute that probability among other 
			for (int i=0;i<4;i++) {
				if (i != this.PrevDirectionTaken) {
					TurningFloorCellMap[this.PrevTurnFloorCellIndex].cornerProbabilities[(int)i]+=temp/3;
				}
			}
		} else {
			if (this.PrevTurnFloorCellIndex != index) {
				//Compare distances between the current cell and the previous cell. 
				int previousX = TurningFloorCellMap[this.PrevTurnFloorCellIndex].LocationX;
				int previousY = TurningFloorCellMap[this.PrevTurnFloorCellIndex].LocationY;
				if (distanceFromExit(currentX, currentY) >= distanceFromExit(previousX, previousY)) {
					//Increment all the other probabilities.   
					for (int i=0;i<4;i++) {
						TurningFloorCellMap[this.PrevTurnFloorCellIndex].cornerProbabilities[(int)i]+=0.04;
					}
					TurningFloorCellMap[this.PrevTurnFloorCellIndex].cornerProbabilities[this.PrevDirectionTaken]-=0.16;
				} else {
					//Decrememnt all the other probabilities.   
					for (int i=0;i<4;i++) {
						TurningFloorCellMap[this.PrevTurnFloorCellIndex].cornerProbabilities[i]-=0.04;
					}
					TurningFloorCellMap[this.PrevTurnFloorCellIndex].cornerProbabilities[this.PrevDirectionTaken]+=0.16;
				}
			}
		}
		//Dont let any probability fall below zero. 
		for (int i=0;i<4;i++) {
			if (TurningFloorCellMap[this.PrevTurnFloorCellIndex].cornerProbabilities[i] < 0) {
				TurningFloorCellMap[this.PrevTurnFloorCellIndex].cornerProbabilities[i]=0.0;
			}
		}
	}

	/*
	 * Get the distance of the current location from the exit.
	 */
	public double distanceFromExit(int locationX, int locationY) {
		return Math.Sqrt(((locationX-GameConstants.exitX)*(locationX-GameConstants.exitX)) + ((locationY-GameConstants.exitY)*(locationY-GameConstants.exitY)));
	}

	/*
	 * For the current turning cell, find the best direction that could be. 
	 * Decides the best path based on the epsilong greedy strategy so exploration can be controlled. 
	 */
	public int getBestDirectionTurnCell(TurningFloorCell turnCell) {
		//Compare probabilities of all the directions to see which one is best. 
		int nextDirection = GameConstants.Left;
		double bestProbability = 0.0;
		int invalidDirection = 0;
		if (this.PrevDirectionTaken == 0 || this.PrevDirectionTaken == 1) {invalidDirection = (int)(this.PrevDirectionTaken + 2);}
		if (this.PrevDirectionTaken == 2 || this.PrevDirectionTaken == 3) {invalidDirection = (int)(this.PrevDirectionTaken - 2);}
		for (int i=0;i<4;i++) {

			if ((i!=invalidDirection) && turnCell.cornerProbabilities[i] >=bestProbability && isDirectionValid(i, turnCell.LocationX, turnCell.LocationY)) {
				nextDirection = i;
				bestProbability = turnCell.cornerProbabilities[i];
			}
		}
		return nextDirection;
	}

	/*
	 * For a  normal cell, there are no choices whenit comes to choosing. 
	 * The cellmate should alwaya follow the same direction it came from. 
	 * Only in the cases of a dead end does the cellmate opt for the opposite direction that it came from. 
	 */
	public int getBestDirectionNormalCell(int previousDirection, int locationX, int locationY) {
		int result = previousDirection;
		int invalidDirection = 0;
		//Check to see if this is a dead end
		int invalidDirectionCount = 0;
		for (int i=0; i<4; i++) {
			if (!isDirectionValid(i, locationX, locationY)) {
				invalidDirectionCount++;
			} else {
				result = i;
			}
		}
		//If the invalid Direction count is 3, then we have hit a dead end, and we can return the selected direction. 
		if (invalidDirectionCount == 3) {
			//Update the previous turn cell so that it cant take this direction. 
			if (previousDirection == 0 || previousDirection == 1) {invalidDirection = (int)(previousDirection + 2);}
			if (previousDirection == 2 || previousDirection == 3) {invalidDirection = (int)(previousDirection - 2);}
			result = invalidDirection;
			this.updatePreviousTurnCell(this.PrevTurnFloorCellIndex, true);
		} else {
			//Find the best direction that is not the opposite of the previousdirection.
			if (previousDirection == 0 || previousDirection == 1) {invalidDirection = (int)(previousDirection + 2);}
			if (previousDirection == 2 || previousDirection == 3) {invalidDirection = (int)(previousDirection - 2);}
			for (int i=0; i<4; i++) {
				if (i!=invalidDirection && isDirectionValid(i, locationX, locationY)) {result = i;break;}
			}
		}
		return result;
	}

	/*
	 * Check if the current cell is a turn cell. Just ask the game map array to get the values. 
	 */
	public bool isTurnCell(int locationX, int locationY) {
		bool result = false;
		if (GameMap.GameMapArray [locationX, locationY] == GameConstants.MapTurningFloorcell) {
			result = true;
		}
		return result;
	}

	/*
	 * You should always consider changing direction when reaching the middle of a cell. Keeps thing's consistent. 
	 */
	public bool shouldChangeDirection(int locationX, int locationY) {
		bool result = false;
		int cellCenter = GameConstants.TurningFloorCellHeightPixels / 2 + 1;
		int modX = (locationX - cellCenter) % GameConstants.TurningFloorCellWidthPixels;
		int modY = (locationY - cellCenter) % GameConstants.TurningFloorCellHeightPixels;
		if (modX == 0 && modY == 0) {
			result = true;
		}
		return result;
	}

	/*
	 * Gets new location and directionfor cellmate
	 */
	public int[] GetNextLocationWithDirection(int currentLocationX, int currentLocationY) {
		// TODO: change
		int[] location = new int[3];
		// location[0] is x
		// location[1] is y
		// location[2] is direction
		return location;
	}
}
