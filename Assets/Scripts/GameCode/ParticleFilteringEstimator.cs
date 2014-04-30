using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/*
 * Represents all GuardAgents knowledge
 */
using System.Linq;


public class ParticleFilteringEstimator {
	/* Particle */
	public List<Particle> Particles { get; set; }
	
	/* Probability of PlayerAgent being at a GameMap xy location (noise caused by RatAgents) */
	public static int[,] FloorCellProbabilities { get; set; }
	
	/* Contains GameMap positions that contain floor sensors (hash value of floor cell)   */
	List<ulong> FloorSensors;
	
	/* Contains GameMap positions that a respective particle have covered (key: hash of particle number, locationX, and locationY)  */
	List<ulong> FloorcellPartcileCover;
	
	public ulong ParticleUniqueIdCount { get; set; }
	
	
	public List<Particle> particlesToRemove { get; set; }
	public List<Particle> particlesToAdd { get; set; }
	
	public int NumberOfParticlesSetOff { get; set; }



	public static int[,] NewFloorCellProbabilities { get; set; }
	System.Random random = new System.Random ();
	public static int AdditionalRadius { get; set; }

	public int Counter { get; set; }


	/*
	 * Construtor
	 */
	public ParticleFilteringEstimator () {
		Particles = new List<Particle>();
		FloorCellProbabilities = new int[GameConstants.MapWidthPixels, GameConstants.MapHeightPixels];
		FloorSensors = new List<ulong>();
		FloorcellPartcileCover = new List<ulong>();
		particlesToRemove = new List<Particle> ();
		particlesToAdd = new List<Particle> ();
	}
	
	/*
	 * Updates Agents knowledge of the environment
	 */	
	public void UpdateEstimator() {
		UpdateParticleSpace (GameEngine.player.LocationX, GameEngine.player.LocationY);
		//GameDebugger.PrintArray (0, "Probs", ParticleFilteringEstimator.NewFloorCellProbabilities);
		Counter++;

		if (Counter % 50 == 0 && AdditionalRadius > 0) {
			Counter = 0;
			--AdditionalRadius;
		}
	}

	public void UpdateParticleSpace(int cx, int cy) {
		double step = (Math.PI * 2.0) / 100.0;
		
		NewFloorCellProbabilities = new int[100, 100];
		for (int r = 1; r<(AdditionalRadius+GameConstants.ParticleFilterRadius); r++) {
			int randomN = random.Next(0,4);
			for (double theta = 0; theta<(2.0*Math.PI); theta += step) {
				double x = cx+random.Next(0,4) + r*Math.Cos (theta);
				double y = cy+random.Next(0,4) + r*Math.Sin (theta);
				
				if (x >= 0 && x < 100 && y >= 0 && y < 100) {
					if (GameMap.GameMapArray[(int)x,(int)y] == GameConstants.MapHallwayFloorcell || GameMap.GameMapArray[(int)x,(int)y] == GameConstants.MapTurningFloorcell) {
						NewFloorCellProbabilities[(int)x,(int)y] = 51-r;
					}
				}
			}
		}
	}
	
	/*
	 * Adds a floor sensor to GameMap
	 * 
	 * locationX - x coordinate of where to place floor sensor
	 * locationY - y coordinate of where to place floor floor sensor
	 * 
	 * Returns true if floor sensor did not exist prior otherwise returns true
	 */	
	public bool AddFloorSensor(int locationX, int locationY){
		ulong hash = GameMap.HashFloorCell (locationX, locationY);
		if (!FloorSensors.Contains(hash)) {
			FloorSensors.Add(hash);
			return true;
		}
		
		return false;
	}
	
	/*
	 * Determines if floor sensor is at given location
	 * 
	 * locationX - x coordinate of where to check for floor sensor
	 * locationY - y coordinate of where to check for floor sensor
	 */	
	public bool IsFloorSensorAtLocation(int locationX, int locationY){
		ulong hash = GameMap.HashFloorCell (locationX, locationY);
		if (FloorSensors.Contains(hash)) {
			return true;
		}
		
		return false;
	}
	
	/*
	 * Determines where PlayerAgent is most likely to be within range of Guard's limited sight.
	 * 
	 * agentId - Uniquely identifies Agent type with constants from GameConstants
	 * locationX - x coordinate of where to create particle
	 * locationY - y coordinate of where to create particle
	 */	
	public void CreateParticle(int agentId, int locationX, int locationY) {
		/*int value = 0;
		if (agentId == GameConstants.PlayerAgentId) {
			value = GameConstants.PlayerAgentParticleValue;
		} else if (agentId == GameConstants.RatAgentId) {
			value = GameConstants.RatAgentParticleValue;
		}
		
		// add up, down, left, right particle directions
		Particle newParticle = new Particle (GameConstants.Up, locationX, locationY, value, GameConstants.ParticleLifeSpan, true, ++ParticleUniqueIdCount);
		Particles.Add (newParticle);
		++NumberOfParticlesSetOff;
		
		AddParticleToCoverHash (newParticle, newParticle.CurrentLocationX, newParticle.CurrentLocationY);*/
		if (agentId == GameConstants.PlayerAgentId) {
			++AdditionalRadius;
		}

		if (agentId == GameConstants.RatAgentId) {
			//
		}
	}
	
	/*
	 * Determines where PlayerAgent is most likely to be within range of Guard's limited sight.
	 * 
	 * Returns xy location value in int[0] = x, int[1] = y, int[2] = random or not,  int[3] = direction,  int[4] = tracking value
	 */	
	public int[] GetGuardTargetLocation(GuardAgent guard, bool canGoUp, bool canGoDown, bool canGoLeft, bool canGoRight) {
		int[] points = new int[5];
		points [3] = guard.MovingDirection;

		double closestDistance = 32000;
		double distance = 0;
		int xToSave = -1;
		int yToSave = -1;
		
		int startX = 0;
		int startY = 0;
		int endX = 0;
		int endY = 0;

		int xAddValue = 0;
		int yAddValue = 0;

		if (guard.LocationX < GameEngine.player.LocationX && guard.LocationY < GameEngine.player.LocationY) {
			startX = guard.LocationX-(GameConstants.GuardSearchDistancePixels/2);
			startY = guard.LocationY-(GameConstants.GuardSearchDistancePixels/2);
			endX = guard.LocationX+(GameConstants.GuardSearchDistancePixels/2);
			endY = guard.LocationY+(GameConstants.GuardSearchDistancePixels/2);
			xAddValue = 1;
			yAddValue = 1;
		}
		if (guard.LocationX > GameEngine.player.LocationX && guard.LocationY < GameEngine.player.LocationY) {
			endX = guard.LocationX-(GameConstants.GuardSearchDistancePixels/2);
			startY = guard.LocationY-(GameConstants.GuardSearchDistancePixels/2);
			startX = guard.LocationX+(GameConstants.GuardSearchDistancePixels/2);
			endY = guard.LocationY+(GameConstants.GuardSearchDistancePixels/2);
			xAddValue = -1;
			yAddValue = 1;
		}
		if (guard.LocationX < GameEngine.player.LocationX && guard.LocationY > GameEngine.player.LocationY) {
			startX = guard.LocationX-(GameConstants.GuardSearchDistancePixels/2);
			endY = guard.LocationY-(GameConstants.GuardSearchDistancePixels/2);
			endX = guard.LocationX+(GameConstants.GuardSearchDistancePixels/2);
			startY = guard.LocationY+(GameConstants.GuardSearchDistancePixels/2);
			xAddValue = 1;
			yAddValue = -1;
		}
		if (guard.LocationX > GameEngine.player.LocationX && guard.LocationY > GameEngine.player.LocationY) {
			endX = guard.LocationX-(GameConstants.GuardSearchDistancePixels/2);
			endY = guard.LocationY-(GameConstants.GuardSearchDistancePixels/2);
			startX = guard.LocationX+(GameConstants.GuardSearchDistancePixels/2);
			startY = guard.LocationY+(GameConstants.GuardSearchDistancePixels/2);
			xAddValue = -1;
			yAddValue = -1;
		}
		

		//Debug.Log ("startX:"+startX+", endX:"+endX+", xAddValue:"+xAddValue+", startY:"+startY+", endY:"+endY+", yAddValue:"+yAddValue);

		/*if ((guard.MovingDirection == GameConstants.Up && !canGoUp) 
		    || (guard.MovingDirection == GameConstants.Down && !canGoDown) 
		    || (guard.MovingDirection == GameConstants.Left && !canGoLeft) 
		    || (guard.MovingDirection == GameConstants.Right && !canGoRight)) { */
		if (guard.OnTurningFloorcell && !guard.MadeTurnChoice) {	
		    // get closest higher value than current position
			for (int x = startX; x != endX; x += xAddValue) {
				for (int y = startY; y != endY; y += yAddValue) {
					distance = Math.Sqrt (Math.Pow(guard.LocationX-x,2) + Math.Pow(guard.LocationY-y,2));

					if (x >= 0 && x < 100 && y >= 0 && y < 100 && NewFloorCellProbabilities[x,y] > guard.TrackingValue) {
						xToSave = x;
						yToSave = y;
						closestDistance = distance;
						points[4] = NewFloorCellProbabilities[x,y];

						//Debug.Log ("Before: "+guard.TrackingValue);
						//Debug.Log ("After: "+NewFloorCellProbabilities[x,y]);
					}
				}
			}
		}
	

		// if higher value is found and guard is currently tracking keep moving in same direction
		bool setPos = false;
		if (guard.Tracking) {
			switch (guard.MovingDirection) {
			case GameConstants.Up:
				if (canGoUp) { points[0] = guard.LocationX-1; points[1] = guard.LocationY; setPos = true; }
				break;
			case GameConstants.Down:
				if (canGoDown) { points[0] = guard.LocationX+1; points[1] = guard.LocationY; setPos = true; }
				break;
			case GameConstants.Left:
				if (canGoLeft) { points[1] = guard.LocationY-1; points[0] = guard.LocationX; setPos = true; }
				break;
			case GameConstants.Right:
				if (canGoRight) { points[1] = guard.LocationY+1; points[0] = guard.LocationX; setPos = true;}
				break;
			}
		}


		// if higher value is found and guard is currently not tracking turn in that direction
		// or make turn if at dead end and guard is current tracking
		if (xToSave != -1 && yToSave != -1 && (!guard.Tracking || !setPos)) {
			int newX = xToSave;
			int newY = yToSave;
			int oldX = guard.LocationX;
			int oldY = guard.LocationY;
			
			bool[] directions = new bool[4];
			double[] distances = new double[4];
			distances[GameConstants.Up] = Math.Sqrt (Math.Pow((oldX-1)-newX,2) + Math.Pow(oldY-newY,2));
			distances[GameConstants.Down] = Math.Sqrt (Math.Pow((oldX+1)-newX,2) + Math.Pow(oldY-newY,2));
			distances[GameConstants.Left] = Math.Sqrt (Math.Pow(oldX-newX,2) + Math.Pow((oldY-1)-newY,2));
			distances[GameConstants.Right] = Math.Sqrt (Math.Pow(oldX-newX,2) + Math.Pow((oldY+1)-newY,2));	
			directions[GameConstants.Up] = canGoUp;
			directions[GameConstants.Down] = canGoDown;
			directions[GameConstants.Left] = canGoLeft;
			directions[GameConstants.Right] = canGoRight;
			
			double bestDistance = 320000;
			int bestDirection = 0;
			
			for (int k = 0; k<4; k++) {
				if (distances[k] <= bestDistance && directions[k] == true) {
					bestDistance = distances[k];
					bestDirection = k;
				}
			}
			
			points[3] = bestDirection;
			switch (bestDirection) {
			case GameConstants.Up: // up 
				points[0] = guard.LocationX-1;
				points[1] = guard.LocationY;
				break;
			case  GameConstants.Down: // down
				points[0] = guard.LocationX+1;
				points[1] = guard.LocationY;
				break;
			case  GameConstants.Left: // left
				points[1] = guard.LocationY-1;
				points[0] = guard.LocationX;
				break;
			case  GameConstants.Right: // right
				points[1] = guard.LocationY+1;
				points[0] = guard.LocationX;
				break;
			}
		}
		
		
		// if no higher value is found then set target to random
		if (xToSave == -1 || yToSave == -1) {
			points[2] = 1;
		}

		return points;
	}
	
	/*
	 * Adds particle to cover hash list
	 */	
	public void AddParticleToCoverHash(Particle particle, int locationX, int locationY){
		FloorcellPartcileCover.Add (ParticleCoverHash(particle, locationX, locationY));
	}
	
	/*
	 * Creates unique value for floor cell and particle combination
	 */	
	public ulong ParticleCoverHash(Particle particle, int locationX, int locationY) {
		return (ulong)(((ulong)locationX)*((ulong)123) + ((ulong)locationY)*((ulong)23413) + particle.UniqueId);
	}
}
